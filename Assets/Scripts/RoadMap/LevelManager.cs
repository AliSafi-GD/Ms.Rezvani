using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public Level[] levels;
    public static int currenLevel=0;

    public int CurrenLevel
    {
        get
        {
            return currenLevel;
        }
        set
        {
            currenLevel = value;
            if (currenLevel == 0)
                levels[currenLevel].UnlockLevel();
            else
                for (int i = 0; i < currenLevel + 1; i++)
                {
                    levels[i].UnlockLevel();
                }


        }
    }
    private void Start()
    {
        CurrenLevel = currenLevel;
        DB.instance.StartTimer();
    }
    public static void ChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

}
