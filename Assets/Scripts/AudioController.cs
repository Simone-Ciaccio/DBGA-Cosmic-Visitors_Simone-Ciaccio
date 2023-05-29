using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource[] audioSources;

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    private void Start()
    {
        EventManager.Instance.OnBulletSpawnAudio += PlayClip;
        EventManager.Instance.OnEnemyDefeatedAudio += PlayClip;
        EventManager.Instance.OnBossDefeatedAudio += PlayClip;
        EventManager.Instance.OnPlayerDefeatedAudio += PlayClip;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBulletSpawnAudio -= PlayClip;
        EventManager.Instance.OnEnemyDefeatedAudio -= PlayClip;
        EventManager.Instance.OnBossDefeatedAudio -= PlayClip;
        EventManager.Instance.OnPlayerDefeatedAudio -= PlayClip;
    }

    private void PlayClip (AudioClip audioClip)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].clip = audioClip;
                audioSources[i].Play();
                break;
            }
        }
    }
}
