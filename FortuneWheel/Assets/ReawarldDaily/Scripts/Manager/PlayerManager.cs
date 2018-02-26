using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DailyReward
{
    public class PlayerManager : MonoBehaviour
    {
        public int moneyCount = 50;
        public int PropsCount = 0;//道具数量

        private static PlayerManager instance;

        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<PlayerManager>();
                    go.name = "playerManager";
                    go.transform.parent = GameObject.Find("PanelsHold").transform;

                }
                return instance;
            }
        }
    }
}
