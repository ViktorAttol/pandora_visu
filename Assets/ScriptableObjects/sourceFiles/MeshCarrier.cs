using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "MeshCarrier", menuName = "Pandora Customs/MeshCarrier", order = 1)]

public class MeshCarrier : ScriptableObject
{
    [Header("Data")]
    [TextArea]
    public string Note = "Due to a modified mesh index format in Iris.cs the mesh is displayed as missmatched type.";
    public Mesh mesh;

}
