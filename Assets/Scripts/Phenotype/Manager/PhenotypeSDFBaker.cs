using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;
using UnityEngine.VFX.SDF;

using System;

public class PhenotypeSDFBaker: MonoBehaviour
{
    [HideInInspector] public enum SDF_Res {normal, high, max}
    [HideInInspector] public enum SDF_SPC {normal, high, max}

    [Header("SDF Baker Settings")]
    // SDF Baker Settings
    public SDF_Res Resolution;
    int maxResolution_internal;
    public Vector3 center;
    public Vector3 sizeBox = new Vector3 (8f,8f,8f);
    public SDF_SPC signPassCount;
    int signPassCount_internal;
    [Range(0f, 1f)] public float threshold = 0.5f;

    // SDF Baker
    Mesh sdf_input;
    MeshToSDFBaker meshBaker;

    /*
    [Space(10)]
    [Header("Editor Settings")]
    [Range(0f,2f)] public float scale = 2f;
    [SerializeField] bool useSDF;
    */

    void Start()
    {
        // evaluate quality settings
        switch (Resolution)
        {
            case SDF_Res.normal:
                maxResolution_internal = 64;
                break;
            case SDF_Res.high:
                maxResolution_internal = 128;
                break;
            case SDF_Res.max:
                maxResolution_internal = 256;
                break;
        }

        switch (signPassCount)
        {
            case SDF_SPC.normal:
                signPassCount_internal = 1;
                break;
            case SDF_SPC.high:
                signPassCount_internal = 8;
                break;
            case SDF_SPC.max:
                signPassCount_internal = 16;
                break;
        }
        
        // Setup SDF Baker
        sdf_input = new Mesh();
        meshBaker = new MeshToSDFBaker(sizeBox, center, maxResolution_internal, sdf_input, signPassCount_internal, threshold);

        // Setup Box Size
        // vfx.SetVector3("boxSize", meshBaker.GetActualBoxSize());

    }

    void Update ()
    {
       
    }

    public RenderTexture GetSDF ( Mesh mesh)
    {
        // Baking
        UpdateMesh (mesh);
        meshBaker.BakeSDF();

        //RenderTexture output = new RenderTexture(meshBaker.SdfTexture);
        //Graphics.Blit(meshBaker.SdfTexture, output);
        //meshBaker.SdfTexture.
        //return output;
        return meshBaker.SdfTexture;
    }

    void UpdateMesh(Mesh mesh)
    {
        sdf_input.Clear();
        sdf_input.vertices = mesh.vertices;
        sdf_input.triangles = mesh.triangles;
        sdf_input.normals = mesh.normals;
        sdf_input.tangents = mesh.tangents;
    }

    void OnDestroy()
    {
        if (meshBaker != null)
        {
            meshBaker.Dispose();
        }
    }
}
