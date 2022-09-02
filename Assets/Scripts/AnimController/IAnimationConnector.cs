using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to control a single animation.
/// Should be implemented by the Gameobject which holds an animation
/// Should be used by the AnimationController
/// </summary>
public interface IAnimationConnector
{ 
    void SetDataManager(IDataManager dataManager);

    void StartAnimation();

    void FadeOutAnimation();
    
    void EndAnimation();

    void SubscribeForAnimationFinished(Action finished);
    void UnSubscribeForAnimationFinished(Action finished);
}
