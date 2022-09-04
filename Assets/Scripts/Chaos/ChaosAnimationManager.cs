using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosAnimationManager : MonoBehaviour, IAnimationConnector
{
    private IDataManager dataManager;
    // Start is called before the first frame update
    public ChaosAnimationDataProcessor chaosAnimationDataProcessor;
    public ChaosController chaosController;
    
     void Start()
     {
         if (chaosAnimationDataProcessor != null)
        {
            chaosAnimationDataProcessor.SetChaosController(chaosController);
        }
     }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
        chaosAnimationDataProcessor.SetDataManager(dataManager);
    }

    public void StartAnimation()
    {
        chaosController.state = ChaosController.AnimationState.Run;
    }

    public void FadeOutAnimation()
    {
        chaosController.state = ChaosController.AnimationState.FadeOut;
    }

    public void EndAnimation()
    {
        chaosController.state = ChaosController.AnimationState.Inactive;
        Destroy(gameObject);
    }

    public void SubscribeForAnimationFinished(Action finished)
    {
    }

    public void UnSubscribeForAnimationFinished(Action finished)
    {
    }
}
