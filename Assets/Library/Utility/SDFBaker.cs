using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;
using UnityEngine.VFX.SDF;

public class SDFBaker : MonoBehaviour
{
    public static RenderTexture ConvertToSDF (Mesh mesh, IrisSettings config)
    {
        MeshToSDFBaker meshBaker = new MeshToSDFBaker
        (
            config.sizeBox,
            config.center,
            config.maxResolution,
            mesh,
            config.signPassCount,
            config.threshold
        );

        meshBaker.BakeSDF();
        RenderTexture sdf = new RenderTexture(meshBaker.SdfTexture);

        if (meshBaker != null)
        {
            meshBaker.Dispose();
        }
        
        return sdf;
    }
}
