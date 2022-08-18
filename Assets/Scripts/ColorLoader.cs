using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLoader : MonoBehaviour
{
    private Dictionary<string, Texture2D> colorTextures = new Dictionary<string, Texture2D>();

    // Start is called before the first frame update
    void Start()
    {
        Object[] textures =  Resources.LoadAll("PhenotypeColorTextures/", typeof(Texture2D));

        foreach (var texture in textures)
        {
            colorTextures.Add(texture.name, (Texture2D)texture);
        }
    }

    public Texture2D GetTextureForColor(Phenotype phenotype, string color)
    {
        string name = phenotype + "_" + color;
        return colorTextures.GetValueOrDefault(name);
    }
}
