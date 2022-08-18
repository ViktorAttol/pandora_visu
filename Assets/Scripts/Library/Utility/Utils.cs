using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PandoraUtils
{
    public class util
    {
        
        // PRNG Stuff

        // NOT SURE ABOUT IN- OR EXCLUSIVE -> C# Reference
        public static float RangeFromRead(System.Random rand, float minInclusive, float maxExclusive)
        {
            float no = (float)rand.NextDouble() * (maxExclusive - minInclusive) + minInclusive;
            return no;
        }

        public static int RangeFromRead(System.Random rand, int minInclusive, int maxExclusive)
        {
            int no = rand.Next(minInclusive, maxExclusive);
            return no;
        }

        //

        // Custom modulo operation dealing with negative numbers
        // see: https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain/1082938
        public static int mod (int k, int n)
        {
            return ((k %= n) < 0) ? k+n : k;
        }

        // Modiefied call to Mathf.Perlin
        // see: https://medium.com/geekculture/how-to-use-math-noises-for-procedural-generation-in-unity-c-44902a21d8e
        public static float PerlinNoise(float x, float y, float scale)
        {
            float new_x = x * scale;
            float new_y = y * scale;
            float val = Mathf.PerlinNoise(new_x, new_y);
            return val;
        }

        /*
        public static Vector3 RandomVector (float range)
        {
            Vector3 vec = new Vector3
            (
                Random.Range(-range, range),
                Random.Range(-range, range),
                Random.Range(-range, range)
            );
            return vec;
        }
        */

        public static Vector3 RandomVector (System.Random rand, float range)
        {
            Vector3 vec = new Vector3
            (
                RangeFromRead(rand, -range, range),
                RangeFromRead(rand, -range, range),
                RangeFromRead(rand, -range, range)
            );
            return vec;
        }

        /*
        public static Vector3 RandomVector2D (float range)
        {
            Vector3 vec = new Vector3
            (
                Random.Range(-range, range),
                Random.Range(-range, range),
                0
            );
            return vec;
        }
        */

        public static Vector3 RandomVector2D (System.Random rand, float range)
        {
            Vector3 vec = new Vector3
            (
                RangeFromRead(rand, -range, range),
                RangeFromRead(rand, -range, range),
                0
            );
            return vec;
        }

        public static Vector3 CircleCoordinate (float radius, float angle)
        {
            Vector3 vec = new Vector3
            (
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0f
            );
            return vec;
        }

        /*
        public static Vector3 RandomCircleCoordinate (float radius)
        {
            float angle = Random.Range(0f,360f);
            Vector3 vec = new Vector3
            (
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0
            );
            return vec;
        }
        */

        public static Vector3 RandomCircleCoordinate (System.Random rand, float radius)
        {
            float angle = RangeFromRead(rand, 0f,360f);
            Vector3 vec = new Vector3
            (
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                0
            );
            return vec;
        }

        public static List<Vector3> ReturnCircle(Vector3 position, Quaternion rotation, float radius, int resolution)
        {
            List<Vector3> circle = new List<Vector3>();
            float angle = 0;
            float step = 2 * Mathf.PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                Vector3 vec = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                vec = rotation * vec;
                vec += position;
                circle.Add(vec);
                angle += step;
            }
            return circle;
        }

        // courtesy of jvo3dc: https://forum.unity.com/threads/random-number-without-repeat.497923/ (modified)
        /*
        public static List<int> RandomIDs(int count, List<int> available)
        {
            if (count >= available.Count)
            {
                return available;
            }
            else
            {
                List<int> result = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int selected = available[Random.Range(0, available.Count)];
                    available.Remove(selected);
                    result.Add(selected);
                }
                return result;
            }
        }
        */

        // modified to work with custom Random instance
        public static List<int> RandomIDs(System.Random rand, int count, List<int> available)
        {
            if (count >= available.Count)
            {
                return available;
            }
            else
            {
                List<int> result = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int selected = available[RangeFromRead(rand, 0, available.Count)];
                    available.Remove(selected);
                    result.Add(selected);
                }
                return result;
            }
        }

        public static int RandomID(System.Random rand, List<int> available)
        {
            return available[RangeFromRead(rand, 0, available.Count)];
        }

        /*
        public static int WeightedRandomInRange(float weight, Vector2Int range)
        {
            int result = Random.Range((int)(range.x * weight), (int)((range.y + 1) * weight));
            // + 1 since when using Random.Range as int the upper range changes into an exclusive
            return result;
        }
        */

        public static int WeightedRandomInRange(System.Random rand, float weight, Vector2Int range)
        {
            int result = RangeFromRead(rand, (int)(range.x * weight), (int)((range.y + 1) * weight));
            // + 1 since when using Random.Range as int the upper range changes into an exclusive
            return result;
        }

        /*
        public static float WeightedRandomInRange(float weight, Vector2 range)
        {
            float result = Random.Range((range.x * weight), (range.y * weight));
            return result;
        }
        */

        public static float WeightedRandomInRange(System.Random rand, float weight, Vector2 range)
        {
            float result = RangeFromRead(rand, (range.x * weight), (range.y * weight));
            return result;
        }
    }
}
