using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameController GameController;

    public GameObject EnemyPrefab;
    public GameObject BossPrefab;

    public int NumOfEnemies;

    public const char EMPTY = '-';
    public const char ENEMY = '+';

    private Camera cam;

    private Vector2 enemySpriteSize;

    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private float halfScreenHeight;

    private int currentNumOfEnemies = 0;

    private Dictionary<Vector2, char> tiles = new Dictionary<Vector2, char>();

    private void Awake()
    {
        cam = Camera.main;

        boundRight = Helper.GetScreenBoundRight(cam);
        boundLeft = Helper.GetScreenBoundLeft(cam);
        boundTop = Helper.GetScreenBoundTop(cam);
        halfScreenHeight = Helper.GetScreenBoundTop(cam) / 2;

        SpriteRenderer enemySpriteRenderer = EnemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteSize = new Vector2(enemySpriteRenderer.bounds.size.x, enemySpriteRenderer.bounds.size.y);
    }

    public void CreateLevel()
    {
        currentNumOfEnemies = 0;

        InitTiles();

        for (int i = 0; i < 1000; i++)
        {
            int tileIndexToCheck = Random.Range(0, tiles.Count -1);

            if (GetTile(tileIndexToCheck) != EMPTY)
                continue;
            else
            {
                if(currentNumOfEnemies >= NumOfEnemies) 
                {
                    break;
                }
                else
                {
                    SetTile(tileIndexToCheck, ENEMY);
                    Vector2 tilePosition = tiles.ElementAt(tileIndexToCheck).Key;
                    SpawnEnemy(tilePosition);
                    currentNumOfEnemies++;
                }
            }
        }
    }

    private void InitTiles()
    {
        tiles.Clear();

        for (float y = halfScreenHeight + enemySpriteSize.y; y < boundTop - enemySpriteSize.y; y += enemySpriteSize.y)
        {
            for (float x = boundLeft + enemySpriteSize.x; x < boundRight - enemySpriteSize.x; x += enemySpriteSize.x)
            {
                tiles.Add(new Vector2(x, y), EMPTY);
            }
        }
    }

    private void SetTile(int index, char tile)
    {
        Vector2 tileKey = tiles.ElementAt(index).Key;

        tiles[tileKey] = tile;
    }

    private char GetTile(int index)
    {
        char tile = tiles.ElementAt(index).Value;

        return tile;
    }

    public void CreateBossLevel()
    {
        Vector3 spawnPosition = new Vector3((boundRight / 2), boundTop, 0);
        SpawnBoss(spawnPosition);
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemyGO = Instantiate(EnemyPrefab, position, Quaternion.identity);

        GameController.Enemies.Add(enemyGO);

        SpriteRenderer enemyRenderer = enemyGO.GetComponent<SpriteRenderer>();
        Sprite enemySprite = enemyRenderer.sprite;

        Helper.UpdateColliderShapeToSprite(enemyGO, enemySprite);
    }

    private void SpawnBoss(Vector3 position)
    {
        GameObject bossGO = Instantiate(BossPrefab, position, Quaternion.identity);

        GameController.Enemies.Add(bossGO);

        SpriteRenderer bossRenderer = bossGO.GetComponent<SpriteRenderer>();
        Sprite bossSprite = bossRenderer.sprite;

        Helper.UpdateColliderShapeToSprite(bossGO, bossSprite);
    }
}
