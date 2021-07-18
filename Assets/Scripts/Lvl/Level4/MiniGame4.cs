using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MiniGame4 : MiniTask
{
    public GameObject enemy;
    public Transform prtEnemy;
    public List<GameObject> collEnemy;
    [SerializeField] public PlaceEnemy[] placesEnemies;
    public int currentEnemy;
    public GameObject wndGameOver;
    public MoveSpaceShip spaceShip;

    public int destroyENemy;
    public Text txtShowEnemy;
    public bool isStart;
    private void Start()
    {
        StartCoroutine(SetPlaceEmenies());
    }
    public void ActiveStart()
    {
        isStart = true;
    }
    IEnumerator SetPlaceEmenies()
    {
        yield return new WaitUntil(() => isStart);
        for (int i = 0; i < 3; i++)
        {
            var e = Instantiate(enemy, placesEnemies[i].trs);
            placesEnemies[i].isPlaced = true;
            e.GetComponent<Enemy>().place = placesEnemies[i];
            e.GetComponent<Enemy>().parent = prtEnemy;
            currentEnemy++;
            yield return new WaitForSeconds(1);


        }
    }
    public void ResetGame()
    {
        FindObjectOfType<LifeSpaceShip>().Life = 20;
        FindObjectOfType<LifeSpaceShip>().anim.Play("Idle");
        isStart = true;
        wndGameOver.SetActive(false);
        
    }
    public void SkipLvl()
    {
        isComlete = true;
        DB.instance.SetCoinAndScore(-150, 0);
    }
    public void CreateNewEnemy()
    {

        destroyENemy++;
        txtShowEnemy.text = $"{destroyENemy} : 30";
        if (destroyENemy <= 27)
        {
            
            var p = (from x in placesEnemies where !x.isPlaced select x).ToList();
            
            var e = Instantiate(enemy, p[0].trs);
            p[0].isPlaced = true;
            e.GetComponent<Enemy>().place = p[0];
            e.GetComponent<Enemy>().parent = prtEnemy;
        }
        else if (destroyENemy >= 30)
        {
            DB.instance.SetCoinAndScore(0, 346);
            isComlete = true;
            print("ENd");
        }
            

        

    }
    [Serializable]
    public class PlaceEnemy
    {
        public Transform trs;
        public bool isPlaced;
    }
}
