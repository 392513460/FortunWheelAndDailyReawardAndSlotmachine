//#define MYDEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class FortuneWheelManager : MonoBehaviour
{
    public RectTransform luckyRoll;
    public FortuneWheelPointerManager pointer;//定义一个转盘指针管理的类
    public Transform[] bubbles;
    public FortuneWheelItemManager[] itemsManagers;//image管理
    public Image[] sectors;//区域黄色管理

    public Sprite activeBubble;//亮的泡泡
    public Sprite disabledBubble;//比较暗的泡泡

    public Button centerButton;//中心Button
    public Text centerButtonFreeTitle;//CenterButtonText
    public Text centerButtonPrice;
    public GameObject centerButtonPayObject;//spinfor

    public GameObject timerObject;//时间GameObjcet方便setActive(false)
    public Text timerHours;//小时
    public Text timerMins;//分钟
    public Text timerSecs;//秒

    [HideInInspector]
    public bool inAction;//是否在旋转

    private float _rotateStartSpeed = 0;//初始旋转速度
    private float _rotateCurrentSpeed = 0;//
    private bool _allowRotate = false;
    private bool _allowSpeedChange = false;//速度是否改变
    private float _fixedSpeedChange = 0;//最终速度

    private float _pointerWheelDelta = 36;//？
    private float _pointerWheelShift = 18;//？

    private int _targetItemNum = 0;//奖品数量


    private bool _freeTimerEnabled = true;//是否是免费时间
    private float _freeTimerLastUpdate = 0;//?免费时间更新
    public Button backButton;
    public static FortuneWheelManager instance;
    public void Init()
    {
#if MYDEBUG
        DebugMy.Log("[FortuneWheelManager] [Init] FortuneWheelController.Instance.wheelOptions.Count = " + FortuneWheelController.Instance.wheelOptions.Count);
#endif
        FortuneWheelController.Instance.fortuneWheelManager = this;//传递FortunWheelManager

        for (int i = 0; i < this.itemsManagers.Length; i++)//item遍历
        {
            if (FortuneWheelController.Instance.wheelOptions.Count > i)
            {
                //this.itemsManagers[i].Init(FortuneWheelController.Instance.wheelOptions[i]);//初始化奖品
            }
            else
            {
                UDebug.LogError("[FortuneWheelManager] [Init] no option for Item Manager " + i);
            }
        } // for

        float timeLeft = FortuneWheelController.Instance.freeTryAvaiableFrom - Time.realtimeSinceStartup;//剩余时间
#if MYDEBUG
        DebugMy.Log("[FortuneWheelManager] [Init] timeLeft = " + timeLeft);
#endif
        if (timeLeft > 0)
        {
            this.EnableTimer();//设置付费Button
        }
        else
        {
            this.DisableTimer();//设置免费Button
        }

        this.centerButton.interactable = true;
        this.inAction = false;
        this.UpdateBubblesView(this.disabledBubble);//Bubble全部为暗的状态
    } // Init

    public void Go()//转盘开始旋转
    {
        //Debug.Log("[FortuneWheelManager] GO!");
       int nowDay=  System.DateTime.Now.Day;
        PlayerPrefs.SetInt("nowDay", nowDay);
        if (/*this._freeTimerEnabled*/TimeManager.instance.isOver==false) // it means we have to pay now
        {
            // need pay!
            
            DisableTimer();
            if (FortuneWheelController.Instance.hardPrice > PlayerController.Instance.hardMoney)//钱不够的情况下
            {
                // show buy hard money dialog
                BuyHardMoneyDialog buyHardMoneyDialog = (BuyHardMoneyDialog)DialogsController.Instance.GetDialog(DialogsNames.BuyHardMoneyDialog);//显示充值Dialog
                DialogsController.Instance.Show(buyHardMoneyDialog.gameObject, ignoreOpenTest: true);
                return;
            }
            PlayerController.Instance.hardMoney -= FortuneWheelController.Instance.hardPrice;//减去所需的金钱
            this._rotateStartSpeed = 2.5f + Random.Range(0.5f, 1.0f);//随即速度
            this._rotateCurrentSpeed = this._rotateStartSpeed;
            this._allowRotate = true;
            this._allowSpeedChange = false;

            this._fixedSpeedChange = 0;
        }
       else
        {
            Debug.Log("1111");
            EnableTimer();
            TimeManager.instance.isOver = false;
            Debug.Log("isOver::" + TimeManager.instance.isOver);
            TimeManager.instance.startTime = TimeManager.instance.nowTime;
            string startTime = TimeManager.instance.startTime.ToString();
            PlayerPrefs.SetString("startTime", startTime);
          
            this._rotateStartSpeed = 2.5f + Random.Range(0.5f, 1.0f);//随即速度
            this._rotateCurrentSpeed = this._rotateStartSpeed;
            this._allowRotate = true;
            this._allowSpeedChange = false;

            this._fixedSpeedChange = 0;
        }
        if (MainController.Instance.startPoint.wheelDataMode == WheelMode.local)//如果是本地
        {
            Invoke("GetWinDataLocal", 1.0f);
        }
        else
        {
            ServerController.Instance.GetFortuneWinner();
        }

        this.UpdateBubblesView(this.activeBubble);//更新泡泡的状态

        this.centerButton.interactable = false;
        this.centerButton.image.color = Color.gray;
        backButton.image.color = Color.gray;
        this.inAction = true;
    } // Go

    void GetWinDataLocal()
    {
        FortuneOption winnerFO = FortuneWheelController.Instance.GetWinnerOptionByChance();
        string jsonData = "{\"data\":{\"winId\":" + winnerFO.id + ",	\"restore\":" + Random.Range(10, 100) + "}}";
        FortuneWheelController.Instance.WinnerServerResponseParse(jsonData);
    }

    public void SetWinData(int itemId)
    {
        this._allowSpeedChange = true;
        this._targetItemNum = itemId-1 ; // start from 0
#if MYDEBUG
        DebugMy.Log("[SetWinData] " + this.targetItemNum);
#endif
    } // SetWinData

    void FixedUpdate()
    {
        if (this._allowRotate && this._rotateCurrentSpeed > 1.5f)
        {
            this.transform.Rotate(0, 0, -this._rotateCurrentSpeed);

            if (this._allowSpeedChange)//速度递减
            {
                if (this._rotateCurrentSpeed > 2.5f)
                {
                    this._rotateCurrentSpeed -= 0.0068f;
                }
                else if (this._rotateCurrentSpeed > 2f)
                {
                    this._rotateCurrentSpeed -= 0.01015f;
                }
                else
                {
                    this._rotateCurrentSpeed -= 0.0135f;
                }
            }
            this.FixBubbles();
            this.FixPointer();
        } // if

        if (this._allowRotate && this._rotateCurrentSpeed <= 1.5f)//？10/22/20：51
        {
            this.transform.Rotate(0, 0, -this._rotateCurrentSpeed);

            if (this._fixedSpeedChange == 0)
            {
                this._fixedSpeedChange = this.GetFixedSpeedChange();
            }
            if (this._fixedSpeedChange != 0)
            {
                this._rotateCurrentSpeed -= this._fixedSpeedChange;
            }

            this.FixBubbles();

            float mult = this.FixPointer();
            if (this._rotateCurrentSpeed > 0 && this._rotateCurrentSpeed < 0.02f)
            {
                float targetDeg = 360 - this._targetItemNum * 36;
#if MYDEBUG
                DebugMy.Log("this.rotateCurrentSpeed = " + this.rotateCurrentSpeed + " | targetDeg = " + targetDeg + " | current = " + this.transform.localRotation.eulerAngles.z);
#endif
                if (this.transform.localRotation.eulerAngles.z - targetDeg >= 14)
                {
                    this._rotateCurrentSpeed += 0.005f;
#if MYDEBUG
                    DebugMy.LogWarning("ADD SPEED | this.rotateCurrentSpeed = " + this.rotateCurrentSpeed);
#endif
                }
                if (this.transform.localRotation.eulerAngles.z - targetDeg <= -14)
                {
                    this._rotateCurrentSpeed = -0.005f;
                    this.transform.Rotate(0, 0, -this._rotateCurrentSpeed);
#if MYDEBUG
                    DebugMy.LogWarning("MINUS SPEED | this.rotateCurrentSpeed = " + this.rotateCurrentSpeed);
#endif
                }
            }
            if (this._rotateCurrentSpeed <= 0)
            {
                this._allowRotate = false;
                this.centerButton.image.color = Color.white;
                backButton.image.color = Color.white;
#if MYDEBUG
                DebugMy.Log("STOP - mult = " + mult);
#endif
                if (mult < 6 || mult > 33)
                {
                    StartCoroutine(this.FinishRotation(0.05f, 1));
                }
                else if (mult > 30)
                {
                    StartCoroutine(this.FinishRotation(0.025f, -1));
                }
                else
                {
                    this.Done();
                }
            }
        }
    } // FixedUpdate

    void Update()
    {
        //float timeLeft = FortuneWheelController.Instance.freeTryAvaiableFrom - Time.realtimeSinceStartup;
        //if (timeLeft > 0)
        //{
        //    if (!this._freeTimerEnabled)
        //    {
        //        this.EnableTimer();
        //    }
        //    if (Time.realtimeSinceStartup > this._freeTimerLastUpdate + 0.3f)
        //    {
        //        this.UpdateTimer(timeLeft);
        //        this._freeTimerLastUpdate = Time.realtimeSinceStartup;
        //    }
        //}
        //else
        //{
        //    if (this._freeTimerEnabled)
        //    {
        //        this.DisableTimer();
        //    }
        //}
        if(TimeManager.instance.isOver==false)
        {
            UpdateTimer();
            EnableTimer();
        }
        else
        {
            DisableTimer();
        }
    } // Update

    private void EnableTimer()
    {
        this.timerObject.gameObject.SetActive(true);//计时器显示
        this._freeTimerEnabled = true;
        this.SetPayButton();
    }

    private void DisableTimer()
    {
        this.timerObject.gameObject.SetActive(false);
        this._freeTimerEnabled = false;
        this.SetFreeButton();
    }

    private void UpdateTimer(/*float timeLeft*/)
    {
       
        //Debug.Log("hour" + ((TimeManager.instance.currentTime / 60 - TimeManager.instance.currentTime / (60 * 60 * 24) * 24 * 60) / 60));
        //this.timerHours.text = ((TimeManager.instance.currentTime / 60 -TimeManager.instance.currentTime / (60 * 60 * 24) * 24 * 60) / 60).ToString();
        //this.timerMins.text =  ((TimeManager.instance.currentTime / 60) % 60).ToString();
        //this.timerSecs.text =  (TimeManager.instance.currentTime % 60).ToString();
    }

    private void SetFreeButton()
    {
        this.centerButtonFreeTitle.enabled = true;//免费Button可用
        this.centerButtonPayObject.SetActive(false);
    }

    private void SetPayButton()
    {
        this.centerButtonFreeTitle.enabled = false;//免费Button不可用
        this.centerButtonPayObject.SetActive(true);//付费可用

        this.centerButtonPrice.text = "x" + FortuneWheelController.Instance.hardPrice.ToString();//显示每把消耗多少钱
    }

    private void FixBubbles()
    {
        Vector3 currentRotation = this.transform.localRotation.eulerAngles;//
        for (int i = 0; i < this.bubbles.Length; i++)
        {
            this.bubbles[i].localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, -currentRotation.z);
        } // for
    } // FixBubbles

    private float FixPointer()
    {
        Vector3 currentRotation = this.transform.localRotation.eulerAngles;
        float mult = (Mathf.Abs(currentRotation.z) + _pointerWheelShift) % this._pointerWheelDelta;
        this.pointer.UpdateRotation(mult);
        int sector = Mathf.FloorToInt((currentRotation.z + _pointerWheelShift) / this._pointerWheelDelta);
        this.UpdateSectors(sector);
        return mult;
    }

    void UpdateSectors(int sector)
    {
        if (sector == 0)
        {
            sector = 10;
        }
        for (int i = 0; i < this.sectors.Length; i++)
        {
            this.sectors[i].enabled = (i == (10 - sector));
        }
    }

    private float GetFixedSpeedChange()
    {
        //DebugMy.Log("[FortuneWheelManager] [GetFixedSpeedChange]");
        float targetDeg = 360 - this._targetItemNum * 36;
        if (targetDeg == 360)
        {
            targetDeg = 0;
        }
        if (Mathf.Abs(this.transform.localRotation.eulerAngles.z) < targetDeg)
        {
            return 0;
        }
        float degDelta = Mathf.Abs(this.transform.localRotation.eulerAngles.z) - targetDeg;
        if (degDelta <= 10.0f)
        {
            return 0.003f + Random.Range(-0.00007f, 0.00012f);
        }
        return 0;
    } // GetFixedSpeedChange

    IEnumerator FinishRotation(float speed, float direction)
    {
#if MYDEBUG
        DebugMy.Log("[FinishRotation] - speed = " + speed + " | direction = " + direction);
#endif
        float mult;
        while (speed > 0)
        {
            yield return new WaitForEndOfFrame();
            this.transform.Rotate(0, 0, speed * direction);

            this.FixBubbles();
            mult = this.FixPointer();
            if (mult > 6 && mult < 30)
            {
                speed -= 0.3f * Time.deltaTime;
            }
        } // while
        this.Done();
    }

    void Done()
    {
#if MYDEBUG
        DebugMy.Log("DONE! Winner is " + this.targetItemNum);
#endif
        this.inAction = false;

        this.UpdateBubblesView(this.disabledBubble);

        this.centerButton.interactable = true;

        FortuneWheelWinDialog winDialog = (FortuneWheelWinDialog)DialogsController.Instance.GetDialog(DialogsNames.FortuneWheelWinDialog);
        winDialog.option = FortuneWheelController.Instance.wheelOptions[this._targetItemNum];
        DialogsController.Instance.Show(winDialog.gameObject, true);
    } // Done

    void UpdateBubblesView(Sprite sprite)
    {
        for (int i = 0; i < this.bubbles.Length; i++)
        {
            this.bubbles[i].gameObject.GetComponent<Image>().sprite = sprite;
        } // for
    }
    void Start()
    {
        instance = this;
    }
}

    // FortuneWheelManager
    [System.Serializable]
    public class RewardItem
    {
    public int id;
        public Sprite rewardSprite;
        public int rewardQuantity;//数量
        public RewardType rewardType;
    public int winChance;
    }
    public enum RewardType
    {
        Coin,
        Diamond,
        Time,
        Magnet,//磁铁
        Shield//屏蔽
    }

