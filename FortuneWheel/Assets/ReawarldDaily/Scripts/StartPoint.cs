using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DailyReward
{
    public class StartPoint : MonoBehaviour
    {
        public bool isReset = false;
        public AudioSource background;
        void Awake()
        {
            DOTween.Init();
            background.enabled = true;



          
        }
      
        void Start()
        {
          
           
          
            try {
                int count = PlayerPrefs.GetInt("index");

                if (ReawarldDailyManager.instance.reward[0].activeInHierarchy && !ReawarldDailyManager.instance.reward[1].activeInHierarchy)
                {

                   
                }
                else {

                    for (int i = 0; i <= count; i++)
                    {
                        if (i < 8)
                        {
                          
                            ReawarldDailyManager.instance.reward[i].SetActive(false);
                           

                            ReawarldDailyManager.instance.hook[i].SetActive(true);
                        }

                        else {

                        }
                    }
                    if (count == 0 && PlayerPrefs.GetInt("isFirstPress") == 0)
                    {
                        ReawarldDailyManager.instance.reward[0].SetActive(true);
                        ReawarldDailyManager.instance.hook[0].SetActive(false);

                    }

                }
            }
            catch
            {
               
            }
           
        }
        void Update()
        {
            if (isReset)
            {
                PlayerPrefs.DeleteKey("index");
                PlayerPrefs.DeleteKey("isFirstPress");
            }
            if (ReawarldDailyManager.instance.reward[0].activeInHierarchy)
            {       

                TimeManager.instance.isOver = true;

            }
        }
    }
}
