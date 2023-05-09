using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy")]
public class EnemyScriptable : ScriptableObject
{
    public int EnemyHealth;
    public int EnemyDamage;
    public Sprite EnemySprite;
    public GameObject EnemyBulletPrefab;
    public Sprite EnemyBulletSprite;
    public float TimeBetweenShots;
    public int NumberOfBullets;
    public int MinShotAngle;
    public int MaxShotAngle;
}
