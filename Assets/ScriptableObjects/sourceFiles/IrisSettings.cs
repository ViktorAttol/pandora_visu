using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "IrisSettings", menuName = "Pandora Customs/IrisSettings", order = 1)]
public class IrisSettings : ScriptableObject
{
    [Header("Generation Settings")]
    public float generationRate = 0.01f;
    public float totalRadius = 1f;
    [Range(0f,0.04f)] public float displacementLimit = 0.01f;
    [Range(0f,0.4f)] public float depthFactor = 0.2f;
    [MinMaxSlider(10f,200f)] public Vector2Int minMaxStepResolution = new Vector2Int (120, 140);
    public int radialSteps = 130;
    public AnimationCurve weightDistributionCurve;
    public bool debug = false;
    
    [Space(10)]

    [Header("Mesh Settings")]
    [MinMaxSlider(0.0001f,0.01f)] public Vector2 minMaxCylinderRadius = new Vector2 (0.003f, 0.004f);
    [Range(3,16)] public int cylinderResolution;
}
