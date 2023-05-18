using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;

    [SerializeField] private Slider bossHealthBar;

    public void SetInititialPlayerHealth(int maxPlayerHealth)
    {
        playerHealthBar.maxValue = maxPlayerHealth;
        playerHealthBar.value = maxPlayerHealth;
    }

    public void UpdatePlayerHealth(int currentHealth)
    {
        playerHealthBar.value = currentHealth;
    }

    public void SetInititialBossHealth(int maxBossHealth)
    {
        bossHealthBar.maxValue = maxBossHealth;
        bossHealthBar.value = maxBossHealth;
    }

    public void UpdateBossHealth(int currentHealth)
    {
        bossHealthBar.value = currentHealth;
    }
}
