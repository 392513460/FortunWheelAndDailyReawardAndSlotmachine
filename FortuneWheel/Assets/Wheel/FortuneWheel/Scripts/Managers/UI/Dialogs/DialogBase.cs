//#define DEBUG

using UnityEngine;
using System.Collections;

public class DialogBase : MonoBehaviour {
    [HideInInspector]
    public Vector3 defaultLocalPosition;
    [HideInInspector]
    public bool dialogClickAllowed = false;

    protected bool sizeInited = false;

    /*
    public virtual void InitPosition()
    {
        this.dialogClickAllowed = false;

        this.defaultLocalPosition = this.transform.localPosition;
        this.defaultLocalPosition.z = 0;
        this.transform.localPosition = this.defaultLocalPosition;

        //DebugMy.Log("[DialogsParent] [InitPosition] DialogsController.Instance.OpenedDialogs = " + DialogsController.Instance.OpenedDialogs);

        if (DialogsController.Instance.OpenedDialogs < 2)
        {
            //DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UIPanel>().depth = 200;
            UIPanel p = DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UIPanel>();
            if (p != null) p.depth = 200;
        }
        else
        {
            //DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UISprite>().depth = 350;
            UIPanel p = DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UIPanel>();
            if (p != null) p.depth = 350;
        }

        if (DialogsController.Instance.OpenedDialogs > 1)
        {
            // add additional background
            //DebugMy.Log("[DialogsParent] [InitPosition] add additional background");
            NGUITools.SetActive(DialogsController.Instance.dialogs.dialogsBackground, true);

            DialogsController.Instance.dialogs.dialogsBackground.transform.localPosition = new Vector3(0, 0, DialogsController.Instance.OpenedDialogs * -10 + 1);
        }
    }
    */

    public virtual void DialogOpened()
    {
#if DEBUG
        UDebug.Log("[" + this.gameObject.name + "] [DialogOpened] DialogsController.Instance.openedDialogs = " + DialogsController.Instance.openedDialogs);
#endif
        Vector3 openedPosition = this.defaultLocalPosition; // - new Vector3(0, GameController.Instance.screenHeight * 0.03f, DialogsController.Instance.OpenedDialogs * 10);
        this.transform.localPosition = openedPosition;
#if DEBUG
        UDebug.Log("[" + this.gameObject.name + "] [DialogOpened] = " + this.transform.localPosition);
#endif
        Invoke("AllowClick", 0.1f);
    } // DialogOpened

    void AllowClick()
    {
        this.dialogClickAllowed = true;
    } // AllowClick

    public void DialogClosed()
    {
#if DEBUG
        UDebug.Log("[" + this.gameObject.name + "] [DialogClosed]");
#endif
        this.transform.localPosition = this.defaultLocalPosition;
        this.dialogClickAllowed = false;
        this.ResetDialog();
        /*
        if (DialogsController.Instance.OpenedDialogs < 2)
        {
            //DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UISprite>().depth = 200;
            UIPanel p = DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UIPanel>();
            if (p != null) p.depth = 200;
        }
        else
        {
            //DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UISprite>().depth = 350;
            UIPanel p = DialogsController.Instance.dialogs.dialogsBackground.GetComponent<UIPanel>();
            if (p != null) p.depth = 350;
        }
        */
        this.gameObject.SetActive(false);
    } // DialogClosed

    protected virtual void ResetDialog()
    {
    }

    public virtual void InitDialog()
    {
#if DEBUG
        UDebug.Log("[" + this.gameObject.name + "] [InitDialog]");
#endif
        /*
        if (!this.sizeInited)
        {
            DialogsController.Instance.InitDialogToScreen(this.transform.gameObject);
            this.InitSpecialElements();
            this.sizeInited = true;
        }
         * */
    } // InitDialog

    protected virtual void InitSpecialElements()
    {
    }

    public virtual void DoAction(string actionType = "")
    {
    }

} // DialogBase
