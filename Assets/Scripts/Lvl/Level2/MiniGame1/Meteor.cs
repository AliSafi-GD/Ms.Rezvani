using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Meteor : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float speedRotate;
    Image img;
    Animator anim;
    public bool isMove = true;
    AudioSource source;
    // Update is called once per frame
    private void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        speedRotate = Random.Range(-100, 100);
        transform.localScale = Vector3.one * Random.Range(0.3f, 1);
        img = GetComponent<Image>();
    }
    void Update()
    {
        if (isMove)
        {
            transform.transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
            transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
            if (GetComponent<RectTransform>().anchoredPosition.y < -(Screen.height / 2))
            {
                Destroy(gameObject);
                DB.instance.SetCoinAndScore(2, 5);
            }
                
        }
      
    }
    public void DestroyMeteor()
    {
        isMove = false;
        anim.Play("Explosion");
        GetComponent<Collider2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shield")) 
        {
           
        }
        else if(other.CompareTag("Ship"))
        {
            other.GetComponent<MoveSpaceShip>().Hit();
        }
        DestroyMeteor();
        source.Play();
    }
    public void DesFromAnim()
    {
       
        Destroy(gameObject);
    }
}
