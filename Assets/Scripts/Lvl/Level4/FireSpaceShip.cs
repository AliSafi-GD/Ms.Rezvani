using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpaceShip : MonoBehaviour
{
    public GameObject bullet;
    public Transform prtBullet;
    public List<Coroutine> coroutine=new List<Coroutine>();
    public MiniGame4 game4;
    private void Update()
    {
        if (!game4.isStart)
            return;
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                coroutine.Add(StartCoroutine(Fire()));
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (var item in coroutine)
                {
                    StopCoroutine(item);
                }
                
            }
        }
       
    }
    IEnumerator Fire()
    {
        while (true)
        {
            var b = Instantiate(bullet, transform.position, Quaternion.identity, prtBullet);
            b.transform.SetSiblingIndex(prtBullet.transform.childCount - 2);
            Destroy(b, 3);
            yield return new WaitForSeconds(0.2f);
           
        }
    }
}
