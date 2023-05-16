using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameController GameController;

    public GameObject EnemyPrefab;
    public GameObject BossPrefab;

    public int CurrentLevelNumber = 1;
    public int NumOfEnemies;

    public const char EMPTY = '-';
    public const char ENEMY = '+';

    private Camera cam;

    private Vector2 enemySpriteSize;

    private int boundRight;
    private int boundLeft;
    private int boundTop;
    private int halfScreenHeight;

    private char[] tiles;

    private int currentNumOfEnemies = 0;

    private LevelData levelData = new LevelData();

    private void Awake()
    {
        cam = Camera.main;

        boundRight = (int)Helper.GetScreenBoundRight(cam);
        boundLeft = (int)Helper.GetScreenBoundLeft(cam);
        boundTop = (int)Helper.GetScreenBoundTop(cam);
        halfScreenHeight = (int)Helper.GetScreenBoundTop(cam) / 2;

        SpriteRenderer enemySpriteRenderer = EnemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteSize = new Vector2(enemySpriteRenderer.bounds.size.x, enemySpriteRenderer.bounds.size.y);

        CreateLevel();
    }

    private void SetLevelData()
    {
        currentNumOfEnemies = 0;

        if (levelData != null)
        {
            levelData.SetStartHeightPos(halfScreenHeight);
            levelData.SetHeight(boundTop);
            levelData.SetHeightStep((int)enemySpriteSize.y);
            levelData.SetStartWidthPos(boundLeft);
            levelData.SetWidth(boundRight);
            levelData.SetWidthStep((int)enemySpriteSize.x);

            tiles = levelData.GetTiles();
        }
    }

    public void CreateLevel()
    {
        currentNumOfEnemies = 0;

        SetLevelData();

        foreach (char tile in tiles)
        {
            int randomXCoord = Random.Range(boundLeft, levelData.GetWidth());
            int randomYCoord = Random.Range(halfScreenHeight, levelData.GetHeight());

            char tileToCheck = levelData.GetTile(randomXCoord, randomYCoord);
            if (tileToCheck != EMPTY && tileToCheck == ENEMY)
            {
                continue;
            }
            else
            {
                if(currentNumOfEnemies < NumOfEnemies)
                {
                    SpawnEnemy(new Vector3(randomXCoord, randomYCoord, 0));
                    levelData.SetTile(randomXCoord, randomYCoord, ENEMY);
                    currentNumOfEnemies++;
                }
            }
        }
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
        GameObject enemyGO = Instantiate(BossPrefab, position, Quaternion.identity);

        GameController.Enemies.Add(enemyGO);

        SpriteRenderer bossRenderer = enemyGO.GetComponent<SpriteRenderer>();
        Sprite bossSprite = bossRenderer.sprite;

        Helper.UpdateColliderShapeToSprite(enemyGO, bossSprite);
    }
}

class LevelData
{
    private int width = 15;
    private int height = 15;
    private int widthStartPos = 0;
    private int heightStartPos = 0;
    private int widthStep = 1;
    private int heightStep = 1;
    private char[] tiles;

    public LevelData()
    {
        width = GetWidth();
        height = GetHeight();

        tiles = new char[width * height];

        for (int y = heightStartPos; y < height; y += heightStep)
        {
            for (int x = widthStartPos; x < width; x += widthStep)
            {
                SetTile(x, y, LevelGenerator.EMPTY);
            }
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public void SetHeight(int value)
    {
        height = value;
    }

    public void SetWidth(int value)
    {
        width = value;
    }

    public void SetStartHeightPos(int value)
    {
        heightStartPos = value;
    }

    public void SetStartWidthPos(int value)
    {
        widthStartPos = value;
    }

    public void SetHeightStep(int value)
    {
        heightStep = value;
    }

    public void SetWidthStep(int value)
    {
        widthStep = value;
    }

    public char[] GetTiles()
    {
        return tiles;
    }

    public char GetTile(int x, int y)
    {
        if (x < widthStartPos || y < heightStartPos || x >= GetWidth() || y >= GetHeight())
        {
            return LevelGenerator.EMPTY;
        }

        return tiles[x + y * GetWidth()];
    }

    public void SetTile(int x, int y, char tile)
    {
        tiles[x + y * GetWidth()] = tile;
    }
}
