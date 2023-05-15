using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    //private char[] tiles;


    private LevelData levelData = new LevelData();

    private void Awake()
    {
        cam = Camera.main;

        //levelData = GetComponent<LevelData>();
        //levelData = new LevelData();

        //tiles = levelData.GetTiles();

        Vector2 ScreenTopRightInWorld = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 ScreenBottomLeftInWorld = cam.ScreenToWorldPoint(new Vector2(0, 0));
        boundRight = (int)ScreenTopRightInWorld.x;
        boundLeft = (int)ScreenBottomLeftInWorld.x;
        boundTop = (int)ScreenTopRightInWorld.y;
        halfScreenHeight = (int)ScreenTopRightInWorld.y / 2;

        SpriteRenderer enemySpriteRenderer = EnemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteSize = new Vector2(enemySpriteRenderer.bounds.size.x, enemySpriteRenderer.bounds.size.y);


        //SpawnEnemy(new Vector3(0, 3, 0));
        //SetLevelData();
        CreateLevel();
    }

    private void SetLevelData()
    {
        currentNumOfEnemies = 0;

        if (levelData == null)
            return;
        else
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
        //TO DO: Get random position in the levelData,
        currentNumOfEnemies = 0;

        SetLevelData();

        foreach (char tile in tiles)
        {
            int randomXCoord = Random.Range(boundLeft, levelData.GetWidth());
            int randomYCoord = Random.Range(halfScreenHeight, levelData.GetHeight());

            char tileToCheck = levelData.GetTile(randomXCoord, randomYCoord);
            //check if it is an empty tile,
            if (tileToCheck != EMPTY && tileToCheck == ENEMY)
            {
                continue;
            }
            else
            {
                //if it is, spawn an enemy and set it as enemy tile,
                if(currentNumOfEnemies < NumOfEnemies)
                {
                    SpawnEnemy(new Vector3(randomXCoord, randomYCoord, 0));
                    levelData.SetTile(randomXCoord, randomYCoord, ENEMY);
                    currentNumOfEnemies++;
                    //make it repeat until it reaches a number of enemies.
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
        //Vector2 screenTopRightInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //Vector2 screenBottomLeftInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        width = GetWidth();
        height = GetHeight();

        //int boundLeft = (int)screenBottomLeftInWorld.x;
        //int boundBottom = (int)screenBottomLeftInWorld.y;

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
