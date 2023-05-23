using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable, IShooter
{
    public int Health { get; set; }
    public int Damage { get; set; }

    public float speed;

    private enum BOSS_MOVE_STATE
    {
        NO_MOVE = -1,
        MOVE_RIGHT = 0,
        MOVE_LEFT = 1
    }

    public GameObject BossPrefab;
    public BossScriptable BossScriptable;

    private BOSS_MOVE_STATE moveState = 0;

    private Camera cam;
    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private Vector2 halfSpriteSize;

    private float shootTimer;
    private GameObject bulletPrefab;
    private int numberOfBullets;
    private int shotMinAngle, shotMaxAngle;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private float timeBetweenShots;
    private string enemyBulletTag = "EnemyBullet";

    private void Awake()
    {
        cam = Camera.main;

        boundRight = Helper.GetScreenBoundRight(cam);
        boundLeft = Helper.GetScreenBoundLeft(cam);
        boundTop = Helper.GetScreenBoundTop(cam);

        Health = BossScriptable.BossHealth;
        Damage = BossScriptable.BossDamage;
    }

    private void Start()
    {
        UIManager.Instance.SetInititialBossHealth(BossScriptable.BossHealth);
        
        SpriteRenderer bossRenderer = BossPrefab.GetComponent<SpriteRenderer>();
        bossRenderer.sprite = BossScriptable.BossSprite;

        halfSpriteSize = new Vector2((bossRenderer.bounds.size.x / 2), (bossRenderer.bounds.size.y / 2));

        timeBetweenShots = BossScriptable.TimeBetweenShots;
        shotMinAngle = BossScriptable.MinShotAngle;
        shotMaxAngle = BossScriptable.MaxShotAngle;
        numberOfBullets = BossScriptable.NumberOfBullets;
        bulletPrefab = BossScriptable.BossBulletPrefab;
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;

        Move();

        if (shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        shootTimer = timeBetweenShots;

        float angleStep = (shotMaxAngle - shotMinAngle) / numberOfBullets;
        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
        
            bulletGO.tag = enemyBulletTag;

            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.SetbulletData(bulletGO, BossScriptable.BossBulletSprite, Vector3.down, angleStep - (i * angleStep));

            GameController.Instance.Bullets.Add(bulletGO);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        UIManager.Instance.UpdateBossHealth(Health);

        if (Health <= 0)
        {
            GameController.Instance.Enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (transform.position.x >= boundRight - halfSpriteSize.x)
        {
            transform.position = new Vector3(boundRight - (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveState = BOSS_MOVE_STATE.MOVE_LEFT;
        }
        else if (transform.position.x <= boundLeft + halfSpriteSize.x)
        {
            transform.position = new Vector3(boundLeft + (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveState = BOSS_MOVE_STATE.MOVE_RIGHT;
        }

        if (transform.position.y >= boundTop - halfSpriteSize.y)
        {
            transform.position = new Vector3(transform.position.x, boundTop - halfSpriteSize.y, transform.position.z);
        }

        switch (moveState)
        {
            case BOSS_MOVE_STATE.NO_MOVE:
                break;

            case BOSS_MOVE_STATE.MOVE_RIGHT:
                transform.position += speed * Time.deltaTime * Vector3.right;
                break;

            case BOSS_MOVE_STATE.MOVE_LEFT:
                transform.position += speed * Time.deltaTime * Vector3.left;
                break;
        }
    }
}
