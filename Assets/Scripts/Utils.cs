using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    public static IEnumerable<Vector3> GenerateDirections(int outputWays, int width, float outputAccuracy, Vector3 baseVector)
    {
        var ways = outputWays;
        var random = new Random();

        var offset = 1 / outputAccuracy;
        if (ways % 2 != 0)
        {
            yield return  Quaternion.Euler(0, 0, Math.Sign(random.Next(-1, 1)) * offset) 
                          * baseVector;
            ways -= 1;
        }

        if (outputWays == 1) yield break;
        
        var angle = width / ways;
        for (var i = 1; i <= ways / 2; i++)
        {
            
            offset = (float)random.NextDouble() * 10;
            Debug.Log(offset);
            yield return Quaternion.Euler(0, 0, angle * i)
                         * baseVector;
            yield return Quaternion.Euler(0, 0, -angle * i)
                         * baseVector;
        }
            
        
    }
}
