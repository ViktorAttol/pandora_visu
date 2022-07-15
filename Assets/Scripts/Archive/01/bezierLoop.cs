using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezierLoop : MonoBehaviour
{
    [SerializeField] int randomRange = 10;
    [SerializeField] float bezierResolution = 0.2f;
    Vector3[] path = new Vector3[10];
    Vector3 lastPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = new Vector3(Random.Range(-randomRange,randomRange), Random.Range(-randomRange,randomRange), Random.Range(-randomRange,randomRange));
        }
    }

    
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < path.Length;)
            {
                int loops = Mathf.FloorToInt(1f / bezierResolution);

                for (int j = 1; j <= loops; j++)
                {
                    //Which t position are we at?
                    float t = j * bezierResolution;

                    //Find the coordinates between the control points with a Catmull-Rom spline
                    Vector3 newPos = bezierInterpol(t, path, i);

                    //Draw this line segment
                    Gizmos.DrawLine(lastPos, newPos);

                    //Save this pos so we can draw the next line segment
                    lastPos = newPos;
                }

                // Skipping every other point
                i = i + 2;
            }
        }
    }


    Vector3 bezierInterpol(float t, Vector3[] path, int i)
    {
        //Linear interpolation = lerp = (1 - t) * A + t * B
        //Could use Vector3.Lerp(A, B, t)

        //To make it faster
        float oneMinusT = 1f - t;

        Vector3 A = path[i];
        Vector3 B = path[(i + 2) % path.Length];
        Vector3 C = path[(i + 1) % path.Length];
        
        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * C;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }
}
