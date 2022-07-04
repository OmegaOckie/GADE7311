using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

/// <summary>
/// Lists the different states of the game for easier work flow
/// </summary>
public enum BattleState
{
    START,
    PLAYER1TURN,
    PLAYER2TURN,
    ENEMYTURN,
    WON,
    PLAYER1WON,
    PLAYER2WON,
    LOST
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
    int playerTurnCounter = 1;

    public BattleHUDScript playerHUD;
    //public BattleHUDScript player2HUD;
    public BattleHUDScript enemyHUD;

    public bool multiplayer = false;

    public int difficultyScaling;

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

            switch (difficultyScaling)
            {
                //Easy
                case 1:
                    difficultyScaling = 1;
                    break;

                //Medium
                case 2:
                    difficultyScaling = 2;
                    break;

                //Hard
                case 3:
                    difficultyScaling = 3;
                    break;
            }
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
            enemyHUD.SetHUD(player2Robot);
        }
        else
        {
            enemyHUD.SetHUD(enemyRobot);
        }

        //Gives players a chance to read information
        yield return new WaitForSeconds(2f);

        //Rotates game state to player's turn
        state = BattleState.PLAYER1TURN;
        PlayerTurn();
    }

    /// <summary>
    /// Method runs through and simulates attacking the other player.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerAttack()
    {
        bool isDead = false;

        if (!multiplayer)
        {
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
        else
        {
            switch (playerTurnCounter)
            {
                case 1:
                    if (!player2Robot.isGuarded)
                    {
                        //Damages the enemy robot and checks if the attack killed it.
                        isDead = player2Robot.TakeDamage(player2Robot.robotDamage);

                        //Displays updated enemy HP and says you attacked.
                        enemyHUD.SetHP(player2Robot.robotCurrentHealth);
                        dialogueText.text = "The attack is successful!";
                    }
                    else
                    {
                        //Displays that the attack was unsuccessful and that the guard has been lowered
                        dialogueText.text = "The attack is unsuccessful!";
                        player2Robot.isGuarded = false;

                        yield return new WaitForSeconds(2f);

                        dialogueText.text = player2Robot.robotName + "'s guard has been lowered!";
                    }

                    yield return new WaitForSeconds(2f);

                    //Checks if enemy died
                    if (isDead)
                    {
                        //If enemy died changes the state of the game to won and thus ending the game.
                        state = BattleState.PLAYER1WON;
                        EndBattle();
                    }
                    else
                    {
                        //If enemy lives then continue the game and rotates the game state to enemy.
                        state = BattleState.PLAYER2TURN;
                        playerTurnCounter++;
                        PlayerTurn();
                    }
                    break;

                case 2:
                    if (!playerRobot.isGuarded)
                    {
                        //Damages the enemy robot and checks if the attack killed it.
                        isDead = playerRobot.TakeDamage(playerRobot.robotDamage);

                        //Displays updated enemy HP and says you attacked.
                        playerHUD.SetHP(playerRobot.robotCurrentHealth);
                        dialogueText.text = "The attack is successful!";
                    }
                    else
                    {
                        //Displays that the attack was unsuccessful and that the guard has been lowered
                        dialogueText.text = "The attack is unsuccessful!";
                        playerRobot.isGuarded = false;

                        yield return new WaitForSeconds(2f);

                        dialogueText.text = playerRobot.robotName + "'s guard has been lowered!";
                    }

                    yield return new WaitForSeconds(2f);

                    //Checks if enemy died
                    if (isDead)
                    {
                        //If enemy died changes the state of the game to won and thus ending the game.
                        state = BattleState.PLAYER2WON;
                        EndBattle();
                    }
                    else
                    {
                        //If enemy lives then continue the game and rotates the game state to enemy.
                        state = BattleState.PLAYER1TURN;
                        if (playerTurnCounter == 1)
                        {
                            playerTurnCounter++;
                        }
                        else
                        {
                            playerTurnCounter = 1;
                        }
                        PlayerTurn();
                    }
                    break;
            }
        }

    }

    /// <summary>
    /// Heals the player
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerHeal()
    {
        if (!multiplayer)
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
        else
        {
            switch (playerTurnCounter)
            {
                case 1:
                    //heals player by designated amount
                    playerRobot.Heal(5);

                    //Displays player current HP
                    playerHUD.SetHP(playerRobot.robotCurrentHealth);
                    dialogueText.text = "You feel reinvigorated!";

                    yield return new WaitForSeconds(2f);

                    //Rotates game state to the enemy's turn
                    state = BattleState.PLAYER2TURN;
                    if (playerTurnCounter == 1)
                    {
                        playerTurnCounter++;
                    }
                    else
                    {
                        playerTurnCounter = 1;
                    }
                    PlayerTurn();
                    break;
                case 2:
                    //heals player by designated amount
                    player2Robot.Heal(5);

                    //Displays player current HP
                    enemyHUD.SetHP(player2Robot.robotCurrentHealth);
                    dialogueText.text = "You feel reinvigorated!";

                    yield return new WaitForSeconds(2f);

                    //Rotates game state to the enemy's turn
                    state = BattleState.PLAYER1TURN;
                    state = BattleState.PLAYER2TURN;
                    if (playerTurnCounter == 1)
                    {
                        playerTurnCounter++;
                    }
                    else
                    {
                        playerTurnCounter = 1;
                    }
                    PlayerTurn();
                    break;
            }
        }
    }

    /// <summary>
    /// Activates the guard for the player
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerGuard()
    {
        if (!multiplayer)
        {
            //Makes the player guarded
            playerRobot.Guard();

            //Displays that the player haschosen to guard themselves
            dialogueText.text = "You guard yourself against attacks.";

            yield return new WaitForSeconds(2f);

            //Rotates the game state
            state = BattleState.ENEMYTURN;
        }
        else
        {
            switch (playerTurnCounter)
            {
                case 1:
                    //Makes the player guarded
                    playerRobot.Guard();

                    //Displays that the player haschosen to guard themselves
                    dialogueText.text = "You guard yourself against attacks.";

                    yield return new WaitForSeconds(2f);

                    //Rotates the game state
                    state = BattleState.PLAYER2TURN;
                    if (playerTurnCounter == 1)
                    {
                        playerTurnCounter++;
                    }
                    else
                    {
                        playerTurnCounter = 1;
                    }
                    PlayerTurn();
                    break;
                case 2:
                    //Makes the player guarded
                    player2Robot.Guard();

                    //Displays that the player haschosen to guard themselves
                    dialogueText.text = "You guard yourself against attacks.";

                    yield return new WaitForSeconds(2f);

                    //Rotates the game state
                    state = BattleState.PLAYER1TURN;
                    if (playerTurnCounter == 1)
                    {
                        playerTurnCounter++;
                    }
                    else
                    {
                        playerTurnCounter = 1;
                    }
                    PlayerTurn();
                    break;
            }
        }

    }

    //Signifies that the game has ended
    void EndBattle()
    {
        if (!multiplayer)
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
        else
        {
            //Displays that the player has won
            if (state == BattleState.PLAYER1WON)
            {
                dialogueText.text = playerRobot + " won the battle!";
            }
            //Displays that the player has lost
            else if (state == BattleState.PLAYER2WON)
            {
                dialogueText.text = player2Robot + " won the battle!";
            }
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
            if (playerRobot.robotCurrentHealth <= 10*difficultyScaling)
            {
                //Enemy is able to attack since player is not guarded
                isDead = playerRobot.TakeDamage(enemyRobot.robotDamage * difficultyScaling);
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
                isDead = playerRobot.TakeDamage(enemyRobot.robotDamage * difficultyScaling);
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
                enemyRobot.Heal(5 * difficultyScaling);
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
                            enemyRobot.Heal(5 * difficultyScaling);
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
    /// <summary>
    /// Differentiates between Battle states
    /// </summary>
    void PlayerTurn()
    {
        switch (playerTurnCounter)
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
    /// <summary>
    /// When the player presses the Attack button it will run through this code that goes to the Attack method
    /// </summary>
    public void AttackButton()
    {
        if (state != BattleState.PLAYER1TURN && state != BattleState.PLAYER2TURN)
            return;
            StartCoroutine(PlayerAttack());
        
    }
    /// <summary>
    /// When the player presses the Heal button it will run through this code that goes to the Heal method
    /// </summary>
    public void HealButton()
    {
        if (state != BattleState.PLAYER1TURN && state != BattleState.PLAYER2TURN)
            return;
        StartCoroutine(PlayerHeal());

    }
    /// <summary>
    /// When the player presses the Guard button it will run through this code that goes to the Guard method
    /// </summary>
    public void GuardButton()
    {
        if (state != BattleState.PLAYER1TURN && state != BattleState.PLAYER2TURN)
            return;
        StartCoroutine(PlayerGuard());
    }

}
