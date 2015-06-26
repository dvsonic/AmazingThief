using UnityEngine;
using System.Collections;
using System.Security;
using System.Text;

public class GameData 
{
    public static int score = 0;

    private static Hashtable _language;
    public static string getLanguage(string key)
    {
        if (null == _language)
        {
            string language = "config/language_en";
            if (Application.systemLanguage == SystemLanguage.Chinese)
                language = "config/language_cn";
            TextAsset asset = Resources.Load(language) as TextAsset;
            string data = Encoding.UTF8.GetString(asset.bytes);
            string[] ary = data.Split('\n');
            _language = new Hashtable();
            for (int i = 0; i < ary.Length; i++)
            {
                string str = ary[i];
                string[] ary2 = str.Split('\t');
                _language.Add(ary2[0], ary2[1]);
            }
        }
        return _language[key].ToString().Replace("<br>", "\n");
    }
}
