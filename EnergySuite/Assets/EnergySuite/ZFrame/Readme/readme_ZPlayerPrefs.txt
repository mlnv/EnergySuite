ZPlayerPrefs ReadMe


Overview:
	- Secure your your PlayerPrefs data.
	- This script Encrypt / Decrypt your PlayerPrefs.
	- It is very simple to use as same as how you use PlayerPrefs.


How to Use:
	1. Initialize with your password and salt phrase
		ZPlayerPrefs.Initialize("your password", "your salt");
	
	2. Use Same way as PlyerPrefs
		ZPlayerPrefs.SetString("string Key", "String Value");
		ZPlayerPrefs.SetFloat("float key", "float Value");
		ZPlayerPrefs.SetInt("int key", "int Value");
		ZPlayerPrefs.Save();

		string strValue = ZPlayerPrefs.GetString("string Key");
		float strValue = ZPlayerPrefs.GetFloat("float Key");
		int strValue = ZPlayerPrefs.GetInt("int Key");

		...

Example:
	Open Example/Example_ZPlayerPrefs scene and Play.
	This scene will Set string, float, int value and Get them and log out to console.


Version:
	2015. 03. 24.		1.0 		Initial release

Author:
	- Studio ZERO
	- E-mail: studiozero000@gmail.com

