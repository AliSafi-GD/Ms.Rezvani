using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    Vector3 firstPos;
    Rigidbody2D rigid;
    Vector2 vecMove;
    bool isGround;
    public float speed;
    public float powerJump;
    public Animator anim;
    public bool shield=false;
    public GameObject shieldObj;
    public Button btnShield;
    public AudioSource source;
    private void Awake()
    {
        firstPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
       
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
                Jump();

            vecMove = transform.position;
            if (Input.GetAxis("Horizontal") > 0.1f)
                GetComponent<SpriteRenderer>().flipX = false;
            if (Input.GetAxis("Horizontal") < -0.1f)
                GetComponent<SpriteRenderer>().flipX = true;

            if (Input.GetAxis("Horizontal") != 0 && isGround)
                anim.SetBool("Stop", false);
            else
                anim.SetBool("Stop", true);
        
        
    }
    private void LateUpdate()
    {
        vecMove.x += Input.GetAxis("Horizontal")*speed*Time.deltaTime;
        transform.position = vecMove;
    }
    void Jump()
    {
        isGround = false;
        anim.Play("jump");
        rigid.velocity = Vector2.up * powerJump;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            anim.Play("idle");
            isGround = true;
        }
            
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Obs") && !shield)
        {
            DB.instance.SetCoinAndScore(-50, -100);
            transform.position = firstPos;
            source.Play();
        }
            
        else if (col.gameObject.CompareTag("Objects"))
        {
            DB.instance.SetCoinAndScore(Random.Range(50,100), 0);
            Destroy(col.gameObject);
            FindObjectOfType<Minigame3>().CurrentObj++;
            shield = false;
            shieldObj.SetActive(false);
            btnShield.interactable = true;
        }
            
    }
    public void ActiveShield()
    {
        shieldObj.SetActive(true);
        DB.instance.SetCoinAndScore(-25, 0);
        shield = true;
    }
}
