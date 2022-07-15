using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.SDF;

public class sdfGen : MonoBehaviour
{
    [SerializeField] testData config;
    [SerializeField] VisualEffect vfx;
    bool done = false;
    MeshToSDFBaker meshBaker;

    // Update is called once per frame
    void Update()
    {
        //GUARD PATTERN
        if (done && config.mesh == null)
        {
           return;
        }
        
        // Only executed when !done
        print("Baking mesh");
        meshBaker = new MeshToSDFBaker(config.sizeBox, config.center, config.maxResolution, config.mesh, config.signPassCount, config.threshold);
        meshBaker.BakeSDF();
        vfx.SetTexture("SDF", meshBaker.SdfTexture);
        done = true;
        print("Done");
    }
    void OnDestroy()
    {
        if (meshBaker != null)
        {
            meshBaker.Dispose();
        }
    }
}

// ENUMS
//SquareAnimation(Type, Color, ……..)

// (FRACTURED) BROWNING MOTION
// EXPO / LOG -> Verteilung
// Distance based "Randbedingungen" bei kurzer Entfernung wenig oder viel veränderung vice versa