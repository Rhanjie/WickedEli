using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiamondSquareNoise : INoise
{
    private int size;
    
    private float range = 0.5f;

    private int randomStart = 16;

    private float[,] noise;

    private System.Random randomEngine;
    
    public float[,] Generate(uint sizePower, int seed, float roughness)
    {
        randomEngine = new System.Random(seed);

        size = (int) Mathf.Pow(2, sizePower) + 1;
        range = 2f;
        noise = new float[size, size];
        
        var chunkSize = size - 1;

        noise[0, 0] = randomEngine.Next(randomStart / 3, randomStart);
        noise[0, chunkSize] = randomEngine.Next(randomStart / 3, randomStart);
        noise[chunkSize, 0] = randomEngine.Next(randomStart / 3, randomStart);
        noise[chunkSize, chunkSize] = randomEngine.Next(randomStart / 3, randomStart);
        
        while (chunkSize > 1)
        {
            SquareStep(chunkSize);
            DiamondStep(chunkSize);

            range -= range * 0.5f * roughness;
            chunkSize /= 2;
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

                    var average = (left + right + upper + lower) / 4f;
                    var randomValue = (Random.value * (range * 2.0f)) - range;

                    noise[y + half, x + half] = average + randomValue;
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
        
        for (var y = 0; y < size - 1; y += half)
        {
            for (var x = (y + half) % chunkSize; x < size - 1; x += chunkSize)
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
                    right = noise[y, x - half];
                    count += 1;
                }
                
                if (x + half < size)
                {
                    upper = noise[y, x + half];
                    count += 1;
                }

                var average = (left + right + upper + lower) / count;
                var randomValue = (Random.value * (range * 2.0f)) - range;
                
                noise[y, x] = average + randomValue;
                
                if (x == 0) {
                    noise[size - 1, y] = average;
                }

                if (y == 0) {
                    noise[x, size - 1] = average;
                }
            }
        }
    }
}