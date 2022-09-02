using UnityEngine;

public struct PhenotypeData
{
    public PhenotypeData(Phenotype phenotype, string color, float probability)
    {
        this.phenotype = phenotype;
        this.color = color;
        this.probability = probability;
    }
    
    public Phenotype phenotype;
    public string color;
    public float probability;
}