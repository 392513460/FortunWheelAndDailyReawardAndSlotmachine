using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DailyReward
{
    public class ChunSlice : MonoBehaviour
    {
        public int myIndex;
        public Image iconSpirte;
        public Text valueText;
        void Start()
        {
            myIndex = transform.GetSiblingIndex();
            iconSpirte.sprite = ReawarldDailySetUp.Instance.reawrdItems[myIndex].Icon;
            valueText.text = ReawarldDailySetUp.Instance.reawrdItems[myIndex].value.ToString();
        }
        // Use this for initialization

    }
}
