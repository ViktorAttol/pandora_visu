using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;

public class ChaosController : MonoBehaviour
{
    public enum AnimationState
    {
        Idle, Inactive, Start, Run, FadeOut
    }

    private readonly float[] NUC_RATIOS = new float[] { 0.283f, 0.207f, 0.207f, 0.303f };

    public VisualEffect chaos;
    public VisualEffect background;
    public VisualEffect cloud;

    public GameObject cage;
    

    private Bounds cageBounds;
    private Material cageMaterial;

    [SerializeField] private int nucleotide;
    [SerializeField] private int quality;
    [SerializeField] private int signal;

    [SerializeField] private int[] nucCounts = new int[4];
    [SerializeField] private Vector3[] speeds = new Vector3[4];
    [SerializeField] private Vector3[] magnitudes = new Vector3[4];
    [SerializeField] private Vector3[] positions = new Vector3[4];
    [SerializeField] private float[] radia = new float[4];
    [SerializeField] private float intensity = 0.1f;

    public AnimationState state = AnimationState.Inactive;

    // Start is called before the first frame update
    void Start()
    {
        cageBounds = cage.GetComponent<Renderer>().bounds;
        cageMaterial = cage.GetComponent<Renderer>().material;
        cageMaterial.SetFloat("_Alpha", 0.0f);
        nucleotide = -1;
        quality = 0;
        signal = 400;
        chaos.SetVector3("Cage_Size", cageBounds.size);
        chaos.enabled = false;
        background.enabled = false;

        //StartCoroutine(startFadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        StateLoop();
    }

    public void setNucleotide(int nextNuc)
    {
        this.nucleotide = nextNuc;
    }

    public void setQualityScore(int nextScore)
    {
        this.quality = nextScore;
        chaos.SetVector3("Turbulence_Multiplier", getTurbulenceMultiplier());

    }

    public void setSignal(int nextSignal)
    {
        this.signal = nextSignal;
    }

    private void StateLoop()
    {
        switch (state)
        {
            case AnimationState.Idle:
                OnCaseIdle();
                break;
            case AnimationState.Inactive:
                OnCaseInactive();
                break;
            case AnimationState.Start:
                OnCaseStart();
                break;
            case AnimationState.Run:
                OnCaseRun();
                break;
            case AnimationState.FadeOut:
                OnCaseFadeOut();
                break;

        }
    }

    private void OnCaseIdle()
    {

    }

    private void OnCaseInactive()
    {
        if (chaos.isActiveAndEnabled)
            chaos.enabled = false;

        if (background.isActiveAndEnabled)
            background.enabled = false;

        if (cloud.isActiveAndEnabled)
            cloud.enabled = false;

    }

    private void OnCaseStart()
    {
        if (cageMaterial.GetFloat("_Alpha") < 1.0f)
            fadeCage(0.001f);
        else
        {
            state = AnimationState.Run;
        }
    }

    private void OnCaseRun()
    {
        if (!chaos.isActiveAndEnabled)
            chaos.enabled = true;

        if (!background.isActiveAndEnabled)
            background.enabled = true;

        setNucleotideCounts(nucleotide);
        updateNucleotideSpheres();
        updateChaos();
        setBackgroundTransform();
    }

    private void OnCaseFadeOut()
    {
        if (cageMaterial.GetFloat("_Alpha") > 0.0f)
        {
            fadeCage(-0.001f);
            if (cage.transform.position.z < -5.0f)
            {
                moveToCamera(0.001f);
                updateNucleotideSpheres();
            }
        } else 
        {
            cageMaterial.SetFloat("_Alpha", 0.0f);
            StartCoroutine(stopGraphs(10));
        } 
    }

    private void fadeCage(float stepSize)
    {
        float alpha = cageMaterial.GetFloat("_Alpha");
        //Debug.Log("Alpha: " + alpha);
        cageMaterial.SetFloat("_Alpha", alpha + stepSize); 
    }

    private void moveToCamera(float stepSize)
    {
        float z = cage.transform.position.z;
        cage.GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, z + stepSize);
        if (z < 0.0f)
        {
            chaos.GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, z + stepSize);
        }
    }

    private IEnumerator stopGraphs(float delay)
    {
        yield return new WaitForSeconds(delay);

        background.Stop();
        chaos.Stop();
    }


    private IEnumerator startFadeOut()
    {
        yield return new WaitForSeconds(120);
        state = AnimationState.FadeOut;
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
    }

    private Vector3 getTurbulenceMultiplier()
    {
        return new Vector3(0, quality, 0);
    }

    private void updateNucleotideSpheres()
    {
        for (int i = 0; i < 4; i++)
        {
            magnitudes[i] = getMagnitude(i);
            speeds[i] = getSpeed(i);
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

        intensity = getNucTurbulenceIntensity();
        chaos.SetFloat("Turbulence_Intensity", intensity);

    }

    private Vector3 getSpeed(int nucInd)
    {
        float ratio = (float) nucCounts[nucInd] / nucCounts.Sum();
        return new Vector3(1, ratio, 1);
    }

    private Vector3 getMagnitude(int nucInd)
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

    private float getNucTurbulenceIntensity()
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
