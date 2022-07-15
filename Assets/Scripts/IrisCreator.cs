using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisCreator : MonoBehaviour
{
    public IrisSettings irisSet;
    public MeshCarrier meshCar;

    [HideInInspector]
    Iris iris;
    


    // Start is called before the first frame update
    void Start()
    {
        // Create Iris
        iris = new Iris(irisSet, gameObject.transform);

    }

    // Update is called once per frame
    void Update()
    {
        // Update Iris
        iris.Update();
        if (!meshCar.mesh) iris.DebugDraw();
        
        // Check for input
        if (meshCar.mesh == null && !iris.IsRunning())
        {
            meshCar.mesh = iris.GenerateMesh();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !iris.IsRunning())
        {
            meshCar.mesh = null;
            iris = new Iris(irisSet, gameObject.transform);
        }
    }
}