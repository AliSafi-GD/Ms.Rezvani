using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform parent;
    public MiniGame4.PlaceEnemy place;
    Collider2D col;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        StartCoroutine(ControllFire());
    }
    private void Update()
    {
        if (place != null)
        {
            transform.position = Vector3.Lerp(transform.position, place.trs.position, 5 * Time.deltaTime);
            col.enabled = (Vector3.Distance(transform.position, place.trs.position) < 5);
        }
       
    }
    public void Fire()
    {
        Instantiate(bullet, transform.position, transform.rotation,parent);
    }
    IEnumerator ControllFire()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(Random.Range(1, 2));
        }
    }
}
