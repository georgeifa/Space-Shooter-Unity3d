using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;

    public GameObject bar;

    public Image health;
    public Image shield;

    private void Start()
    {
        shield.enabled = false;
        health.enabled = true;
        slider.fillRect = health.rectTransform;
    }

    public void ActivateHealthBar()
    {
        GetComponent<Animator>().SetBool("bossReady", true);

        bar.SetActive(true);
    }

    public void DeActivateHealthBar()
    {
        bar.SetActive(false);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }


    public void SetMaxShield(int shield)
    {
        slider.maxValue = shield;
        slider.value = shield;
    }

    public void SetShield(int shield)
    {
        slider.value = shield;
    }

    public void healthToShield(int shieldHP)
    {
        SetMaxShield(shieldHP);
        SetShield(shieldHP);
        slider.fillRect = shield.rectTransform;
        health.enabled = false;
        shield.enabled = true;
    }

    public void ShieldToHealth(int maxHP,int curHP)
    {
        SetMaxHealth(maxHP);
        SetHealth(curHP);
        slider.fillRect = health.rectTransform;
        shield.enabled = false;
        health.enabled = true;

    }
}
