//#define DEBUG
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<PlayerController>();
                go.name = "PlayerController";
                go.transform.parent = Common.GetControllerHolder();
            }
            return _instance;
        }
    }

    public static bool hasInstance
    {
        get
        {
            if (_instance != null)
            {
                return true;
            }
            return false;
        }
    }
    
    public System.EventHandler onInventoryUpdatedEvent;

    private int _hardMoney;
    private int _softMoney;
    public int hardMoney
    {
        get
        {
            return this._hardMoney;
        }
        set
        {
            int oldValue = this._hardMoney;
            this._hardMoney = value;
            if (this._hardMoney != oldValue)
            {
                if (this.onInventoryUpdatedEvent != null)
                {
                    this.onInventoryUpdatedEvent.Invoke(null, null);
                }
            }
        }
    }
    public int softMoney
    {
        get
        {
            return this._softMoney;
        }
        set
        {
            int oldValue = this._softMoney;
            this._softMoney = value;
            if (this._softMoney != oldValue)
            {
                if (this.onInventoryUpdatedEvent != null)
                {
                    this.onInventoryUpdatedEvent.Invoke(null, null);
                }
            }
        }
    }


    public void InitInventory(string data)
    {
        JSONObject o = new JSONObject(data);
        if (o == null || o.IsNull)
        {
            UDebug.LogError("[PlayerController] [InitInventory] Cannot parse server answer. Text is " + data);
            return;
        }
        if (!o.HasField("data"))
        {
            UDebug.LogError("[PlayerController] [InitInventory] Cannot parse server answer. 'data' field is not found");
            return;
        }
        JSONObject inventory = o["data"];
        this.InitInventory(inventory);
    }

    public void InitInventory(JSONObject inventory)
    {
        UDebug.Log("[PlayerController] [InitInventory] " + inventory.ToString() );

        if (inventory["money"] != null && inventory["money"].IsObject)
        {
            JSONObject inventoryMoney = inventory["money"];
            if (inventoryMoney["hard"] != null && inventoryMoney["hard"].IsNumber)
            {
                this.hardMoney = (int)inventoryMoney["hard"].n;
            }
            else
            {
                UDebug.LogError("Cannot get [hard] field from [inventory][money]");
            }
            if (inventoryMoney["soft"] != null && inventoryMoney["soft"].IsNumber)
            {
                this.softMoney = (int)inventoryMoney["soft"].n;
            }
            else
            {
                UDebug.LogError("Cannot get [soft] field from [inventory][money]");
            }
        } // if
        else
        {
            UDebug.LogError("Cannot get [money] field from [inventory]");
        }

        if (this.onInventoryUpdatedEvent != null)
        {
            this.onInventoryUpdatedEvent.Invoke(null, null);
        }
    } // InitInventory
    

} // PlayerController