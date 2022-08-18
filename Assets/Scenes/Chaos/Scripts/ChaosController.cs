using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;

public class ChaosController : MonoBehaviour
{
    private readonly float[] NUC_RATIOS = new float[]{0.283f, 0.207f, 0.207f, 0.303f};

    public VisualEffect chaos;
    public VisualEffect background;
    //public VisualEffect phenotype;

    public GameObject cage;
    public bool active = true;

    [SerializeField]  private Bounds cageBounds;

    [SerializeField] private int nucleotide = -1;
    [SerializeField] private int quality = 0;
    [SerializeField] private int acc_quality = 0;
    [SerializeField] private int signal = 0;
    [SerializeField] private int count = 0;

    [SerializeField]  private int[] nucCounts = new int[4];
    [SerializeField]  private Vector3[] speeds = new Vector3[4];
    [SerializeField]  private Vector3[] magnitudes = new Vector3[4];
    [SerializeField]  private Vector3[] positions = new Vector3[4];
    [SerializeField]  private float[] radia = new float[4];
    [SerializeField] private float intensity = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        cageBounds = cage.GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            setNucleotideCounts(nucleotide);
            updateNucleotideSpheres();
            updateChaos();
            setBackgroundTransform();
        }
    }


    public void getNextNuc(int nextNuc)
    {
        nucleotide = nextNuc;
    }

    public void getNextScore(int nextScore)
    {
        quality = nextScore;
        acc_quality += quality;
        count ++;
    }

    public void getNextSignal(int nextSignal)
    {
        signal = nextSignal;
    }

    private void setNucleotideCounts(int nextNuc)
    {
        if (nextNuc >= 0)
        {
            nucCounts[nextNuc]++;

            chaos.SetInt("Sequence_Length", nucCounts.Sum());
            chaos.SetInt("A_Count", nucCounts[0]);
            chaos.SetInt("C_Count", nucCounts[1]);
            chaos.SetInt("G_Count", nucCounts[2]);
            chaos.SetInt("T_Count", nucCounts[3]);
        }
    }

    private void updateChaos()
    {
        chaos.SetVector3("Chaos_Bounds", magnitudes[0]);
        chaos.SetVector3("Chaos_Magnitude", magnitudes[1]);
        chaos.SetVector3("Turbulence_Multiplier", getTurbulenceMultiplier());
    }

    private Vector3 getTurbulenceMultiplier()
    {
        int mag = quality;

        return new Vector3(0, mag / 2, 0);
    }

    private void updateNucleotideSpheres()
    {
        for (int i = 0; i < 4; i++)
        {
            magnitudes[i] = setMagnitude(i);
            speeds[i] = setSpeed(i);
            positions[i] = getSpherePosition(speeds[i], magnitudes[i]);
            radia[i] = getSphereRadius(i, 0.1f);
        }

        chaos.SetVector3("A_Pos", positions[0]);
        chaos.SetFloat("A_Radius", radia[0]);

        chaos.SetVector3("C_Pos", positions[1]);
        chaos.SetFloat("C_Radius", radia[1]);

        chaos.SetVector3("G_Pos", positions[2]);
        chaos.SetFloat("G_Radius", radia[2]);

        chaos.SetVector3("T_Pos", positions[3]);
        chaos.SetFloat("T_Radius", radia[3]);

        intensity = setNucTurbulenceIntensity();
        chaos.SetFloat("Turbulence_Intensity", intensity);

    }

    private Vector3 setSpeed(int nucInd)
    {
        float ratio = (float) nucCounts[nucInd] / nucCounts.Sum();
        return new Vector3(1, ratio, 1);
    }

    private Vector3 setMagnitude(int nucInd)
    {
        float mag = magnitudes[nucInd].x;

        if (mag < cageBounds.extents.z)
        {
            mag = nucCounts[nucInd] * 0.001f;
        }

        return new Vector3(mag, mag, mag);
    }

    private Vector3 getSpherePosition(Vector3 speed, Vector3 magnitude)
    {
        float x = Mathf.Sin(Time.time * speed.x) * magnitude.x;
        float y = Mathf.Cos(Time.time * speed.y) * magnitude.y;
        float z = Mathf.Cos(Time.time * speed.z) * magnitude.z;

        return new Vector3(x, y, z);
    }

    private float getSphereRadius(int nucInd, float multiplier)
    {
        float size = NUC_RATIOS[nucInd] * multiplier;

        if (radia[nucInd] < size)
            return radia[nucInd] + multiplier;
        else
        {
            float radius = (float) nucCounts[nucInd] / nucCounts.Sum();
            return radius * multiplier;
        }
    }

    private float setNucTurbulenceIntensity()
    {
        if (intensity > 0.1f)
            intensity -= 0.01f;

        if (signal < 0 || signal > 800)
            intensity = 1.0f;

        return intensity;
    }

    private void setBackgroundTransform()
    {
        if (background.transform.localScale.x < 1.0f)
            background.transform.localScale = magnitudes[1];
    }

}
