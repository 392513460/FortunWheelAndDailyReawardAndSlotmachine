using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class LocalizationController : MonoBehaviour {//本地控制

    private static LocalizationController _instance;
    public static LocalizationController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<LocalizationController>();//初始化一个GameObjct并添加localizationController组件
                go.name = "LocalizationController";
                go.transform.parent = Common.GetControllerHolder();//return ControllersHolder.transform

                //_instance.Init();
            }
            return _instance;
        }
    }

    public static bool hasInstance
    {
        get
        {
            if (_instance != null)
            {
                return true;
            }
            return false;
        }
    }

    public string currentLanguageCode;
    public bool getLocalizationRequestStarted;
    public System.EventHandler onLanguageChangeEvent;//事件句柄

    private Dictionary<string, Dictionary<string, string>> keys;

    public void ParseKeysData(string data)
    {
        JSONObject o = new JSONObject(data);//解析文本资源
        if (o == null || o.IsNull)
        {
            UDebug.LogError("[LocalizationController] [ParseKeysData] Cannot parse server answer. Text is " + data);
            return;
        }
        if (!o.HasField("data"))
        {
            UDebug.LogError("[LocalizationController] [ParseKeysData] Cannot parse server answer. 'data' field is not found");
            return;
        }
        this.ParseKeysData(o["data"]);
    }

    public bool IsLanguageInited(string languageCode)
    {
        if (this.keys == null)
        {
            return false;
        }
        if (!this.keys.ContainsKey(languageCode))
        {
            return false;
        }
        if (this.keys[languageCode] == null)
        {
            return false;
        }
        if (this.keys[languageCode].Count == 0)
        {
            return false;
        }
        return true;
    } // IsLanguageInited

    public void ParseKeysData(JSONObject data)//为文本资源创建建值对
    {
        UDebug.Log("[LocalizationController] [ParseKeysData] localizationData = " + data.ToString());

        if (this.keys == null) 
        {
            this.keys = new Dictionary<string, Dictionary<string, string>>();//如果文本资源为空，为它创建键值对
        }
        if (!this.keys.ContainsKey(this.currentLanguageCode))
        {
            this.keys[this.currentLanguageCode] = new Dictionary<string, string>();//创建["en",string]键值对
        }
        
        for (int i = 0; i < data.list.Count; i++)//遍历键值对集合
        {
            if (!this.keys[this.currentLanguageCode].ContainsKey(data.keys[i]))
            {
                string text = data[data.keys[i]].str;
                text = text.Replace("\\n", System.Environment.NewLine);
                text = text.Trim();
                this.keys[this.currentLanguageCode].Add(data.keys[i], text);//添加locazition里面的键值对
                //DebugMy.Log(data[data.keys[i]].str + " >>> " + text);
            }
        }

        if (this.onLanguageChangeEvent != null)
        {
            this.onLanguageChangeEvent.Invoke(null, null);//不执行
        }
    } // ParseLivesResult

    public string GetText(string key, string[] keyParams = null)
    {
        string result = "";
        if (this.keys == null)
        {
            //DebugMy.LogError("[LocalizationController] [GetText] Localization keys are not inited!");
            return result + " [keys not inited]";
        }
        if (string.IsNullOrEmpty(this.currentLanguageCode))
        {
            //DebugMy.LogError("[LocalizationController] [GetText] currentLanguageCode is not inited!");
            return result + " [no language code]";
        }
        if (!this.keys.ContainsKey(this.currentLanguageCode) || this.keys[this.currentLanguageCode] == null)
        {
            //DebugMy.LogError("[LocalizationController] [GetText] keys for '"+this.currentLanguageCode + "' language are not inited!");
            return result + " [no language keys]";
        }
        if (!this.keys[this.currentLanguageCode].ContainsKey(key) || string.IsNullOrEmpty(this.keys[this.currentLanguageCode][key]))
        {
            UDebug.LogError("[LocalizationController] [GetText] key '" + key + "' is not present in '" + this.currentLanguageCode + "' language keys (or empty)!");
            return result + " [key not found] " + key;
        }
        if (keyParams != null && keyParams.Length > 0)
        {
            // try to apply params
            return string.Format(this.keys[this.currentLanguageCode][key], keyParams);
        }
        return this.keys[this.currentLanguageCode][key];
    } // GetText

} // LocalizationController