using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScripts : MonoBehaviour
{
    //variables
    public RobotController player;
    public RobotController opponent;

    public TurnController turnController;

    private void Start()
    {

    }

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

        turnController.IncrementTurn();
    }

    public void guardRobot()
    {
        player.guarded = true;

        turnController.IncrementTurn();
    }

    public void healUser()
    {
        player.RoboCurrentHealth += player.RoboAttackStrength * 2;

        turnController.IncrementTurn();
    }

}
