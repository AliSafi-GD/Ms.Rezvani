
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MiniTask
{


    private int numberTrue;


    public int NumberTrue
    {
        get
        {
            return numberTrue;
        }
        set
        {
            numberTrue = value;
            if(value == 7)
            {
                
                FindObjectOfType<Lvl1>().RotateRobot();
                isComlete = true;
               
            }
        }
    }

   
}

