using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "VFXSettings", menuName = "Pandora Customs/VFXSettings", order = 1)]
public class VFXSettings : ScriptableObject
{
    [Range(1f,5f)] public float postScale = 1.8f;
    [Range(0f,0.01f)] public float randomStartVelocity = 0.001f;
    [Range(0f,1f)] public float attractionSpeedForce = 0.2f;
    [Range(0f,1f)] public float blobSize = 0.4f;
    [Range(0f,0.3f)] public float knobSize = 0.1f;
    [Range(0f,0.01f)] public float trisSize = 0.005f;
    [Range(1000,150000)] public int trisRate = 100000;
    [Range(1000,150000)] public int blobsKnobsRate = 100000;
    [Range(15,240)] public int crossFadeDuration = 30;
    [Range(60,600)] public int fadeOutDuration = 240;
    [Range(5f,300f)] public float displayDuration;

}