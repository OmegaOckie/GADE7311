using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    BattleSystem BattleSystem = new BattleSystem();
    public void GamePlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GamePlay2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToEasy()
    {
        BattleSystem.difficultyScaling = 1;
    }
    public void SetToMedium()
    {
        BattleSystem.difficultyScaling = 2;
    }
    public void SetToHard()
    {
        BattleSystem.difficultyScaling = 3;
    }
}
