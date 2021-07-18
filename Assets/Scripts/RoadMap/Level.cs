using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int number;
    Animator anim;
    bool unlock;
    Button button;
    public string nameScene;
    private void Start()
    {
        button = GetComponent<Button>();
        button.interactable = (number == LevelManager.currenLevel);

        button.onClick.AddListener(() =>
        {
            if (unlock && number == LevelManager.currenLevel)
            {
                SceneManager.LoadScene(nameScene);
                button.interactable = false;
            }
                

        });
    }
    public void UnlockLevel(){
        if(anim == null)
            anim = GetComponent<Animator>();
        anim.Play("OpenLevel");
        unlock = true;
        //print("Unlock");
    }
    
}
