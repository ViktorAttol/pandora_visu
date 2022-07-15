using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;
using PathCreation;

public class Iris
{
    #region Properties
    static Transform transform;
    static Vector3 origin;
    static IrisSettings irisSet;
    bool running, debug;

    float step, currentStep, currentWeight;
    int stepNo, pointBudget;

    List<IRPath> allPaths;
    List<int> alivePaths;
    #endregion

    #region Constructor
    public Iris(IrisSettings _irisSettings, Transform _trans)
    {
        transform = _trans;
        origin = transform.position;
        irisSet = _irisSettings;
        debug = irisSet.debug;
        running = true;

        stepNo = 0;
        currentStep = 0;
        currentWeight = 0;
        step = irisSet.totalRadius / irisSet.radialSteps;

        allPaths = new List<IRPath>();

        // Debugging
        if (debug) Debug.Log("Iris created!");
    }
    #endregion

    #region Public Access
    public void Update()
    {
        // GUARD PATTERN to stop execution upon completion
        if (stepNo == irisSet.radialSteps)
        {
            if (debug && running)
            {
                running = false;
                Debug.Log("iris compledted.");
            }
            return;
        }

        // CYCLE SETUP
        // Determine step of the current cycle
        currentStep = stepNo * step;

        // Determine gradient factor by sampling
        //currentGrad = SampleGradient(currentStep);
        currentWeight = SampleCurve(currentStep);
        
        // Determine cycles point budget
        pointBudget = GetPointsBudget(currentWeight);
        // Retrieve IDs of alive paths
        alivePaths = GetAlivePaths();
        // Calculate how many points to spawn / kill
        int diffToPrev = pointBudget - alivePaths.Count;

        // PATH MANAGEMENT
        ManagePaths(diffToPrev);

        // EXTRUDE alive paths older than 1 cycle
        foreach (var id in alivePaths)
        {
            allPaths[id].Extrude(irisSet, step, currentStep);
        }

        // COMPLETE CYCLE by calculationg next step number
        stepNo++;
    }

    // create a mesh from the current paths
    public Mesh GenerateMesh()
    {
        // Mesh Data
        List<Mesh> meshes = new List<Mesh>();

        // add all vertices from all paths
        foreach (var path in allPaths)
        {
            Mesh current = path.GetMesh();
            if (current != null) meshes.Add(current);
        }

        return MergeMeshes(meshes);
    }

    public void DebugDraw()
    {
        foreach (var path in allPaths)
        {
            path.DebugDraw();
        }
    }

    public bool IsRunning()
    {
        return running;
    }
    #endregion

    #region Internal Methods
    private float SampleCurve(float radius)
    {
        float t = radius / irisSet.totalRadius;
        float v = Mathf.Clamp(irisSet.weightDistributionCurve.Evaluate(t), 0, 1);
        return v;
    }

    private int GetPointsBudget(float weight)
    {
        int amount = util.WeightedRandomInRange(weight, irisSet.minMaxStepResolution);
        return amount;
    }

    private List<int> GetAlivePaths()
    {
        List<int> alive = new List<int>();

        int i = 0;
        foreach (var path in allPaths)
        {
            if (path.IsAlive()) alive.Add(i);
            i++;
        }
        return alive;
    }

    private void ManagePaths(int diffToPrev)
    {
        if (diffToPrev == 0)
        {
            return;
        }
        else if (diffToPrev < 0)
        {
            diffToPrev = -diffToPrev;

            // Prevent paths with less than 3 points from being killed
            List<int> selection = new List<int>();
            foreach (var path in alivePaths)
            {
                if (allPaths[path].Count() >= 3) selection.Add(path);
            }

            List<int> prey = util.RandomIDs(diffToPrev, selection);

            foreach (var id in prey)
            {
                allPaths[id].Kill();
                alivePaths.Remove(id);
            }
        }
        else
        {
            for (int i = 0; i < diffToPrev; i++)
            {
                allPaths.Add(new IRPath(currentStep));
            }
        }
    }

    // Merges a list of meshes into one mesh
    private Mesh MergeMeshes (List<Mesh> meshes)
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        CombineInstance[] combine = new CombineInstance[meshes.Count];

        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            //combine[i].transform = transform.localToWorldMatrix;
        }

        mesh.CombineMeshes(combine, true, false); // submeshes are merged (true) and transforms are ignored (false)
        return mesh;
    }
    #endregion

    #region Subclasses and Structs
    public class IRPath
    {
        #region Properties
        private bool alive;
        private List<IRPoint> points;
        #endregion

        #region Constructor
        public IRPath(float radius)
        {
            alive = true;
            points = new List<IRPoint>();
            AddPoint(radius);
        }
        #endregion

        #region Public Access
        public void AddPoint(float radius)
        {
            IRPoint point = CreateRandomPoint(radius);
            points.Add(point);
        }

        public Vector3[] GetPoints()
        {
            Vector3[] points = new Vector3[this.points.Count];
            for (int i = 0; i < this.points.Count; i++)
            {
                points[i] = this.points[i].position;
            }
            return points;
        }

        public void Extrude(IrisSettings irisSettings, float step, float currentStep)
        {
            Vector3 position =  points[points.Count - 1].position;
            Vector3 displacement = (position - origin).normalized * step;
            displacement += util.RandomVector2D(irisSettings.displacementLimit);
            Vector3 result = position + displacement;
            result.z = SampleCurve(currentStep) * irisSettings.depthFactor;
            IRPoint point = new IRPoint ( result, currentStep );
            points.Add(point);
        }

        public bool IsAlive()
        {
            if (alive == true)
            {
                return true;
            } else {
                return false;
            }
        }

        public void Kill()
        {
            alive = false;
        }

        public void DebugDraw()
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Debug.DrawLine(points[i].position, points[i + 1].position, Color.magenta);
            }
        }

        public int Count()
        {
            return points.Count;
        }
        
        public Mesh GetMesh()
        {
            if (this.points.Count < 2) return null;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            VertexPath vPath = GetVertexPath(this);

            int pointsCount = this.Count();
            float meshResStep = 1f / (pointsCount + 1);

            float currentStep = 0f;

            for (int i = 0; i < pointsCount + 1; i++) // + 1 to include last point
            {
                //Debug.Log("currentStep: " + currentStep);

                // Sample vertex path
                Vector3 pos = vPath.GetPointAtTime(currentStep);
                Quaternion rot = vPath.GetRotation(currentStep);

                // Sample curve for radius
                float t = Vector3.Distance(pos, origin);
                float weight = SampleCurve(t);
                float radius = util.WeightedRandomInRange(weight, irisSet.minMaxCylinderRadius);

                // Add circle to vertices list
                vertices.AddRange(util.ReturnCircle(pos, rot, radius, irisSet.cylinderResolution));
                
                // Increment mesh step
                currentStep += meshResStep;
            }

            // Add triangles
            triangles.AddRange(GetTubeTris(irisSet.cylinderResolution, pointsCount)); // tubes
            triangles.InsertRange(0, GetCap(irisSet.cylinderResolution, 0)); // insert startcap at the beginning
            triangles.Reverse(); // flip normals
            triangles.AddRange(GetCap(irisSet.cylinderResolution, vertices.Count - irisSet.cylinderResolution)); // add endcap
            
            /*
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Debug.DrawLine(vertices[i], vertices[i + 1], Color.cyan, 2000f);
            }
            */

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            return mesh;
        }
        
        #endregion

        #region Internal Methods
        private IRPoint CreateRandomPoint(float radius)
        {
            Vector3 pos = util.RandomCircleCoordinate(radius);
            pos.z = SampleCurve(radius) * irisSet.depthFactor;
            return new IRPoint (pos, radius);
        }

        private float SampleCurve(float radius)
        {
           float t = radius / irisSet.totalRadius;
           float v = Mathf.Clamp(irisSet.weightDistributionCurve.Evaluate(t), 0, 1);
           return v;
        }

        private VertexPath GetVertexPath(IRPath path)
        {
            BezierPath bPath = new BezierPath(path.GetPoints(), false);
            return new VertexPath(bPath, transform);
        }
        private List<int> GetTubeTris(int resolution, int pathLength)
        {
            List<int> tris = new List<int>();
            for (int i = 0; i < pathLength; i++) // upper level: tube segments
            {   
                int offset = i * resolution;
                int firstV = 0;
                int secondV = 0;
                for (int j = 0; j < resolution; j++) // lower level: quads
                {
                    if (j == 0)
                    {
                        firstV = offset;
                        secondV = offset + resolution;
                    }

                    if (j != resolution - 1)
                    {
                        tris.Add(offset + j);
                        tris.Add(offset + j + resolution);
                        tris.Add(offset + j + 1);

                        tris.Add(offset + j + 1);
                        tris.Add(offset + j + resolution);
                        tris.Add(offset + j + resolution + 1);
                    }else
                    {
                        tris.Add(offset + j);
                        tris.Add(offset + j + resolution);
                        tris.Add(firstV);

                        tris.Add(firstV);
                        tris.Add(offset + j + resolution);
                        tris.Add(secondV);
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
        #endregion
    }

    private struct IRPoint
    {
        public float step;
        public Vector3 position;

        public IRPoint (Vector3 _pos, float _step)
        {
            this.position = _pos;
            this.step = _step;
        }

    }
    #endregion
}