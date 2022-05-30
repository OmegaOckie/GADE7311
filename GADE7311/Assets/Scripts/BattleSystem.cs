using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Robot playerRobot;
    Robot enemyRobot;

    public BattleState state;

    public TMP_Text dialogueText;

    public BattleHUDScript playerHUD;
    public BattleHUDScript enemyHUD;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGameObject = Instantiate(playerPrefab, playerBattleStation);
        playerRobot = playerGameObject.GetComponent<Robot>();

        GameObject enemyGameObject = Instantiate(enemyPrefab, enemyBattleStation);
        enemyRobot = enemyGameObject.GetComponent<Robot>();

        dialogueText.text = "The players have spawned.";

        playerHUD.SetHUD(playerRobot);
        enemyHUD.SetHUD(enemyRobot);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyRobot.TakeDamage(playerRobot.robotDamage);

        enemyHUD.SetHP(enemyRobot.robotCurrentHealth);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerRobot.Heal(5);

        playerHUD.SetHP(playerRobot.robotCurrentHealth);
        dialogueText.text = "You feel reinvigorated!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyRobot.robotName + "'s turn!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerRobot.TakeDamage(enemyRobot.robotDamage);

        playerHUD.SetHP(playerRobot.robotCurrentHealth);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";

        //state = BattleState.PLAYERTURN;
    }

    public void AttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
            StartCoroutine(PlayerAttack());
        
    }

    public void HealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());

    }
}
