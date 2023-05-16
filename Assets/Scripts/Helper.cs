using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Helper
{
    public static T RandomArrayValue<T>(List<T> array)
    {
        if (array.Count <= 0)
            return default;

        return array[Random.Range(0, array.Count)];
    }

    public static T RandomArrayValue<T>(T[] array)
    {
        if (array.Length <= 0)
            return default;

        return array[Random.Range(0, array.Length)];
    }

    public static void UpdateColliderShapeToSprite(GameObject prefabObject, Sprite sprite)
    {
        PolygonCollider2D polygonCollider = prefabObject.GetComponent<PolygonCollider2D>();

        if (polygonCollider != null && sprite != null)
        {
            polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

            List<Vector2> path = new List<Vector2>();

            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                path.Clear();
                sprite.GetPhysicsShape(i, path);
                polygonCollider.SetPath(i, path.ToArray());
            }
        }
    }

    public static float GetScreenBoundRight(Camera cam)
    {
        Vector2 screenTopRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        return screenTopRight.x;
    }

    public static float GetScreenBoundTop(Camera cam)
    {
        Vector2 screenTopRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        return screenTopRight.y;
    }

    public static float ScreenBoundBottom(Camera cam)
    {
        Vector2 screenBottomLeft = cam.ScreenToWorldPoint(Vector2.zero);

        return screenBottomLeft.y;
    }

    public static float GetScreenBoundLeft(Camera cam)
    {
        Vector2 screenBottomLeft = cam.ScreenToWorldPoint(Vector2.zero);

        return screenBottomLeft.x;
    }
}
