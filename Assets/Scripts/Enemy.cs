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
    public bool HasCollider = false;

    private ENEMY_MOVE_STATE moveState = 0;
    private float moveHorizontalAmount = 0.3f;
    private float moveVerticalAmount = 0.3f;
    private float moveTimer = 0.7f;
    private bool moveRight = true;

    private int numberOfBullets;
    private int shotMinAngle, shotMaxAngle;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private float timeBetweenShots;
    private float shootTimer;
    private string enemyBulletTag = "EnemyBullet";

    private SpriteRenderer spriteRenderer;

    private enum ENEMY_MOVE_STATE
    {
        NONE = -1,
        MOVE_RIGHT = 0,
        MOVE_DOWN = 1,
        MOVE_LEFT = 2
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyScriptable = Helper.RandomArrayValue(Enemies);
        Health = EnemyScriptable.EnemyHealth;
        Damage = EnemyScriptable.EnemyDamage;
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
                transform.position += new Vector3(moveHorizontalAmount, 0, 0);
                break;

            case ENEMY_MOVE_STATE.MOVE_LEFT:
                transform.position += new Vector3(-moveHorizontalAmount, 0, 0);
                break;

            case ENEMY_MOVE_STATE.MOVE_DOWN:
                transform.position += new Vector3(0, -moveVerticalAmount, 0);

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

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log("Enemy took this amount of damage: " + damage);

        if (Health <= 0)
        {
            Debug.Log("Enemy should die!");
            //Destroy(gameObject);
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

            SpriteRenderer bulletRenderer = bulletGO.GetComponent<SpriteRenderer>();
            bulletRenderer.sprite = EnemyScriptable.EnemyBulletSprite;
            Sprite bulletSprite = bulletRenderer.sprite;
            Helper.UpdateColliderShapeToSprite(bulletGO, bulletSprite);
            
            float angleToShoot = angleStep - (i * angleStep);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.SetBulletDirection(Vector3.down, angleToShoot);
        }
    }
}