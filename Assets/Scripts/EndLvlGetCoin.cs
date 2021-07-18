using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLvlGetCoin : MonoBehaviour
{
    public Text txtScore,txtCoin;
    public Button btn;
    float score, coin;
    private void Start()
    {
        SetCoin(DB.instance.Coin);
    }
    public void SetCoin(int index)
    {
        StartCoroutine(ICoint(index));
    } 
    public void SetScore(int index)
    {
        StartCoroutine(IScore(index));
    }
    IEnumerator ICoint(float index)
    {
        while (coin < index)
        {
            coin=Mathf.MoveTowards(coin,index,(index/2)*Time.deltaTime);
            txtCoin.text = coin.ToString("0");
            yield return null;
        }
        SetScore(DB.instance.data.infoAccount.score);
    }
    IEnumerator IScore(int index)
    {
        while (score < index)
        {
            score = Mathf.MoveTowards(score, index, (index/2) * Time.deltaTime);
            txtScore.text = score.ToString("0");
            yield return null;
            //print("Score");
        }
        btn.interactable = true;
    }
}
