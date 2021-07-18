using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Minigame3 : MiniTask
{
    public int currentObj;
    public void SkipLvl()
    {
        DB.instance.SetCoinAndScore(-150, 0);
        isComlete = true;
    }
    public int CurrentObj
    {
        get => currentObj;
        set
        {
            currentObj = value;
            if (value == 4)
            {
                isComlete = true;
                DB.instance.SetCoinAndScore(0, Random.Range(500, 800));
            }
                
        }
    }
}
