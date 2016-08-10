/**
 *  @file   Example_ZPlayerPrefs.cs
 *  @brief  Example_ZPlayerPrefs.cs
 *  @create 3/17/2015 3:41:30 PM
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
using System.Collections;
using UnityEngine.UI;

using System;
using System.Security.Cryptography;
using System.Text;


public class Example_ZPlayerPrefs : MonoBehaviour
{
    public Text txtStatus;

    void Start()
    {
        // set false to see plain data
        //ZPlayerPrefs.useSecure = false;

        txtStatus.text = "Initialize";
        ZPlayerPrefs.Initialize("what'sYourName", "salt12issalt");

        txtStatus.text = "Set Value";
        Debug.Log("-------------------------");
        Debug.Log("Set Value");
        SetPlayerPrefs();

        txtStatus.text = "Read Get Value First Time";
        Debug.Log("-------------------------");
        Debug.Log("Read Get Value First Time");
        GetPlayerPrefs();

        txtStatus.text = "Read Get Value Second Time";
        Debug.Log("-------------------------");
        Debug.Log("Read Get Value Second Time");
        GetPlayerPrefs();

        txtStatus.text = "See the console for the result";

        StartCoroutine(BlinkStatus());
    }

    IEnumerator BlinkStatus()
    {
        yield return null;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            txtStatus.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);
            txtStatus.gameObject.SetActive(false);
        }
    }


    public void SetPlayerPrefs()
    {
        // Set Value
        ZPlayerPrefs.SetString("string1", "string 1 is ZETO");
        ZPlayerPrefs.SetString("string2", "string 2 is second");
        ZPlayerPrefs.SetString("string3", "string 3 is 333");

        ZPlayerPrefs.SetFloat("Float1", 0.5234234f);
        ZPlayerPrefs.SetFloat("Float2", 82374234.2328384f);
        ZPlayerPrefs.SetFloat("Float3", 2.386f);

        ZPlayerPrefs.SetInt("Int1", 5000);
        ZPlayerPrefs.SetInt("Int2", -6548);
        ZPlayerPrefs.SetInt("Int3", 37);

        ZPlayerPrefs.Save();
    }

    public void GetPlayerPrefs()
    {
        // Get Value to check 
        Debug.Log("Get Value string1: " + ZPlayerPrefs.GetString("string1") + ", Encrypt: " + ZPlayerPrefs.GetRowString("string1"));
        Debug.Log("Get Value string2: " + ZPlayerPrefs.GetString("string2") + ", Encrypt: " + ZPlayerPrefs.GetRowString("string2"));
        Debug.Log("Get Value string3: " + ZPlayerPrefs.GetString("string3") + ", Encrypt: " + ZPlayerPrefs.GetRowString("string3"));

        Debug.Log("Get Value Float1: " + ZPlayerPrefs.GetFloat("Float1") + ", Encrypt: " + ZPlayerPrefs.GetRowString("Float1"));
        Debug.Log("Get Value Float2: " + ZPlayerPrefs.GetFloat("Float2") + ", Encrypt: " + ZPlayerPrefs.GetRowString("Float2"));
        Debug.Log("Get Value Float3: " + ZPlayerPrefs.GetFloat("Float3") + ", Encrypt: " + ZPlayerPrefs.GetRowString("Float3"));

        Debug.Log("Get Value Int1: " + ZPlayerPrefs.GetInt("Int1") + ", Encrypt: " + ZPlayerPrefs.GetRowString("Int1"));
        Debug.Log("Get Value Int2: " + ZPlayerPrefs.GetInt("Int2") + ", Encrypt: " + ZPlayerPrefs.GetRowString("Int2"));
        Debug.Log("Get Value Int3: " + ZPlayerPrefs.GetInt("Int3") + ", Encrypt: " + ZPlayerPrefs.GetRowString("Int3"));
    }

}


