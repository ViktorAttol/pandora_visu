using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;
using PathCreation;

using System;

public class Hair : IPhenotype
{
    private HairSettings set;
    private Transform transform;
    private System.Random rand;
    private bool debug;

    // output
    Mesh output;

    // events
    public event Action genDoneEvent;
    public event Action meshDoneEvent;

    public Hair(HairSettings _hairSet, System.Random _rand)
    {
        set = _hairSet;
        rand = _rand;

        debug = set.debug;
    }

    public IEnumerator Generate ()
    {
        yield return null;
    }

    public IEnumerator CreateMesh ()
    {
        yield return null;
    }

    public Mesh GetMesh ()
    {
        return output;
    }

    #region Generation Methods

    /// <summary> Distributes Points on the surface of a sphere.</summary>
    void InitLocations ()
    {
        
    }

    /// <summary> Grows "hairs" from start location within sphere.</summary>
    void Extrude ()
    {

    }

    /// <summary> Using sin(t) as x, cos(t) as y and t as z, a twirl is created.</summary>
    Vector3 Twirl (float t)
    {
        return new Vector3
        (
            Mathf.Sin(t),
            Mathf.Cos(t),
            t
        );
    }

    /// <summary> Using sin(t) as x, cos(t) as y and t as z, a twirl is created.</summary>
    Vector3 Displacement (float scale)
    {
        return util.RandomVector(rand, scale);
    }

    #endregion

    #region Callbacks & Actions
    // -> Publish Subscriber Pattern
    public void SubscribeForGenFinished (Action cbGenDoneFunc)
    {
        genDoneEvent += cbGenDoneFunc;
    }

    public void SubscribeForMeshFinished (Action cbMeshDoneFunc)
    {
        meshDoneEvent += cbMeshDoneFunc;
    }

    public void UnsubscribeForGenFinished (Action cbGenDoneFunc)
    {
        genDoneEvent -= cbGenDoneFunc;
    }

    public void UnsubcribeForMeshFinished (Action cbMeshDoneFunc)
    {
        meshDoneEvent -= cbMeshDoneFunc;
    }

    private void OnMeshDone ()
    {
        meshDoneEvent?.Invoke();
    }

    private void OnGenDone ()
    {
        genDoneEvent?.Invoke();
    }
    #endregion
}
