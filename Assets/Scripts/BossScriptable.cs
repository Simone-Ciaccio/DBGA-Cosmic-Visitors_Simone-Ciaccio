using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Enemies/Boss")]
public class BossScriptable : EnemyScriptable
{

    public new int EnemyHealth;
    public new int EnemyDamage;
    public new Sprite EnemySprite;
    public new GameObject EnemyBulletPrefab;
    public new Sprite EnemyBulletSprite;
    public new float TimeBetweenShots;
    public new int NumberOfBullets;
    public new int MinShotAngle;
    public new int MaxShotAngle;
}
