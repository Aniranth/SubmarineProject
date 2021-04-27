using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{

    [TextArea(3, 15)]
    public string text;
    public Text box;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TypeSentence(text));    
    }

    IEnumerator TypeSentence(string text){
        box.text = "";
        foreach(char c in text.ToCharArray()){
            box.text += c;
            yield return null;
        }
    }

}
