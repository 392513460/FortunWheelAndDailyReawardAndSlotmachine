using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DailyReward
{
    public class DialogController : MonoBehaviour
    {

        // Use this for initialization

        private static DialogController _instance;
        public static DialogController Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("DialogsHold");
                    _instance = go.GetComponent<DialogController>();
                }
                return _instance;
            }
        }
        public Sprite GetSprite(Sprite tempSprite)
        {
            return tempSprite;
        }
        public string GetText(string text)
        {
            return text;

        }

    }
}
