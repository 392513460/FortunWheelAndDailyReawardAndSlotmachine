//#define MYDEBUG

using UnityEngine;
using UnityEngine.UI;

//using DG.Tweening;

public class BuyHardMoneyDialog : DialogBase
{

    public override void InitDialog()
    {
        this.transform.localPosition = new Vector3(0f, 0f, 0);

        this.dialogClickAllowed = true;

        SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.Click1);
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
        UDebug.LogWarning("[BuyHardMoneyDialog] [DoAction] actionType NOT allowed. actionType = " + actionType);
    } // DoAction

} // BuyHardMoneyDialog