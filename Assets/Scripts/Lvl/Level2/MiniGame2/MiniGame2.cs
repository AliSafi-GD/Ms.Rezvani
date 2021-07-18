using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame2 : MiniTask
{
    public List<DragObject> dragObjects;

    public int scoreNeeded=6;
    private int currentScore;
    public Button btnHelp;
    private void Start()
    {
        btnHelp.onClick.AddListener(Help);
    }
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            if(value >= scoreNeeded)
            {
                Win();
            }
        }
    }
    public void AddScore(DragObject item)
    {
        DB.instance.SetCoinAndScore(25, 0);
        dragObjects.Remove(item);
        CurrentScore++;
    }
    void Win()
    {
        print("win");
        DB.instance.SetCoinAndScore(0, Random.Range(500, 800));
        isComlete = true;
    }
    void Help()
    {
        btnHelp.interactable = false;
        var item = dragObjects[Random.Range(0, dragObjects.Count)];
        StartCoroutine(MoveToMainPlace(item));
        DB.instance.SetCoinAndScore(-30, 0);
    }
    IEnumerator MoveToMainPlace(DragObject item)
    {
        item.transform.SetAsLastSibling();
        while (item.firstPos != item.GetComponent<RectTransform>().anchoredPosition)
        {
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(item.GetComponent<RectTransform>().anchoredPosition, item.firstPos, 10 * Time.deltaTime);
            yield return null;
            if (Vector2.Distance(item.GetComponent<RectTransform>().anchoredPosition, item.firstPos) < 5)
                item.GetComponent<RectTransform>().anchoredPosition = item.firstPos;

        }
        AddScore(item);
        btnHelp.interactable = true;
    }
}
