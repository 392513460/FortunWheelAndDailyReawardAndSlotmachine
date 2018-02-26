using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum DialogsNames
{
    FortuneWheelWinDialog = 50,
    BuyHardMoneyDialog = 100,
    ErrorMessageDialog = 200,
} // DialogsNames

public class Dialogs : MonoBehaviour {
	public GameObject[] dialogsWindows;

    [HideInInspector]
    public List<string> dialogsBlockers;
    /*
    public Sprite titleBlueGlow;
    public Sprite titleGoldGlow;

    public Sprite titleBlueDots;
    public Sprite titleGoldDots;

    public Sprite titleBlueNeonline;
    public Sprite titleGoldNeonline;

    public Font goldTitleFont;
    public Font silverTitleFont;
    */
    void Awake()
    {
        DialogsController.Instance.dialogs = this;
    }

    void Start()
    {
        this.dialogsBlockers = new List<string>();
        this.dialogsBlockers.Add(DialogsNames.ErrorMessageDialog.ToString());
    } // Start

} // Dialogs