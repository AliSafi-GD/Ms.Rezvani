using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValueInp : MonoBehaviour
{
    public QuizAnswer quiz;
    void Start()
    {
        var s = $"چقدر از { FindObjectOfType<Lvl3>().answerUser } در زندگی واقعی لذت می بری ؟";
        quiz.arr[0] = s;
        quiz.txtQuiz.text = s;
    }
    
}
