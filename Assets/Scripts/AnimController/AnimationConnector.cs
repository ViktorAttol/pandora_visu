using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConnector : MonoBehaviour
{
    private GameObject animationPrefab;
    
    private GameObject animation;
    
    private float minTimeForAnimation = 4f;
    private float currTimeForAnimation = 0f;

    private Action cbAnimFinished;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTimeForAnimation += Time.deltaTime;
        if(currTimeForAnimation >= minTimeForAnimation) OnAnimationFinished();
    }

    public void SetAnimationPrefab(GameObject animationPrefab)
    {
        this.animationPrefab = animationPrefab;
    }

    public void StartAnimation()
    {
        animation = Instantiate(animationPrefab, this.transform);
    }

    public void EndAnimation()
    {
        Destroy(animation);
    }

    private void OnAnimationFinished()
    {
        cbAnimFinished?.Invoke();
    }

    public void SubscribeForAnimationFinished(Action finished)
    {
        cbAnimFinished += finished;
    }
    public void UnSubscribeForAnimationFinished(Action finished)
    {
        cbAnimFinished -= finished;
    }
}
