using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//custom
using PathCreation;
using PandoraUtils;

public class TestingPathCreator : MonoBehaviour
{
    [SerializeField] Vector3[] points;
    VertexPath path;

    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[10];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = util.RandomVector(5);
        }
        path = GeneratePath(points, false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < path.NumPoints - 1; i++)
        {
            Debug.DrawLine(path.GetPoint(i), path.GetPoint(i + 1), Color.white);
        }
    }

    VertexPath GeneratePath(Vector3[] points, bool closedPath)
    {
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xyz);
        return new VertexPath(bezierPath, gameObject.transform);
    }
}
