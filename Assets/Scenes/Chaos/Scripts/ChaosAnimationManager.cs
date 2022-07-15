using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChaosAnimationManager : MonoBehaviour
{
    public ChaosController chaosController;

    // ich arbeite aktuell nur mit den Sequenzdaten, noch nicht mit Qualität und Signalen.
    // deshalb habe ich noch nicht das ReadDataStruct berücksichtigt, sondern eben nur eine liste für die datenstrings
    // ich habe schon eine Funktion geschrieben, um das struct zu entpacken (war ja auch nicht sehr kompliziert)
    // die ist nur noch auskommentiert weil mein programm das struct nicht kennt
    private LinkedList<string> reads = new LinkedList<string>();

    // kopiere diesen string am besten in deine dummy daten als rd.data in NPInput, der string den du drin hast funktioniert nicht mit meiner Animation
    /*
    private string dummy = "CATTGTACTTCGTTCAATTTTTCGAATTTGAGTGTTTAACCGTTTTCGCATTTATCGTGAAACGCTTTCGCGTTTTTCGTGCACCGCT" +
            "TCAATATACCAAATGTCATATCTATAATCTGGTTTTGTTTTTTTGAATAATAAATATTTTCATTCTTGCGGTTTGGAGGAATTGATTCAAATTCAAGCAGA" +
            "AATAATTCCAGGAGTCCAAAATATGTATCAATGCAGCATTTGAGCAAGTGCGATAAATCTTTAAGTGCTTCTTTCCCATGGTTTTAGTCATAAAACTCTCC" +
            "ATTTTGATAGGTTGCATGCTAGATGCTGAAGTATATTTTTGAAAATTTGTCGATGCTACTTAACTGTCAATATGGCCACAAGTTGTTTGATCTTTGCAATG" +
            "ATTTATATCAGAAACCATATAGTAAATTAGTTACACAGGAAATTTTTATATGTCCTTATTATCATTCATTATGTATTAAAATTAGAGTTGTGGCTTGGCTC" +
            "TGCTAACACGTTGCTCATAGGAGATATGGTAGAGCCGCAGACACGTCGTATGCAGGAACGTGCTGCGGCTGGCTGGTGAACTTCCGATAGTGCGGGTGTTA" +
            "GACGTTGATTCTTATACCGATTTTACATATTTTTTGCATGAGAA";
    */
    private string dummy = "CGGTACTTCGTTCAGTTCAGATGCAGGTGTTTAACCGTTTTCGCATTTATCGTGAAACGCTTTCGCGTTTTTCGTGCGCCGCTTCAGCTGCCATCAATTTTTTCGTAATAAGCGCATCTCTTTCCTCCATCCGCGATGAACCATGAGAACGTCGTTGACGATGGCGTGCATTTTCCCGTCTTTATCATCAACGTATTTTCTGACCGTACCGCGACTACATTTCAGTCTGCGTGCTACTTCTGTCTGATTTCCGTATGTTTCAACGAGCATGTCTGGAATGGTTTTTACTGAGAACGTCATGCGGCCTCACTTCTGCTATTTCGCAGGTCTTTGAGTTTCTGTTGGTACTCTGCCTTGATCGCCTTGCACTCTTCGATAGTCCAGCGATGGCGGTTATGGATTTGATTCGATTTCGTCTACTGCTTCCTGCCCGATGCGGCTAATCAGTTCGACGCGATACGGAACTTGAA";

    // Start is called before the first frame update
    void Start()
    {
        //das kann dann wahrscheinlich komplett raus, das war nur für die vom back end losgelöste version
        reads.AddFirst(dummy);
    }

    // Update is called once per frame
    void Update()
    {
        if (reads != null && reads.First != null)
        {
            chaosController.getNextNuc(getNextNucleotide(reads.First.Value));
            trimSequence();
        }

    }

    /// <summary>
    /// unpacks the data of incoming reads
    /// </summary>
    /// <param name="read"></param>
    //public void recieveRead(ReadData read)
    //{
    //    reads.AddLast(read.data);
    //}

    /// <summary>
    /// extracts the individual nucleotides from the dna sequence and maps them to numeric values
    /// </summary>
    /// <param name="sequence">dna sequence</param>
    /// <returns>an integer representing one of the four nucleotides</returns>
    private int getNextNucleotide(string sequence)
    {
        int nextNuc = -1; 

        if (sequence.Length >= 1)
        {
            //https://www.tutorialspoint.com/How-to-find-the-first-character-of-a-string-in-Chash (12.07.2022)
            string firstNuc = sequence.Substring(0,1);

            switch (firstNuc)
            {
                case "A":
                    nextNuc = 0;
                    break;
                case "C":
                    nextNuc = 1;
                    break;
                case "G":
                    nextNuc = 2;
                    break;
                case "T":
                    nextNuc = 3;
                    break;
                default:
                    nextNuc = -1;
                    break;
            }
        }

        return nextNuc;
    }

    /// <summary>
    /// removes nucleotide data that has already been fed into the animation
    /// </summary>
    private void trimSequence()
    {
        if (reads.First.Value.Length > 1)
        {
            //https://www.delftstack.com/howto/csharp/csharp-remove-first-character-from-string/ (11.07.2022)
            string trimmedSeq = reads.First.Value.Remove(0, 1);

            //https://stackoverflow.com/questions/8480875/how-to-replace-an-element-in-a-linkedlist (11.07.2022)
            reads.RemoveFirst();
            reads.AddFirst(trimmedSeq);
        } else
        {
            reads.RemoveFirst();

            // diese zeile muss raus, die war nur für mich damit ich besser testen kann wie's aussieht wenns über längere zeit läuft
            reads.AddFirst(dummy);
        }
    }


}
