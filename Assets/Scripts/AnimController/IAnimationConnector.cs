using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationConnector
{ 
    void SetDataManager(IDataManager dataManager);

    void StartAnimation();

    void FadeOutAnimation();
    
    void EndAnimation();

    void SubscribeForAnimationFinished(Action finished);
    void UnSubscribeForAnimationFinished(Action finished);
}
