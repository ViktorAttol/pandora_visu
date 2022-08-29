using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "SkinSettings", menuName = "Pandora Customs/SkinSettings", order = 1)]
public class SkinSettings : ScriptableObject
{
    [Header("Generation Settings")]
    public float generationRate = 0.01f;
    [MinMaxSlider(.1f,.8f)] public Vector2 distributionRadius = new Vector2(.4f, .5f);
    [MinMaxSlider(.1f,1f)] public Vector2 blobRadius = new Vector2(.15f, .25f);
    [Range(1f,10f)] public float perlinScale = 5f;
    [Range(0.1f,1f)] public float displacementIntensity = 0.4f;
    public int blobResolution = 20, blobAmount = 3;
    public bool debug = false;
}
