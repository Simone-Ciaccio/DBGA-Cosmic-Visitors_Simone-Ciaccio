using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoSingleton<EventManager>
{
    public delegate void OnGameEvent();
    public delegate void OnGameIntEvent(int value);
    public delegate void OnGameGameObjectEvent(GameObject GO);
    public delegate void OnGameDamageEvent(GameObject GO, int amount);
    public delegate void OnGameAudioEvent(AudioClip audioClip);

    public delegate void OnBulletSpawnEvent(GameObject GO, Sprite sprite, Vector3 generalDirection, float angleToShoot);

    public OnGameEvent OnGameStart;
    public OnGameEvent OnGamePlaying;
    public OnGameEvent OnNormalLevelStart;
    public OnGameEvent OnBossLevelStart;
    public OnGameEvent OnGamePause;
    public OnGameEvent OnGameOver;
    public OnGameEvent OnGameOverWin;
    public OnGameEvent OnGameOverLose;
    public OnGameEvent OnGameReInit;

    public OnGameIntEvent OnInitPlayerHealth;
    public OnGameIntEvent OnInitPlayerLives;
    public OnGameIntEvent OnInitBossHealth;
    public OnGameIntEvent OnUpdatePlayerLives;
    public OnGameIntEvent OnUpdatePlayerHealth;
    public OnGameIntEvent OnUpdateBossHealth;
    public OnGameIntEvent OnBulletSpawnInt;

    public OnGameGameObjectEvent OnBulletSpawnGO;
    public OnGameGameObjectEvent OnBulletDestroyed;
    public OnGameGameObjectEvent OnEnemySpawnedGO;
    public OnGameGameObjectEvent OnEnemyDefeated;

    public OnGameDamageEvent OnEnemyDamage;
    public OnGameDamageEvent OnPlayerDamage;

    public OnGameAudioEvent OnBulletSpawnAudio;
    public OnGameAudioEvent OnEnemyDefeatedAudio;
    public OnGameAudioEvent OnBossDefeatedAudio;
    public OnGameAudioEvent OnPlayerDefeatedAudio;

    public OnBulletSpawnEvent OnBulletSpawnConstructor;

    public void StartGameStartEvent()
    {
        OnGameStart?.Invoke();
    }

    public void StartGamePlayingEvent()
    {
        OnGamePlaying?.Invoke();
    }

    public void StartNormalLevelEvent()
    {
        OnNormalLevelStart?.Invoke();
    }

    public void StartBossLevelEvent()
    {
        OnBossLevelStart?.Invoke();
    }

    public void StartGamePauseEvent()
    {
        OnGamePause?.Invoke();
    }

    public void StartGameOverEvent()
    {
        OnGameOver?.Invoke();
    }

    public void StartGameOverWinEvent()
    {
        OnGameOverWin?.Invoke();
    }

    public void StartGameOverLoseEvent()
    {
        OnGameOverLose?.Invoke();
    }

    public void StartGameReInitEvent()
    {
        OnGameReInit?.Invoke();
    }


    public void StartBulletSpawnGOEvent(GameObject GO)
    {
        OnBulletSpawnGO?.Invoke(GO);
    }

    public void StartEnemySpawnGOEvent(GameObject GO)
    {
        OnEnemySpawnedGO?.Invoke(GO);
    }

    public void StartEnemyDefeatEvent(GameObject GO)
    {
        OnEnemyDefeated?.Invoke(GO);
    }

    public void StartBulletDestroyedEvent(GameObject GO)
    {
        OnBulletDestroyed?.Invoke(GO);
    }
    public void StartUpdatePlayerLivesEvent(int value)
    {
        OnUpdatePlayerLives?.Invoke(value);
    }

    public void StartUpdatePlayerHealthEvent(int value)
    {
        OnUpdatePlayerHealth?.Invoke(value);
    }

    public void StartUpdateBossHealthIntEvent(int value)
    {
        OnUpdateBossHealth?.Invoke(value);
    }

    public void StartBulletSpawnIntEvent(int value)
    {
        OnBulletSpawnInt?.Invoke(value);
    }

    public void StartInitPlayerHealthEvent(int value)
    {
        OnInitPlayerHealth?.Invoke(value);
    }

    public void StartInitPlayerLivesEvent(int value)
    {
        OnInitPlayerLives?.Invoke(value);
    }

    public void StartInitBossHealthIntEvent(int value)
    {
        OnInitBossHealth?.Invoke(value);
    }

    public void StartEnemyDamageEvent(GameObject GO, int amount)
    {
        OnEnemyDamage?.Invoke(GO, amount);
    }

    public void StartPlayerDamageEvent(GameObject GO, int amount)
    {
        OnPlayerDamage?.Invoke(GO, amount);
    }

    public void StartBulletSpawnAudioEvent(AudioClip audioClip)
    {
        OnBulletSpawnAudio?.Invoke(audioClip);
    }

    public void StartEnemyDefeatedAudioEvent(AudioClip audioClip)
    {
        OnEnemyDefeatedAudio?.Invoke(audioClip);
    }

    public void StartBossDefeatedAudioEvent(AudioClip audioClip)
    {
        OnBossDefeatedAudio?.Invoke(audioClip);
    }

    public void StartPlayerDefeatedAudioEvent(AudioClip audioClip)
    {
        OnPlayerDefeatedAudio?.Invoke(audioClip);
    }

    public void StartBulletSpawnConstructorEvent(GameObject GO, Sprite sprite, Vector3 generalDirection, float angleToShoot)
    {
        OnBulletSpawnConstructor?.Invoke(GO, sprite, generalDirection, angleToShoot);
    }
}
