using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    public static TimeManager instance;
    public long nowTime;//现在时间
    public long fixedTime=86400;//限定24小时
    public long startTime;
    public bool isOver = false;
    public long currentTime;
    public bool isPassNowDay = true;
    void Start () {
        instance = this;
        try {
            if (PlayerPrefs.GetString("startTime", "DefaultValue") != null)
            {
                string tempStartTime = PlayerPrefs.GetString("startTime", "DefaultValue");
                startTime = long.Parse(tempStartTime);
            }
        }
        catch
        {

        }
        InvokeRepeating("CountDown", 0, 1);//隔一秒调用此函数   
    }
    void Update()
    {
     
        if (nowTime - startTime >= fixedTime)
        {

            isOver = true;
        }
    }
    void CountDown()
    {
        //fixedTime -= 1;
        Debug.Log("isPassnowDay::" + isPassNowDay);
        try
        {
           if(PlayerPrefs.GetInt("nowDay")!=System.DateTime.Now.Day)
            {
                isPassNowDay = true;
            }
        }
        catch
        {

        }
        nowTime = (System.DateTime.Now.Ticks - System.DateTime.Parse("1970-01-01").Ticks) / 10000000;


        long currenttime = fixedTime - (nowTime - startTime);
        if (FortuneWheelManager.instance != null)
        {
            FortuneWheelManager.instance.timerHours.text = ((currenttime / 60 - currenttime / (60 * 60 * 24) * 24 * 60) / 60).ToString();
            FortuneWheelManager.instance.timerMins.text = ((currenttime / 60) % 60).ToString();
            FortuneWheelManager.instance.timerSecs.text = (currenttime % 60).ToString();

        }

    }



}
