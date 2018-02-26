using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class StartPoint : MonoBehaviour {
    public Environments environment;//定义个环境枚举变量
    public WheelMode wheelDataMode;//定义个本地还是服务器枚举
    public GameObject payMoney;
    public bool isMoveWheel = false;
    public static StartPoint instance;
  
    
    [HideInInspector]
    public int expectIndex;

    [Header("")]
    public int initialWinChanceFromOne;
    public int[] probabilityUpFromOne;
    public int expectIndexFromOne;

    [Header("")]
    public int initialWinChanceFromTwo;
    public int expectIndexFromTwo;
    public int[] probabilityUpFromTwo;
    [HideInInspector]
    public int initialWinchance;
  
  
    [HideInInspector]
    public int[] probabilityUp;
    void Awake()
	{
        instance = this;
        //Application.targetFrameRate = 30;//移动平台默认侦速率为30真

        DOTween.Init();//自定义初始化Tween
        Config.Init();//？暂且认为是Json的一种初始化

        TextAsset loc = Resources.Load("localization") as TextAsset;//加载一个叫localization的文件
        LocalizationController.Instance.currentLanguageCode = "en";//可以说是创建键
        LocalizationController.Instance.ParseKeysData(loc.ToString());//判断字符串转的JSON是否为空，或该文件是否存在，如果存在将该键值对调价到["en":Dication<string,string]中


        MainController.Instance.startPoint = this;//Creat一个MainControl的GameObject
        MainController.Instance.environment = this.environment;
        if (MainController.Instance.environment == Environments.dev)//测试环境
        {
            UDebug.enabled = true;
        }
        else if (MainController.Instance.environment == Environments.prod)//生产环境
        {
            UDebug.enabled = false;
        }
        else
        {
            UDebug.LogError("[StartPoint] environment mode is not supported.");
        }
        payMoney.SetActive(true);

    }

    public void initFortuneWheel(string _ref)
    {
        
        // added by ben start
        if (isMoveWheel == false)
        {
            MoveWheelSetUp.Instance.rewardItem = MoveWheelSetUp.Instance.rewardItemOne;
            MoveWheelSetUp.Instance.rewardItem[expectIndex].winChance = initialWinChanceFromOne;
            expectIndex = expectIndexFromOne;
            probabilityUp = probabilityUpFromOne;
            initialWinchance = initialWinChanceFromOne;
            Debug.Log("rewardItem = rewardItemOne");
        }
        else
        {
            MoveWheelSetUp.Instance.rewardItem = MoveWheelSetUp.Instance.rewardItemTwo;

            MoveWheelSetUp.Instance.rewardItem[expectIndex].winChance = initialWinChanceFromTwo;
            expectIndex = expectIndexFromTwo;
            probabilityUp = probabilityUpFromTwo;
            initialWinchance = initialWinChanceFromTwo;
            Debug.Log("rewardItem = rewardItemTwo");
        }
        //// added by ben end


        MainController.Instance.StartInit();//开始游戏初始化播放音乐，隐藏Dialog,show FortunWheel

    }

    
} // StartPoint