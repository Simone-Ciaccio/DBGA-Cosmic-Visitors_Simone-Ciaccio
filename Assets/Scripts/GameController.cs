using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public Player Player;

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

    private int currentLevelNumber = 0;

    private bool isPlayingLevel = false;

    private void Start()
    {
        EventManager.Instance.OnBulletSpawnGO += AddBulletToList;
        EventManager.Instance.OnBulletDestroyed += RemoveBulletFromList;

        EventManager.Instance.OnEnemyDefeated += RemoveEnemyFromList;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBulletSpawnGO -= AddBulletToList;
        EventManager.Instance.OnBulletDestroyed -= RemoveBulletFromList;

        EventManager.Instance.OnEnemyDefeated -= RemoveEnemyFromList;
    }

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

    private void InitGameData()
    {
        currentLevelNumber = 0;

        EventManager.Instance.StartGameReInitEvent();
        //Player.Lives = 3;
        //Player.gameObject.transform.position = Player.playerStartingPosition;

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

        EventManager.Instance.StartInitPlayerLivesEvent(Player.Lives);
        EventManager.Instance.StartInitPlayerHealthEvent(Player.PlayerHealth);
    }

    public void StartGame()
    {
        GameState = GAME_STATE.PLAYING_STATE;
    }

    public void PauseGame()
    {
        GameState = GAME_STATE.PAUSE_STATE;
    }

    public void ResumeGame()
    {
        GameState = GAME_STATE.PLAYING_STATE;
    }

    public void ExitGame()
    {
        GameState = GAME_STATE.START_GAME_STATE;
    }

    public void TryAgain()
    {
        InitGameData();
        UIManager.Instance.GameOverPanel.SetActive(false);
        UIManager.Instance.InGamePanel.SetActive(true);
        GameState = GAME_STATE.PLAYING_STATE;
    }

    private void GameStart()
    {
        Time.timeScale = 0f;
        InitGameData();

        EventManager.Instance.StartGameStartEvent();
    }

    private void GamePlaying()
    {
        EventManager.Instance.StartGamePlayingEvent();

        Time.timeScale = 1f;

        if (Player.Lives <= 0)
        {
            GameState = GAME_STATE.GAME_OVER_STATE;
        }

        if (Enemies.Count <= 0)
        {
            isPlayingLevel = false;

            if (currentLevelNumber < MaxNumberOfLevels)
            {
                NextLevel();
            }
            else
            {
                GameState = GAME_STATE.GAME_OVER_STATE;
                return;
            }
        }
    }

    private void NextLevel()
    {
        if (isPlayingLevel == false)
        {
            currentLevelNumber++;
        }

        if (currentLevelNumber % 5 == 0)
        {
            EventManager.Instance.StartBossLevelEvent();
            isPlayingLevel = true;
        }
        else
        {
            LevelGenerator.NumOfEnemies = currentLevelNumber + LevelGenerator.NumOfEnemies;
            EventManager.Instance.StartNormalLevelEvent();
            isPlayingLevel = true;
        }
    }

    private void GamePaused()
    {
        Time.timeScale = 0f;

        EventManager.Instance.StartGamePauseEvent();
    }

    private void GameOver()
    {
        Time.timeScale = 0f;

        EventManager.Instance.StartGameOverEvent();

        if (Enemies.Count <= 0)
        {
            EventManager.Instance.StartGameOverWinEvent();
        }
        else
        {
            EventManager.Instance.StartGameOverLoseEvent();
        }
    }

    private void AddBulletToList(GameObject bulletGO)
    {
        Bullets.Add(bulletGO);
    }

    private void RemoveBulletFromList(GameObject bulletGO)
    {
        Bullets.Remove(bulletGO);
    }

    private void RemoveEnemyFromList(GameObject enemyGO)
    {
        Enemies.Remove(enemyGO);
    }
}
