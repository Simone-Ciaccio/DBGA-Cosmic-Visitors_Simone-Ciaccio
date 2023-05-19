using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Enemies/Boss")]
public class BossScriptable : ScriptableObject
{
    public int BossHealth;
    public int BossDamage;
    public Sprite BossSprite;
    public GameObject BossBulletPrefab;
    public Sprite BossBulletSprite;
    public float TimeBetweenShots;
    public int NumberOfBullets;
    public int MinShotAngle;
    public int MaxShotAngle;
}
