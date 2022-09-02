using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;
using PathCreation;
using System;

using System.Linq;

public class Hair : IPhenotype
{
    private HairSettings set;
    private Transform transform;
    private System.Random rand;
    private bool debug;

    // custom
    List<Path> paths;

    // output
    Mesh output;

    // events
    public event Action genDoneEvent;
    public event Action meshDoneEvent;

    // transform fix, not very elegant, works however
    private void TransformSetup()
    {
        GameObject old = GameObject.Find("pheno_transform");
        if (old == null)
        {
            transform = new GameObject("pheno_transform").transform;
        } else {
            transform = old.transform;
        }
    }

    public Hair(HairSettings _hairSet, System.Random _rand)
    {
        TransformSetup();

        set = _hairSet;
        rand = _rand;

        debug = set.debug;

        paths = new List<Path>();
    }

    public IEnumerator Generate ()
    {
        float stepSize = set.lengthRadius / set.resolution;

        if (debug) Debug.Log("step size: " + stepSize + " | step check: " + stepSize * set.resolution);

        for (int i = 0; i < set.resolution + 1; i++) // + 1 to account for the zero iteration
        {
            if (debug) Debug.Log("HAIR UPDATE CYCLE NO. " + i);
            float step = i * stepSize;
            UpdateGeneration(step);
            yield return new WaitForSeconds(set.generationRate);
        }

        CleanUpUnfinished(set.cleanThresh);

        if (debug) Debug.Log("DEBUG DRAWING STARTED");
        OnGenDone();
    }

    public IEnumerator CreateMesh ()
    {
        List<Mesh> all = new List<Mesh>();
        foreach (var path in paths)
        {
            all.Add(GetMesh(path));
            yield return new WaitForSeconds(set.generationRate);
        }

        output = MergeMeshes(all);
        CenterOutputMesh();
        
        OnMeshDone();
    }

    public Mesh GetMesh ()
    {
        return output;
    }

    #region Generative Methods

    private void UpdateGeneration (float step)
    {
        int dMount;
        float currentRadius;

        CurrentCurveSample(step, out currentRadius, out dMount);
        
        List<int> alive = GetAlivePaths();
        if (debug) Debug.Log("Amount of path(s) alive: " + alive.Count);
        ExtrudeAlive(step, currentRadius, alive);

        if (paths.Count == dMount)
        {
            if (debug) Debug.Log ("Path(s) unchanged.");
            return;

        } else if (paths.Count < dMount)
        {
            int toAdd = dMount - paths.Count;
            Add(step, toAdd, currentRadius);
            if (debug) Debug.Log (toAdd + " path(s) added.");

        } else {
            
            int toKill = paths.Count - dMount;
            Kill(toKill, alive);
        }

    }

    private void Add (float step, int toAdd, float inputRadius)
    {
        float radius = util.RangeFromRead(rand, 0, inputRadius);
        Quaternion orient =  Quaternion.AngleAxis(90, Vector3.up); // 90 degree rotation for circle operations
        var points = util.RandomCircleCoordinates(rand, radius, toAdd, orient);

        foreach (var point in points)
        {
            point.Set(point.x + step, point.y, point.z);
            paths.Add(new Path(point, rand, set));
        }
    }

    
    private void Kill (int toKill, List<int> alive)
    {
        var prey = util.RandomIDs(rand, toKill, alive);
        int killCount = 0;

        foreach (var id in prey)
        {
            int killThresh = util.RangeFromRead(rand, set.minMaxHairLength.x, set.minMaxHairLength.y);

            if (paths[id].points.Count >= killThresh)
            {
                paths[id].Kill(); // only paths with more that 3 points shall be killed !

                killCount++;
                if (debug) Debug.Log ("Killed path at id: " + id + " containing " + paths[id].points.Count +  " point(s) | new status -> alive: " + paths[id].alive);
            } else {
                if (debug) Debug.Log ("Path at id: " + id + " wasn't killed because it contains only " + paths[id].points.Count + " point(s).");
            }
        }

        if (debug) Debug.Log(killCount + " path(s) were killed.");
    }

    private void ExtrudeAlive (float t, float currentRadius, List<int> alive)
    {
        foreach (var id in alive)
        {
            int count = paths[id].points.Count - 1; // reference to previous point

            Vector3 next = new Vector3(
                t,
                paths[id].points[count].y,
                paths[id].points[count].z
            );

            // PATH MODDING
            next += (Twirl(t + paths[id].shiftTwirl, paths[id].curlFreq, paths[id].curlRad) * paths[id].curly);
            next += RandomDisplacement(set.minMaxDisplacement);

            paths[id].points.Add(next);
        }

    }

    private void CleanUpUnfinished(int threshhold)
    {
        int index = 0;
        List<int> unfinished = new List<int>();

        foreach (var path in paths)
        {
            if (path.points.Count < threshhold)
            {
                unfinished.Add(index);
                if (debug) Debug.Log("Path at " + index + " will be removed because it contains only " + path.points.Count + " point(s).");
            }
            index++;
        }

        // courtesy of RBW_IN as seen on https://stackoverflow.com/questions/3867475/how-to-delete-multiple-entries-from-a-list-without-going-out-of-range
        paths.RemoveAll(x => unfinished.Contains(paths.IndexOf(x)));

        if (debug) Debug.Log("CLEANUP: " + unfinished.Count + " path(s) removed.");
    }

    #endregion

    #region  Helpers

    /// <summary> Using a time value t the desired amount is derived by weighing the minMax setting with a curve dependant bias. </summary>
    private void CurrentCurveSample(float step, out float currentRadius, out int desiredAmount)
    {
        float correctedStep = step / set.lengthRadius;
        float weight = set.distributionCurve.Evaluate(correctedStep);
        desiredAmount = util.WeightedRandomInRange(rand, weight, set.minMaxHairAmount);
        currentRadius = util.WeightedRandomInRange(rand, weight, set.minMaxHeightRadius);

        if (debug) Debug.Log("step: " + step + " | corrected step: " + correctedStep + " | weight: " + weight);
        if (debug) Debug.Log("current amount of path(s): " + paths.Count + " , desired amount: " + desiredAmount + " | current radius: " + currentRadius);
    }

    private List<int> GetAlivePaths()
    {
        var alive = new List<int>();
        int index = 0;

        foreach (var path in paths)
        {
            if (path.alive) alive.Add(index);
            index++;
        }

        return alive;
    }

    /// <summary> Using sin(t) as x, cos(t) as y and t as z, a twirl is created.</summary>
    Vector3 Twirl (float t, float frequency, float radius)
    {
        return new Vector3
        (
            0,
            Mathf.Sin(t * frequency),
            Mathf.Cos(t * frequency)
        ) * radius;
    }

    /// <summary> Using sin(t) as x, cos(t) as y and t as z, a twirl is created.</summary>
    Vector3 RandomDisplacement (Vector2 range)
    {
        float scale = util.RangeFromRead(rand, range.x, range.y);
        return util.RandomVector(rand, scale);
    }

    #endregion

    #region Structs
    
    private class Path
    {
        public List<Vector3> points;
        public bool alive;
        public float shiftTwirl;
        public float curly, curlFreq, curlRad;

        public Path (Vector3 origin, System.Random rand, HairSettings _set)
        {
            this.points = new List<Vector3>();
            this.points.Add(origin);
            this.alive = true;
            this.shiftTwirl = util.RangeFromRead(rand, 0f, 1f);
            this.curly = GetCurly(rand);
            this.curlFreq = util.RangeFromRead(rand, _set.minMaxTwirlFrequency.x, _set.minMaxTwirlFrequency.y);
            this.curlRad = util.RangeFromRead(rand, _set.minMaxTwirlRadius.x, _set.minMaxTwirlRadius.y);
        }

        public void Kill ()
        {
            this.alive = false;
        }

        private float GetCurly(System.Random rand)
        {
            return util.RangeFromRead(rand, 0f, 1f);
        }
    }
    
    #endregion

    #region Debugger

    public List<Vector3> DebugDraw ()
    {
        var points = new List<Vector3>();


        int index = 1;
        foreach (var path in paths)
        {
            if (debug) Debug.Log("path " + index + " of " + paths.Count + " (total) has " + path.points.Count + " point(s).");

            for (int i = 0; i < path.points.Count - 1; i++)
            {
                Debug.DrawLine(path.points[i], path.points[i + 1], Color.magenta, 999f);
            }

            points.AddRange(path.points);
            index++;
        }

        if (debug) Debug.Log(points.Count + " point(s) in total.");

        return points;
    }

    #endregion

    #region Mesh Methods

    /// <summary> Merges a list of meshes into one (submeshes stay seperate)</summary>
    private Mesh MergeMeshes (List<Mesh> meshes)
    {
        Mesh mesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[meshes.Count];

        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
        }

        mesh.CombineMeshes(combine, debug, false); // submeshes are not merged (false) and transforms are ignored (false)
        return mesh;
    }

    /// <summary>Returns the cylindrical mesh surounding the input path</summary>
    private Mesh GetMesh(Path path)
        {
            int pointsCount = path.points.Count;

            if (pointsCount < 3) return null; // exclude paths with too little anchor points

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // PathToolOperations
            BezierPath bPath = new BezierPath(path.points, false);
            VertexPath vPath = new VertexPath(bPath, transform);
            
            float meshResStep = 0.99999f / pointsCount; // temporal workaround -> instead of 1f
            float currentStep = 0f;

            
            // currentStep is NORMALIZED
            // This section is BUGGED -> Path Creation Tool maybe?
            for (int i = 0; i < pointsCount + 1; i++) // + 1 to include last point
            {
                // Debugging
                if (debug) Debug.Log("currentStep: " + currentStep);

                // Sample vertex path
                Vector3 pos = vPath.GetPointAtTime(currentStep);
                Quaternion rot = vPath.GetRotation(currentStep);

                // Sample curve for radius
                float weight = set.morphCurve.Evaluate(currentStep);
                float radius = util.WeightedRandomInRange(rand, weight, set.minMaxCylinderRadius);

                // Add circle to vertices list
                vertices.AddRange(util.ReturnCircle(pos, rot, radius, set.cylinderResolution));
                
                // Increment mesh step
                currentStep += meshResStep;
            }

            /*
            foreach (var point in path.points)
            {
                // Debugging
                if (debug) Debug.Log("currentStep: " + currentStep);

                // Sample vertex path
                Vector3 pos = vPath.GetPointAtTime(currentStep);
                Quaternion rot = vPath.GetRotation(currentStep);

                // Sample curve for radius
                float weight = set.morphCurve.Evaluate(currentStep);
                float radius = util.WeightedRandomInRange(rand, weight, set.minMaxCylinderRadius);

                // Add circle to vertices list
                vertices.AddRange(util.ReturnCircle(pos, rot, radius, set.cylinderResolution));
                
                // Increment mesh step
                currentStep += meshResStep;
            }
            */

            // Add triangles
            triangles.AddRange(GetTubeTris(set.cylinderResolution, pointsCount)); // tubes
            triangles.InsertRange(0, GetCap(set.cylinderResolution, 0)); // insert startcap at the beginning
            triangles.Reverse(); // flip normals
            triangles.AddRange(GetCap(set.cylinderResolution, vertices.Count - set.cylinderResolution)); // add endcap

            if (vertices.Count < 2 * set.cylinderResolution) return null; // exclude incomplete tubes

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }

        private List<int> GetTubeTris(int resolution, int pathLength)
        {
            List<int> tris = new List<int>();

            for (int i = 0; i < pathLength; i++) // upper level: tube segments
            {
                int offset = i * resolution;
                int firstVert = 0;
                int secondVert = 0;

                for (int j = 0; j < resolution; j++) // lower level: quads
                {
                    if (j == 0)
                    {
                        firstVert = offset;
                        secondVert = offset + resolution;
                    }

                    if (j != resolution - 1)
                    {
                        tris.Add(offset + j);
                        tris.Add(offset + j + resolution);
                        tris.Add(offset + j + 1);

                        tris.Add(offset + j + 1);
                        tris.Add(offset + j + resolution);
                        tris.Add(offset + j + resolution + 1);
                    } else
                    {
                        tris.Add(offset + j);
                        tris.Add(offset + j + resolution);
                        tris.Add(firstVert);

                        tris.Add(firstVert);
                        tris.Add(offset + j + resolution);
                        tris.Add(secondVert);
                    }
                }
            }
            return tris;
        }

        private List<int> GetCap(int resolution, int offset)
        {
            //Debug.Log("resolution: " + resolution);
            List<int> tris = new List<int>();
            for (int i = 0, j = 0; i < resolution; i++)
            {
                tris.Add(offset + ((resolution - j) % resolution));
                //Debug.Log(offset + ((resolution - j) % resolution));
                tris.Add(offset + (i + 1));
                //Debug.Log(offset + (i + 1));
                tris.Add(offset + (resolution - 1 - i));
                //Debug.Log(offset + (resolution - 1 - i));

                j++;
                //Debug.Log("i: " + i + " j: " + j);
                if (tris.Count == (resolution - 2) * 3)
                {
                    //Debug.Log("first break");
                    break;
                }

                tris.Add(offset + ((resolution - j) % resolution));
                //Debug.Log(offset + ((resolution - j) % resolution));
                tris.Add(offset + (i + 1));
                //Debug.Log(offset + (i + 1));
                tris.Add(offset + (i + 2));
                //Debug.Log(offset + (i + 2));

                //Debug.Log("i: " + i + " j: " + j);
                if (tris.Count == (resolution - 2) * 3)
                {
                    //Debug.Log("second break");
                    break;
                }
            }
            return tris;
        }

        private void CenterOutputMesh ()
        {
            output.RecalculateBounds();

            Bounds offCenter = output.bounds;
            Vector3 translation = transform.position - offCenter.center;

            Vector3[] verts = output.vertices;

            if (debug) Debug.Log (" Mesh center needs to be moved -> " + translation);

            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 old = verts[i];
                verts[i] += translation;
                //if (debug) Debug.Log("Moved from " + old + " to " + verts[i]);
            }

            output.vertices = verts;
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
