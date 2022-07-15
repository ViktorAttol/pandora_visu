using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class irisGenOld : MonoBehaviour
{
    [SerializeField] testData iData;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        CreateCube();
        UploadMesh();
    }

    void UploadMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        // load mesh into scriptable object
        iData.mesh = mesh;
    }

    void CreateCube ()
    {
        // dummy cube
        vertices.Add(new Vector3(-1, -1, -1)); //0
        vertices.Add(new Vector3(-1, -1, 1)); //1
        vertices.Add(new Vector3(-1, 1, -1)); //2
        vertices.Add(new Vector3(-1, 1, 1)); //3
        vertices.Add(new Vector3(1, -1, -1)); //4
        vertices.Add(new Vector3(1, -1, 1)); //5
        vertices.Add(new Vector3(1, 1, -1)); //6
        vertices.Add(new Vector3(1, 1, 1)); //7

        triangles.Add(4);
        triangles.Add(6);
        triangles.Add(7);

        triangles.Add(4);
        triangles.Add(7);
        triangles.Add(5);

        triangles.Add(5);
        triangles.Add(7);
        triangles.Add(3);

        triangles.Add(5);
        triangles.Add(3);
        triangles.Add(1);

        triangles.Add(3);
        triangles.Add(2);
        triangles.Add(0);

        triangles.Add(3);
        triangles.Add(0);
        triangles.Add(1);

        triangles.Add(2);
        triangles.Add(6);
        triangles.Add(4);

        triangles.Add(2);
        triangles.Add(4);
        triangles.Add(0);

        triangles.Add(6);
        triangles.Add(3);
        triangles.Add(7);

        triangles.Add(6);
        triangles.Add(2);
        triangles.Add(3);

        triangles.Add(5);
        triangles.Add(1);
        triangles.Add(4);

        triangles.Add(1);
        triangles.Add(0);
        triangles.Add(4);
    }
}