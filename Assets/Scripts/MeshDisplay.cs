using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDisplay : MonoBehaviour
{
    public MeshCarrier meshCar;
    MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        // Setup Mesh
        meshFilter = GetComponent<MeshFilter>();

    }

    // Update is called once per frame
    void Update()
    {
        if (meshCar.mesh != null)
        {
            meshFilter.mesh = meshCar.mesh;
        }
    }
}
