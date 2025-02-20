using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour
{
    public Slider Slider;
    public Color WhenLow;
    public Color WhenHigh;
    public Vector3 offSet;
  

    public void SetHealth (int health, int maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;
        Slider.minValue = 0;
        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(WhenLow, WhenHigh, Slider.normalizedValue);
    }

    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offSet);

    }
}
