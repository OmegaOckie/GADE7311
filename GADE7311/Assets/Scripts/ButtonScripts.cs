using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScripts : MonoBehaviour
{
    //variables
    public RobotController player;
    public RobotController opponent;
    

    public void damageOpponent()
    {

        if (opponent.guarded == true)
        {
            opponent.guarded = false;
        }
        else
        {
            opponent.RoboCurrentHealth -= player.RoboAttackStrength;
        }
    }

    public void guardRobot()
    {
        player.guarded = true;
    }

    public void healUser()
    {
        player.RoboCurrentHealth += player.RoboAttackStrength * 2;
    }
}
