using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DailyReward
{
    public class MoneyHoldManager : MonoBehaviour
    {
        public static MoneyHoldManager instance;
        public Text moneyText;
        public Text propsText;
        void Start()
        {
            instance = this;

        }
        void OnEnable()
        {
            moneyText.text = PlayerManager.Instance.moneyCount.ToString();
            propsText.text = PlayerManager.Instance.PropsCount.ToString();


        }
        public void UpdateSoftMoney(int count)
        {
            PlayerManager.Instance.moneyCount += count;
            moneyText.text = PlayerManager.Instance.moneyCount.ToString();
        }
        public void UpdateProps(int count)
        {
            PlayerManager.Instance.PropsCount += count;
            propsText.text = PlayerManager.Instance.PropsCount.ToString();
        }


    }
}
