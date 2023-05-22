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

    public enum GAME_STATE
    {
        START_GAME_STATE = 0,
        PLAYING_STATE = 1,
        PAUSE_STATE = 2,
        GAME_OVER_STATE = 3
    }

    private int currentLevelNumber = 0;

    public GAME_STATE GameState = 0;

    private void Update()
    {
        switch (GameState)
        {
            case GAME_STATE.START_GAME_STATE:
                Time.timeScale = 0f;
                currentLevelNumber = 0;

                if (Enemies.Count > 0)
                {
                    for (int i = Enemies.Count - 1; i >= 0; i--)
                    {
                        Destroy(Enemies[i]);
                    }

                    Enemies.Clear();
                }

                UIManager.Instance.GameStartPanel.SetActive(true);
                UIManager.Instance.InGamePanel.SetActive(false);
                UIManager.Instance.GamePausePanel.SetActive(false);
                UIManager.Instance.GameOverPanel.SetActive(false);
                break;

            case GAME_STATE.PLAYING_STATE:
                UIManager.Instance.GameStartPanel.SetActive(false);
                UIManager.Instance.GamePausePanel.SetActive(false);
                UIManager.Instance.InGamePanel.SetActive(true);

                if(Enemies.Count <= 0)
                {
                    //Cimocs TODO: when player pauses the game, exits and starts again the game should be able to restart from level 1,
                    //also, if the player dies or reaches the game over panel, the try again button doesn't work and the exit game button lets you get to the
                    //starting screen but if you start the game the game over panel appears back on the screen.
                }

                Time.timeScale = 1f;

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
                break;

            case GAME_STATE.PAUSE_STATE:
                Time.timeScale = 0f;

                UIManager.Instance.GamePausePanel.SetActive(true);
                UIManager.Instance.InGamePanel.SetActive(false);
                break;

            case GAME_STATE.GAME_OVER_STATE:
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
                break;
        }
    }

    //public void GameStart()
    //{
    //    Time.timeScale = 0f;
    //
    //    currentLevelNumber = 0;
    //
    //    UIManager.Instance.GameStartPanel.SetActive(true);
    //    UIManager.Instance.InGamePanel.SetActive(false);
    //    UIManager.Instance.GamePausePanel.SetActive(false);
    //    UIManager.Instance.GameOverPanel.SetActive(false);
    //}
    //
    //public void PauseGame()
    //{
    //    UIManager.Instance.GamePausePanel.SetActive(true);
    //    UIManager.Instance.InGamePanel.SetActive(false);
    //
    //    Time.timeScale = 0f;
    //}
    //
    //public void ResumeGame()
    //{
    //    Time.timeScale = 1f;
    //    UIManager.Instance.GamePausePanel.SetActive(false);
    //    UIManager.Instance.GameStartPanel.SetActive(false);
    //    UIManager.Instance.InGamePanel.SetActive(true);
    //
    //    GameState = GAME_STATE.PLAYING_STATE;
    //}
    //
    //public void GamePlaying()
    //{
    //    Time.timeScale = 1f;
    //
    //    if (Player.Lives <= 0)
    //    {
    //        GameState = GAME_STATE.GAME_OVER_STATE;
    //    }
    //
    //    if (Enemies.Count <= 0)
    //    {
    //        if (currentLevelNumber < MaxNumberOfLevels)
    //        {
    //            NextLevel();
    //        }
    //        else
    //            GameState = GAME_STATE.GAME_OVER_STATE;
    //    }
    //}
    //
    //public void GameOver()
    //{
    //    Time.timeScale = 0f;
    //
    //    UIManager.Instance.InGamePanel.SetActive(false);
    //    UIManager.Instance.GameOverPanel.SetActive(true);
    //
    //    if(Enemies.Count <= 0)
    //    {
    //        UIManager.Instance.GameOverWinText.SetActive(true);
    //        UIManager.Instance.GameOverLoseText.SetActive(false);
    //    }
    //    else
    //    {
    //        UIManager.Instance.GameOverWinText.SetActive(false);
    //        UIManager.Instance.GameOverLoseText.SetActive(true);
    //    }
    //}

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
}
