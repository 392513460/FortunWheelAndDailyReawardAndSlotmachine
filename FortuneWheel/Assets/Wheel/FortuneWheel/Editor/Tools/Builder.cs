#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class Builder
{
    private static List<Object> linkedElements;
    private static List<string> linkedElementsNames;
    
    [MenuItem("Builder/Update Linked Resources")]
    public static void UpdateLinkedResources()
    {
        Debug.Log("Updating links to resources is started.");
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            UnityEngine.SceneManagement.Scene mainScene = EditorSceneManager.OpenScene("Assets/FortuneWheel/Scenes/FortuneWheelScene.unity");
            Builder.LinkResources();
            EditorSceneManager.SaveScene(mainScene);
        }
        else
        {
            Debug.LogWarning("Linking process is canceled by user.");
        }
        Debug.Log("Updating links - DONE");
    } // UpdateLinkedResources

    static void ClearConsole()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
        System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
        System.Reflection.MethodInfo method = type.GetMethod("Clear");
        UnityEngine.Object o = new UnityEngine.Object();
        method.Invoke(o, null);
    }
    
    static void LinkResources()
    {
        LinkedResourcesManager linkedResourcesManager = GameObject.FindObjectOfType<LinkedResourcesManager>();

        Builder.linkedElements = new List<Object>();
        Builder.linkedElementsNames = new List<string>();

        string resoursesFolder = "Assets/FortuneWheel/LinkedResources";
        // SPRITES
        string[] assets = AssetDatabase.FindAssets("t:Sprite", new string[] { resoursesFolder });
        for (int i = 0; i < assets.Length; i++)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(assets[i]));
            string name = AssetDatabase.GUIDToAssetPath(assets[i]);
            Object o = AssetDatabase.LoadAssetAtPath(name, typeof(Sprite));
            name = Builder.CleanElementPath(name, resoursesFolder);
            Builder.linkedElements.Add(o);
            Builder.linkedElementsNames.Add(name);
        }

        // SOUNDS
        assets = AssetDatabase.FindAssets("t:AudioClip", new string[] { resoursesFolder });
        for (int i = 0; i < assets.Length; i++)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(assets[i]));
            string name = AssetDatabase.GUIDToAssetPath(assets[i]);
            Object o = AssetDatabase.LoadAssetAtPath(name, typeof(AudioClip));
            name = Builder.CleanElementPath(name, resoursesFolder);
            Builder.linkedElements.Add(o);
            Builder.linkedElementsNames.Add(name);
        }

        // PREFABS
        assets = AssetDatabase.FindAssets("t:GameObject", new string[] { resoursesFolder });
        for (int i = 0; i < assets.Length; i++)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(assets[i]));
            string name = AssetDatabase.GUIDToAssetPath(assets[i]);
            Object o = AssetDatabase.LoadAssetAtPath(name, typeof(GameObject));
            name = Builder.CleanElementPath(name, resoursesFolder);
            Builder.linkedElements.Add(o);
            Builder.linkedElementsNames.Add(name);
        }

        linkedResourcesManager.links = Builder.linkedElements.ToArray();
        linkedResourcesManager.names = Builder.linkedElementsNames.ToArray();

        EditorUtility.SetDirty(linkedResourcesManager);
    } // LinkResources

    static string CleanElementPath(string fullName, string resoursesFolder)
    {
        string name = fullName.Replace(resoursesFolder + "/", "");
        name = name.Replace(".png", "");
        name = name.Replace(".mp3", "");
        name = name.Replace(".wav", "");
        name = name.Replace(".prefab", "");
        return name;
    } // CleanElementPath
}

#endif