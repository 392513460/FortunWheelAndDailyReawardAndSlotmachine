using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#else
#endif
using System.IO;
public class MoveWheelSetUp : ScriptableObject {
    public RewardItem[] rewardItem; //= new RewardItem[10]; modified by ben

    public RewardItem[] rewardItemOne = new RewardItem[10];//added by ben
    public RewardItem[] rewardItemTwo = new RewardItem[10];//added by ben


    const string assetDataPath = "Assets/FortuneWheel/Resources/";
    const string assetName = "MoveWheelSetUp";
    const string assetExt = ".asset";
    public static MoveWheelSetUp instance;
    public static MoveWheelSetUp Instance
    {
        get
        {
            if(instance==null)
            {
                instance = Resources.Load(assetName) as MoveWheelSetUp;
                if(instance==null)
                {
                    instance = CreateInstance<MoveWheelSetUp>();
                    if(!Directory.Exists(assetDataPath))
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
    // Use this for initialization

 
}
