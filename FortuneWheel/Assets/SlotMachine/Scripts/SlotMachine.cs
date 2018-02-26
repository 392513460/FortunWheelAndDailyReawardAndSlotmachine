using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  [System.Serializable]
public class Item
{
    public int winChance;
}

public class SlotMachine : MonoBehaviour
{
    int totalSymbols;
    int totalLine = 3;//总列数
    Transform[] lines;
    Line[] lineList;
    bool isInput = true;
    public bool isAuto = false;
    public GameObject cellItemPrefab;
    [HideInInspector]
    public int suitableIndex;//适合赢的分数下标索引
    public static SlotMachine instance;
    public Item[] items=new Item[7];
    public AudioSource stopSound;
    public AudioClip wheel;
    public AudioSource soundManager;
    // Use this for initialization
    void Start()
    {
        totalSymbols = PayData.totalSymbols;
        InitArena();
        isInput = true;
        instance = this;
    }
    void InitArena()//界面初始化
    {
        lines = new Transform[totalLine];
        lineList = new Line[totalLine];
        for (int i = 0; i < totalLine; i++)
        {
            GameObject pgo = new GameObject();//创建每列的父物体方便管理
            pgo.name = "Base" + (i + 1).ToString("000");
            pgo.transform.parent = transform;
            pgo.transform.localScale = Vector3.one;
            pgo.transform.localPosition = Vector3.zero;

            GameObject go = new GameObject();
            go.transform.parent = pgo.transform;
            Line script = go.AddComponent<Line>();
            script.idx = i;

            Transform tf = go.transform;
            lines[i] = tf;
            tf.parent = pgo.transform;


            tf.localPosition = new Vector3(-120 + i * 120, 0, 0);
            tf.localScale = Vector3.one;
            go.name = "Line" + (i + 1).ToString("000");
            script.items = new Tile[PayData.totalCell];

            for (int j = 0; j < PayData.totalCell; j++)
            {
                GameObject g = Instantiate(cellItemPrefab) as GameObject;
                g.name = "Tile" + (j + 1).ToString("000");
                Transform t = g.transform;
                Tile c = g.GetComponent<Tile>();
                c.slotMachine = this;
                c.SetTileType(Random.Range(0, totalSymbols) % totalSymbols);

                c.lineScript = script;
                script.items[j] = c;
                c.idx = j;
                t.parent = tf;
                t.localPosition = new Vector3(0, 225- j * 75, 0);//Vector3.forward * j * cellSize.y;
                t.localScale = Vector3.one;
                t.localRotation = Quaternion.identity;
            }
            script.idx = i;
            lineList[i] = script;
        }

    }
    public void DoSpin()
    {
        if (isAuto) return;
        if (ScoreManager.instance.score + ScoreManager.instance.win <= ScoreManager.instance.bet)
        {

        }
        else {

            Spin();

        }
    }
    void Spin()
    {
        if (!isInput) return;
        ScoreManager.instance.AddScore();
        ScoreManager.instance.ClaspScore();
        DoRoll();


    }
   public float r;
    Line lim;
    void DoRoll()
    {
        soundManager.PlayOneShot(wheel);

        if(ScoreManager.instance.count>ScoreManager.instance.MaxCount)
        {
            ScoreManager.instance.lowRate = 60;
            ScoreManager.instance.count = 0;
        }
        if(ScoreManager.instance.winCount>=ScoreManager.instance.winMaxCount)
        {
            ScoreManager.instance.lowRate = 99;
            ScoreManager.instance.winCount = 0;
        }
        if(ScoreManager.instance.winPoint>60)
        {
            ScoreManager.instance.lowRate = 10;
           
        }
        r = Random.Range(1, 100);
        Debug.Log("r::" + r);
        if (r > ScoreManager.instance.lowRate)
        {
            //   suitablePoint = ScoreManager.instance.winPoint * (1 - ScoreManager.instance.lowRate);
            //lim = lines[0].GetComponent<Line>();
            //   int count=6;
            //   for (  int i = 0; i < lim.items.Length; i++)
            //   {
            //       if (ScoreManager.instance.winPoint * (1 - ScoreManager.instance.lowRate) - ScoreManager.instance.bet * i*10 > 0)
            //       {
            //           if (suitablePoint <ScoreManager.instance.winPoint * (1 - ScoreManager.instance.lowRate) - ScoreManager.instance.bet * i*10)
            //           {
            //               suitablePoint = ScoreManager.instance.bet * i*10;
            //               count = i;
            //           }
            //       }
            //   }
         
            for(int i=0;i<items.Length;i++)
            {
                int rate = Random.Range(1, 101);
                if (rate<items[i].winChance)
                {
                    suitableIndex = i;
                    break;
                }
                else
                {
                    rate -= items[i].winChance;
                }
            }
            Debug.Log("suitableIndex::" + suitableIndex);
        }

        for (int i = 0; i < totalLine; i++)
        {
            Transform line = lines[i];
            //开启协同
            StartCoroutine(RepeatAction(0.1f, 8 + i * 3, () =>
              {
                  line.SendMessage("RollCells", true, SendMessageOptions.RequireReceiver);
              }, () =>
              {
                  stopSound.Play();
              }
            ));
        }
        isInput = false;
        StartCoroutine(DelayAction(2.2f, () =>
        {
            isInput = true;
            FindMatch();
        }));
    }
    Line lin;
    int itemIndex = 0;
    public float speed;
    IEnumerator RepeatAction(float dTime,  int count, System.Action callback1, System.Action callback2)
    {

        if (count > 1) callback1();

        if (--count > 0)
        {
           
           
            
            if (count == 1)
            {
                soundManager.Stop();
               callback2();
            }
            yield return new WaitForSeconds(dTime);

            StartCoroutine(RepeatAction(dTime, count, callback1, callback2));
        }

    }
    IEnumerator DelayAction(float dTime, System.Action callback)
    {
        yield return new WaitForSeconds(dTime);
        callback();
    }
    void FindMatch()
    {
        List<int> obtain = new List<int>();
        for (int i = 0; i < lines.Length; i++)
        {
            Line lin = lines[i].GetComponent<Line>();
            for (int j = 0; j < lin.items.Length; j++)
            {
                float y = lin.items[j].transform.localPosition.y;
                if (y <= 10 && y >= -30)
                {
                    obtain.Add(lin.items[j].GetTileType());
                    //break;
                }
            }
        }
        Debug.Log("获得物品类型：" + obtain[0] + "__" + obtain[1] + "___" + obtain[2]);
        for (int i = 0; i < 3; i++)
        {
            int count = 0;
            if (obtain[i] == 0)
            {
                count++;
            }
            if (count == 1)
            {
                ScoreManager.instance.IncreaseScore(1);
            }
            if (count == 2)
            {
                ScoreManager.instance.IncreaseScore(2);
            }
            if (count == 3)
            {
                ScoreManager.instance.IncreaseScore(3);
            }
        }
        if (obtain[0] == obtain[1] && obtain[1] == obtain[2])
        {
            if (obtain[0] == 1)
            {
                ScoreManager.instance.IncreaseScore(10);
            }
            if (obtain[0] == 2)
            {
                ScoreManager.instance.IncreaseScore(20);
            }
            if (obtain[0] == 3)
            {
                ScoreManager.instance.IncreaseScore(30);
            }
            if (obtain[0] == 4)
            {
                ScoreManager.instance.IncreaseScore(40);
            }
            if (obtain[0] == 5)
            {
                ScoreManager.instance.IncreaseScore(50);
            }
            if (obtain[0] == 6)
            {
                ScoreManager.instance.IncreaseScore(60);
            }

        }
    }

}
