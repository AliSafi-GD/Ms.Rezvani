using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4 : TaskGame
{
    public MiniTask[] skips;
    public MiniTask[] skips2;

    public void SkipTasks()
    {
        foreach (var item in skips)
        {
            item.isComlete = true;
        }
    }
    public void SkipTasks2()
    {
        foreach (var item in skips2)
        {
            item.isComlete = true;
        }
    }
}
