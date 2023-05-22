using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject GameStartPanel;
    public GameObject InGamePanel;
    public GameObject GamePausePanel;
    public GameObject GameOverPanel;

    public Slider PlayerHealthBar;

    public List<GameObject> PlayerLives = new List<GameObject>();

    public Slider BossHealthBar;

    public GameObject GameOverWinText;
    public GameObject GameOverLoseText;

    public void StartGame()
    {
        GameController.Instance.GameState = GameController.GAME_STATE.PLAYING_STATE;
    }

    public void PauseGame()
    {
        GameController.Instance.GameState = GameController.GAME_STATE.PAUSE_STATE;
    }

    public void ResumeGame()
    {
        GameController.Instance.GameState = GameController.GAME_STATE.PLAYING_STATE;
    }

    public void ExitGame()
    {
        GameController.Instance.GameState = GameController.GAME_STATE.START_GAME_STATE;
    }

    public void SetInititialPlayerHealth(int maxPlayerHealth)
    {
        PlayerHealthBar.maxValue = maxPlayerHealth;
        PlayerHealthBar.value = maxPlayerHealth;
    }

    public void UpdatePlayerHealth(int currentHealth)
    {
        PlayerHealthBar.value = currentHealth;
    }

    public void SetInititialBossHealth(int maxBossHealth)
    {
        BossHealthBar.maxValue = maxBossHealth;
        BossHealthBar.value = maxBossHealth;
    }

    public void UpdateBossHealth(int currentHealth)
    {
        BossHealthBar.value = currentHealth;
    }

    public void UpdatePlayerLives(int playerLives)
    {
        for (int i = PlayerLives.Count - 1; i >= 0; i--)
        {
            PlayerLives[i].SetActive(playerLives > i);
        }
    }
}
