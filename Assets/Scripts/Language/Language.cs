using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyTonaShooterTest.Languages
{
	public class Language
	{
		private static Dictionary<string, string> _data;
		public static Dictionary<string, string> data => _data;

		static Language()
		{
			LoadLanguage();
		}

		public static void ChangeLanguage(string language)
		{
			PlayerPrefs.SetString("Language", language);
			LoadLanguage();
			SceneManager.LoadScene(0);
		}

		private static void LoadLanguage()
		{
			_data = new Dictionary<string, string>();

			JSONObject json;
			TextAsset langFile;
			switch (PlayerPrefs.GetString("Language", "English"))
			{
				case "Russian":
					langFile = Resources.Load<TextAsset>("Languages/ru");
					json = new JSONObject(langFile.text);
					break;
				default:
					langFile = Resources.Load<TextAsset>("Languages/en");
					json = new JSONObject(langFile.text);
					break;
			}

			for (int i = 0; i < json.list.Count; i++)
			{
				string key = (string)json.keys[i];
				JSONObject value = (JSONObject)json.list[i];
				//Debug.Log("Key: " + key + " Value: " + value.str);
				data.Add(key, value.str);
			}

		}

	}

}
