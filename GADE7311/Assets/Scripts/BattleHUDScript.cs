using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUDScript : MonoBehaviour
{

    public TMP_Text robotNametext;
    public Image healthBarImage;

    public void SetHUD(Robot robot)
    {
        robotNametext.text = robot.robotName;
        healthBarImage.fillAmount = robot.robotCurrentHealth / robot.robotMaxHealth; ;

    }

    public void SetHP(float hp)
    {
        healthBarImage.fillAmount = hp/100;
    }

}
