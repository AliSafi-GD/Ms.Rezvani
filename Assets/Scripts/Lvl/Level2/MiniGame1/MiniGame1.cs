using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MiniTask
{
    public GameObject[] objs;
    public int currentPhase;
    public int numbermeteor;
    public RectTransform ship;
    public Text txtCurrentMeteor;
    public Slider sld;
    public bool isDead;
    [Header("ChangeRound")]
    public GameObject objRound;
    public Text txtShowRound;
    public Animator animRound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateObj());
     
    }
    public void SkipLvl()
    {
        isComlete = true;
        DB.instance.SetCoinAndScore(-200, 0);
    }
    IEnumerator GenerateObj()
    {

        for (int i = 0; i < 4; i++)
        {
            objRound.SetActive(true);
            txtShowRound.text = $"موج {i + 1}";
            animRound.Play("Show");
            var timeOut = 0f;
            var speed = 0;
            if (i == 0) { timeOut = Random.Range(0.5f, 1f); speed = 500; }
            else if (i == 1) { timeOut = Random.Range(0.6f, 1f); speed = 600; }
            else if (i == 2) { timeOut = Random.Range(0.75f, 0.9f); speed = 800; }
            else if (i == 3) { timeOut = Random.Range(0.5f, 0.75f); speed = 1000; }
            else if (i == 4) { timeOut = Random.Range(0.3f, 0.5f); speed = 1300; }
            
            yield return new WaitForSeconds(2f);
            animRound.Play("Hide");
            yield return new WaitForSeconds(1f);
            objRound.SetActive(false);
            for (int r = 0; r < 40; r++)
            {

                txtCurrentMeteor.text = $"{(i + 1)}  ﺝﻮﻣ";
                sld.value = r;
                var x = Random.Range(ship.anchoredPosition.x - 200, ship.anchoredPosition.x + 200);
                var rock = Instantiate(objs[Random.Range(0, objs.Length)], Vector3.zero, Quaternion.identity, transform);
                rock.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, (Screen.height / 2) + rock.GetComponent<RectTransform>().sizeDelta.y);
                rock.GetComponent<Meteor>().direction = new Vector2(x < -0.1 ? Random.Range(0, 1) : Random.Range(0, -1), -1);
                rock.GetComponent<Meteor>().speed = speed;
                //objCreated.Add(rock);
                yield return new WaitForSeconds(timeOut);

            }
            
            //objCreated.Clear();
        }
        DB.instance.SetCoinAndScore(0, Random.Range(500, 800));
        isComlete = true;
    }
    
    
}
