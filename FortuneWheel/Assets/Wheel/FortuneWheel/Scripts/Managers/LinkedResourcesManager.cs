using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LinkedResourcesManager : MonoBehaviour {
    private static LinkedResourcesManager _instance;//
    public static LinkedResourcesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("LinkedResourcesManager");
                if (go == null)
                {
                    return null;
                }
                _instance = go.GetComponent<LinkedResourcesManager>();
            }
            return _instance;
        }
    }

    [SerializeField] public string[] names;
    [SerializeField] public Object[] links;

    private Dictionary<string, Object> cachedLinks = new Dictionary<string, Object>();

    public Object GetLinkedObjectByFullPath(string path)
    {
        if (this.cachedLinks.ContainsKey(path))
        {
            return this.cachedLinks[path];
        }
        for (int i = 0; i < this.names.Length; i++)
        {
            if (this.names[i] == path)
            {
                this.cachedLinks.Add(path, this.links[i]);
                return this.links[i];
            }
        }
        Debug.LogWarning("[LinkedResourcesManager] [GetLinkedObjectByPath] cannot find link by name '" + path + "'");
        return null;
    } // GetLinkedObjectByFullPath

    public T[] GetLinkedObjectsByFolder<T>(string folder) where T : Object
    {
        List<T> resultList = new List<T>();
        string fullFolder = folder + "/";
        for (int i = 0; i < this.names.Length; i++)
        {
            if (this.names[i].IndexOf(fullFolder) == 0)
            {
                resultList.Add((T)this.links[i]);
            }
        }
        return resultList.ToArray();
    } // GetLinkedObjectsByFolder

} // LinkedResourcesManager