using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FortuneWheelItemManager : MonoBehaviour {

    public Image itemIcon;
    public Text itemText;

    public void Init(FortuneOption option)
    {
        this.itemText.text = option.qty.ToString();
        this.itemIcon.sprite = FortuneWheelController.Instance.GetSpriteForOption(option);//奖品sprite管理
        //this.itemIcon.SetNativeSize();
        this.itemIcon.enabled = true;//是否显示图片
    } // Init

} // FortuneWheelItemManager