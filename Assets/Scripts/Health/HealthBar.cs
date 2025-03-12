using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image bar;

    public void UpdateHealth(float health, float maxHealth)
    {
        int currentHealth = (int)health;
        int maximumHealth = (int)maxHealth;

        healthText.text = currentHealth.ToString() + " / " + maximumHealth.ToString();
        bar.fillAmount = (float)currentHealth / (float)maximumHealth;
    }

    public void UpdateBar(int value, int maxValue, string text)
    {
        healthText.text = text;
        bar.fillAmount = (float)value / (float)maxValue;
    }
}