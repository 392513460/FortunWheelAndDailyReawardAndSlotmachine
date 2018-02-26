using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DailyReward
{
    public class ReawarldDailyManager : MonoBehaviour
    {
        public GameObject[] hook = new GameObject[7];
        public GameObject[] reward = new GameObject[7];
        public static ReawarldDailyManager instance;
        public GameObject ReceiveButton;
        public AudioSource click;
        public AudioClip clickSound;
        public AudioClip congratuation;
        public bool isPress = false;
        public int myIndex;
        public RectTransform[] animObjects;
        public RectTransform[] pos;
        public GameObject coinAnimParent;
        public RectTransform targetPos1, targetPos2;
        public int current = 0;

        public void OnTest()
        {
            
            ShowAnim(current);
            current++;
            current = current % 7 == 0 ? 0 : current;
            StartCoroutine(animExit());
           
        }
     public    IEnumerator animExit()
        {
            yield return new WaitForSeconds(1.2f);
            coinAnimParent.SetActive(false);
        }
        public void ShowAnim(int i=0)
        {
            coinAnimParent.SetActive(true);
            foreach(var item in animObjects)
            {
                item.localScale = Vector3.one;
              
               
                item.localPosition = pos[i].localPosition;
            
            }
            StartCoroutine(AnimateObjects(i));
        }
        IEnumerator AnimateObjects(int i)
        {
            foreach (var item in animObjects)
            {
                StartCoroutine(AnimateOneObject(item, i));
                yield return new WaitForSeconds(0.08f);
            }
        }
        IEnumerator AnimateOneObject(RectTransform obj, int id, float delay0 = 0.1f, float delay1 = 0.9f)
        {
            float elapsedTime = 0.0f, percent;
            Vector3 trg = GetStartPos(id);
            while (elapsedTime < delay0)
            {
                elapsedTime += Time.deltaTime;
                percent = elapsedTime / delay0;
                obj.localPosition = Vector3.Lerp(obj.localPosition, trg, percent);
                obj.localScale = Vector3.Lerp(obj.localScale, Vector3.one * 1.2f, percent);
                yield return null;
            }
            obj.anchoredPosition = trg;
            obj.localScale = Vector3.one * 1.2f;
            elapsedTime = percent = 0;
            trg = t2;
            while (elapsedTime < delay1)
            {
                elapsedTime += Time.deltaTime;
                percent = elapsedTime / delay1;
                obj.localPosition = Vector3.Lerp(obj.localPosition, trg, percent);
                obj.localScale = Vector3.Lerp(obj.localScale, Vector3.one, percent);
                yield return null;
            }
            obj.anchoredPosition = trg;
            obj.localScale = Vector3.one;
        }
        Vector2 GetStartPos(int v)
        {
            return new Vector2(pos[v].localPosition.x + RandomFloat,
                pos[v].localPosition.y + RandomFloat);
        }
        public float RandomFloat
        {
            get
            {
                return UnityEngine.Random.Range(-55, 55);
            }
        }
        public Vector2 t2
        {
            get
            {
                return new Vector2(targetPos2.localPosition.x + RandomFloat1, targetPos2.localPosition.y + RandomFloat1);
            }
        }
        public float RandomFloat1
        {
            get
            {   
                return 0;// UnityEngine.Random.Range(-15.0f, 15.0f);
            }
        }
        public void Receive()
        {
            TimeManager.instance.nextPickUpTime.SetActive(true);
            click.PlayOneShot(clickSound);

            int index = 0;
            int currentDay = System.DateTime.Now.Day;
            PlayerPrefs.SetInt("nearDay", currentDay);
            PlayerPrefs.Save();
            foreach (GameObject temp in reward)
            {
                if (temp.activeInHierarchy)
                {
                    TimeManager.instance.isOver = false;
                    temp.SetActive(false);
                    isPress = true;
                    break;

                }
            }
            foreach (GameObject temp in hook)
            {

                if (temp.activeInHierarchy)
                {
                    TimeManager.instance.isOver = false;

                    index++;

                }
                else
                {
                    index++;

                    temp.SetActive(true);
                    Dialogs.instance.OpenDialogs();
                    click.PlayOneShot(congratuation);
                    PlayerPrefs.SetInt("index", index - 1);
                    PlayerPrefs.Save();
                    PlayerPrefs.SetInt("isFirstPress", index);
                    PlayerPrefs.Save();
                    TimeManager.instance.isOver = false;

                    break;
                }
                myIndex = index;
            }


        }

        // Use this for initialization

        void Awake()
        {

            instance = this;


        }

        // Update is called once per frame
        void Update()
        {

            if (TimeManager.instance.isOver)
            {
                ReceiveButton.SetActive(true);
                TimeManager.instance.nextPickUpTime.SetActive(false);
            }
            else
            {

                ReceiveButton.SetActive(false);
            }
            if (isPress && CongratulationDialog.instance != null)
            {


                CongratulationDialog.instance.rewadrdIcon.sprite = ReawarldDailySetUp.instance.reawrdItems[myIndex].Icon;
                CongratulationDialog.instance.quanity.text = ReawarldDailySetUp.instance.reawrdItems[myIndex].value.ToString();
                PlayerPrefs.SetInt("myIndex", myIndex);
                isPress = false;
            }
        }
    }
}
