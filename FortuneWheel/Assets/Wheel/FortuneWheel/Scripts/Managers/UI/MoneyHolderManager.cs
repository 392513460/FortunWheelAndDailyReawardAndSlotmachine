using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyHolderManager : MonoBehaviour {
    public string moneyType;
    public Text num;
    public Image icon;

    private int oldValue;
    private int toValue;

    void OnEnable()
    {
        PlayerController.Instance.onInventoryUpdatedEvent += this.OnInventoryUpdated;
        this.UpdateVisual();
    }

    void OnDisable()
    {
        if (PlayerController.hasInstance)
        {
            PlayerController.Instance.onInventoryUpdatedEvent -= this.OnInventoryUpdated;
        }
    }

    public void AddMoney()
    {
        if (DialogsController.Instance.IsBlockerOpened())
        {
            UDebug.Log("Blocker popup is opened");
            return;
        }
        BuyHardMoneyDialog buyHardMoneyDialog = (BuyHardMoneyDialog)DialogsController.Instance.GetDialog(DialogsNames.BuyHardMoneyDialog);
        DialogsController.Instance.Show(buyHardMoneyDialog.gameObject, ignoreOpenTest: true);
    } // AddMoney

    public void OnInventoryUpdated(object sender, System.EventArgs args)
    {
        this.UpdateVisual();
    }

    public void UpdateVisual()
    {
        //DebugMy.Log("[MoneyHolderManager] [UpdateVisual] this.moneyType = " + this.moneyType);
        if (this.moneyType == "soft")
        {
            this.UpdateVisualSoftMoney();
        } else
        {
            this.UpdateVisualHardMoney();
        }
        
    } // UpdateLevelVisual

    private void UpdateVisualSoftMoney()
    {
        this.toValue = PlayerController.Instance.softMoney;

        if (string.IsNullOrEmpty(this.num.text))
        {
            this.oldValue = this.toValue;
        }
        else
        {
            if (!int.TryParse(this.num.text, out this.oldValue))
            {
                this.oldValue = this.toValue;
            }
        }

        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(this.DoUpdateCounterVisual());
        } else
        {
            this.num.text = this.toValue.ToString();
        }
    }

    private void UpdateVisualHardMoney()
    {
        this.toValue = PlayerController.Instance.hardMoney;
        
        if (string.IsNullOrEmpty( this.num.text))
        {
            this.oldValue = this.toValue;
        } else
        {
            if (!int.TryParse(this.num.text, out this.oldValue))
            {
                this.oldValue = this.toValue;
            }
        }
        
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(this.DoUpdateCounterVisual());
        }
        else
        {
            this.num.text = this.toValue.ToString();
        }
    }

    private IEnumerator DoUpdateCounterVisual()
    {
        if (this.toValue <= this.oldValue)
        {
            this.num.text = this.toValue.ToString();
            yield break;
        }

        int steps = 10;
        if (this.toValue - this.oldValue < steps)
        {
            steps = this.toValue - this.oldValue;
        }
        int stepDelta = (this.toValue - this.oldValue) / steps;
        for (int i = 1; i <= steps; i++)
        {
            this.num.text = (this.oldValue + i * stepDelta).ToString();
            yield return new WaitForSeconds(0.05f);
        }
        this.num.text = this.toValue.ToString();
        yield break;
    } // DoUpdateCounterVisual

} // MoneyHolderManager