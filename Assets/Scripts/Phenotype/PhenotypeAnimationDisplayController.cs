using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VFX;

public class PhenotypeAnimationDisplayController : MonoBehaviour
{
    public PhenotypeSDFBaker sdfBaker;
    
    private List<PhenoDisplayData> preprocessedPhenotypes = new List<PhenoDisplayData>(); 
    private List<PhenoDisplayData> displayedPhenotypes = new List<PhenoDisplayData>();

    public PhenotypeAnimationDataPreprocessor dataPreprocessor;

    private float remainingPlayTime = -1f;

    private bool isRunning = false;
    
    [SerializeField] VisualEffect vfx;
    
    // Start is called before the first frame update
    void Start()
    {
        dataPreprocessor.SubscribeForPhenotypeDataPreprocessed(OnPhenotypePreprocessed);
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
                remainingPlayTime = PhenoDisplayData.phenoAnimationDuration;
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
        //preprocessedPhenotypes[0].sdf = sdfBaker.GetSDF(preprocessedPhenotypes[0].phenoClassData.GetMesh());
        vfx.SetTexture("sdf", sdfBaker.GetSDF(preprocessedPhenotypes[0].phenoClassData.GetMesh()));
        vfx.SetMesh("inputMesh", preprocessedPhenotypes[0].phenoClassData.GetMesh());
    }

    private void EndPhenotypeAnimation()
    {
        // not in use
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
}
