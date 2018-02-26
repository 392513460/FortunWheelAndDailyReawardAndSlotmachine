using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DailyReward
{
    public class Dialogs : MonoBehaviour
    {
        public GameObject dialogs;
        public static Dialogs instance;
        public Image icon;
        public Text qty;
        public bool isPressOk = false;
        public AudioSource click;
        public AudioClip clickSound;
        public int  currentDay ;
        public void OpenDialogs()
        {
            dialogs.SetActive(true);
        }
        public void CloseDialogs()
        {
            click.PlayOneShot(clickSound);
            //isPressOk = true;

            Close();
        }
        void Start()
        {
            instance = this;
            try {
                currentDay = PlayerPrefs.GetInt("currentDay");


            }
            catch
            {
                currentDay = 0;
            }
        }
        public void Close()
        {

            if (icon.sprite.name == "crystal_icon_green"||icon.sprite.name=="gift_crystals")
            {
                MoneyHoldManager.instance.UpdateSoftMoney(int.Parse(qty.text));

            }
            else
            {
                MoneyHoldManager.instance.UpdateProps(int.Parse(qty.text));

            }
            dialogs.SetActive(false);
            icon.rectTransform.localPosition = new Vector3(7.2f, 13.1f);
            icon.rectTransform.localScale = new Vector3(1, 1f);
          for(int i=0;i<ReawarldDailyManager.instance.reward.Length;i++)
            {
                if(ReawarldDailyManager.instance.reward[i].activeInHierarchy==true)
                {
                    currentDay = i - 1;
                    break;
                      
                }
                else
                {
                    currentDay = 6;
                }
            }
            //currentDay = currentDay % 7 == 0 ? 0 : currentDay;
            ReawarldDailyManager.instance.ShowAnim(currentDay);
        
           
            StartCoroutine(ReawarldDailyManager.instance.animExit());







        }
    }
}
