using UnityEngine;
using System.Collections;

public class Panels : MonoBehaviour {
    public Canvas canvas;//UICanvas
    public LoadingPanelManager loadingPanel;//���صı���
    public FortuneWheelPanelManager fortuneWheelPanelManager;//����ת��Panel
    public MenuPanelManager menuPanelManager;//�˵�Panel

    public GameObject version;//�汾����

    void Awake()
    {
        DialogsController.Instance.panels = this;//����ģʽ
    }

    void Start()
    {
        if (MainController.Instance.environment != Environments.dev)//��������ڲ��Ի���hideInactiveInHirechay
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
    //    Invoke("ShowLoadingSmoothly", delay);//��Delayʱ����ȡ�������
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