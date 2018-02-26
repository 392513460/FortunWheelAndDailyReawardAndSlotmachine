using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#else
#endif
namespace DailyReward
{
    public class ReawarldDailySetUp : ScriptableObject
    {
        public ReawarldItems[] reawrdItems = new ReawarldItems[7];
        const string assetDataPath = "Assets/ReawarldDaily/Resources/";
        const string assetName = "ReawarldItemsSetUp";
        const string assetExt = ".asset";
        public static ReawarldDailySetUp instance;
        public static ReawarldDailySetUp Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load(assetName) as ReawarldDailySetUp;
                    if (instance == null)
                    {
                        instance = CreateInstance<ReawarldDailySetUp>();
                        if (!Directory.Exists(assetDataPath))
                        {
                            Directory.CreateDirectory(assetDataPath);

                        }
                        string fullPath = assetDataPath + assetName + assetExt;
#if UNITY_EDITOR
                        AssetDatabase.CreateAsset(instance, fullPath);
                        AssetDatabase.SaveAssets();
#else
#endif
                    }

                }

                return instance;
            }
        }
    }
}
