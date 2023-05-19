using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Player Player;
    public GameObject Boss;
    public BossScriptable BossScriptable;

    public GameObject BossHealthBar;

    public LevelGenerator LevelGenerator;

    public int MaxNumberOfLevels;

    public List<GameObject> Enemies = new List<GameObject>();

    [SerializeField]private int currentLevelNumber = 0;

    private void Update()
    {
        if (Player.Lives <= 0)
        {
            GameOver();
        }
    
        if (Enemies.Count <= 0)
        {
            if (currentLevelNumber < MaxNumberOfLevels)
            {
                NextLevel();
            }
            else
                GameOver();
        }
    }

    private void NextLevel()
    {
        currentLevelNumber++;

        if (currentLevelNumber % 5 == 0)
        {
            BossHealthBar.SetActive(true);
            LevelGenerator.CreateBossLevel();
        }
        else
        {
            BossHealthBar.SetActive(false); 
            LevelGenerator.NumOfEnemies = currentLevelNumber + LevelGenerator.NumOfEnemies;
            LevelGenerator.CreateLevel();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
    }
}
