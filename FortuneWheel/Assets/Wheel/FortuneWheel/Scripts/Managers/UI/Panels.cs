using UnityEngine;
using System.Collections;

public class Panels : MonoBehaviour {
    public Canvas canvas;//UICanvas
    public LoadingPanelManager loadingPanel;//加载的背景
    public FortuneWheelPanelManager fortuneWheelPanelManager;//幸运转盘Panel
    public MenuPanelManager menuPanelManager;//菜单Panel

    public GameObject version;//版本控制

    void Awake()
    {
        DialogsController.Instance.panels = this;//单利模式
    }

    void Start()
    {
        if (MainController.Instance.environment != Environments.dev)//如果不等于测试环境hideInactiveInHirechay
        {
            this.version.SetActive(false);
        }
    }

    //public void ShowLoadingSmoothlyInDelay(float delay)
    //{
    //    UDebug.Log("[Panels] [ShowLoadingSmoothlyInDelay]");
    //    DialogsController.Instance.panels.loadingPanel.panel.alpha = 0;
    //    DialogsController.Instance.panels.loadingPanel.panel.interactable = false;
    //    DialogsController.Instance.panels.loadingPanel.gameObject.SetActive(true);
    //    Invoke("ShowLoadingSmoothly", delay);//在Delay时间后调取这个函数
    //}

    //public void ShowLoadingSmoothly()
    //{
    //    UDebug.Log("[Panels] [ShowLoadingSmoothly] DialogsController.Instance.panels.loadingPanel.gameObject.activeInHierarchy = " + DialogsController.Instance.panels.loadingPanel.gameObject.activeInHierarchy);
    //    if (DialogsController.Instance.panels.loadingPanel.gameObject.activeInHierarchy)
    //    {
    //        DialogsController.Instance.panels.loadingPanel.ShowInstantly();
    //    }
    //}
} // Panels