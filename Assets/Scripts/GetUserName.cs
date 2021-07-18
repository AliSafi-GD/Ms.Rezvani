using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUserName : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txtName;
    void Start()
    {
        txtName = GetComponent<Text>();
        txtName.text = DB.instance.data.infoAccount.userName;
    }

}
