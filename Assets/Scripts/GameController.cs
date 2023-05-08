using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject EnemyPrefab;

    private void Awake()
    {
        SpawnEnemy(new Vector3(0, 3, 0));
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemyGO = Instantiate(EnemyPrefab, position, Quaternion.identity);

        Enemy enemy = enemyGO.GetComponent<Enemy>();
        if (!enemy.HasCollider)
        {
            enemyGO.AddComponent<PolygonCollider2D>();
            enemy.HasCollider = true;
        }
        else
        {
            PolygonCollider2D enemyCollider = enemyGO.GetComponent<PolygonCollider2D>();
            Destroy(enemyCollider);
            enemyGO.AddComponent<PolygonCollider2D>();
            enemy.HasCollider = true;
        }
    }
}
