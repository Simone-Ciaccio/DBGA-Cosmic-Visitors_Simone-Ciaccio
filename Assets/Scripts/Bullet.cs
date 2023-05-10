using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 BulletDirection;
    public Sprite BulletSprite;
    public float BulletSpeed;
    public bool HasCollider = false;
    public float BulletDestroyTimer;

    private float bulletDestroyTimer;

    private void Awake()
    {
        bulletDestroyTimer = BulletDestroyTimer;
    }

    private void Update()
    {
        bulletDestroyTimer -= Time.deltaTime;

        transform.position += BulletDirection.normalized * BulletSpeed * Time.deltaTime;

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
