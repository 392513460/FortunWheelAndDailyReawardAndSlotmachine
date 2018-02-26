using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
public class Tile : MonoBehaviour {

    public SlotMachine slotMachine;
    public UISprite sprites;
    private int _type;
    public Line lineScript;
   public  int idx;
    public void SetTileType(int type)
    {
        _type = type;
        sprites.spriteName= PayData.itemName[type];
    }
    public void Move(int seq)
    {
   
        transform.localPosition = new Vector3(0, 225 - seq * 75, 0);
    }
    public void TweenMoveTo(int seq, bool isLinear)
    {
        if (isLinear)
            TweenMove(transform, transform.localPosition, new Vector3(0, 225- seq * 75, 0));
    }
    public int GetTileType()
    {
        return _type;
    }
    float speed;
    void TweenMove(Transform tr, Vector3 pos1, Vector3 pos2)
    {
        
        tr.localPosition = pos1;
        TweenParms parms = new TweenParms().Prop("localPosition", pos2).Ease(EaseType.Linear);
        HOTween.To(tr, 0.1f, parms);

       
    }
}
