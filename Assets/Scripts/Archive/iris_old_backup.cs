
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;

/*
public class Iris
{
    #region Fields

    // properties
    List<IRPath> paths;
    public int circleID;

    //statics
    static IrisData irisData;

    #endregion

    #region Constructors

    /// <summary>Iris object used to manage processes for creation of a unique iris mesh.</summary>

    // cunstructor
    public Iris(IrisData _irisData)
    {
        circleID = 0;
        irisData = _irisData;
        paths = new List<IRPath>();
        Initialize();
    }

    #endregion

    #region Methods

    // methods
    public void Initialize()
    {
        for (int i = 0; i < irisData.numInitPoints; i++)
        {
            paths.Add(new IRPath());
        }
    }

    public void Expand()
    {
        foreach (var IRPath in paths)
        {
            IRPath.Extrude(circleID);
        }
        circleID += 1;
    }

    public void DebugDraw()
    {
        foreach (IRPath IRPath in paths)
        {
            IRPath.DebugDraw();
        }
    }

    
    public List<IRPoint> GetNeighbors(int circleID)
    {
        List<IRPoint> results = new List<IRPoint>(); 
        foreach (var IRPath in paths)
        {
            results.Add(IRPath.GetPoint(circleID));
        }
        return results;
    }
    

    #endregion

    #region  Helper - Classes and Structs

    public class IRPath
    {
        #region Fields

        // properties
        List<IRPoint> points;
        bool alive;
        int pathID;
        float step;
        float currentRadius;

        // statics
        static int pathCount;

        #endregion

        #region Constructors

        /// <summary>An IrisPath consists of a list of IRPoints.</summary>
        /// <param name="start"> A startpoint is required for instantiation.</param>

        // constructor
        public IRPath()
        {
            alive = true;
            pathID = pathCount;
            step = irisData.totalRadius / irisData.radialSteps;
            points = new List<IRPoint>();
            AddPoint(util.RandomCircleCoordinate(2), 0);
            pathCount++;
        }

        #endregion

        #region Methods
        // methods

        /// <summary>Adds a point to the IRPath.</summary>
        /// <param name="point">Vector 3 to be added.</param>
        void AddPoint(Vector3 point, int id)
        {
            points.Add(new IRPoint(point, id));
        }

        
        public IRPoint GetPoint(int pointID)
        {
            return points[pointID];
        }

        /// <summary>Disables IRPath executed on.</summary>
        void Kill()
        {
            alive = false;
        }
        
        public void Extrude(int circleID)
        {
            Vector3 position =  points[points.Count - 1].position;
            Vector3 displacement = (position - irisData.origin).normalized * step;
            displacement += util.RandomVector(irisData.displacementLimit);
            AddPoint(position + displacement, circleID);
            currentRadius += step;
        }

        public void DebugDraw()
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Debug.DrawLine(points[i].position, points[i + 1].position, Color.white);
            }
        }

        #endregion
    }

    public class IRPoint
    {
        #region Fields

        public Vector3 position;
        public float radius;
        public int circleID;

        #endregion

        #region Statics

        static int pointCount;

        #endregion

        #region Constructor

        /// <summary>An IrisPoint consists of a position (Vector3), radius (float) and ID (int). </summary>
        public IRPoint (Vector3 _pos, int _circleID)
        {
            position = _pos;
            radius = GetRadius(_pos);
            circleID = _circleID;
            pointCount++;
        }

        #endregion

        #region Methods

        /// <summary>Returns radius based on distribution gradient.</summary>
        float GetRadius(Vector3 point)
        {
            //Debug.Log("Radius");
            float distToOrigin = Vector3.Distance(point, irisData.origin);
            Color sampleGradient = irisData.stageDistribution.Evaluate(distToOrigin);
            Color.RGBToHSV(sampleGradient, out float h, out float s, out float v);
            float radius = v * irisData.maxPointRadius;
            return radius;
        }

        #endregion
    }

    #endregion
}

// WORKING PROTOTYPE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PandoraUtils;

public class Iris
{
    #region Fields
    static IrisData irisData;
    int circleID;
    float stepSize;
    float radialStep;
    List<IRPath> paths;
    bool debug;
    bool iterationRunning;
    int pointsAmount;
    #endregion

    #region Constructor
    public Iris(IrisData _irisData, bool debugMode)
    {
        irisData = _irisData;
        stepSize = irisData.totalRadius / irisData.radialSteps;
        circleID = 0;
        radialStep = 0;
        paths = new List<IRPath>();
        debug = debugMode;
        if (debug) Debug.Log("Iris created! \n stepSize = " + stepSize);
        iterationRunning = true;
    }
    #endregion

    #region Public Access
    public void Iterate()
    {
        // GUARD PATTERN
        if (radialStep >= irisData.totalRadius)
        {
            if (debug && iterationRunning)
            {
                iterationRunning = false;
                Debug.Log("iteration ended.");
            }
            return;
        }

        // DEBUG BLOCK
        if (debug) Debug.Log(
            "IRIS FIELDS DEBUG LOOP \n" +
            "circleID = " + circleID + "\n" +
            "radialStep = " + radialStep + "\n" +
            "allPathsAmount = " + paths.Count + "\n" +
            "alivePathsAmount = " + CountPathsAlive()
        );

        // METHOD CALLING
        pointsAmount = GetPointsAmount(radialStep);
        int pathsAlive = CountPathsAlive();
        UpdatePaths(pathsAlive);
        UpdatePoints(pathsAlive);

        // ITERATION COUNT
        radialStep += stepSize;
        circleID++;
    }

    public void DebugDraw()
    {
        foreach (var IRPath in paths)
        {
            IRPath.DebugDraw();
        }
    }
    #endregion

    #region Internal Methods
    internal void UpdatePaths(int pathsAlive)
    {
        int pathsDiff;
        if (pathsAlive == pointsAmount)
        {
            if (debug) Debug.Log("No path update required.");
            return;
        } else if (pathsAlive > pointsAmount) {
            pathsDiff = pathsAlive - pointsAmount;
            if (debug) Debug.Log("pathsDiff (toKill) = " + pathsDiff);
            KillPaths(pathsDiff);
        } else {
            pathsDiff = pointsAmount - pathsAlive;
            if (debug) Debug.Log("pathsDiff (toAdd) = " + pathsDiff);
            AddPaths(pathsDiff);
        }
    }

    internal void AddPaths(int toAdd)
    {
        for (int i = 0; i < toAdd; i++)
        {
            IRPoint start = DistributeNewPoints(1)[0];
            paths.Add(new IRPath(start));
        }
        Debug.Log("path(s) added.");
    }

    internal void KillPaths(int toKill)
    {
        List<int> killID = util.RandomIDs(toKill, GetPathsAlive());

        for (int i = 0; i < toKill; i++)
        {
            paths[killID[i]].Kill();
        }
    }

    internal void UpdatePoints(int pathsAlive)
    {
        List<IRPoint> points = DistributeNewPoints(pointsAmount - pathsAlive);
        if (points != null) Debug.Log(points.Count + " new point(s)");
        
        foreach (var IRPath in paths)
        {
            if (IRPath.IsAlive())
            {
                ExtrudeExisting(IRPath);
            }
        }

        Debug.Log("point(s) updated.");
    }

    internal void ExtrudeExisting(IRPath path)
    {
        Vector3 position =  path.points[path.points.Count - 1].position;
        Vector3 displacement = (position - irisData.origin).normalized * radialStep;
        displacement += util.RandomVector2D(irisData.displacementLimit);
        IRPoint point = new IRPoint (
            position + displacement,
            circleID,
            path.points[path.points.Count - 1].angle);
        path.AddPoint(point);
    }

    internal float LastAngle(IRPath Path)
    {
        int lastPoint = Path.points.Count - 1;
        float theta = Path.points[lastPoint].angle;
        Debug.Log("theta = " + theta);
        return theta;
    }

    internal IRPoint FindClosest(float angle, List<IRPoint> points)
    {
        Debug.Log(points.Count + " point(s) in list sent into method");
        float rangeMin = (angle - irisData.displacementLimit) % 360f;
        float rangeMax = (angle + irisData.displacementLimit) % 360f;
        List<IRPoint> results = points.FindAll(x => x.angle >= rangeMin && x.angle <= rangeMax);
        Debug.Log(results.Count + " candidate(s)");
        IRPoint result = results[Random.Range(0,results.Count)];
        return result;
    }

    internal List<IRPoint> DistributeNewPoints(int count)
    {
        if (count == 0) return null;

        List<IRPoint> points = new List<IRPoint>();

        for (int i = 0; i < count; i++)
        {
            float angleStep = 360 / count;
            float random = Random.Range(-angleStep, angleStep);
            float theta = i * angleStep + random;

            points.Add(new IRPoint(
                util.CircleCoordinate(radialStep, theta),
                circleID,
                theta
            ));
        }

        // Sorting points by angle
        points.Sort((x, y) => x.angle.CompareTo(y.angle));

        return points;
    }

    internal int GetPointsAmount(float radius)
    {
        float t = radius / irisData.totalRadius;
        float weight = SampleDistributionGradient(t);
        //Debug.Log("gradient sampled at " + t + ", weight factor: " + weight);
        int amount = Random.Range((int)(irisData.minStepResolution * weight),(int)((irisData.maxStepResolution + 1) * weight));
        // + 1 since when using Random.Range as int the upper range changes into an exclusive
        return amount;
    }

    internal List<int> GetPathsAlive()
    {
        List<int> aliveIDs = new List<int>();

        int i = 0;
        foreach (var IRPath in paths)
        {
            bool check = IRPath.IsAlive();
            if (check) aliveIDs.Add(i);
            i++;
        }
        return aliveIDs;
    }

    internal int CountPathsAlive()
    {
        int amt = 0;
        foreach (var IRPath in paths)
        {
            bool check = IRPath.IsAlive();
            if (check) amt++;
        }
        return amt;
    }

    internal float SampleDistributionGradient(float t)
    {
        Color sampleGradient = irisData.weightDistribution.Evaluate(t);
        Color.RGBToHSV(sampleGradient, out float h, out float s, out float v);
        return v;
    }
    #endregion

    #region Subclasses and Helpers
    internal class IRPath
    {
        #region Fields
        internal bool alive;
        internal List<IRPoint> points;
        #endregion

        #region Constructor
        internal IRPath(IRPoint start)
        {
            alive = true;
            points = new List<IRPoint>();
            points.Add(start);
        }
        #endregion

        #region Methods
        internal void AddPoint(IRPoint point)
        {
            points.Add(point);
        }

        internal bool IsAlive()
        {
            if (alive == true)
            {
                return true;
            } else {
                return false;
            }
        }

        internal bool IsNew()
        {
            if (points.Count == 1)
            {
                return true;
            } else {
                return false;
            }
        }

        internal void Kill()
        {
            alive = false;
        }

        internal void DebugDraw()
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Debug.DrawLine(points[i].position, points[i + 1].position, Color.white);
            }
        }
        #endregion
    }

    internal struct IRPoint
    {
        internal int circleID;
        internal float angle;
        internal float cylRadius;
        internal Vector3 position;

        internal IRPoint (Vector3 _pos, int _cID, float _angle)
        {
            this.position = _pos;
            this.circleID = _cID;
            this.angle = _angle;
            this.cylRadius = 0;
        }
    }
    #endregion
}
*/