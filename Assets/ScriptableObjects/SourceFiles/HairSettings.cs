using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GD.MinMaxSlider;

[CreateAssetMenuAttribute(fileName = "HairSettings", menuName = "Pandora Customs/HairSettings", order = 1)]
public class HairSettings : ScriptableObject
{
    [Header("Generation Settings")]
    public bool debug;

    
}
