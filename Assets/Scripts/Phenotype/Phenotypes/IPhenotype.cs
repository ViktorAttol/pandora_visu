using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public interface IPhenotype
{
    
    // Methods
    public IEnumerator Generate ();
    public IEnumerator CreateMesh ();
    public Mesh GetMesh ();

    // Callbacks
    void SubscribeForGenFinished (Action cbGenDoneFunc);
    void SubscribeForMeshFinished (Action cbMeshDoneFunc);
    void UnsubscribeForGenFinished (Action cbGenDoneFunc);
    void UnsubcribeForMeshFinished (Action cbMeshDoneFunc);
}