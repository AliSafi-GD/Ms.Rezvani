using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpaceShip : MonoBehaviour
{

    public float speed;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            DB.instance.SetCoinAndScore(5, 10);
            collision.gameObject.GetComponent<Enemy>().place.isPlaced = false;
            Destroy(gameObject);
            Destroy(collision.collider.gameObject);
            FindObjectOfType<MiniGame4>().CreateNewEnemy();
        }
    }

}
