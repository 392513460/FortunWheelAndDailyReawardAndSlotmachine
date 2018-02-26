//#define MYDEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FortuneWheelPanelManager : PanelBase
{
    public FortuneWheelManager fortuneWheelManager;
    public MoneyHolderManager crystalsMoneyHolder;//金钱管理系统

    public override void PanelOpened()//FortuneWheel
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.fortuneWheelManager.Init();//Init
    } // PanelOpened


    public void BackToMainMenu()
    {
#if MYDEBUG
        DebugMy.Log("[FortuneWheelPanelManager] [BackToMainMenu]");
#endif
        if (!this.fortuneWheelManager.inAction)
        {
            this.crystalsMoneyHolder.num.text = "";
            DialogsController.Instance.panels.fortuneWheelPanelManager.HideSmoothly();

            DialogsController.Instance.panels.menuPanelManager.ShowSmoothly();
        }
    }

} // FortuneWheelPanelManager