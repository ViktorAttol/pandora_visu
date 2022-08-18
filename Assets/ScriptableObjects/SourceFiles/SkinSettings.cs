using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "SkinSettings", menuName = "Pandora Customs/SkinSettings", order = 1)]
public class SkinSettings : ScriptableObject
{
    [Header("Generation Settings")]
    public bool debug;
    public float generationRate = 0.01f;
    [MinMaxSlider(.1f,.5f)] public Vector2 distributionRadius = new Vector2(.1f, .2f);
    [MinMaxSlider(.1f,1f)] public Vector2 blobRadius = new Vector2(.5f, .85f);
    [Range(1f,10f)] public float perlinScale = 2f;
    [Range(0.1f,2f)] public float displacementIntensity = 1f;
    public int blobResolution = 40, blobAmount = 2;
}
