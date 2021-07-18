using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllAsemblyRobot : MonoBehaviour
{
    public Slider sld;
    public float currentValue;
    private void Update()
    {
        sld.value = Mathf.MoveTowards(sld.value, currentValue, 2 * Time.deltaTime);
    }
    public void SetValue(float value)
    {
        currentValue = value;
    }
}
