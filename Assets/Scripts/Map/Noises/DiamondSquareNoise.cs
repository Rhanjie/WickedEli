using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Map.Noises
{
    [CreateAssetMenu(fileName = "DiamondSquareNoise", menuName = "Generators/DiamondSquareNoise")]
    public class DiamondSquareNoise : ScriptableObject, INoise
    {
        [Tooltip("If zero, then seed will be randomized")] [SerializeField]
        private int customSeed;

        [SerializeField] private int randomStart = 16;

        [Range(0, 8)] [Tooltip("Randomizing noise")] [SerializeField]
        private float startRandomRange = 2f;

        [SerializeField] private bool decreaseRandomRange = true;

        [ShowIf("decreaseRandomRange")] [Tooltip("Smaller roughness = smoother results")] [Range(1, 2)] [SerializeField]
        private float roughness = 2f;

        private float[,] noise;

        private Random randomEngine;
        private int size;
        private int seed;
        
        private int ConvertToPowerOfTwo(int n)
        {
            n -= 1;
            
            var result = 0;
            for (var i = n; i >= 1; i--)
            {
                if ((i & (i - 1)) == 0)
                {
                    result = i;
                    break;
                }
            }
            
            return result * 2;
        }

        public Task<float[,]> Generate(uint rawSize)
        {
            seed = customSeed != 0 ? customSeed
                : Guid.NewGuid().GetHashCode();

            randomEngine = new Random(seed);
            
            size = ConvertToPowerOfTwo((int) rawSize) + 1;
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
                    randomRange = (float)Math.Max(randomRange / roughness, 0.1);

                chunkSize /= 2;
            }

            return Task.FromResult(noise);
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

                        var range = startRandomRange;
                        var randomValue = (float) randomEngine.NextDouble() * (range * 2.0f) - range;

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
                var randomValue = (float) randomEngine.NextDouble() * (startRandomRange * 2.0f) - startRandomRange;

                noise[y, x] = average + randomValue;

                if (x == 0) 
                    noise[size - 1, y] = average;
                
                if (y == 0)
                    noise[x, size - 1] = average;
            }
        }
    }
}