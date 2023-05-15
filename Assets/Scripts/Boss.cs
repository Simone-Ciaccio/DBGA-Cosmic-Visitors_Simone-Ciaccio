using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable, IShooter
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public float speed;
    //public float TimeBetweenShots;

    private enum BOSS_MOVE_STATE
    {
        NO_MOVE = -1,
        MOVE_RIGHT = 0,
        MOVE_LEFT = 1
    }

    public GameObject BossPrefab;
    public BossScriptable BossScriptable;

    private SpriteRenderer spriteRenderer;
    private GameController gameController;

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
        Vector2 ScreenTopRightInWorld = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 ScreenBottomLeftInWorld = cam.ScreenToWorldPoint(new Vector2(0, 0));
        boundRight = ScreenTopRightInWorld.x;
        boundLeft = ScreenBottomLeftInWorld.x;
        boundTop = ScreenTopRightInWorld.y;

        //BossScriptable = GetComponent<BossScriptable>();
        spriteRenderer = BossPrefab.GetComponent<SpriteRenderer>();
        halfSpriteSize = new Vector2((spriteRenderer.bounds.size.x / 2), (spriteRenderer.bounds.size.y / 2));

        Health = BossScriptable.EnemyHealth;
        Damage = BossScriptable.EnemyDamage;
        timeBetweenShots = BossScriptable.TimeBetweenShots;
        shotMinAngle = BossScriptable.MinShotAngle;
        shotMaxAngle = BossScriptable.MaxShotAngle;
        numberOfBullets = BossScriptable.NumberOfBullets;
        bulletPrefab = BossScriptable.EnemyBulletPrefab;

        spriteRenderer.sprite = BossScriptable.EnemySprite;

        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;

        Move();

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
            //Debug.Log("Enemy should die!");
            Destroy(gameObject);
            gameController.Enemies.Remove(gameObject);
        }
    }

    private void Move()
    {
        if (transform.position.x >= boundRight - halfSpriteSize.x)
        {
            transform.position = new Vector3(boundRight - (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveState = BOSS_MOVE_STATE.MOVE_LEFT;
            //moveRight = false;
        }
        else if (transform.position.x <= boundLeft + halfSpriteSize.x)
        {
            transform.position = new Vector3(boundLeft + (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveState = BOSS_MOVE_STATE.MOVE_RIGHT;
            //moveRight = true;
        }

        if (transform.position.y >= boundTop - halfSpriteSize.y)
        {
            transform.position = new Vector3(transform.position.x, boundTop - (halfSpriteSize.y * 2), transform.position.z);
        }

        switch (moveState)
        {
            case BOSS_MOVE_STATE.NO_MOVE:
                break;

            case BOSS_MOVE_STATE.MOVE_RIGHT:
                transform.position += Vector3.right * speed * Time.deltaTime;
                break;

            case BOSS_MOVE_STATE.MOVE_LEFT:
                transform.position += Vector3.left * speed * Time.deltaTime;
                break;
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
            bulletRenderer.sprite = BossScriptable.EnemyBulletSprite;
            Sprite bulletSprite = bulletRenderer.sprite;
            Helper.UpdateColliderShapeToSprite(bulletGO, bulletSprite);

            float angleToShoot = angleStep - (i * angleStep);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.SetBulletDirection(Vector3.down, angleToShoot);
        }
    }
}
