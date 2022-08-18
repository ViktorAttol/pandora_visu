using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PhenotypeAnimationManager : MonoBehaviour, IAnimationConnector
{
    public PhenotypeAnimationDataPreprocessor dataPreprocessor;

    private IDataManager dataManager;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = new DataManager();
        dataPreprocessor.SetDataManager(dataManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void StartAnimation()
    {
    }

    public void EndAnimation()
    {
    }

    public void SubscribeForAnimationFinished(Action finished)
    {
    }

    public void UnSubscribeForAnimationFinished(Action finished)
    {
    }

}
