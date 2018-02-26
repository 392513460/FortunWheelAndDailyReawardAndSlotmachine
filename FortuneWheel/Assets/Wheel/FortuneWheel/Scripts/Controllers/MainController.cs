using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Environments
{
    dev,
    prod
}

public enum WheelMode
{
    local,
    server
}

public class MainController : MonoBehaviour { 

    [HideInInspector]
    public StartPoint startPoint; // self registered

    public Environments environment;

    private static MainController _instance;

    public static MainController Instance 
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<MainController>();
                go.name = "MainController";
                go.transform.parent = Common.GetControllerHolder();
            }
            return _instance;
        }
    }

    public void StartInit() //FortuneWheel开始初始化
    {
        UDebug.Log("[MainController] [Init]");
        SoundEffectsController.Instance.PlayMusic(SoundEffectsTypes.Music1, 15);//开始播放音乐,音量=15

        // for demo
        PlayerController.Instance.hardMoney = 50;//初始化金钱
        // for demo

        if (MainController.Instance.startPoint.wheelDataMode == WheelMode.local)//本地测试
        {
            // demo delay
            Invoke("ShowFortuneWheel", 2.0f);
            FortuneWheelController.Instance.ParseInitData(Resources.Load<TextAsset>("local_fortune_options").ToString());
        } else
        {
            ServerController.Instance.InitFortuneWheel();
        }
    } // StartInit

    public void ShowFortuneWheel()
    {
        DialogsController.Instance.panels.loadingPanel.HideSmoothly();//隐藏Loading UI
        DialogsController.Instance.CloseAllOpenDialogs();//2017/10/22 关闭所有对话
        DialogsController.Instance.panels.fortuneWheelPanelManager.ShowSmoothly();
    } // ShowFortuneWheel

} // MainController