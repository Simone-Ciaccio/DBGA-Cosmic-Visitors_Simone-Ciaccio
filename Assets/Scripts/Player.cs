using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IShooter
{
    public int Lives;

    public int PlayerHealth;
    public int PlayerDamage;

    public AudioClip ShotAudio;
    public AudioClip DestroySound;

    public int Health { get; set; }
    public int Damage { get; set; }

    private Vector3 playerStartingPosition;

    public GameObject BulletPrefab;
    public Sprite BulletSprite;
    public float TimeBetweenBullets;

    private float shootTimer;
    private string playerBulletTag = "PlayerBullet";
    private SpriteRenderer bulletRenderer;
    private Vector3 bulletSpawnOffset = Vector3.up;

    private void Awake()
    {
        Health = PlayerHealth;
        Damage = PlayerDamage;

        EventManager.Instance.StartInitPlayerHealthEvent(Health);
        EventManager.Instance.StartInitPlayerLivesEvent(Lives);

        playerStartingPosition = transform.position;

        bulletRenderer = BulletPrefab.GetComponent<SpriteRenderer>();
        bulletRenderer.sprite = BulletSprite;
    }

    private void Start()
    {
        EventManager.Instance.OnGameReInit += ResetPlayerData;
        EventManager.Instance.OnPlayerDamage += TakeDamage;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGameReInit -= ResetPlayerData;
        EventManager.Instance.OnPlayerDamage -= TakeDamage;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void TakeDamage(GameObject damagedObject, int damage)
    {
        if (gameObject == damagedObject)
        {
            Health -= damage;

            EventManager.Instance.StartUpdatePlayerHealthEvent(Health);

            if (Health <= 0)
            {
                EventManager.Instance.StartPlayerDefeatedAudioEvent(DestroySound);

                Health = PlayerHealth;
                EventManager.Instance.StartUpdatePlayerHealthEvent(Health);

                Lives--;
                EventManager.Instance.StartUpdatePlayerLivesEvent(Lives);

                transform.position = playerStartingPosition;

                if (Lives <= 0)
                    GameController.Instance.GameState = GameController.GAME_STATE.GAME_OVER_STATE;
            }
        }
    }

    public void Shoot()
    {
        shootTimer = TimeBetweenBullets;
        GameObject playerBullet = Instantiate(BulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
        Helper.UpdateColliderShapeToSprite(playerBullet, BulletSprite);
        playerBullet.tag = playerBulletTag;
        Bullet bullet = playerBullet.GetComponent<Bullet>();
        bullet.BulletDirection = Vector3.up;

        EventManager.Instance.StartBulletSpawnIntEvent(Damage);
        EventManager.Instance.StartBulletSpawnGOEvent(playerBullet);
        EventManager.Instance.StartBulletSpawnAudioEvent(ShotAudio);
    }

    private void ResetPlayerData()
    {
        Lives = 3;
        transform.position = playerStartingPosition;
    }
}
