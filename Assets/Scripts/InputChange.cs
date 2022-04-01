using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ValueUpdate(float value)
    {
        this.GetComponentInChildren<TMP_InputField>().text = "" + (int)value;
    }
}
