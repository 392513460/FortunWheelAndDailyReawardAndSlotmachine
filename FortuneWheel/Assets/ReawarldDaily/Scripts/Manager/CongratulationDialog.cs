using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace DailyReward
{
    public class CongratulationDialog : Dialogs
    {
        public Image rewadrdIcon;
        public Text quanity;


        public Vector3[] tween = new Vector3[10];

        Tweener tweener;
        Tweener tweener1;
        // Use this for initialization
        public static CongratulationDialog instance;
        void Start()
        {

            instance = this;

            tweener = rewadrdIcon.rectTransform.DOScale(tween[1], 0.05f);
            tweener1 = rewadrdIcon.rectTransform.DOLocalMove(tween[0], 0.05f).OnComplete(Close);
            if (int.Parse(quanity.text) >= 2)
            {
                tweener.SetLoops(30);
                tweener1.SetLoops(30);
            }

            tweener.Pause();
            tweener1.Pause();
        }

        // Update is called once per frame
        void Update()
        {

            //if (Dialogs.instance.isPressOk && icon.sprite.name == "crystal_icon_green")
            //{
            //    Dialogs.instance.Close();

            //}
            //if (Dialogs.instance.isPressOk && icon.sprite.name != "crystal_icon_green")
            //{
            //    tween[0] = new Vector3(-346, 245);
            //    tweener = rewadrdIcon.rectTransform.DOScale(tween[1], 0.8f);
            //    tweener1 = rewadrdIcon.rectTransform.DOLocalMove(tween[0], 0.8f).OnComplete(Close);

            //    tweener.Play();
            //    tweener1.Play();
            //    tweener.SetAutoKill(true);
            //    tweener1.SetAutoKill(true);
            //    Dialogs.instance.isPressOk = false;

            //}

        }

    }
}
