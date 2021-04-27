using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<Text>();
        healthText.text = "HP: 30";
        PlayerHealth.HealthLost += UpdateHealthUI;
    }

    public void UpdateHealthUI(int currHealth)
    {
        healthText.text = "HP: " + currHealth;
    }

    private void OnDestroy()
    {
        PlayerHealth.HealthLost -= UpdateHealthUI;
    }
}
