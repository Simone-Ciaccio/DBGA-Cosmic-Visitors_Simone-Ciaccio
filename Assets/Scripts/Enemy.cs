using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IShooter
{
    public int Health { get; set; }
    public int Damage { get; set; }

    public List<EnemyScriptable> Enemies = new List<EnemyScriptable>();
    public GameObject bulletPrefab;
    public EnemyScriptable EnemyScriptable;

    private GameController gameController;
    private Camera cam;
    private Player player;

    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private float boundBottom;

    private ENEMY_MOVE_STATE moveState = 0;
    private float moveHorizontalAmount = 0.5f;
    private float moveVerticalAmount;
    private float moveTimer = 0.4f;
    private bool moveRight = true;

    private Vector2 halfSpriteSize;

    private int numberOfBullets;
    private int shotMinAngle, shotMaxAngle;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private float timeBetweenShots;
    private float shootTimer;
    private string enemyBulletTag = "EnemyBullet";

    private SpriteRenderer spriteRenderer;

    private enum ENEMY_MOVE_STATE
    {
        NO_MOVE = -1,
        MOVE_RIGHT = 0,
        MOVE_DOWN = 1,
        MOVE_LEFT = 2
    }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gameController = FindObjectOfType<GameController>();

        cam = Camera.main;

        boundRight = Helper.GetScreenBoundRight(cam);
        boundLeft = Helper.GetScreenBoundLeft(cam);
        boundTop = Helper.GetScreenBoundTop(cam);
        boundBottom = Helper.GetScreenBoundBottom(cam);

        spriteRenderer = GetComponent<SpriteRenderer>();
        halfSpriteSize = new Vector2(spriteRenderer.bounds.size.x / 2, spriteRenderer.bounds.size.y / 2);
        moveVerticalAmount = halfSpriteSize.y * 2;

        EnemyScriptable = Helper.RandomArrayValue(Enemies);
    }

    private void Start()
    {
        spriteRenderer.sprite = EnemyScriptable.EnemySprite;
        
        Health = EnemyScriptable.EnemyHealth;
        Damage = EnemyScriptable.EnemyDamage;
        numberOfBullets = EnemyScriptable.NumberOfBullets;
        shotMinAngle = EnemyScriptable.MinShotAngle;
        shotMaxAngle = EnemyScriptable.MaxShotAngle;
        timeBetweenShots = EnemyScriptable.TimeBetweenShots;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            Move();
        }

        if (shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            gameController.Enemies.Remove(gameObject);
            Destroy(gameObject);
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

            bullet.SetbulletData(bulletGO, EnemyScriptable.EnemyBulletSprite, Vector3.down, angleStep - (i * angleStep));
        }
    }

    private void Move()
    {
        moveTimer = 0.4f;

        if (transform.position.x >= boundRight - halfSpriteSize.x)
        {
            transform.position = new Vector3(boundRight - (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveState = ENEMY_MOVE_STATE.MOVE_DOWN;
            moveRight = false;
        }
        else if (transform.position.x <= boundLeft + halfSpriteSize.x)
        {
            transform.position = new Vector3(boundLeft + (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveState = ENEMY_MOVE_STATE.MOVE_DOWN;
            moveRight = true;
        }

        if (transform.position.y >= boundTop - halfSpriteSize.y)
        {
            transform.position = new Vector3(transform.position.x, halfSpriteSize.y, transform.position.z);
        }

        if (transform.position.y < boundBottom + halfSpriteSize.y)
        {
            player.Lives--;
        }

        switch (moveState)
        {
            case ENEMY_MOVE_STATE.NO_MOVE:
                break;

            case ENEMY_MOVE_STATE.MOVE_RIGHT:
                transform.Translate(new Vector3(moveHorizontalAmount, 0, 0));
                break;

            case ENEMY_MOVE_STATE.MOVE_LEFT:
                transform.Translate(new Vector3(-moveHorizontalAmount, 0, 0));
                break;

            case ENEMY_MOVE_STATE.MOVE_DOWN:
                transform.Translate(new Vector3(0, -moveVerticalAmount, 0));

                if (moveRight)
                {
                    moveState = ENEMY_MOVE_STATE.MOVE_RIGHT;
                }
                else if (!moveRight)
                {
                    moveState = ENEMY_MOVE_STATE.MOVE_LEFT;
                }

                break;
        }
    }
}