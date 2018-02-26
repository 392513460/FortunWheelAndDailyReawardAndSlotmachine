using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingPanelManager : PanelBase
{
    public LocalizationManager description;

    //void OnEnable()
    //{
    //    this.ConnectionProblemMode(false);
    //}

    //public void PauseMode(bool pause)
    //{
    //    if (pause)
    //    {
    //        this.description.key = "panel.loading.gamepaused";
    //        this.description.Localize(); 
    //    } else
    //    {
    //        if (this.description.text != null)
    //        {
    //            this.description.text.text = "";
    //        }
    //    }
    //} // PauseMode

    public void ConnectionProblemMode(bool status)
    {
        if (status)
        {
            this.description.key = "panel.loading.connectionproblem";
            this.description.Localize();
        }
        else
        {
            if (this.description.text != null)
            {
                this.description.text.text = "";
            }
        }
    }

} // LoadingPanelManager