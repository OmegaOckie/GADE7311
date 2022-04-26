using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnController : MonoBehaviour
{
    //variables
    private int counter;
    private int playerCounter;

    public TextMeshProUGUI turnCounterTxt;

    // Start is called before the first frame update
    void Start()
    {
        counter = 1;
        playerCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncrementTurn()
    {
        if (playerCounter == 1)
        {
            playerCounter++;
        }
        else
        {
            counter++;
            turnCounterTxt.text = "Turn: " + counter;
            playerCounter = 1;
        }

    }
}
