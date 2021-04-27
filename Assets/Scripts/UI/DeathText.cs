using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathText : MonoBehaviour
{
    Text deathText;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.StartSink += this.GameOver;
        deathText = GetComponent<Text>();
        deathText.text = "";
    }

    private void GameOver()
    {
        Debug.Log("Death text");
        deathText.text = "You Have Sunk";
    }

    private void OnDestroy()
    {
        PlayerHealth.StartSink -= this.GameOver;
    }
}
