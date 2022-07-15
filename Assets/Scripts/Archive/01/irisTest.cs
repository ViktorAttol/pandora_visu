using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;

public class irisTest : MonoBehaviour
{
    // data input
    [SerializeField] testData iris_data;
    //[SerializeField] float debugRadius = 0.01f;
    //[SerializeField] bool debug = true;
    [SerializeField] Mesh mesh;

    /*
    private void OnDrawGizmos()
    {
        List<Vector3> vertices = ReturnCircle(new Vector3(0,0,0), 1f, 8);
        vertices.AddRange(ReturnCircle(new Vector3(0,0,4), 1f, 16));

        Gizmos.color = Color.red;
        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawWireCube(vertices[i], new Vector3(debugRadius,debugRadius,debugRadius));
        }
    }
    */

    void Update()
    {
        Vector3 pos01 = new Vector3(0, 0, 0);
        Vector3 pos02 = new Vector3(0, 0, 4);
        mesh = TubeSegment(pos01, pos02);

        /*
        if (debug == true)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Debug.DrawLine(vertices[i], vertices[(i + 1) % vertices.Count], Color.red);
            }
        }
        */
    }

    // Custom Functions
    

    Mesh TubeSegment(Vector3 start, Vector3 end)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        /*
        vertices.AddRange(util.ReturnCircle(start, iris_data.tube_radius, iris_data.tube_resolution));
        vertices.AddRange(util.ReturnCircle(end, iris_data.tube_radius, iris_data.tube_resolution));
        */

        triangles = TubeSegmentTriangles(iris_data.tube_resolution);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    List<int> TubeSegmentTriangles(int resolution)
    {
        List<int> triangles = new List<int>();
        for (int i = 0; i < resolution; i++)
        {
            triangles.Add(i);
            triangles.Add((i + 1) % resolution);
            triangles.Add(i + resolution);

            triangles.Add(i + resolution);
            triangles.Add((i + 1) % resolution);
            triangles.Add((i + 1) % resolution + resolution);
        }
        return triangles;
    }

    void UploadMesh(Mesh mesh)
    {
        // load mesh into scriptable object
        iris_data.mesh = mesh;
    }
}
