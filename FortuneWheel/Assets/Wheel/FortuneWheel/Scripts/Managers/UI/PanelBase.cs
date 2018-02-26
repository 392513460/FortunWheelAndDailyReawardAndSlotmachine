using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PanelBase : MonoBehaviour {
    public CanvasGroup panel;
    [HideInInspector]
    public bool panelClickAllowed = false;

    public void HideSmoothly(float time = 0.2f, float delay = 0)//隐藏Loading
    {
        UDebug.Log("[PanelBase] [HideSmoothly] this.gameObject.activeInHierarchy = " + this.gameObject.activeInHierarchy);
        if (this.gameObject.activeInHierarchy)
        {
            if (delay == 0)
            {
                DOTween.To(() => this.panel.alpha, a => this.panel.alpha = a, 0.0f, time).OnComplete(this.HideInstantly);
            }
            else
            {
                StartCoroutine(this.HideSmoothlyIE(time, delay));
            }
        }
    } // HideSmoothly

    private IEnumerator HideSmoothlyIE(float time, float delay)//延迟加载
    {
        if (this.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(delay);
            DOTween.To(() => this.panel.alpha, a => this.panel.alpha = a, 0.0f, time).OnComplete(this.HideInstantly);//如果该动画执行完毕就调取该函数隐藏
        }
    } // HideSmoothly

    public void ShowSmoothly(float time = 0.2f, bool setActive = true)
    {
        if (setActive)
        {
            this.gameObject.SetActive(true); 
        }
        //this.PanelOpened();
        if (this.gameObject.activeInHierarchy)
        {
            DOTween.To(() => this.panel.alpha, a => this.panel.alpha = a, 1.0f, time).OnComplete(this.ShowInstantly);
        }
    } // ShowSmoothly

    public void HideInstantly()
    {
        this.gameObject.SetActive(false);
        this.panel.interactable = false;
        this.panel.alpha = 0.0f;
        this.PanelClosed();
    } // HideInstantly

    public void ShowInstantly()
    {
        this.gameObject.SetActive(true);
        this.panel.alpha = 1.0f;
        this.panel.interactable = true;
        this.PanelOpened();
    } // ShowInstantly

    public virtual void PanelOpened()
    {
        UDebug.Log("[PanelOpened] this.gameObject.name = " + this.gameObject.name);
        Invoke("AllowClick", 0.1f);
    } // PanelOpened

    public virtual void PanelClosed()
    {
        this.panelClickAllowed = false;
        this.panel.interactable = false;
    } // PanelOpened

    void AllowClick()
    {
        this.panelClickAllowed = true;
        this.panel.interactable = true;

        //DebugMy.LogWarning("[PanelBase] [AllowClick] this.panel.interactable = " + this.panel.interactable);
    } // AllowClick

    public virtual void DoAction(string actionType = "")
    {
    }

} // PanelBase