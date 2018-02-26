//#define DEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
//using DG.Tweening;

public class ErrorMessageDialog : DialogBase
{
    public Text title;
    public Text message;

    public GameObject button;

    [HideInInspector]
    public bool showButton;

    public override void InitDialog()
    {
        this.transform.localPosition = new Vector3(0f, 0f, 0);

        this.InitStuff();
        
        this.dialogClickAllowed = true;

        SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.Click1);
    } // InitDialog

    /*
    public override void DialogOpened()
    {
        base.DialogOpened();
    }
    */

    public override void DoAction(string actionType = "")
    {
        if (!this.dialogClickAllowed)
        {
            UDebug.Log("[ErrorMessageDialog] click NOT allowed");
            return;
        }
        UDebug.Log("[DoAction] actionType = " + actionType);
        if (actionType == "Close")
        {
            DialogsController.Instance.Close(this.gameObject);
            return;
        }
        
        UDebug.LogWarning("[ErrorMessageDialog] [DoAction] actionType NOT allowed. actionType = " + actionType);
    } // DoAction
    
    private void InitStuff()
    {
        if (this.showButton)
        {
            this.button.SetActive(true);
        } else
        {
            this.button.SetActive(false);
        }
    } // InitStuff

} // ErrorMessageDialog