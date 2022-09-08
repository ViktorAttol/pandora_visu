using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PhenotypeAnimationManager : MonoBehaviour, IAnimationConnector
{
    public PhenotypeAnimationDataPreprocessor dataPreprocessor;
    public PhenotypeAnimationDisplayController displayController;

    private IDataManager dataManager;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
        dataPreprocessor.SetDataManager(dataManager);
    }

    public void StartAnimation()
    {

    }

    public void FadeOutAnimation()
    {
        displayController.FadeOutPhenotypes();
    }

    public void EndAnimation()
    {
        Destroy(gameObject);
    }

    public void SubscribeForAnimationFinished(Action finished)
    {
    }

    public void UnSubscribeForAnimationFinished(Action finished)
    {
    }

}
