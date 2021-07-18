using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameRefremce : MonoBehaviour {

    
    public Text txtTimer;
    Animator anim;

    public static GameRefremce instance;

    float timer;

    float _Timer{
        get{
            return timer;
        }
        set{
            timer = value;
            txtTimer.text = $"{(value/60).ToString("00")} : {(value % 60).ToString("00") }";
        }
    }
   
    IEnumerator ITimer;

    bool isRun;
    public bool IsRun{
        get{
            return isRun;
        }
        set{
            isRun = value;
            if(value){
                StartCoroutine(ITimer);
                anim.Play("Show_Timer");
            }
                
            else{
                StopCoroutine(ITimer);
                anim.Play("Hide_Timer");
            }
                
        }
    }
    private void Awake() {
        anim = GetComponent<Animator>();
        ITimer = Timer();
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }else{
            Destroy(this);
        }
    }

    IEnumerator Timer(){
        while (isRun)
        {
            yield return new WaitForSeconds(1.0f);
            _Timer++;
        }
    }
    
}