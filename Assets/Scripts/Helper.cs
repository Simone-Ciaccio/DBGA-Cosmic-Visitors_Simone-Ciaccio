using System.Collections;
using System.Collections.Generic;
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
}
