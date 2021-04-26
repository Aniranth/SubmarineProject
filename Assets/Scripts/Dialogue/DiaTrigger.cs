using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaTrigger : MonoBehaviour {

    public DiaScript diascript;

    public void TriggerDialogue() {
        FindObjectOfType<DiaManager>().StartDialogue(diascript);
    }
    
}
