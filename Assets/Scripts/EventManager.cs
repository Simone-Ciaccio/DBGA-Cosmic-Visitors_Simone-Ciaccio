using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoSingleton<EventManager>
{
    public delegate void OnGameEvent();
    public delegate int OnGameIntEvent();

    public OnGameEvent OnGameStart;
    public OnGameEvent OnNormalLevelStart;
    public OnGameEvent OnBossLevelStart;

    public OnGameIntEvent OnLevelStartInt;

    public void GameStarted()
    {
        OnGameStart?.Invoke();
    }

    public void StartNormalLevelEvent()
    {
        OnNormalLevelStart?.Invoke();
    }

    public void StartBossLevelEvent()
    {
        OnBossLevelStart?.Invoke();
    }

    public void StartLevelIntEvent()
    {
        OnLevelStartInt?.Invoke();
    }
}
