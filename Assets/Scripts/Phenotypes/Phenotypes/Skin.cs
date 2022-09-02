using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;
using System;

public class Skin : IPhenotype
{
    // generation settings
    private SkinSettings set;
    private Transform transform;

    // PRNG
    private System.Random rand;

    // data
    private SphereMesh[] blobs;
    private Vector3[] positions;
    private Vector3[] shiftDir;

    // output
    Mesh output;

    // events
    public event Action genDoneEvent;
    public event Action meshDoneEvent;


    public Skin(SkinSettings _skinSet, System.Random _rand)
    {
        // transform fix, not very elegant, works however
        GameObject old = GameObject.Find("pheno_transform");
        if (old == null)
        {
            transform = new GameObject("pheno_transform").transform;
        } else {
            transform = old.transform;
        }

        // write input properties
        set = _skinSet;

        // init PRNG
        rand = _rand;

        if (set.debug) Debug.Log("Skin instance created.");
    }

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

    public IEnumerator Generate ()
    {
        // determine positions for blobs
        positions = GetPositions(set.blobAmount);


        // create blobs and set their position
        blobs = new SphereMesh[set.blobAmount];

        for (int i = 0; i < blobs.Length; i++)
        {
            float scale = util.RangeFromRead(rand, set.blobRadius.x, set.blobRadius.y);
            blobs[i] = new SphereMesh(set.blobResolution, scale);
            blobs[i].Translate(positions[i]);
            yield return new WaitForSeconds(set.generationRate);
        }

        if (genDoneEvent != null) genDoneEvent();
        if(set.debug) Debug.Log("Skin generation complete.");
    }

    public IEnumerator CreateMesh ()
    {
        // list with all meshes
        List<Mesh> meshes = new List<Mesh>();

        // add all blobs
        foreach (var blob in blobs)
        {
            Mesh current = blob.GetMesh(set.perlinScale, set.displacementIntensity);
            if (current != null) meshes.Add(current);
            yield return new WaitForSeconds(set.generationRate);
        }

        output =  MergeMeshes(meshes);
        if (meshDoneEvent != null) meshDoneEvent();
        if (set.debug) Debug.Log("Skin mesh created.");
    }

    public Mesh GetMesh ()
    {
        return output;
    }

    private Mesh MergeMeshes (List<Mesh> meshes)
    {
        Mesh mesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[meshes.Count];

        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
        }

        mesh.CombineMeshes(combine, false, false); // submeshes are merged (true) and transforms are ignored (false)
        return mesh;
    }

    private Vector3[] GetPositions(int amt)
    {
        // determine angular spacing
        float spacing = 360f / amt;

        // generate prandom start angle
        float start = util.RangeFromRead(rand, 0f, 360f);

        // calculate circle params
        float radius = util.RangeFromRead(rand, set.distributionRadius.x, set.distributionRadius.y);

        // calculate prandom rotation
        float x = util.RangeFromRead(rand, 0f, 360f);
        float y = util.RangeFromRead(rand, 0f, 360f);
        float z = util.RangeFromRead(rand, 0f, 360f);
        Quaternion rot = Quaternion.Euler(x,y,z);

        // populate angles[], dist[] & shiftDir[]
        float[] angles = new float[amt];
        Vector3[] dist = new Vector3[amt];
        shiftDir = new Vector3[amt];

        for (int i = 0; i < angles.Length; i++)
        {
            angles[i] = (start + i * spacing) % 360f;
            dist[i] = rot * util.CircleCoordinate(radius, angles[i]);
            shiftDir[i] = Vector3.zero - dist[i];
        }
        
        if (set.debug)
        {
            for (int i = 0; i < shiftDir.Length; i++)
            {
                Debug.DrawLine(Vector3.zero, shiftDir[i], Color.red, 5f);
            }
        }

        return dist;
    }

    public List<Vector3> DebugDraw ()
    {
        return null;
    }
}
