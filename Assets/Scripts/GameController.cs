using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player Player;
    public GameObject Boss;
    public BossScriptable BossScriptable;

    public LevelGenerator LevelGenerator;

    public int MaxNumberOfLevels;

    public List<GameObject> Enemies = new List<GameObject>();

    private void Update()
    {
        if (Player.Health <= 0)
        {
            GameOver();
        }
    
        if (Enemies.Count <= 0)
        {
            if (LevelGenerator.CurrentLevelNumber <= MaxNumberOfLevels)
            {
                NextLevel();

                if (LevelGenerator.CurrentLevelNumber % 5 == 0)
                BossLevel();
            }
            else
                GameOver();
        }
    }

    private void NextLevel()
    {
        LevelGenerator.CurrentLevelNumber++;
        ResetLevel();
    }

    private void BossLevel()
    {
        //LevelGenerator.CurrentLevelNumber++;
        LevelGenerator.CreateBossLevel();
    }

    private void ResetLevel()
    {
        LevelGenerator.NumOfEnemies = LevelGenerator.CurrentLevelNumber + LevelGenerator.NumOfEnemies;
        LevelGenerator.CreateLevel();
    }

    private void GameOver()
    {
        Time.timeScale = 0;
    }
}
