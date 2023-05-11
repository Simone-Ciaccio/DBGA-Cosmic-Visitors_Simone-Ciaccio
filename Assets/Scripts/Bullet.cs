using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 BulletDirection;
    public Sprite BulletSprite;
    public float BulletSpeed;
    public float BulletDestroyTimer;

    private float bulletDestroyTimer;
    private string enemyTag = "Enemy";
    private string playerTag = "Player";

    private void Awake()
    {
        bulletDestroyTimer = BulletDestroyTimer;
    }

    private void Update()
    {
        bulletDestroyTimer -= Time.deltaTime;

        transform.position += BulletDirection.normalized * BulletSpeed * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(BulletDirection.x, BulletDirection.y), 0.1f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag(playerTag))
            {
                Player player = hit.collider.GetComponent<Player>();
                int playerDamage = player.PlayerDamage;

                player.TakeDamage(playerDamage);

                Destroy(gameObject);
            }

            if (hit.collider.gameObject.CompareTag(enemyTag))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                int enemyDamage = enemy.EnemyScriptable.EnemyDamage;

                enemy.TakeDamage(enemyDamage);

                Destroy(gameObject);
            }
        }

        if (bulletDestroyTimer <= 0)
        {
            Destroy(gameObject);
            bulletDestroyTimer = BulletDestroyTimer;
        }
    }

    public void SetBulletDirection(Vector3 baseDirection, float angle)
    {
        Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * baseDirection;
        BulletDirection = bulletDirection;
    }
}
