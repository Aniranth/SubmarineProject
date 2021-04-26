using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Points : MonoBehaviour
{
    Text pointText;

    private void Start()
    {
        pointText = GetComponent<Text>();
        Collectable.Collect += UpdatePointText;
    }

    void OnDestroy(){
        Collectable.Collect -= UpdatePointText;
    }

    private void UpdatePointText(int value) // value of point probably just 1 but could change
    {
        string number = "";
        foreach(char c in pointText.text)
        {
            if(char.IsDigit(c))
            {
                number += c;
            }
        }
        pointText.text = "Collectables Found: " + (Int32.Parse(number) + value);
    }
}
