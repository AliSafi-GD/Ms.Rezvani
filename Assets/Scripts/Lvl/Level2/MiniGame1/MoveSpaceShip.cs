using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSpaceShip : MonoBehaviour
{
    public float speed = 100;
    public float speedRot = 100;
    public Vector3 mainPos;
    public Vector2 mainSize = new Vector2(1280, 720);
    public float calcx;
    public float calcy;
    public bool isShield;
    public Image imgShield;
    Animator anim;
    public Button btnShield;
    public MiniGame4 game4;
    public bool IsShiel
    {
        get => isShield;
        set
        {
            isShield = value;
            imgShield.gameObject.SetActive(value);
            btnShield.interactable = !value;
            StartCoroutine(ShieldTime());
        }
    }
    IEnumerator ShieldTime()
    {
        yield return new WaitForSeconds(5);
        IsShiel = false;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        btnShield.onClick.AddListener(() =>
        {
            if (!IsShiel)
            {
                IsShiel = true;
                DB.instance.SetCoinAndScore(-50, 0);
            }
        });
        mainPos =transform.position;
        calcx =((mainSize.x- Screen.width))/2;
        calcy =((mainSize.y- Screen.height))/2;
    }
    private void LateUpdate()
    {
        if (game4!= null && !game4.isStart)
            return;

        var vec2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var spVec2 = GetComponent<RectTransform>().anchoredPosition += vec2*speed*Time.deltaTime;


        //var mx = (((Input.mousePosition.x - mainPos.x)/mainPos.x)*calcx+ (Input.mousePosition.x - mainPos.x));
        //var my = (((Input.mousePosition.y - mainPos.y)/mainPos.y)*calcy+ (Input.mousePosition.y - mainPos.y));
        var w = GetComponent<RectTransform>().sizeDelta.x / 2;
        var h = GetComponent<RectTransform>().sizeDelta.y / 2;
        spVec2.x = Mathf.Clamp(spVec2.x, (-mainSize.x/2)+w, (mainSize.x/2)-w);
        spVec2.y = Mathf.Clamp(spVec2.y, (-mainSize.y/2)+h, (mainSize.y/2)-h);
        //var r = (GetComponent<RectTransform>().anchoredPosition.x- mx);
        //r = Mathf.Clamp(r, -20, 20);
        //GetComponent<RectTransform>().rotation = Quaternion.Lerp(GetComponent<RectTransform>().rotation,Quaternion.Euler(0, 0, r),speedRot*Time.deltaTime);
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition,spVec2,speed * Time.deltaTime);


        // transform.position = new Vector2(mx.x, 0);
    }

    public void Hit()
    {
        anim.Play("ShipHit");
        DB.instance.SetCoinAndScore(-4, -6);
    }
    
}
