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

    private void Start()
    {
        EventManager.Instance.OnInitPlayerHealth += SetInitialPlayerHealth;
        EventManager.Instance.OnInitPlayerLives += UpdatePlayerLives;

        EventManager.Instance.OnUpdatePlayerLives += UpdatePlayerLives;
        EventManager.Instance.OnUpdatePlayerHealth += UpdatePlayerHealth;

        EventManager.Instance.OnInitBossHealth += SetInitialBossHealth;
        EventManager.Instance.OnUpdateBossHealth += UpdateBossHealth;

        EventManager.Instance.OnGameStart += SetInitialGameUI;
        EventManager.Instance.OnGamePlaying += SetGamePlayingUI;
        EventManager.Instance.OnGamePause += SetGamePauseUI;
        EventManager.Instance.OnNormalLevelStart += SetNormalLevelUI;
        EventManager.Instance.OnBossLevelStart += SetBossLevelUI;
        EventManager.Instance.OnGameOver += SetGameOverUI;
        EventManager.Instance.OnGameOverWin += SetGameOverWinText;
        EventManager.Instance.OnGameOverLose += SetGameOverLoseText;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnInitPlayerHealth -= SetInitialPlayerHealth;
        EventManager.Instance.OnInitPlayerLives -= UpdatePlayerLives;

        EventManager.Instance.OnUpdatePlayerLives -= UpdatePlayerLives;
        EventManager.Instance.OnUpdatePlayerHealth -= UpdatePlayerHealth;

        EventManager.Instance.OnInitBossHealth -= SetInitialBossHealth;
        EventManager.Instance.OnUpdateBossHealth -= UpdateBossHealth;

        EventManager.Instance.OnGameStart -= SetInitialGameUI;
        EventManager.Instance.OnGamePlaying -= SetGamePlayingUI;
        EventManager.Instance.OnGamePause -= SetGamePauseUI;
        EventManager.Instance.OnNormalLevelStart -= SetNormalLevelUI;
        EventManager.Instance.OnBossLevelStart -= SetBossLevelUI;
        EventManager.Instance.OnGameOver -= SetGameOverUI;
        EventManager.Instance.OnGameOverWin -= SetGameOverWinText;
        EventManager.Instance.OnGameOverLose -= SetGameOverLoseText;
    }

    private void SetInitialPlayerHealth(int maxPlayerHealth)
    {
        PlayerHealthBar.maxValue = maxPlayerHealth;
        PlayerHealthBar.value = maxPlayerHealth;
    }

    private void UpdatePlayerHealth(int currentHealth)
    {
        PlayerHealthBar.value = currentHealth;
    }

    private void SetInitialBossHealth(int maxBossHealth)
    {
        BossHealthBar.maxValue = maxBossHealth;
        BossHealthBar.value = maxBossHealth;
    }

    private void UpdateBossHealth(int currentHealth)
    {
        BossHealthBar.value = currentHealth;
    }

    private void UpdatePlayerLives(int playerLives)
    {
        for (int i = PlayerLives.Count - 1; i >= 0; i--)
        {
            PlayerLives[i].SetActive(playerLives > i);
        }
    }

    private void SetInitialGameUI()
    {
        GameStartPanel.SetActive(true);
        InGamePanel.SetActive(false);
        GamePausePanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    private void SetGamePlayingUI()
    {
        GameStartPanel.SetActive(false);
        GamePausePanel.SetActive(false);
        InGamePanel.SetActive(true);
    }

    private void SetGamePauseUI()
    {
        GamePausePanel.SetActive(true);
        InGamePanel.SetActive(false);
    }

    private void SetNormalLevelUI()
    {
        BossHealthBar.gameObject.SetActive(false);
    }

    private void SetBossLevelUI()
    {
        BossHealthBar.gameObject.SetActive(true);
    }

    private void SetGameOverUI()
    {
        InGamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    private void SetGameOverWinText()
    {
        GameOverWinText.SetActive(true);
        GameOverLoseText.SetActive(false);
    }

    private void SetGameOverLoseText()
    {
        GameOverWinText.SetActive(false);
        GameOverLoseText.SetActive(true);
    }
}
