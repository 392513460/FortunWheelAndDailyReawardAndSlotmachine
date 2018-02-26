using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector]
    public int score;
    public UILabel scoreTxt;
    [HideInInspector]
    public int bet = 1;
    public UILabel betTxt;
    [HideInInspector]
    public int win;
    public UILabel winText;
    [HideInInspector]
    public int crystalNum;
    public UILabel crystal;

    public int lowRate = 0;//抽水率
    [HideInInspector]
   public int winPoint = 0;//累计系统吃掉的分数
    public int MaxCount = 10;//当玩家输的次数超过10次做出改变
    [HideInInspector]
   public int count = 0;//累计玩家输的次数
    [HideInInspector]
    public int winCount = 0;//累计玩家赢了多少分
    public int winMaxCount=50;
    public static ScoreManager instance;
    void Awake()
    {
        instance = this;
        score = int.Parse(scoreTxt.text);
        bet = int.Parse(betTxt.text);
        win = int.Parse(winText.text);
        crystalNum = int.Parse(crystal.text);
    }
    void Start()
    {
        count = PlayerPrefs.GetInt("count");
        winPoint = PlayerPrefs.GetInt("winPoint");
        winCount = PlayerPrefs.GetInt("winCount");
    }
    public void ClaspScore()
    {if (score >= bet)
        {
            score -= bet;
            count++;
            Debug.Log("count::" + count);
            PlayerPrefs.SetInt("count", count);
            PlayerPrefs.Save();
            scoreTxt.text = score.ToString();
            winPoint += bet;
            PlayerPrefs.SetInt("winPoint", winPoint);
            PlayerPrefs.Save();
            Debug.Log("winPoint::" + winPoint);
        }
    }
    public void IncreaseScore(int multiple)//倍数
    {
        win += bet*multiple;
        lowRate = 90;
        winCount += win;
        Debug.Log("winCout::" + winCount);
        PlayerPrefs.SetInt("winCount", winCount);
        PlayerPrefs.Save();
        winText.text = win.ToString();

    }
    public void AddScore()
    {
        score += win;
        scoreTxt.text = score.ToString();
        if (win > 0)
        {
            winPoint = 0;
        }
        win = 0;
       
        winText.text = win.ToString();
        
    }
    public void ConvertScore()
    {
        crystalNum--;
        crystal.text = crystalNum.ToString();
        score=score+20;
        scoreTxt.text = score.ToString();
    }
    public void AddBet()
    {
        if (bet >= 1 && bet <= 5)
        {
            bet++;
        }
        betTxt.text = bet.ToString();

    }
    public void DecBet()
    {
        if(bet>1)
        {
            bet--;
        }
        betTxt.text = bet.ToString();
    }
    public void CashOut()
    {
        if (score+win>= 20)
        {
            crystalNum += (score+win) / 20;

            crystal.text = crystalNum.ToString();
            score = 0;
            scoreTxt.text = score.ToString();
        }
    }
}
