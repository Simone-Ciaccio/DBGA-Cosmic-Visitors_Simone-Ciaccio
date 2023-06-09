using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 BulletDirection;
    public Sprite BulletSprite;
    public float BulletSpeed;
    public float BulletDestroyTimer;

    private int bulletDamage;
    private float bulletDestroyTimer;
    private string enemyTag = "Enemy";
    private string enemyBulletTag = "EnemyBullet";
    private string bossTag = "Boss";
    private string playerTag = "Player";
    private string playerBulletTag = "PlayerBullet";

    private void Awake()
    {
        bulletDestroyTimer = BulletDestroyTimer;
    }

    private void Start()
    {
        EventManager.Instance.OnBulletSpawnConstructor += SetbulletData;
        EventManager.Instance.OnBulletSpawnInt += SetBulletDamage;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBulletSpawnConstructor -= SetbulletData;
        EventManager.Instance.OnBulletSpawnInt -= SetBulletDamage;
    }

    private void Update()
    {
        bulletDestroyTimer -= Time.deltaTime;

        transform.Translate(BulletSpeed * Time.deltaTime * BulletDirection.normalized);

        if (bulletDestroyTimer <= 0)
        {
            EventManager.Instance.StartBulletDestroyedEvent(gameObject);
            Destroy(gameObject);
            bulletDestroyTimer = BulletDestroyTimer;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(BulletDirection.x, BulletDirection.y), 0.1f);
        foreach (RaycastHit2D hit in hits)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), hit.collider, true);

            if (hit.collider.gameObject.CompareTag(playerTag) && this.CompareTag(enemyBulletTag))
            {
                EventManager.Instance.StartPlayerDamageEvent(hit.collider.gameObject, bulletDamage);

                EventManager.Instance.StartBulletDestroyedEvent(gameObject);
                Destroy(gameObject);
            }

            if (hit.collider.gameObject.CompareTag(enemyTag) && this.CompareTag(playerBulletTag))
            {
                EventManager.Instance.StartEnemyDamageEvent(hit.collider.gameObject, bulletDamage);

                EventManager.Instance.StartBulletDestroyedEvent(gameObject);
                Destroy(gameObject);
            }

            if(hit.collider.gameObject.CompareTag(bossTag) && this.CompareTag(playerBulletTag))
            {
                EventManager.Instance.StartEnemyDamageEvent(hit.collider.gameObject, bulletDamage);

                EventManager.Instance.StartBulletDestroyedEvent(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void SetbulletData(GameObject bulletGO, Sprite bulletSprite, Vector3 generalDirection, float angleToShoot)
    {
        SpriteRenderer bulletRenderer = bulletGO.GetComponent<SpriteRenderer>();
        bulletRenderer.sprite = bulletSprite;

        Helper.UpdateColliderShapeToSprite(bulletGO, bulletSprite);

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.SetBulletDirection(generalDirection, angleToShoot);
    }

    private void SetBulletDirection(Vector3 generalDirection, float angle)
    {
        Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * generalDirection;
        BulletDirection = bulletDirection;
    }

    private void SetBulletDamage(int damageAmount)
    {
        bulletDamage = damageAmount;
    }
}
