using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject EnemyPrefab;

    private Camera cam;
    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private float halfScreenHeight;

    private Vector2 enemySpriteSize;

    private char[] tiles;

    private const char EMPTY = '-';
    private const char ENEMY = '+';

    private void Awake()
    {
        cam = Camera.main;
        Vector2 ScreenTopRightInWorld = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 ScreenBottomLeftInWorld = cam.ScreenToWorldPoint(new Vector2(0, 0));
        boundRight = ScreenTopRightInWorld.x;
        boundLeft = ScreenBottomLeftInWorld.x;
        boundTop = ScreenTopRightInWorld.y;
        halfScreenHeight = ScreenTopRightInWorld.y / 2;

        SpriteRenderer enemySpriteRenderer = EnemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteSize = new Vector2(enemySpriteRenderer.bounds.size.x, enemySpriteRenderer.bounds.size.y);

        //SpawnEnemy(new Vector3(0, 3, 0));
        CreateLevelData();
        //TO DO: Get random position in the levelData,
        //check if it is an empty tile,
        //if it is, spawn an enemy and set it as enemy tile,
        //make it repeat until it reaches a number of enemies.
    }

    private void CreateLevelData()
    {
        tiles = new char[(int)boundRight * (int)halfScreenHeight];

        for (float y = halfScreenHeight; y < boundTop; y += enemySpriteSize.y)
        {
            for (float x = boundLeft; x < boundRight; x += enemySpriteSize.x)
            {
                SetTile(x, y, EMPTY);
            }
        }
    }

    private char GetTile(float x, float y)
    {
        if (x < 0 || y < 0 || x >= boundRight || y >= boundTop)
        {
            return EMPTY;
        }

        return tiles[(int)x + (int)y * (int)boundRight];
    }

    private void SetTile(float x, float y, char tile)
    {
        tiles[(int)x + (int)y * (int)boundRight] = tile;
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemyGO = Instantiate(EnemyPrefab, position, Quaternion.identity);

        SpriteRenderer enemyRenderer = enemyGO.GetComponent<SpriteRenderer>();
        Sprite enemySprite = enemyRenderer.sprite;

        Helper.UpdateColliderShapeToSprite(enemyGO, enemySprite);
    }
}
