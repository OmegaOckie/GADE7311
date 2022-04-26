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

    public Button player1AttackButton;
    public Button player1GuardButton;
    public Button player1HealButton;

    public Button player2AttackButton;
    public Button player2GuardButton;
    public Button player2HealButton;

    public GameObject player1;
    public GameObject player2;

    public bool hasMoved;

    // Start is called before the first frame update
    void Start()
    {
        counter = 1;
        playerCounter = 2;
        hasMoved = false;

        player2AttackButton.interactable = false;
        player2GuardButton.interactable = false;
        player2HealButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCounter == 2)
        {
            if (Input.GetKeyDown("w") && hasMoved == false)
            {
                player1.transform.Translate(-1, 0, 0);
                if (player1.transform.position == player2.transform.position)
                {
                    player1.transform.Translate(1, 0, 0);
                }
                hasMoved = true;
            }
            if (Input.GetKeyDown("a") && hasMoved == false)
            {
                player1.transform.Translate(0, 0, -1);
                if (player1.transform.position == player2.transform.position)
                {
                    player1.transform.Translate(0, 0, 1);
                }
                hasMoved = true;
            }
            if (Input.GetKeyDown("s") && hasMoved == false)
            {
                player1.transform.Translate(1, 0, 0);
                if (player1.transform.position == player2.transform.position)
                {
                    player1.transform.Translate(-1, 0, 0);
                }
                hasMoved = true;
            }
            if (Input.GetKeyDown("d") && hasMoved == false)
            {
                player1.transform.Translate(0, 0, 1);
                if (player1.transform.position == player2.transform.position)
                {
                    player1.transform.Translate(0, 0, -1);
                }
                hasMoved = true;
            }
        }
        else
        {
                if (Input.GetKeyDown("w") && hasMoved == false)
                {
                    player2.transform.Translate(-1, 0, 0);
                    if (player2.transform.position == player1.transform.position)
                    {
                        player2.transform.Translate(1, 0, 0);
                    }
                hasMoved = true;
            }
                if (Input.GetKeyDown("a") && hasMoved == false)
                {
                    player2.transform.Translate(0, 0, -1);
                    if (player2.transform.position == player1.transform.position)
                    {
                        player2.transform.Translate(0, 0, 1);
                    }
                hasMoved = true;
            }
                if (Input.GetKeyDown("s") && hasMoved == false)
                {
                    player2.transform.Translate(1, 0, 0);
                    if (player2.transform.position == player1.transform.position)
                    {
                        player2.transform.Translate(-1, 0, 0);
                    }
                hasMoved = true;
            }
                if (Input.GetKeyDown("d") && hasMoved == false)
                {
                    player2.transform.Translate(0, 0, 1);
                    if (player2.transform.position == player1.transform.position)
                    {
                        player2.transform.Translate(0, 0, -1);
                    }
                hasMoved = true;
            }
        }
    }
    public void IncrementTurn()
    {
        if (playerCounter == 1)
        {

            player1AttackButton.interactable = true;
            player1GuardButton.interactable = true;
            player1HealButton.interactable = true;

            player2AttackButton.interactable = false;
            player2GuardButton.interactable = false;
            player2HealButton.interactable = false;

            playerCounter++;
            hasMoved = false;
        }
        else
        {
            player1AttackButton.interactable = false;
            player1GuardButton.interactable = false;
            player1HealButton.interactable = false;

            player2AttackButton.interactable = true;
            player2GuardButton.interactable = true;
            player2HealButton.interactable = true;

            counter++;
            playerCounter = 1;
            turnCounterTxt.text = "Turn: " + counter;
            hasMoved = false;
        }

    }

    public void DisplayDeath(RobotController winner)
    {
        turnCounterTxt.text = winner.RoboName + " WINS";

        player1AttackButton.interactable = false;
        player1GuardButton.interactable = false;
        player1HealButton.interactable = false;

        player2AttackButton.interactable = false;
        player2GuardButton.interactable = false;
        player2HealButton.interactable = false;
    }
}
