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

    private ENEMY_MOVE_STATE  moveState = 0;
    private float moveHorizontalAmount = 0.5f;
    private float moveVerticalAmount = 0.3f;
    private float moveTimer = 1.5f;
    private bool moveRight = true;

    private int numberOfBullets;
    private int shotMinAngle, shotMaxAngle;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private float timeBetweenShots;
    private float shootTimer;

    private enum ENEMY_MOVE_STATE
    {
        NONE = -1,
        MOVE_RIGHT = 0,
        MOVE_DOWN = 1,
        MOVE_LEFT = 2,
    }

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer bulletRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyScriptable = Helper.RandomArrayValue(Enemies);
        spriteRenderer.sprite = EnemyScriptable.EnemySprite;
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

    private void Move()
    {
        moveTimer = 0.5f;

        float boundRight = 8;
        float boundLeft = -8;

        if (transform.position.x >= boundRight)
        {
            transform.position = new Vector3(boundRight - 0.1f, transform.position.y, transform.position.z);
            moveState = ENEMY_MOVE_STATE.MOVE_DOWN;
            moveRight = false;
        }
        else if (transform.position.x <= boundLeft)
        {
            transform.position = new Vector3(boundLeft + 0.1f, transform.position.y, transform.position.z);
            moveState = ENEMY_MOVE_STATE.MOVE_DOWN;
            moveRight = true;
        }

        switch (moveState)
        {
            case ENEMY_MOVE_STATE.NONE:
                break;

            case ENEMY_MOVE_STATE.MOVE_RIGHT:
                transform.position += new Vector3 ( moveHorizontalAmount, 0, 0);
                break;

            case ENEMY_MOVE_STATE.MOVE_LEFT:
                transform.position += new Vector3 ( -moveHorizontalAmount, 0, 0);
                break;

            case ENEMY_MOVE_STATE.MOVE_DOWN:
                transform.position += new Vector3( 0, -moveVerticalAmount, 0);

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

    public void Shoot()
    {
        shootTimer = timeBetweenShots;

        float angleStep = (shotMaxAngle - shotMinAngle) / numberOfBullets;
        for(int i = 0; i < numberOfBullets; i++)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            SetBulletData(bulletGO);
            float angleToShoot = angleStep - (i * angleStep);
            bullet.SetBulletDirection(Vector3.down, angleToShoot);
        }
    }

    private void SetBulletData(GameObject bulletGameObject)
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
