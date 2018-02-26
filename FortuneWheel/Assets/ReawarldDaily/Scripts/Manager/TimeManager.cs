using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DailyReward
{
    public class TimeManager : MonoBehaviour
    {
        public bool isOver = false;
        public static TimeManager instance;
        int currentDay;
        public Text hour;
        public Text second;
        public Text min;
        public GameObject nextPickUpTime;
        public System.TimeSpan spanTime = new System.TimeSpan(0, 31, 0);
        void Start()
        {
            instance = this;
            InvokeRepeating("CountDown", 0, 1);
            try
            {
                currentDay = PlayerPrefs.GetInt("nearDay");
            }
            catch
            {

            }
        }
       public void ShowNextPickUpTime()
        {
            hour.text = (23 - System.DateTime.Now.Hour).ToString();
            second.text = (59 - System.DateTime.Now.Minute).ToString();
            min.text = (59 - System.DateTime.Now.Second).ToString();
            if (23 - System.DateTime.Now.Hour < 0.1f&& 59 - System.DateTime.Now.Minute<0.1f&& 59 - System.DateTime.Now.Second<0.1f)
            {
                nextPickUpTime.SetActive(false);
            }
        }
        void Update()
        {
            try
            {
                currentDay = PlayerPrefs.GetInt("nearDay");
            }
            catch
            {

            }
            if (System.DateTime.Now.Day != currentDay&&currentDay!=0)
            {
                isOver = true;
                //isResetTime = true;
            }
        }

        void CountDown()
        {
          
          
            ShowNextPickUpTime();

        }

    }
}
