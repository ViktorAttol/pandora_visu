using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum PreprocessState
{
    ChoosePhenotype, CreateClass, GenerateData, GenerateMesh, GenerateSDF, GenerateColor, PhenotypeDataComplete, AllPhenotypeDataPrepared
}

/// <summary>
/// Manages the phenotype animation
/// </summary>
public class PhenotypeAnimationDataPreprocessor : MonoBehaviour
{
    public PhenotypeSDFBaker sdfBaker;
    public ColorLoader colorLoader;
    
    
    public IrisSettings _irisSet;
    public SkinSettings _skinSet;
    public HairSettings _hairSet;
    private IDataManager dataManager;
    
    private System.Random pseudoRandomNumberGenerator;

    private bool phenoDataGenerated = false;
    private bool phenoMeshGenerated = false;

    private int phenoDisplayDataPosition = 0;

    private PreprocessState state = PreprocessState.ChoosePhenotype;

    private Action<PhenoDisplayData> phenotypePreprocessedInformationCB;

    private List<PhenoDisplayData> displayDataList = new List<PhenoDisplayData>();
    private List<PhenotypeData> inputPhenoTypeData = new List<PhenotypeData>();
    private PhenoDisplayData currentProcessData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inputPhenoTypeData.Count == 0) // todo change to subscription 
        {
            inputPhenoTypeData.AddRange(dataManager.GetPhenotypes());
        }
        else StateLoop();
    }

    public void StateLoop()
    {
        switch (state)
        {
            case PreprocessState.AllPhenotypeDataPrepared:
                //return;
                break;
            case PreprocessState.ChoosePhenotype:
                OnCaseChoosePhenotype();
                break;
            case PreprocessState.CreateClass:
                OnCaseCreateClass();
                break;
            case PreprocessState.GenerateData:
                OnCaseGenerateData();
                break;
            case PreprocessState.GenerateMesh:
                OnCaseGenerateMesh();
                break;
            case PreprocessState.GenerateSDF:
                OnCaseGenerateSDF();
                break;
            case PreprocessState.GenerateColor:
                OnCaseGenerateColor();
                break;
            case PreprocessState.PhenotypeDataComplete:
                OnCasePhenotypeDataComplete();
                break;
        }
    }

    public void OnCaseChoosePhenotype()
    {
        if (displayDataList.Count == phenoDisplayDataPosition)
        {
            currentProcessData = new PhenoDisplayData();
            currentProcessData.phenotype = inputPhenoTypeData[phenoDisplayDataPosition].phenotype;
            currentProcessData.color = inputPhenoTypeData[phenoDisplayDataPosition].color;

            state = PreprocessState.CreateClass;
        }
        else
        {
            phenoDisplayDataPosition++;
        }
    }
    
    public void OnCaseCreateClass()
    {
        currentProcessData.phenoClassData = CreatePhenoClass((Phenotype)phenoDisplayDataPosition);
        state = PreprocessState.GenerateData;
    }
    
    public void OnCaseGenerateData()
    {
        GeneratePhenoClassData(currentProcessData.phenoClassData);
        state = PreprocessState.GenerateMesh;
    }
    
    public void OnCaseGenerateMesh()
    {
        if (phenoDataGenerated)
        {
            CreatePhenotypeMesh(currentProcessData.phenoClassData);
            state = PreprocessState.GenerateSDF;
        }
    }
    
    public void OnCaseGenerateSDF()
    {
        if (phenoMeshGenerated)
        {
            //currentProcessData.sdf = sdfBaker.GetSDF(currentProcessData.phenoClassData.GetMesh());
            state = PreprocessState.GenerateColor;
        }
    }
    
    public void OnCaseGenerateColor()
    {
        GenerateColor(currentProcessData);
        state = PreprocessState.PhenotypeDataComplete;
    }
    
    public void OnCasePhenotypeDataComplete()
    {
        displayDataList.Add(currentProcessData);
        OnPhenotypePreprocessed(currentProcessData);
        if (phenoDisplayDataPosition + 1 >= inputPhenoTypeData.Count) state = PreprocessState.AllPhenotypeDataPrepared;
        else state = PreprocessState.ChoosePhenotype;
        phenoDataGenerated = false;
        phenoMeshGenerated = false;
    }
   

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
        SetRandomSeed();
    }

    public void SetRandomSeed()
    {
        int seed = dataManager.GetRead().data.GetHashCode();
        pseudoRandomNumberGenerator = new System.Random(seed);
    }

    private IPhenotype CreatePhenoClass(Phenotype phenotype)
    {
        // added visibility

        IPhenotype phenoClass = null;
        switch (phenotype)
        {
            case Phenotype.Iris:
                phenoClass = new Iris(_irisSet, pseudoRandomNumberGenerator);
                currentProcessData.phenoAlphaValue = _irisSet.visibility;
                break;
            case Phenotype.Skin:

                phenoClass = new Skin(_skinSet, pseudoRandomNumberGenerator);
                currentProcessData.phenoAlphaValue = _skinSet.visibility;
                break;
            case Phenotype.Hair:

                phenoClass = new Hair(_hairSet, pseudoRandomNumberGenerator);
                currentProcessData.phenoAlphaValue = _hairSet.visibility;
                break;
        }
        return phenoClass;
    }

    private void GeneratePhenoClassData(IPhenotype phenotype)
    {
        phenotype.SubscribeForGenFinished(PhenoDataGenerated);
        StartCoroutine(phenotype.Generate());
    }
    
    private void CreatePhenotypeMesh(IPhenotype phenotype)
    {
        phenotype.SubscribeForMeshFinished(PhenoMeshGenerated);
        StartCoroutine(phenotype.CreateMesh());
    }

    private void GenerateColor(PhenoDisplayData displayData)
    {
        //print("returned phenotype: " + displayData.phenotype);
        //print("returned color: " + displayData.color);
        currentProcessData.colorTexture = colorLoader.GetTextureForColor(displayData.phenotype, displayData.color);
    }
    

    private void PhenoDataGenerated()
    {
        phenoDataGenerated = true;
    }
    
    private void PhenoMeshGenerated()
    {
        phenoMeshGenerated = true;
    }

    private void OnPhenotypePreprocessed(PhenoDisplayData phenoDisplayData)
    {
        phenotypePreprocessedInformationCB?.Invoke(phenoDisplayData);
    }

    public void SubscribeForPhenotypeDataPreprocessed(Action<PhenoDisplayData> phenoDataPreprocessedFunc)
    {
        phenotypePreprocessedInformationCB += phenoDataPreprocessedFunc;
    }
    
    public void UnbscribeForPhenotypeDataPreprocessed(Action<PhenoDisplayData> phenoDataPreprocessedFunc)
    {
        phenotypePreprocessedInformationCB -= phenoDataPreprocessedFunc;
    }
}

public struct PhenoDisplayData
{
    public Phenotype phenotype;
    public IPhenotype phenoClassData;
    public float phenoAlphaValue;
    public static float phenoAnimationDuration = 10f;
    //public Texture3D sdf;
    public RenderTexture sdf;
    public string color;
    public Texture2D colorTexture;
    

    public bool IsCalculated()
    {
        if (phenoClassData == null || sdf == null || colorTexture == null) return false;
        return true;
    }
}