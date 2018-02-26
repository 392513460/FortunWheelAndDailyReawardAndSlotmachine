using UnityEngine;
using System.Collections;

public class WheelButtonManager : MonoBehaviour {
    public Transform icon;

    public void OpenWheelDialog()
    {
        if (DialogsController.Instance.IsBlockerOpened())
        {
            UDebug.Log("Blocker popup is opened");
            return;
        }
        MainController.Instance.ShowFortuneWheel();
    }

    public void DoHoverEffect()
    {
        StartCoroutine(this.ShowEffect());
    }

    IEnumerator ShowEffect() 
    {
        this.icon.localRotation = Quaternion.identity;
        SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.FortuneWheel1);
        bool secondSound = false;
        do
        {
            this.icon.Rotate(0, 0, 300 * Time.deltaTime);
            if (this.icon.localRotation.eulerAngles.z > 90 && !secondSound)
            {
                secondSound = true;
                SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.FortuneWheel2);
            }
            yield return new WaitForEndOfFrame();
        } while (this.icon.localRotation.eulerAngles.z < 180);
        this.icon.localRotation = Quaternion.identity;
    } // ShowEffect
} // WheelButtonManager