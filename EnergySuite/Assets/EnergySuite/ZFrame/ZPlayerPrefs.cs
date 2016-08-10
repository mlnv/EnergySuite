/**
 *  @file   ZPlayerPrefs.cs
 *  @brief  ZPlayerPrefs.cs
 *  @create 3/17/2015 3:26:21 PM
 *  @author ZETO
 *  @Copyright (c) 2015 Studio ZERO. All rights reserved.
 */

/*==============================================================================
                        EDIT HISTORY FOR MODULE
when        who     what, where, why
DD/MM/YYYY
----------  ---     ------------------------------------------------------------
            ZETO    Initial Create

==============================================================================*/

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
 

public static class ZPlayerPrefs
{
    // Set false if you don't want to use encrypt/decrypt value
    // You could use #if UNITY_EDITOR for check your value
    public static bool useSecure = true;

    const int Iterations = 555;

    // You should Change following password and IV value using Initialize
    static string strPassword = "IamZETOwow!123";
    static string strSalt = "IvmD123A12";
    static bool hasSetPassword = false;

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static float GetFloat(string key)
    {
        return GetFloat(key, 0.0f);
    }

    public static float GetFloat(string key, float defaultValue, bool isDecrypt = true)
    {
        float retValue = defaultValue;

        string strValue = GetString(key);

        if (float.TryParse(strValue, out retValue))
        {
            return retValue;
        }
        else
        {
            return defaultValue;
        }
    }

    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    public static int GetInt(string key, int defaultValue, bool isDecrypt = true)
    {
        int retValue = defaultValue;

        string strValue = GetString(key);

        if (int.TryParse(strValue, out retValue))
        {
            return retValue;
        }
        else
        {
            return defaultValue;
        }
    }

    
    public static string GetString(string key)
    {
        string strEncryptValue = GetRowString(key);

        return Decrypt(strEncryptValue, strPassword);
    }

    public static string GetRowString(string key)
    {
        CheckPasswordSet();

        string strEncryptKey = Encrypt(key, strPassword);
        string strEncryptValue = PlayerPrefs.GetString(strEncryptKey);

        return strEncryptValue;
    }

    public static string GetString(string key, string defaultValue)
    {
        string strEncryptValue = GetRowString(key, defaultValue);
        return Decrypt(strEncryptValue, strPassword);
    }

    public static string GetRowString(string key, string defaultValue)
    {
        CheckPasswordSet();

        string strEncryptKey = Encrypt(key, strPassword);
        string strEncryptDefaultValue = Encrypt(defaultValue, strPassword);

        string strEncryptValue = PlayerPrefs.GetString(strEncryptKey, strEncryptDefaultValue);

        return strEncryptValue;
    }

    public static bool HasKey(string key)
    {
        CheckPasswordSet();
        return PlayerPrefs.HasKey(Encrypt(key, strPassword));
    }
    
    public static void Save()
    {
        CheckPasswordSet();
        PlayerPrefs.Save();
    }
    
    public static void SetFloat(string key, float value)
    {
        string strValue = System.Convert.ToString(value);
        SetString(key, strValue);
    }
    
    public static void SetInt(string key, int value)
    {
        string strValue = System.Convert.ToString(value);
        SetString(key, strValue);
    }
   
    public static void SetString(string key, string value)
    {
        CheckPasswordSet();
        PlayerPrefs.SetString(Encrypt(key, strPassword), Encrypt(value, strPassword));
    }

    /////////////////////////////////////////////////////////////////
    // Help Function
    /////////////////////////////////////////////////////////////////
    public static void Initialize(string newPassword, string newSalt)
    {
        strPassword = newPassword;
        strSalt = newSalt;

        hasSetPassword = true;
    }

    
    static void CheckPasswordSet()
    {
        if (!hasSetPassword)
        {
            Debug.LogWarning("Set Your Own Password & Salt!!!");
        }
    }

    static byte[] GetIV()
    {
        byte[] IV = Encoding.UTF8.GetBytes(strSalt);
        return IV;
    }

    static string Encrypt(string strPlain, string password)
    {
        if (!useSecure)
            return strPlain;

        try
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, GetIV(), Iterations);

            byte[] key = rfc2898DeriveBytes.GetBytes(8);

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, GetIV()), CryptoStreamMode.Write))
            {
                memoryStream.Write(GetIV(), 0, GetIV().Length);

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(strPlain);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        catch (Exception e)
        {
//            Debug.LogWarning("Encrypt Exception: " + e);
            return strPlain;
        }
    }

    static string Decrypt(string strEncript, string password)
    {
        if (!useSecure)
            return strEncript;

        try
        {
            byte[] cipherBytes = Convert.FromBase64String(strEncript);

            using (var memoryStream = new MemoryStream(cipherBytes))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] iv = GetIV();
                memoryStream.Read(iv, 0, iv.Length);

                // use derive bytes to generate key from password and IV
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, iv, Iterations);

                byte[] key = rfc2898DeriveBytes.GetBytes(8);

                using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    string strPlain = streamReader.ReadToEnd();
                    return strPlain;
                }
            }
        }
        catch (Exception e)
        {
//            Debug.LogWarning("Decrypt Exception: " + e);
            return strEncript;
        }

    }

}


