using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public Player Player;
    public GameObject Boss;
    public BossScriptable BossScriptable;

    public GameObject BossHealthBar;

    public LevelGenerator LevelGenerator;

    public int MaxNumberOfLevels;

    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> Bullets = new List<GameObject>();

    public enum GAME_STATE
    {
        START_GAME_STATE = 0,
        PLAYING_STATE = 1,
        PAUSE_STATE = 2,
        GAME_OVER_STATE = 3
    }
    public GAME_STATE GameState = 0;

    [SerializeField]private int currentLevelNumber = 0;

    private void Update()
    {
        switch (GameState)
        {
            case GAME_STATE.START_GAME_STATE:
                GameStart();
                break;

            case GAME_STATE.PLAYING_STATE:
                GamePlaying();
                break;

            case GAME_STATE.PAUSE_STATE:
                GamePaused();
                break;

            case GAME_STATE.GAME_OVER_STATE:
                GameOver();
                break;
        }
    }

    public void InitGameData()
    {
        currentLevelNumber = 0;

        Player.Lives = 3;
        Player.gameObject.transform.position = Player.PlayerStartingPosition;

        if (Enemies.Count > 0)
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(Enemies[i]);
                Enemies.RemoveAt(i);
            }
        }

        if (Bullets.Count > 0)
        {
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(Bullets[i]);
                Bullets.RemoveAt(i);
            }
        }

        LevelGenerator.NumOfEnemies = 5;

        UIManager.Instance.UpdatePlayerLives(Player.Lives);
        UIManager.Instance.UpdatePlayerHealth(Player.PlayerHealth);
    }

    private void GameStart()
    {
        Time.timeScale = 0f;
        InitGameData();

        UIManager.Instance.GameStartPanel.SetActive(true);
        UIManager.Instance.InGamePanel.SetActive(false);
        UIManager.Instance.GamePausePanel.SetActive(false);
        UIManager.Instance.GameOverPanel.SetActive(false);
    }

    private void GamePlaying()
    {
        //EventManager.Instance.GameStarted();

        UIManager.Instance.GameStartPanel.SetActive(false);
        UIManager.Instance.GamePausePanel.SetActive(false);
        UIManager.Instance.InGamePanel.SetActive(true);

        Time.timeScale = 1f;
        
        if (currentLevelNumber <= 0)
        {
            NextLevel();
        }

        if (Player.Lives <= 0)
        {
            GameState = GAME_STATE.GAME_OVER_STATE;
        }

        if (Enemies.Count <= 0)
        {
            if (currentLevelNumber < MaxNumberOfLevels)
            {
                NextLevel();
            }
            else
                GameState = GAME_STATE.GAME_OVER_STATE;
        }
    }

    private void GamePaused()
    {
        Time.timeScale = 0f;

        UIManager.Instance.GamePausePanel.SetActive(true);
        UIManager.Instance.InGamePanel.SetActive(false);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;

        UIManager.Instance.InGamePanel.SetActive(false);
        UIManager.Instance.GameOverPanel.SetActive(true);

        if (Enemies.Count <= 0)
        {
            UIManager.Instance.GameOverWinText.SetActive(true);
            UIManager.Instance.GameOverLoseText.SetActive(false);
        }
        else
        {
            UIManager.Instance.GameOverWinText.SetActive(false);
            UIManager.Instance.GameOverLoseText.SetActive(true);
        }
    }

    private void NextLevel()
    {
        currentLevelNumber++;

        if (currentLevelNumber % 5 == 0)
        {
            EventManager.Instance.StartBossLevelEvent();
            //BossHealthBar.SetActive(true);
            //LevelGenerator.CreateBossLevel();
        }
        else
        {
            LevelGenerator.NumOfEnemies = currentLevelNumber + LevelGenerator.NumOfEnemies;
            EventManager.Instance.StartNormalLevelEvent();
            //BossHealthBar.SetActive(false); 
            //LevelGenerator.CreateLevel();
        }
    }
}
