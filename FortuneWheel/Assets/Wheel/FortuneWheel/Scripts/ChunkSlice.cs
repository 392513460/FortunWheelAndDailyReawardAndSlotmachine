using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSlice : MonoBehaviour {
    int myIndex;
    public SpriteRenderer iconSpRender;
    public TextMesh valueText;
    public int winChance;
    public int id;
    // Use this for initialization
    void Start()
    {
        //if (StartPoint.instance.isMoveWheel == false)
        //{
        //    myIndex = transform.GetSiblingIndex();
        //    iconSpRender.sprite = SpinWheelSetUp.Instance.rewardItem[myIndex].rewardSprite;
        //    valueText.text = SpinWheelSetUp.Instance.rewardItem[myIndex].rewardQuantity.ToString();
        //}
        //else
        //{
        //    myIndex = transform.GetSiblingIndex();
        //    iconSpRender.sprite = MoveWheelSetUp.Instance.rewardItem[myIndex].rewardSprite;
        //    valueText.text = MoveWheelSetUp.Instance.rewardItem[myIndex].rewardQuantity.ToString();
        //}

        //added by ben 去掉StartPoint.instance.isMoveWheel == false判断  
        myIndex = transform.GetSiblingIndex();
        iconSpRender.sprite = MoveWheelSetUp.Instance.rewardItem[myIndex].rewardSprite;
        valueText.text = MoveWheelSetUp.Instance.rewardItem[myIndex].rewardQuantity.ToString();
        this.winChance = MoveWheelSetUp.Instance.rewardItem[myIndex].winChance;
        this.id = MoveWheelSetUp.Instance.rewardItem[myIndex].id;
    }
	
	// Update is called once per frame

}
