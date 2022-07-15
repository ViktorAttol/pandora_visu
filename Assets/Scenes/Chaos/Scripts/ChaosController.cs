using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;

public class ChaosController : MonoBehaviour
{
    public VisualEffect chaos;

    [SerializeField]
    private int nucleotide = -1;
    private int[] nucCounts = new int[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setNucleotideCounts(nucleotide);
    }

    public void getNextNuc(int nextNuc)
    {
        nucleotide = nextNuc;
    }

    private void setNucleotideCounts(int nextNuc)
    {
        if (nextNuc >= 0)
        {
            nucCounts[nextNuc]++;

            //https://code-maze.com/csharp-sum-up-elements-of-an-array/
            chaos.SetInt("Sequence_Length", nucCounts.Sum());
            chaos.SetInt("A_Count", nucCounts[0]);
            chaos.SetInt("C_Count", nucCounts[1]);
            chaos.SetInt("G_Count", nucCounts[2]);
            chaos.SetInt("T_Count", nucCounts[3]);
        }
    }

}
