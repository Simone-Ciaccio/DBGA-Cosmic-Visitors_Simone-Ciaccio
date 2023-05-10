using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IShooter
{
    public int PlayerHealth;
    public int PlayerDamage;

    public int Health { get; set; }
    public int Damage { get; set; }

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
        Debug.Log("Player took this amount of damage: " + damage);

        if (Health <= 0)
        {
            Debug.Log("Player should die!");
            //Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        shootTimer = TimeBetweenBullets;
        GameObject playerBullet = Instantiate(BulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
        playerBullet.tag = playerBulletTag;
        Bullet bullet = playerBullet.GetComponent<Bullet>();
        bullet.BulletDirection = Vector3.up;

        if (!bullet.HasCollider)
        {
            playerBullet.AddComponent<PolygonCollider2D>();
            bullet.HasCollider = true;
        }
        else
        {
            PolygonCollider2D playerBulletCollider = GetComponent<PolygonCollider2D>();
            Destroy(playerBulletCollider);
            playerBullet.AddComponent<PolygonCollider2D>();
            bullet.HasCollider = true;
        }
    }
}
