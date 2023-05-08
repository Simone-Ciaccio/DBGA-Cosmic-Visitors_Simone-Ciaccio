using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IShooter
{
    public int Health 
    {
        get => EnemyScriptable.EnemyHealth;
        set => Health = EnemyScriptable.EnemyHealth;
    }
    public int Damage 
    {
        get => EnemyScriptable.EnemyDamage;
        set => Damage = EnemyScriptable.EnemyDamage;
    }
    public List<EnemyScriptable> Enemies = new List<EnemyScriptable>();
    public GameObject bulletPrefab;
    public EnemyScriptable EnemyScriptable;
    public bool HasCollider = false;
    public int ShotMinAngle, ShotMaxAngle;
    

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer bulletRenderer;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private float timeBetweenShots;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyScriptable = Helper.RandomArrayValue(Enemies);
        spriteRenderer.sprite = EnemyScriptable.EnemySprite;
        timeBetweenShots = EnemyScriptable.TimeBetweenShots;
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
        timer = timeBetweenShots;
        int numberOfBullets;
        if (EnemyScriptable.TripleShot)
        {
            numberOfBullets = 3;
            for (int i = 0; i < numberOfBullets; i++)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                InitBullet(bulletGO);
                float angleToShoot = ShotMinAngle - (i * ShotMinAngle);
                bullet.SetBulletDirection(Vector3.down, angleToShoot);
            }
        }
        else
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
            InitBullet(bulletGO);

            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.BulletDirection = Vector3.down;
        }
    }

    private void InitBullet(GameObject bulletGameObject)
    {
        bulletRenderer = bulletGameObject.GetComponent<SpriteRenderer>();
        bulletRenderer.sprite = EnemyScriptable.EnemyBulletSprite;

        Bullet enemyBullet = bulletGameObject.GetComponent<Bullet>();
        if (!enemyBullet.HasCollider)
        {
            bulletGameObject.AddComponent<PolygonCollider2D>();
            enemyBullet.HasCollider = true;
        }
        else
        {
            PolygonCollider2D enemyBulletCollider = GetComponent<PolygonCollider2D>();
            Destroy(enemyBulletCollider);
            bulletGameObject.AddComponent<PolygonCollider2D>();
            enemyBullet.HasCollider = true;
        }
    }
}
