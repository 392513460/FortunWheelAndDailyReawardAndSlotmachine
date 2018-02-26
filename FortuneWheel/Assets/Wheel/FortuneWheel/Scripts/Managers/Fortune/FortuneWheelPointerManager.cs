//#define MYDEBUG

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FortuneWheelPointerManager : MonoBehaviour {

    private bool inTween = false;
    private bool inSoundEffect;
    private int nextSoundEffectType = 1;

    public void UpdateRotation(float angle)
    {
        //DebugMy.Log("[UpdateRotation] angle = " + angle);
        if (angle > 3 && angle < 6)
        {
            this.transform.DOKill();
            this.inTween = false;
            float a = 6 - angle;
            this.transform.localRotation = Quaternion.Euler(0, 0, a * 16.66f);

            if (!this.inSoundEffect)
            {
                switch (this.nextSoundEffectType)
                {
                    case 1:
                        SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.FortuneWheel1);
                        this.nextSoundEffectType = 2;
                        break;
                    case 2:
                        SoundEffectsController.Instance.PlayEffect(SoundEffectsTypes.FortuneWheel2);
                        this.nextSoundEffectType = 1;
                        break;
                    default:
                        UDebug.LogError("[FortuneWheelPointerManager] [UpdateRotation] nextSoundEffectType is not allowed (" + this.nextSoundEffectType + " provided)");
                        break;
                }
                this.inSoundEffect = true;
            }

            return;
        }
        if (angle <= 3 || angle > 30) 
        {
            this.transform.DOKill();
            this.inTween = false;
            this.transform.localRotation = Quaternion.Euler(0, 0, 52);

            if (angle > 30)
            {
                this.inSoundEffect = false;
            }
            return;
        }

        if (this.transform.localRotation != Quaternion.identity && !this.inTween)
        {
            this.inTween = true;
            this.transform.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast).OnComplete(this.TweenIsDone);
        }
    } // UpdateRotation

    private void TweenIsDone()
    {
#if MYDEBUG
        DebugMy.Log("[FortuneWheelPointerManager] [TweenIsDone]");
#endif
        this.inTween = false;
        this.inSoundEffect = false;
    }
} // FortuneWheelPointerManager