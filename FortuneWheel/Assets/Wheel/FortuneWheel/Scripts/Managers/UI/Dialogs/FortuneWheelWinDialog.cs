//#define MYDEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
//using DG.Tweening;

public class FortuneWheelWinDialog : DialogBase
{
    public Image icon;
    public Text count;

    [HideInInspector]
    public FortuneOption option;

    public override void InitDialog()
    {
        this.transform.localPosition = new Vector3(0f, 0f, 0);

        
        this.icon.sprite = FortuneWheelController.Instance.GetSpriteForOption(option);
        this.count.text = "x" + option.qty.ToString();
        //this.icon.sprite = TournamentsController.Instance.GetRewardIcon(option.name);
        this.icon.SetNativeSize();

        this.dialogClickAllowed = true;

        SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.CongratulationFortuneWheel);
    } // InitDialog

    public override void DoAction(string actionType = "")
    {
        if (!this.dialogClickAllowed)
        {
#if MYDEBUG
            DebugMy.Log("[FortuneWheelWinDialog] click NOT allowed");
#endif
            return;
        }
#if MYDEBUG
        DebugMy.Log("[DoAction] actionType = " + actionType);
#endif
        if (actionType == "Close")
        {
            DialogsController.Instance.Close(this.gameObject);
            return;
        }
        UDebug.LogWarning("[FortuneWheelWinDialog] [DoAction] actionType NOT allowed. actionType = " + actionType);
    } // DoAction

} // FortuneWheelWinDialog