using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpBar;
    public PlayerController2D player;

    // Update is called once per frame
    public void Update()
    {
      hpBar.fillAmount = player.health / player.maxHealth;

    }
}