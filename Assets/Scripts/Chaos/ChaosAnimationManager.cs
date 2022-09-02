using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosAnimationManager : MonoBehaviour, IAnimationConnector
{
    private IDataManager dataManager;
    // Start is called before the first frame update
    public SequenceAnimationManager sequenceAnimationManager;
    
     void Start()
     {
         //7sequenceAnimationManager = GetComponentInChildren<SequenceAnimationManager>();
         //if(sequenceAnimationManager == null)Debug.Log("no chaos sequence animation manager found!!!");
     }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
        sequenceAnimationManager.SetDataManager(dataManager);
    }

    public void StartAnimation()
    {
    }

    public void FadeOutAnimation()
    {
        throw new NotImplementedException();
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
