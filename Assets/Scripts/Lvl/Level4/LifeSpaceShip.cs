using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSpaceShip : MonoBehaviour
{
    int life = 20;
    public Slider sld;
    public Animator anim;
    public MiniGame4 game4;
    public FireSpaceShip fireSpace;
    public int Life
    {
        get => life;
        set
        {
            DB.instance.SetCoinAndScore(0, Random.Range(-3, -6));
            life = value;
            sld.value = value;
            if (life <= 0)
                DestroyShip();
        }
    }
    public void DestroyShip()
    {
        game4.isStart = false;
        foreach (var item in fireSpace.coroutine)
        {
            fireSpace.StopCoroutine(item);
        }
        
        anim.Play("Explosion");
        FindObjectOfType<MiniGame4>().wndGameOver.SetActive(true);
    }
}
