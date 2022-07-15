using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PandoraUtils
{
    public class util
    {
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

        public static Vector3 CircleCoordinate (float radius, float angle)
        {
            Vector3 vec = new Vector3
            (
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );
            return vec;
        }

        public static Vector3 RandomCircleCoordinate (float radius)
        {
            float angle = Random.Range(0f,360f);
            Vector3 vec = new Vector3
            (
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
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

        public static int WeightedRandomInRange(float weight, Vector2Int range)
        {
            int result = Random.Range((int)(range.x * weight), (int)((range.y + 1) * weight));
            // + 1 since when using Random.Range as int the upper range changes into an exclusive
            return result;
        }

        public static float WeightedRandomInRange(float weight, Vector2 range)
        {
            float result = Random.Range((range.x * weight), (range.y * weight));
            return result;
        }
    }
}
