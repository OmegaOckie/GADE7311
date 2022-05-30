using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public string robotName;

    public float robotMaxHealth;
    public float robotCurrentHealth;

    public float robotDamage;

    public bool TakeDamage(float damage)
    {
        robotCurrentHealth -= damage;

        if (robotCurrentHealth <= 0)
            return true;
        else
            return false;
    }

    public void Heal(float healAmount)
    {
        robotCurrentHealth += healAmount;
    }
}