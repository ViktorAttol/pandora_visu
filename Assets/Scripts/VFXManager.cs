using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    [SerializeField] MeshCarrier meshCar;
    bool newMesh;

    void Setup()
    {
        if (!vfx || !meshCar) throw new System.Exception("VFXManager is missing references");
    }

    void Update()
    {
        if (meshCar.mesh == null) newMesh = true;
        if (!newMesh) return;

        if (meshCar.mesh != null && meshCar.mesh != vfx.GetMesh("inputMesh"))
        {
            vfx.SetMesh("inputMesh", meshCar.mesh);
            newMesh = false;
        }
    }
}
