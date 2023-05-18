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

    [SerializeField]private int currentLevelNumber = 1;

    private void Update()
    {
        if (Player.Lives <= 0)
        {
            GameOver();
        }

        //TODO Cimocs: for some reason after player beats the boss the levelNumber jumps straight to 10
    
        if (Enemies.Count <= 0)
        {
            if (currentLevelNumber <= MaxNumberOfLevels)
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
        ResetLevel();
    }

    //private void BossLevel()
    //{
    //    LevelGenerator.CreateBossLevel();
    //
    //    BossHealthBar.SetActive(true);
    //
    //    Slider bossHealthSlider = BossHealthBar.GetComponent<Slider>();
    //    if (bossHealthSlider.value <= 0)
    //    {
    //        BossHealthBar.SetActive(false);
    //        LevelGenerator.CurrentLevelNumber++;
    //    }
    //}

    private void ResetLevel()
    {
        if (currentLevelNumber % 5 == 0)
        {
            LevelGenerator.CreateBossLevel();
            BossHealthBar.SetActive(true);

            Slider bossHealthSlider = BossHealthBar.GetComponent<Slider>();
            if (bossHealthSlider.value <= 0)
            {
                BossHealthBar.SetActive(false);
            }
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
