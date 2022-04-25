using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image healthBar;
    private float currentHealth;
    private float maxHealth;
    public RobotController Player;

    private void Start()
    {
        //Finds variables
        maxHealth = Player.RoboMaxHealth;
    }

    private void Update()
    {
        currentHealth = Player.RoboCurrentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
