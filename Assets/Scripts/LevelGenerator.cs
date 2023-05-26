using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoSingleton<LevelGenerator>
{
    public GameObject EnemyPrefab;
    public GameObject BossPrefab;

    public int NumOfEnemies;

    private const char EMPTY = '-';
    private const char ENEMY = '+';

    private Camera cam;

    private Vector2 enemySpriteSize;

    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private float halfScreenHeight;

    private int currentNumOfEnemies = 0;

    private Dictionary<Vector2, char> tiles = new Dictionary<Vector2, char>();

    protected override void Awake()
    {
        base.Awake();

        cam = Camera.main;

        boundRight = Helper.GetScreenBoundRight(cam);
        boundLeft = Helper.GetScreenBoundLeft(cam);
        boundTop = Helper.GetScreenBoundTop(cam);
        halfScreenHeight = Helper.GetScreenBoundTop(cam) / 2;

        SpriteRenderer enemySpriteRenderer = EnemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteSize = new Vector2(enemySpriteRenderer.bounds.size.x, enemySpriteRenderer.bounds.size.y);
    }

    private void Start()
    {
        EventManager.Instance.OnNormalLevelStart += CreateLevel;
        EventManager.Instance.OnBossLevelStart += CreateBossLevel;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNormalLevelStart -= CreateLevel;
        EventManager.Instance.OnBossLevelStart -= CreateBossLevel;
    }

    private void CreateLevel()
    {
        currentNumOfEnemies = 0;

        InitTiles();

        for (int i = 0; i < tiles.Count; i++)
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
                    SpawnEnemy(EnemyPrefab, tilePosition);
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

    private void CreateBossLevel()
    {
        Vector3 spawnPosition = new Vector3((boundRight / 2), boundTop, 0);
        SpawnEnemy(BossPrefab, spawnPosition);
    }

    private void SpawnEnemy(GameObject prefab, Vector3 position)
    {
        GameObject enemyGO = Instantiate(prefab, position, Quaternion.identity);

        EventManager.Instance.StartEnemySpawnGOEvent(enemyGO);

        SpriteRenderer enemyRenderer = enemyGO.GetComponent<SpriteRenderer>();
        Sprite enemySprite = enemyRenderer.sprite;

        Helper.UpdateColliderShapeToSprite(enemyGO, enemySprite);
    }
}
