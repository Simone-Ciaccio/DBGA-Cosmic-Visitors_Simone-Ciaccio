using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider PlayerHealthBar;

    public List<GameObject> PlayerLives = new List<GameObject>();

    public Slider BossHealthBar;

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
