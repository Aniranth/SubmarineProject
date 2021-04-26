using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaManager : MonoBehaviour {

    // link up to dialogue box
    public Text nameText;
    public Text lineText;
    
    private Queue<DiaLine> sentences;

    void Start() {
        sentences = new Queue<DiaLine>();
    }

    public void StartDialogue(DiaScript diascript){
        sentences.Clear();
        
        foreach (DiaLine dialine in diascript.lines){
            sentences.Enqueue(dialine);
        }

        NextSentence();
    }

    public void NextSentence(){
        if(sentences.Count == 0){
            EndDialogue();
            return;
        }

        DiaLine curr = sentences.Dequeue();

        nameText.text = curr.name;
        // lineText.text = curr.text;
        StopAllCoroutines(); // don't animate two sentences at once
        StartCoroutine(TypeSentence(curr.line));
    }

    IEnumerator TypeSentence(string text){
        lineText.text = "";
        foreach(char c in text.ToCharArray()){
            lineText.text += c;
            yield return null;
        }
    }

    void EndDialogue(){
        // End Dialogue
    }
}
