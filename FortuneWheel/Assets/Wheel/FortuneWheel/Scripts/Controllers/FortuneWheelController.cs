//#define MYDEBUG

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;
public class FortuneOption
{
    public int id;
    public string type;
    public string name;
    public int qty;
    public int chance;
}

public class FortuneWheelController : MonoBehaviour {

    private static FortuneWheelController _instance;
    
    public static FortuneWheelController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<FortuneWheelController>();
                go.name = "FortuneWheelController";
                go.transform.parent = Common.GetControllerHolder();
                _instance.Init();
            }
            return _instance;
        }
    }

    public float freeTryAvaiableFrom;
    public int hardPrice;
    public int softPrice;

    [HideInInspector]
    public List<FortuneOption> wheelOptions;//转盘选项
    //[HideInInspector]
    public FortuneWheelManager fortuneWheelManager; // selfregister
    [HideInInspector]
    public Dictionary<string, Sprite> optionsIcons;

    private bool showedOnStart = false;

    public bool ShowOnStartIfAllowed()
    {
        if (!this.showedOnStart && (FortuneWheelController.Instance.freeTryAvaiableFrom - Time.realtimeSinceStartup) <= 0)
        {
            // show
            MainController.Instance.ShowFortuneWheel();
            DialogsController.Instance.panels.loadingPanel.HideSmoothly(0.3f, 0.5f);
            return true;
        }
        this.showedOnStart = true;
        return false;
    }

    public void ParseInitData(string data, int code = 200)
    {
#if MYDEBUG
        DebugMy.Log("[FortuneWheelController] [ParseInitData]");
#endif
        if (code != 200)
        {
            // some another problem
            UDebug.LogError("[FortuneWheelController] [ParseInitData] code = " + code + " | some problem");
            return;
        }

        JSONObject jsonData = new JSONObject(data);
        if (jsonData == null || jsonData.IsNull)
        {
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] something wrong with server response. [data] = " + data);
            return;
        }
        if (!jsonData.HasField("data") || !jsonData["data"].IsObject)
        {
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] no 'data' field in response. data = " + data);
            return;
        }

        jsonData = jsonData["data"];

        this.GetRestoreTime(jsonData);

        this.wheelOptions = new List<FortuneOption>();

        if (jsonData.HasField("cost") && jsonData["cost"].IsObject)
        {
            if (jsonData["cost"].HasField("hard") && jsonData["cost"]["hard"].IsNumber)
            {
                this.hardPrice = (int)jsonData["cost"]["hard"].n;
            }
            if (jsonData["cost"].HasField("soft") && jsonData["cost"]["soft"].IsNumber)
            {
                this.softPrice = (int)jsonData["cost"]["soft"].n;
            }
            if (this.hardPrice == 0 && this.softPrice == 0)
            {
                UDebug.LogError("[FortuneWheelController] [ParseInitData] not 'hard' and 'soft' parameters in 'cost' object");
            }
        }
        else
        {
            UDebug.LogError("[FortuneWheelController] [ParseInitData] 'cost' parameter is not found");
        }

        if (jsonData.HasField("options") && jsonData["options"].IsObject)
        {
            for (int i = 0; i < jsonData["options"].list.Count; i++)
            {
                this.AddOption(jsonData["options"].keys[i], jsonData["options"][i]);
            }
        }
        else
        {
            UDebug.LogError("[FortuneWheelController] [ParseInitData] 'options' parameter is not found OR is not Array");
        }
#if MYDEBUG
        DebugMy.Log("[FortuneWheelController] [ParseInitData] total options = " + this.wheelOptions.Count);
#endif
    } // ParseInitData

    public void WinnerServerResponseParse(string data, int code = 200)
    {
        if (code == 402)
        {
            // problem with money
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] code = " + code + " | problem with money");
            return;
        }
        if (code != 200)
        {
            // some another problem
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] code = " + code + " | some another problem");
            return;
        }

        JSONObject jsonData = new JSONObject(data);
        if (jsonData == null || jsonData.IsNull)
        {
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] something wrong with server response. [data] = " + data);
            return;
        }
        if (!jsonData.HasField("data") || !jsonData["data"].IsObject)
        {
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] no 'data' field in response. data = " + data);
            return;
        }

        this.GetRestoreTime(jsonData["data"]);

        if (jsonData["data"].HasField("winId") && jsonData["data"]["winId"].IsNumber)
        {
            if (fortuneWheelManager == null)
            {
                fortuneWheelManager = GameObject.Find("Wheel").GetComponent<FortuneWheelManager>();
               
            }
            this.fortuneWheelManager.SetWinData((int)jsonData["data"]["winId"].n);
        } else
        {
            UDebug.LogError("[FortuneWheelController] [WinnerServerResponseParse] no 'winId' field in response. data = " + data);
            return;
        }
    } // WinnerServerResponseParse
   
    public Sprite GetSpriteForOption(FortuneOption option)//返回奖品sprite
    {

        string name;
        //if (this.optionsIcons.ContainsKey(name))
        //{
        //    return this.optionsIcons[name];
        //}
        //Debug.Log(fortuneWheelManager.luckyRoll.localEulerAngles.z);
        //UDebug.LogError("[FortuneWheelController] [GetSpriteForOption] Cannot find icon for '" + name + "'");
        if ((Mathf.Abs(fortuneWheelManager. luckyRoll.localEulerAngles.z)>=0&&Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z)<18)||(Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z )>= 342 &&Mathf.Abs( fortuneWheelManager.luckyRoll.eulerAngles.z) < 360))
        {

            // 去掉10个硬写的 StartPoint.instance.isMoveWheel == false 判断，其实你当初在复制黏贴的时候肯定觉得这里不对了，
            //如果程序中出现了大量重复的写法和逻辑，一定是用了特别复杂和笨的办法，而且这种复制黏贴出来的代码极难维护。
            //把逻辑判断挪到前面去会简化后面代码中的判断数量。
            name = MoveWheelSetUp.Instance.rewardItem[0].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[0].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 18 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 54)
        {
            name = MoveWheelSetUp.Instance.rewardItem[1].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[1].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 54 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 90)
        {
            name = MoveWheelSetUp.Instance.rewardItem[2].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[2].rewardSprite;
        }
        if (Mathf.Abs( fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 90&&Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z)< 126)
        {
            name = MoveWheelSetUp.Instance.rewardItem[3].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[3].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 126 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 162)
        {
            name = MoveWheelSetUp.Instance.rewardItem[4].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[4].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 162 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 198)
        {
            name = MoveWheelSetUp.Instance.rewardItem[5].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[5].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 198 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 234)
        {
            name = MoveWheelSetUp.Instance.rewardItem[6].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[6].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 234 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 270)
        {
            name = MoveWheelSetUp.Instance.rewardItem[7].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[7].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 270 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 306)
        {
            name = MoveWheelSetUp.Instance.rewardItem[8].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[8].rewardSprite;
        }
        if (Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) >= 306 && Mathf.Abs(fortuneWheelManager.luckyRoll.localEulerAngles.z) < 342)
        {
            name = MoveWheelSetUp.Instance.rewardItem[9].rewardQuantity.ToString();
            option.qty = int.Parse(name);
            return MoveWheelSetUp.Instance.rewardItem[9].rewardSprite;
        }
      
        return null;
    } // GetSpriteForOption

    // for local mode only
   
    private int pressCount = 0;

    public FortuneOption GetWinnerOptionByChance()//得到赢得机会
    {
        //var chancesSum = 0;
        var chancesSum = 100;
        int count = 0;
        //this.wheelOptions.ForEach(o => chancesSum += o.chance);
        List<FortuneOption> shuffledOptions = new List<FortuneOption>(this.wheelOptions);
        shuffledOptions.Shuffle();
        //var selectedChance = Random.Range(0, chancesSum);
        //chancesSum = 0;
        foreach (FortuneOption fo in shuffledOptions)
        {
            float random = Random.Range(0, chancesSum);

            fo.chance = MoveWheelSetUp.Instance.rewardItem[count].winChance;
            //chancesSum += fo.chance;
            //if (selectedChance <= chancesSum)
            //    return fo;
          
            if (random<=fo.chance)
            {
               
                fo.id = MoveWheelSetUp.Instance.rewardItem[count].id;
             

                Debug.Log("pressCount::" + pressCount);
                if (TimeManager.instance.isPassNowDay == true)
                {
                    if (count == StartPoint.instance.expectIndex)
                    {
                        pressCount = 0;
                        MoveWheelSetUp.Instance.rewardItem[count].winChance = StartPoint.instance.initialWinchance;
                        TimeManager.instance.isPassNowDay = false;
                    }
                    else
                    {
                        pressCount++;
                        if (pressCount == 1)
                        {
                            MoveWheelSetUp.Instance.rewardItem[StartPoint.instance.expectIndex].winChance = StartPoint.instance.probabilityUp[0];
                        }
                        if (pressCount >= 2)
                        {
                            MoveWheelSetUp.Instance.rewardItem[StartPoint.instance.expectIndex].winChance = StartPoint.instance.probabilityUp[1];
                        }
                        Debug.Log("reawaredItem[" + StartPoint.instance.expectIndex + "].chance::" + MoveWheelSetUp.Instance.rewardItem[StartPoint.instance.expectIndex].winChance);

                    }
                }
                else
                {

                }
                
                chancesSum = 100;
                return fo;
            }
            else
            {
                random -= fo.chance;
            }
          


        if(count<10)   
            {
                count++;
            }
        }
        
        return shuffledOptions[0];
    } // GetWinnerOptionByChance

    private void GetRestoreTime(JSONObject data)
    {
        if (data.HasField("restore") && data["restore"].IsNumber)
        {
            float val = (float)data["restore"].n;
            if (val > 0)
            {
                this.freeTryAvaiableFrom = Time.realtimeSinceStartup + val;
#if MYDEBUG
                DebugMy.Log("[FortuneWheelController] [GetRestoreTime] this.freeTryAvaiableFrom = " + this.freeTryAvaiableFrom + " | val = " + val);
#endif
            }
        }
        else
        {
            UDebug.LogError("[FortuneWheelController] [GetRestoreTime] 'restore' parameter is not found");
        }
    } // GetRestoreTime

    private void AddOption(string key, JSONObject data)
    {
#if MYDEBUG
        DebugMy.Log("[AddOption] " + key);
#endif
        FortuneOption option = new FortuneOption();
        option.id = int.Parse(key);
#if MYDEBUG
        DebugMy.Log("[AddOption] data = " + data.ToString());
#endif
        if (data.HasField("type") && data["type"].IsString)
        {
            option.type = data["type"].str;
        }
        if (data.HasField("name") && data["name"].IsString)
        {
            option.name = data["name"].str;
        }
        if (data.HasField("qty") && data["qty"].IsNumber)
        {
            option.qty = (int)data["qty"].n;
        }
        if (data.HasField("chance") && data["chance"].IsNumber)
        {
            option.chance = (int)data["chance"].n;
        }
        this.wheelOptions.Add(option);
    } // AddOption

    private void Init()
    {
        // load icons
        string folder = "FortuneWheelIcons";
        if (LinkedResourcesManager.Instance == null)
        {
            Invoke("Init", 1.0f);
            return;
        }
        Sprite[] sprites = LinkedResourcesManager.Instance.GetLinkedObjectsByFolder<Sprite>(folder);  //Resources.LoadAll<Sprite>(folder);
        if (sprites == null || sprites.Length == 0)
        {
            UDebug.LogError("[FortuneWheelController] [Init] error! Cannot load icons from Resourses/" + folder);
            return;
        }
        this.optionsIcons = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            this.optionsIcons.Add(sprites[i].name, sprites[i]);
        } // for
    } // Init

} // FortuneWheelController