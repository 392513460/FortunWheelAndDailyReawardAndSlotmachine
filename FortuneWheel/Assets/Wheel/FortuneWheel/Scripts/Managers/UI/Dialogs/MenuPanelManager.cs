//#define MYDEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuPanelManager : PanelBase
{
    public override void PanelOpened()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
    } // PanelOpened

    public void ToFortuneWheel()
    {
        StartPoint.instance.initFortuneWheel("from button");
        DialogsController.Instance.panels.menuPanelManager.HideSmoothly();//����BUTTON
        DialogsController.Instance.panels.fortuneWheelPanelManager.ShowSmoothly();//��ʾFortunewheel
    }

} // MenuPanelManager