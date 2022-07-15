using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PathCreation;

public class bezierTester : MonoBehaviour
{
    BezierPath path;
    Vector3[] points;
    int range = 5;
    PathCreator pathCreator;

    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[10];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(Random.Range(-range,range),Random.Range(-range,range),Random.Range(-range,range));
        }
        path = new BezierPath(points, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
