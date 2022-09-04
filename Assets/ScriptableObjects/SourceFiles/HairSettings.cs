using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "HairSettings", menuName = "Pandora Customs/HairSettings", order = 1)]
public class HairSettings : ScriptableObject
{
    [Header("Generation Settings")]
    public float generationRate = 0.01f;
    public AnimationCurve morphCurve;
    public AnimationCurve distributionCurve;
    [Range(1f,5f)] public float lengthRadius = 2f;
    [MinMaxSlider(0.1f, 2f)] public Vector2 minMaxHeightRadius = new Vector2(0.5f, 0.7f);
    [Range(20,160)] public int resolution = 140;
    [MinMaxSlider(1,20)] public Vector2Int minMaxHairAmount = new Vector2Int(10,15);
    [MinMaxSlider(3,80)] public Vector2Int minMaxHairLength = new Vector2Int(40,60);
    [MinMaxSlider(0f,0.05f)] public Vector2 minMaxDisplacement = new Vector2(0.0f, 0.005f);
    [MinMaxSlider(0f,0.5f)] public Vector2 minMaxTwirlRadius = new Vector2(0.0f, 0.05f);
    [MinMaxSlider(0f,100f)] public Vector2 minMaxTwirlFrequency = new Vector2(10f, 30f);
    public int cleanThresh = 3;
    public bool debug = false;

    [Space(10)]

    [Header("Mesh Settings")]
    [MinMaxSlider(0.0001f,0.05f)] public Vector2 minMaxCylinderRadius = new Vector2(0.001f, 0.005f);
    [Range(3,10)] public int cylinderResolution = 5;

    [Space(10)]

    [Header("Display Settings")]
    [Range(0f,1f)] public float visibility;

    
}
