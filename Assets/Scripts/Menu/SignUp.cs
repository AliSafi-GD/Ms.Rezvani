using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UPersian.Utils;

public class SignUp : MonoBehaviour
{

    [Header("Sign Up Page")]
    public InputField[] inputFields;
    public Dropdown[] dropdowns;

    public Button[] buttons;
    public GameObject[] pages;
    public static bool isDoneSignup;
    private void Start()
    {
        if (!isDoneSignup)
        {
            foreach (var item in inputFields)
            {
                item.onEndEdit.AddListener((string s) => SetInputField(item.GetComponent<InputFieldType>().inputMode, s));
                if (item.GetComponent<InputFieldType>().inputMode == InputFieldType.InputMode.Mail)
                    SetInputField(item.GetComponent<InputFieldType>().inputMode, "null");
            }
            foreach (var item in dropdowns)
            {

                item.onValueChanged.AddListener((int b) => SetToggleGroup(item.GetComponent<ToggleGroupType>().togglesMode, item.options[item.value].text));
                //if (item.isOn)
                SetToggleGroup(item.GetComponent<ToggleGroupType>().togglesMode, item.options[item.value].text);
            }
            foreach (var item in buttons)
            {
                item.onClick.AddListener(() => ButtonAction(item.GetComponent<ButtonType>().mode));
            }
            SwitchPage(2);
        }
        
        else
            SwitchPage(3);
        
    }
    void CheckIsFullSpecification()
    {
        DB.instance.data.infoAccount.IsFullAllSpecification((result) =>
        {
            
            if (result)
            {

                SwitchPage(1);

            }
            else
            {
                Color c = new Color();
                ColorUtility.TryParseHtmlString("#FB4D4D", out c);
                foreach (var item in inputFields)
                {
                    if (item.GetComponent<InputFieldType>().inputMode != InputFieldType.InputMode.Mail && (item.text == null || item.text == string.Empty))
                        item.placeholder.color = c;
                }

            }
        });
    }
    void SetInputField(InputFieldType.InputMode mode, string str)
    {
        switch (mode)
        {
            case InputFieldType.InputMode.Name:
                DB.instance.data.infoAccount.userName = str;
                break;
            case InputFieldType.InputMode.City:
                DB.instance.data.infoAccount.city = str;
                break;
            case InputFieldType.InputMode.Age:
                DB.instance.data.infoAccount.age = str;
                break;
            case InputFieldType.InputMode.Major:
                DB.instance.data.infoAccount.major = str;
                break;
            case InputFieldType.InputMode.job:
                DB.instance.data.infoAccount.job = str;
                break;
            case InputFieldType.InputMode.Mail:
                DB.instance.data.infoAccount.mail = str.Length == 0 ? "null" : str;
                break;
        }
        //CheckIsFullSpecification();
    }
    void SetToggleGroup(ToggleGroupType.TogglesMode mode, string str)
    {
        switch (mode)
        {
            case ToggleGroupType.TogglesMode.Gender:
                DB.instance.data.infoAccount.gender = str;
                break;
            case ToggleGroupType.TogglesMode.Grade:
                DB.instance.data.infoAccount.education = str;
                break;
            case ToggleGroupType.TogglesMode.marital:
                DB.instance.data.infoAccount.marital = str;
                break;
        }
        //DB.instance.infoAccount.gender = str;
        
    }



    public void ButtonAction(ButtonType.Mode mode)
    {
        switch (mode)
        {
            case ButtonType.Mode.Next:
                
                isDoneSignup = true;
                CheckIsFullSpecification();
                //GameRefremce.instance.IsRun = true;
                break;
            //case ButtonType.Mode.Finish:
            //    SwitchPage(2);
                
                
            //    break;
        }
    }
    void SwitchPage(int index)
    {
        foreach (var item in pages)
        {
            item.SetActive(false);
        }
        if (index == 2)
            pages[0].SetActive(true);

        if (index == 1)
            pages[2].SetActive(true);
        pages[index].SetActive(true);
    }


}
