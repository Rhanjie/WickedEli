﻿using System;
using UnityEngine;

public class DiamondSquareNoise : INoise
{
    private int size;

    private Vector2Int randomRange = new Vector2Int(-2, 2);
    
    
    private int randomStart = 16;

    private float[,] noise;

    private System.Random randomEngine;
    
    public float[,] Generate(uint sizePower, int seed)
    {
        randomEngine = new System.Random(seed);

        size = (int) Mathf.Pow(2, sizePower) + 1;
        noise = new float[size, size];

        var roughness = 2f;
        var chunkSize = size - 1;

        noise[0, 0] = randomEngine.Next(1, randomStart);
        noise[0, chunkSize] = randomEngine.Next(1, randomStart);
        noise[chunkSize, 0] = randomEngine.Next(1, randomStart);
        noise[chunkSize, chunkSize] = randomEngine.Next(1, randomStart);
        
        while (chunkSize > 1)
        {
            SquareStep(chunkSize);
            DiamondStep(chunkSize);

            //randomRange /= 2;
            chunkSize /= 2;
            roughness /= 2f;
        }

        return noise;
    }
    
    private void SquareStep(int chunkSize)
    {
        var half = chunkSize / 2;

        for (var y = 0; y < size - 1; y += chunkSize)
        {
            for (var x = 0; x < size - 1; x += chunkSize)
            {
                try
                {
                    var left = noise[y, x];
                    var right = noise[y, x + chunkSize];
                    var upper = noise[y + chunkSize, x];
                    var lower = noise[y + chunkSize, x + chunkSize];

                    var randomValue = randomEngine.Next(randomRange.x, randomRange.y);

                    noise[y + half, x + half] = (float)Math.Round(left + right + upper + lower) / 4f + randomValue;
                }

                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
            }
        }
    }
        
    private void DiamondStep(int chunkSize)
    {
        var half = chunkSize / 2;
        
        for (var y = 0; y < size; y += half)
        {
            for (var x = (y + half) % chunkSize; x < size; x += chunkSize)
            {
                var count = 0;

                var left = 0f;
                var right = 0f;
                var upper = 0f;
                var lower = 0f;
                
                if (y - half >= 0)
                {
                    left = noise[y - half, x];
                    count += 1;
                }
                
                if (y + half < size)
                {
                    lower = noise[y + half, x];
                    count += 1;
                }
                
                if (x - half >= 0)
                {
                    right = noise[x - half, x];
                    count += 1;
                }
                
                if (x + half < size)
                {
                    upper = noise[x + half, x];
                    count += 1;
                }
                
                var randomValue = randomEngine.Next(randomRange.x, randomRange.y);
                
                noise[y, x] = (left + right + upper + lower) / count + randomValue;
            }
        }
    }
}