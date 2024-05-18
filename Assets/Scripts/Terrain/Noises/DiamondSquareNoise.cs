﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain.Noises
{
    [CreateAssetMenu(fileName = "DiamondSquareNoise", menuName = "Generators/DiamondSquareNoise")]
    public class DiamondSquareNoise : ScriptableObject, INoise
    {
        private int size;
    
        [Tooltip("If zero, then seed will be randomized")]
        [SerializeField] private int seed;
        [SerializeField] private int randomStart = 16;
        
        [Range(0, 8)] [Tooltip("Randomizing noise")]
        [SerializeField] private float startRandomRange = 2f;
        
        [SerializeField] private bool decreaseRandomRange = true;
        
        [ShowIf("decreaseRandomRange")] [Tooltip("Smaller roughness = smoother results")]
        [Range(1, 2)] [SerializeField] private float roughness = 2f;

        private float[,] noise;

        private System.Random randomEngine;
    
        public float[,] Generate(uint sizePower)
        {
            if (seed == 0)
                seed = Random.Range(0, 20000);
            
            randomEngine = new System.Random(seed);

            //TODO: Change to universal solution
            size = (int) Mathf.Pow(2, sizePower) + 1;
            
            noise = new float[size, size];

            var randomRange = startRandomRange;
            var chunkSize = size - 1;

            noise[0, 0] = randomEngine.Next(randomStart / 3, randomStart);
            noise[0, chunkSize] = randomEngine.Next(randomStart / 3, randomStart);
            noise[chunkSize, 0] = randomEngine.Next(randomStart / 3, randomStart);
            noise[chunkSize, chunkSize] = randomEngine.Next(randomStart / 3, randomStart);
        
            while (chunkSize > 1)
            {
                SquareStep(chunkSize);
                DiamondStep(chunkSize);

                if (decreaseRandomRange)
                    randomRange = (float) Math.Max(randomRange / roughness, 0.1);

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
                        var randomValue = (Random.value * (startRandomRange * 2.0f)) - startRandomRange;

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
                    var randomValue = (Random.value * (startRandomRange * 2.0f)) - startRandomRange;
                
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
}