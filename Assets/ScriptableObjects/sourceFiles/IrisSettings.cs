using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "IrisSettings", menuName = "Pandora Customs/IrisSettings", order = 1)]
public class IrisSettings : ScriptableObject
{
    [Header("Generation Settings")]
    public float generationRate = 0.01f;
    public float totalRadius;
    [Range(0f,0.04f)] public float displacementLimit;
    [Range(0f,0.4f)] public float depthFactor;
    [MinMaxSlider(0f,800f)] public Vector2Int minMaxStepResolution;
    public int radialSteps;
    public AnimationCurve weightDistributionCurve;
    public bool debug;
    
    [Space(10)]

    [Header("Mesh Settings")]
    [MinMaxSlider(0.0001f,0.1f)] public Vector2 minMaxCylinderRadius;
    [Range(3,16)] public int cylinderResolution;

    // SDF Settings (not visible, only default stuff)
    [HideInInspector] public int maxResolution = 64, signPassCount = 1;
    [HideInInspector] public Vector3 center, sizeBox;
    [HideInInspector] public float threshold = 0.5f;
}
