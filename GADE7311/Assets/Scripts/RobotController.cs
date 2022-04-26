using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    //variables
    public string RoboName = "Terminator";
    public int RoboMaxHealth = 100;
    public int RoboCurrentHealth = 100;
    public int RoboAttackStrength = 15;
    public bool guarded = false;

    public RobotController enemyRobot;
    public RobotController playerRobot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
