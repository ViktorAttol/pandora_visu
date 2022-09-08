using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;

public class PhenotypeAnimationDisplayController : MonoBehaviour
{
    public PhenotypeSDFBaker sdfBaker;
    public PhenotypeAnimationDataPreprocessor dataPreprocessor;
    public VFXSettings vfxSet;
    private float  lastAlpha;
    
    private List<PhenoDisplayData> preprocessedPhenotypes = new List<PhenoDisplayData>();
    private List<PhenoDisplayData> displayedPhenotypes = new List<PhenoDisplayData>();

    private float remainingPlayTime = -1f;

    private bool isRunning = false;
    
    [SerializeField] VisualEffect vfx;
    
    // Start is called before the first frame update
    void Start()
    {
        dataPreprocessor.SubscribeForPhenotypeDataPreprocessed(OnPhenotypePreprocessed);
        lastAlpha = 0;
        StartVfx();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            remainingPlayTime -= Time.deltaTime;
            if (remainingPlayTime <= 0)
            {
                EndPhenotypeAnimation();
                isRunning = false;
                PhenoDisplayData currPheno = preprocessedPhenotypes[0];
                displayedPhenotypes.Add(currPheno);
                preprocessedPhenotypes.Remove(currPheno);
            }
        }
        else
        {
            if (preprocessedPhenotypes.Count > 0)
            {
                isRunning = true;
                //remainingPlayTime = PhenoDisplayData.phenoAnimationDuration;
                remainingPlayTime = vfxSet.displayDuration;
                StartPhenotypeAnimation();
            }
        }
    }

    private void OnPhenotypePreprocessed(PhenoDisplayData phenoData)
    {
        Debug.Log("phenotype preprocessed and ready to display " + phenoData.phenotype);
        preprocessedPhenotypes.Add(phenoData);
    }

    private void StartPhenotypeAnimation()
    {
        print("phenotype animation started.");
        //preprocessedPhenotypes[0].sdf = sdfBaker.GetSDF(preprocessedPhenotypes[0].phenoClassData.GetMesh());
        vfx.SetTexture("sdf", sdfBaker.GetSDF(preprocessedPhenotypes[0].phenoClassData.GetMesh()));
        vfx.SetTexture("color_input", preprocessedPhenotypes[0].colorTexture);
        vfx.SetMesh("inputMesh", preprocessedPhenotypes[0].phenoClassData.GetMesh());

        InitializeVFX(); // -> Apply settings and set up box size from sdf baker
        StartCoroutine(FadeAlpha(vfxSet.crossFadeDuration, lastAlpha, preprocessedPhenotypes[0].phenoAlphaValue)); // -> Start Fade into next Phenotype
        lastAlpha = preprocessedPhenotypes[0].phenoAlphaValue;
    }

    private void InitializeVFX()
    {
        vfx.SetVector3("boxSize", sdfBaker.sizeBox); // Read from PhenotypeSDFBaker Settings

        // Apply Settings from SO
        vfx.SetFloat("post_scale", vfxSet.postScale);
        vfx.SetFloat("randomStartVelocity", vfxSet.randomStartVelocity);
        vfx.SetFloat("attractionSpeedForce", vfxSet.attractionSpeedForce);
        vfx.SetFloat("blobSize", vfxSet.blobSize);
        vfx.SetFloat("knobSize", vfxSet.knobSize);
        vfx.SetFloat("trisSize", vfxSet.trisSize);
        vfx.SetInt("trisRate", vfxSet.trisRate);
        vfx.SetInt("blobsKnobsRate", vfxSet.blobsKnobsRate);
    }


    private void EndPhenotypeAnimation()
    {
        print("phenotype animation ended."); 
    }

    private void StartVfx()
    {
        vfx.enabled = true;
    }

    private void OnDestroy()
    {
        EndVfx();
    }

    private void EndVfx()
    {
        vfx.enabled = false;
    }

    // Fade Stuff
    IEnumerator FadeAlpha (float duration, float start, float end)
    {
        print("fade started.");

        bool active = true;
        float remaining = duration;

        while (active)
        {
            float t = 1 - remaining / duration; // t : 0 -> 1
            float v = Mathf.Lerp(start, end, t);

            vfx.SetFloat("fadeFactor", v);

            remaining -= Time.deltaTime;
            if (remaining <= 0) active = false;
            yield return null;
        }

        vfx.SetFloat("fadeFactor", end);
        print("fade finished.");
    }

    // Interface Methods
    public void FadeOutPhenotypes()
    {
        StartCoroutine(FadeAlpha(vfxSet.fadeOutDuration, lastAlpha, 0));
    }
}