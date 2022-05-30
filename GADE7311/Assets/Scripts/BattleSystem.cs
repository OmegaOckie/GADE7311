using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum BattleState
{
    START,
    PLAYER1TURN,
    PLAYER2TURN,
    ENEMYTURN,
    WON,
    LOST
}

public enum AiDifficulty
{

}

public class BattleSystem : MonoBehaviour
{
    //Declaring variables

    public GameObject playerPrefab;
    public GameObject player2Prefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform player2BattleStation;
    public Transform enemyBattleStation;

    Robot playerRobot;
    Robot player2Robot;
    Robot enemyRobot;

    public BattleState state;

    public TMP_Text dialogueText;
    public TMP_Text turnCounterText;

    int turnCounter = 1;

    public BattleHUDScript playerHUD;
    public BattleHUDScript player2HUD;
    public BattleHUDScript enemyHUD;

    public bool multiplayer;

    // Start is called before the first frame update
    void Start()
    {
        //Puts the game into its start state
        state = BattleState.START;
        
        //Sets up pre-battle data
        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// Instantiates robots and declares pre-battle data
    /// </summary>
    /// <returns></returns>
    IEnumerator SetupBattle()
    {

        //Instantiation
        GameObject playerGameObject = Instantiate(playerPrefab, playerBattleStation);
        playerRobot = playerGameObject.GetComponent<Robot>();

        //Switches between AI prefab or player 2 prefab if multiplayer is active
        if (multiplayer)
        {
            GameObject player2GameObject = Instantiate(player2Prefab, player2BattleStation);
            player2Robot = player2GameObject.GetComponent<Robot>();
        }
        else
        {
            GameObject enemyGameObject = Instantiate(enemyPrefab, enemyBattleStation);
            enemyRobot = enemyGameObject.GetComponent<Robot>();
        }

        //Displays visual information
        dialogueText.text = "The players have spawned.";
        turnCounterText.text = "Turn: " + turnCounter;

        playerHUD.SetHUD(playerRobot);

        //Switches between AI prefab or player 2 prefab if multiplayer is active
        if (multiplayer)
        {
            player2HUD.SetHUD(player2Robot);
        }
        else
        {
            enemyHUD.SetHUD(enemyRobot);
        }

        //Gives players a chance to read information
        yield return new WaitForSeconds(2f);

        //Rotates game state to player's turn
        state = BattleState.PLAYER1TURN;
        PlayerTurn(1);
    }

    /// <summary>
    /// Method runs through and simulates attacking the other player.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerAttack()
    {
        bool isDead = false;
        if (!enemyRobot.isGuarded)
        {
            //Damages the enemy robot and checks if the attack killed it.
            isDead = enemyRobot.TakeDamage(playerRobot.robotDamage);

            //Displays updated enemy HP and says you attacked.
            enemyHUD.SetHP(enemyRobot.robotCurrentHealth);
            dialogueText.text = "The attack is successful!";
        }
        else
        {
            //Displays that the attack was unsuccessful and that the guard has been lowered
            dialogueText.text = "The attack is unsuccessful!";
            enemyRobot.isGuarded = false;

            yield return new WaitForSeconds(2f);

            dialogueText.text = enemyRobot.robotName + "'s guard has been lowered!";
        }


        yield return new WaitForSeconds(2f);

        //Checks if enemy died
        if (isDead)
        {
            //If enemy died changes the state of the game to won and thus ending the game.
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //If enemy lives then continue the game and rotates the game state to enemy.
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    /// <summary>
    /// Heals the player
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerHeal()
    {

        //heals player by designated amount
        playerRobot.Heal(5);

        //Displays player current HP
        playerHUD.SetHP(playerRobot.robotCurrentHealth);
        dialogueText.text = "You feel reinvigorated!";

        yield return new WaitForSeconds(2f);

        //Rotates game state to the enemy's turn
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    /// <summary>
    /// Activates the guard for the player
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerGuard()
    {
        //Makes the player guarded
        playerRobot.Guard();

        //Displays that the player haschosen to guard themselves
        dialogueText.text = "You guard yourself against attacks.";

        yield return new WaitForSeconds(2f);

        //Rotates the game state
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    //Signifies that the game has ended
    void EndBattle()
    {
        //Displays that the player has won
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        //Displays that the player has lost
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }
    }

    /// <summary>
    /// Runs through the actions an enemy can take.
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyTurn()
    {
        bool isDead = false;
        dialogueText.text = enemyRobot.robotName + "'s turn!";
        
        yield return new WaitForSeconds(1f);

        int randomChance = new System.Random().Next(3);

        //Enemy checks if it can attack 
        if (!playerRobot.isGuarded)
        {
            //If player is one shot away from dying then attacking will be priotised
            if (playerRobot.robotCurrentHealth <= 10)
            {
                //Enemy is able to attack since player is not guarded
                isDead = playerRobot.TakeDamage(enemyRobot.robotDamage);
                dialogueText.text = enemyRobot.robotName + " decided to attack you!";
            }
            //Will cause the AI to block Randomly
            else if (randomChance == 1)
            {
                //Makes the AI guarded
                enemyRobot.Guard();

                //Displays that the player haschosen to guard themselves
                dialogueText.text = "AI guards itself against attacks.";
            }
            else
            {
                //Enemy is able to attack since player is not guarded
                isDead = playerRobot.TakeDamage(enemyRobot.robotDamage);
                dialogueText.text = enemyRobot.robotName + " decided to attack you!";
            }

        }
        //Player is guarded and thus enemy cannot attack directly
        else
        {
            
            dialogueText.text = playerRobot.robotName + " is guarded!";

            yield return new WaitForSeconds(2f);

            //AI decides to heal while below 50% HP
            if (enemyRobot.robotCurrentHealth < enemyRobot.robotMaxHealth)
            {
                enemyRobot.Heal(5);
                dialogueText.text = enemyRobot.robotName + " has decided to heal!";
            }
            //AI decides next action randomly
            else
            {

                switch (randomChance)
                {
                    case 0:
                        //Resets Guarded Status
                        dialogueText.text = enemyRobot.robotName + " decided to attack you to lower your guard!";

                        yield return new WaitForSeconds(2f);

                        playerRobot.isGuarded = false;
                        dialogueText.text = playerRobot.robotName + "'s defenses have been lowered!";

                        break;
                    case 1:

                        enemyRobot.Guard();
                        dialogueText.text = enemyRobot.robotName + " decided to guard itself!";

                        break;
                    case 2:
                         
                        if (enemyRobot.robotCurrentHealth !>= enemyRobot.robotMaxHealth)
                        {
                            enemyRobot.Heal(5);
                            dialogueText.text = enemyRobot.robotName + " has decided to heal!";

                        }
                        else
                        {
                            //Resets Guarded Status
                            dialogueText.text = enemyRobot.robotName + " decided to attack you to lower your guard!";

                            yield return new WaitForSeconds(2f);

                            playerRobot.isGuarded = false;
                            dialogueText.text = playerRobot.robotName + "'s defenses have been lowered!";
                        }

                        break;
                }
            }
        }
        //Update HP for both robots
        playerHUD.SetHP(playerRobot.robotCurrentHealth);
        enemyHUD.SetHP(enemyRobot.robotCurrentHealth);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYER1TURN;
            PlayerTurn();
        }

        turnCounter++;
        turnCounterText.text = "Turn: " + turnCounter;
    }

    void PlayerTurn(int player)
    {
        switch (player)
        {
            case 1:
                dialogueText.text = "Choose an action: " + playerRobot.robotName;

                state = BattleState.PLAYER1TURN;
                break;
            case 2:
                dialogueText.text = "Choose an action: " + player2Robot.robotName;

                state = BattleState.PLAYER2TURN;
                break;

        }

    }

    public void AttackButton()
    {
        if (state != BattleState.PLAYER1TURN || state != BattleState.PLAYER2TURN)
            return;
            StartCoroutine(PlayerAttack());
        
    }

    public void HealButton()
    {
        if (state != BattleState.PLAYER1TURN || state != BattleState.PLAYER2TURN)
            return;
        StartCoroutine(PlayerHeal());

    }

    public void GuardButton()
    {
        if (state != BattleState.PLAYER1TURN || state != BattleState.PLAYER2TURN)
            return;
        StartCoroutine(PlayerGuard());
    }
}
