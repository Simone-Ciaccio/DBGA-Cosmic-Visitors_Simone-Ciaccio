using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IShooter
{
    public int Lives;

    public int PlayerHealth;
    public int PlayerDamage;

    public int Health { get; set; }
    public int Damage { get; set; }

    public GameObject BulletPrefab;
    public Sprite BulletSprite;
    public float TimeBetweenBullets;

    [SerializeField] private UIManager uiManager;

    private float shootTimer;
    private string playerBulletTag = "PlayerBullet";
    private SpriteRenderer bulletRenderer;
    private Vector3 bulletSpawnOffset = Vector3.up;

    private string enemyTag = "Enemy";

    private Vector3 startingPosition; 

    private void Awake()
    {
        Health = PlayerHealth;
        Damage = PlayerDamage;
        uiManager.SetInititialPlayerHealth(PlayerHealth);


        startingPosition = transform.position;

        bulletRenderer = BulletPrefab.GetComponent<SpriteRenderer>();
        bulletRenderer.sprite = BulletSprite;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        uiManager.UpdatePlayerHealth(Health);

        if (Health <= 0)
        {
            Health = PlayerHealth;
            uiManager.UpdatePlayerHealth(PlayerHealth);
            Lives--;
            transform.position = startingPosition;

            if(Lives <= 0)
                Destroy(gameObject);
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Lives--;
        }
    }
}
