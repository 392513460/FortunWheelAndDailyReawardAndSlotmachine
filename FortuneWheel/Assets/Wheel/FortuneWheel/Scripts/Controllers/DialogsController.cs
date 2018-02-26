//#define DEBUG

using UnityEngine;
using System.Collections.Generic;

public class DialogsController : MonoBehaviour
{
    private static DialogsController _instance;
	public static DialogsController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("DialogsController");
                _instance = go.GetComponent<DialogsController>();
            }
            return _instance;
        }
    }
	
	public Dialogs dialogs;
    public Panels panels;

    public List<string> openedDialogsList;
    public Queue<GameObject> dialogsQueue;

    private Dictionary<string, GameObject> dialogsList;
    private Dictionary<DialogsNames, int> regularDialogsCounter;

    public int openedDialogs
    {
        get
        {
            return this.openedDialogsList.Count;
        }
    }

    public bool IsDialogOpened(string name)
    {
        if (this.openedDialogsList != null && this.openedDialogsList.Contains(name))
        {
            return true;
        }
        return false;
    } // IsDialogOpened

    public void ResetRegularDialogsCounters(int toCounter)
    {
        if (this.regularDialogsCounter == null)
        {
            this.regularDialogsCounter = new Dictionary<DialogsNames, int>();
        }
        if (this.regularDialogsCounter.Count == 0)
            return;
        foreach(KeyValuePair<DialogsNames, int> entry in this.regularDialogsCounter)
        {
            this.regularDialogsCounter[entry.Key] = toCounter;
        }
    } // ResetRegularDialogsCounters

    public void RegularDialogsCountersStep()
    {
        if (this.regularDialogsCounter == null)
        {
            this.regularDialogsCounter = new Dictionary<DialogsNames, int>();
        }
        if (this.regularDialogsCounter.Count == 0)
            return;
        Dictionary<DialogsNames, int> updatedData = new Dictionary<DialogsNames, int>();
        foreach (KeyValuePair<DialogsNames, int> entry in this.regularDialogsCounter)
        {
            updatedData.Add(entry.Key, entry.Value - 1);
        }
        this.regularDialogsCounter = updatedData;
    } // RegularDialogsCountersStep

    public void AddDialogToRegularList(DialogsNames name, int counter = 0)
    {
        if (this.regularDialogsCounter == null)
        {
            this.regularDialogsCounter = new Dictionary<DialogsNames, int>();
        }
        if (this.regularDialogsCounter.ContainsKey(name))
        {
            this.regularDialogsCounter[name] = counter;
            return;
        }
        this.regularDialogsCounter.Add(name, counter);
    } // AddDialogToRegularList

    public void AddRegularDialogsCounters()
    {
        if (this.regularDialogsCounter == null)
        {
            this.regularDialogsCounter = new Dictionary<DialogsNames, int>();
            return;
        }
        Dictionary<DialogsNames, int> updatedCounters = new Dictionary<DialogsNames, int>();
        foreach (KeyValuePair<DialogsNames, int> entry in this.regularDialogsCounter)
        {
            if (entry.Value < 1)
            {
                updatedCounters.Add(entry.Key, 1);
            } else
            {
                updatedCounters.Add(entry.Key, entry.Value);
            }
        }
        this.regularDialogsCounter = updatedCounters;
    } // AddRegularDialogsCounters

    public bool IsRegularDialogAllowed(DialogsNames name)
    {
        UDebug.Log("[DialogsController] [IsRegularDialogAllowed] name = " + name);
        if (this.regularDialogsCounter == null)
        {
            this.regularDialogsCounter = new Dictionary<DialogsNames, int>();
            return false;
        }
        if (!this.regularDialogsCounter.ContainsKey(name))
        {
            return false;
        }
        UDebug.Log("[DialogsController] [IsRegularDialogAllowed] this.regularDialogsCounter[" + name + "] = " + this.regularDialogsCounter[name]);
        if (this.regularDialogsCounter[name] <= 0)
        {
            return true;
        }
        return false;
    } // IsRegularDialogAllowed

    public void CloseRegularDialogsIfOpened()
    {
        foreach (KeyValuePair<DialogsNames, int> entry in this.regularDialogsCounter)
        {
            if (this.IsDialogOpened(entry.Key.ToString()))
            {
                this.CloseImmediately(this.GetOpenedDialogObject(entry.Key));
            }
        }
    } // CloseRegularDialogsIfOpened

    public GameObject GetOpenedDialogObject(DialogsNames name)
    {
        if (!this.IsDialogOpened(name.ToString()))
        {
            return null;
        }
        if (this.dialogsList == null || !this.dialogsList.ContainsKey(name.ToString()))
        {
            return null;
        }
        return this.dialogsList[name.ToString()];
    } // GetOpenedDialogObject

    public void AddToQueue(GameObject dialog)
    {
        if (this.openedDialogs < 1)
        {
            this.Show(dialog);
            return;
        }
        this.dialogsQueue.Enqueue(dialog);//入队
    } // AddToQueue
	
	public void Show(GameObject dialog, bool ignoreOpenTest = false, bool allowSound = true)
	{
        if (!ignoreOpenTest)
        {
            if (this.IsBlockerOpened())
            {
                UDebug.LogWarning("[DialogsController] [Show] new dialog [" + dialog.name+ "] is not allowed! this.openedDialogs = " + this.openedDialogs);
                if (dialog.name != "ConnectionProblemDialog(Clone)")
                {
                    this.AddToQueue(dialog);
                    return;
                }
            }
        }

        if (allowSound)
        {
            //SoundEffectsController.Instance.PlayEffect(soundEffectsTypes.ScreenSwitch);
        }

#if DEBUG
        UDebug.Log("[DialogsController] [Show] this.openedDialogs = " + this.openedDialogs);
#endif
        if (this.openedDialogsList.Contains(dialog.name))
        {
            this.openedDialogsList.Remove(dialog.name);
        }
		this.openedDialogsList.Add(dialog.name);

        dialog.SetActive(true);
		dialog.BroadcastMessage("InitDialog", SendMessageOptions.RequireReceiver);
	} // Show

	public void Close(GameObject dialog, bool allowSound = true) 
	{
#if DEBUG
        UDebug.Log("[DialogsController] [Close] " + dialog.name);
#endif
        if (allowSound)
        {
            //SoundEffectsController.Instance.PlayEffect(soundEffectsTypes.ui_openClosePanel);
        }

        if (this.openedDialogsList.Contains(dialog.name))
        {
            this.openedDialogsList.Remove(dialog.name);
        }

        dialog.BroadcastMessage("DialogClosed", SendMessageOptions.DontRequireReceiver);

        this.CheckForQueue();
    } // Close
	
	public void CloseImmediately(GameObject dialog) 
	{
        if (this.openedDialogsList.Contains(dialog.name))
        {
            this.openedDialogsList.Remove(dialog.name);
        }
        dialog.BroadcastMessage("DialogClosed", SendMessageOptions.DontRequireReceiver);

        this.CheckForQueue();
    } // CloseImmediately

    public void CloseAllOpenDialogs()//关闭全部的Dialogs
    {
        for (int i = 0; i < this.openedDialogsList.Count; i++)
        {
            if (this.dialogsList != null && this.dialogsList.ContainsKey(this.openedDialogsList[i]))
            {
                this.dialogsList[this.openedDialogsList[i]].BroadcastMessage("DialogClosed", SendMessageOptions.RequireReceiver);//关闭对话框
            }
        }
        this.openedDialogsList = new List<string>();

        this.CheckForQueue();
    }

    public object GetDialog(DialogsNames name)
    {
        if (!this.dialogsList.ContainsKey(name.ToString()))
        {
            // create Dialog
            this.CreateDialog(name);
        }
        if (!this.dialogsList.ContainsKey(name.ToString()))
        {
            UDebug.LogWarning("[DialogsController] [GetDialog] Error. Dialog not found ["+name+"]");
            return null;
        }
        GameObject dialog = this.dialogsList[name.ToString()];//存储某个Dialog

        object comp = this.GetDialogComponent(dialog, name.ToString());
        if (comp == null)
        {
            UDebug.LogError("[DialogsController] [GetDialog] ERROR. Component [" + name + "] not found on GameObject [" + dialog.name + "]");
            return null;
        }
        return comp;
    } // GetDialog

    private void CheckForQueue()
    {
        if (this.dialogsQueue.Count > 0 && !this.IsBlockerOpened())
        {
            this.Show(this.dialogsQueue.Dequeue());
        }
    } // CheckForQueue

    private object GetDialogComponent(GameObject dialog, string name)
    {
        if (name.IndexOf("_") > 0)
        {
            name = name.Substring(0, name.IndexOf("_"));
        }
        return dialog.GetComponent(name.ToString());//得到name Component
    }

    private void CreateDialog(DialogsNames name)
    {
#if DEBUG
        UDebug.Log("[DialogsController] [CreateDialog] name = " + name.ToString());
#endif
        for (int i = 0; i < this.dialogs.dialogsWindows.Length; i++)
        {
            if (this.dialogs.dialogsWindows[i].name == name.ToString())
            {
                this.dialogsList.Add(name.ToString(), this.dialogs.dialogsWindows[i]);
                return;
            }
        }
        UDebug.LogError("[DialogsController] [CreateDialog] Error. Dialog name = '" + name + "' not found in [this.dialogs.dialogsWindows]");
    } // CreateDialog

    public bool IsBlockerOpened()
    {
        foreach(KeyValuePair<string, GameObject> entry in this.dialogsList)
        {
            if (this.IsDialogOpened(entry.Key) && this.dialogs.dialogsBlockers.Contains(entry.Key))
            {
                UDebug.Log("[IsBlockerOpened] -> " + entry.Key);
                return true;
            }
        }
        return false;
    } // IsBlockerOpened


    // ^^^^^^^^^^^^^ //
    // MonoBehaviour //
    // vvvvvvvvvvvvv //

    void Awake()
    {
        this.dialogsList = new Dictionary<string, GameObject>();
        this.openedDialogsList = new List<string>();
        this.dialogsQueue = new Queue<GameObject>();

        //DialogsController.Instance.AddDialogToRegularList(DialogsNames.SelectFriendsDialog, 3);
    }
} // DialogsController