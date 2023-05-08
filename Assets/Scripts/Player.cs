using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IShooter
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public GameObject BulletPrefab;
    public Sprite BulletSprite;
    public float TimeBetweenBullets;

    private SpriteRenderer bulletRenderer;
    private Vector3 bulletSpawnOffset = Vector3.up;
    private float timer;

    private void Awake()
    {
        bulletRenderer = BulletPrefab.GetComponent<SpriteRenderer>();
        bulletRenderer.sprite = BulletSprite;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        timer = TimeBetweenBullets;
        GameObject playerBullet = Instantiate(BulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
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
