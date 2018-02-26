using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    public int idx = 0;
    public Tile[] items;
    int A = 1;
    public void RollCells(bool isLinear)
    {
        List<Tile> tlist = new List<Tile>();
        int y = 0, t = 7;
        int totalSymbols = PayData.totalSymbols;
        int index1 = PayData.totalCell-1;
        if (PayData.down)
        {
            for (int i = index1; i < PayData.totalCell; i++)
            {
                tlist.Add(items[i]);
                items[i].idx = y++;
                items[i].Move(0);

                int total = totalSymbols;
                if (idx == 0 || idx == 4) total--;
                items[i].SetTileType(Random.Range(0, total) % total);

                if (SlotMachine.instance.r > ScoreManager.instance.lowRate)
                {
                 
                    if (items[i].transform.localPosition.y <150&&items[i].transform.localPosition.y>=74)
                        items[i].SetTileType(SlotMachine.instance.suitableIndex);
                    else
                    {
                        items[i].SetTileType(Random.Range(0, total) % total);
                    }
                }


            }
            for (int i = 0; i < index1; i++)
            {
                tlist.Add(items[i]);
                items[i].idx = y++;
                if (SlotMachine.instance.r > ScoreManager.instance.lowRate)
                {

                    if (items[i].transform.localPosition.y <150&&items[i].transform.localPosition.y >= 74)
                        items[i].SetTileType(SlotMachine.instance.suitableIndex);
                    else
                    {
                        items[i].SetTileType(Random.Range(0, totalSymbols) % totalSymbols);
                    
                    }
                }
             

            }

        }
       
        items = tlist.ToArray();
        for (int i =0 ; i < 7; i++)
        {
            items[i].TweenMoveTo(i, isLinear);
            
        }
    }
}
