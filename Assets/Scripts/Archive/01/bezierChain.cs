using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezierChain : MonoBehaviour
{
    Vector3[] points;
    int range = 5;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[20];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3[] bezierPoints = drawBezierCurve(points, 2);
            for (int i = 0; i < bezierPoints.Length; i++)
            {
                Gizmos.DrawSphere(bezierPoints[i], 0.1f);
            }
        }
    }

    // function to draw a 3D bezier curve through a set of points
    public static Vector3[] drawBezierCurve(Vector3[] points, int segments)
    {
        // create a new array of vectors to store the points of the curve
        Vector3[] curvePoints = new Vector3[segments + 1];

        // calculate the number of points in the curve
        int curvePointCount = (points.Length - 1) * segments;

        // calculate the number of points in the curve
        for (int i = 0; i < curvePointCount; i++)
        {
            // calculate the point on the curve
            float t = (float)i / (float)curvePointCount;
            curvePoints[i] = calculateBezierPoint(t, points);
        }

        // return the curve points
        return curvePoints;
    }

    // function to calculate a point on a bezier curve
    public static Vector3 calculateBezierPoint(float t, Vector3[] points)
    {
        // create a new vector to store the point
        Vector3 point = new Vector3();

        // calculate the number of points in the curve
        int pointCount = points.Length - 1;

        // calculate the number of points in the curve
        for (int i = 0; i < pointCount; i++)
        {
            // calculate the point on the curve
            point += (Mathf.Pow(1 - t, pointCount - i) * Mathf.Pow(t, i) * points[i]);
        }

        // return the point
        return point;
    }
}
