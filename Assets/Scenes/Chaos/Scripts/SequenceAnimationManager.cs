using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using AnimController;
using System;

public class SequenceAnimationManager : MonoBehaviour, IAnimationController
{
    public ChaosController seqController;

    private LinkedList<string> reads = new LinkedList<string>();
    private LinkedList<string> qualities = new LinkedList<string>();
    private LinkedList<int[]> signals = new LinkedList<int[]>();

    private int signalInd = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reads.First != null)
        {
            int nextNuc = getNextNucleotide(reads.First.Value);
            seqController.getNextNuc(nextNuc);
            trimSequence(reads, reads.First.Value);
        }

        if (qualities.First != null)
        {
            int nextScore = getNextPhredScore(qualities.First.Value);
            seqController.getNextScore(nextScore);
            trimSequence(qualities, qualities.First.Value);
        }

        if (signals.First != null)
        {
            int nextSignal = getNextSignal();
            seqController.getNextSignal(nextSignal);
        }
            

    }
    
    /// <summary>
    /// for feeding data
    /// </summary>
    /// <param name="data"></param>
    
    public void recieveSequenceData(string data)
    {
        reads.AddLast(data);
    }

    public void recieveQualityData(string data)
    {
        qualities.AddLast(data);
    }

    public void recieveSignalData(int[] data)
    {
        signals.AddLast(data);
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

        if (sequence != null && sequence.Length >= 1)
        {
            
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

    private int getNextPhredScore(string quality_seq)
    {
        string firstChar = quality_seq.Substring(0, 1);

        byte[] ascii = Encoding.ASCII.GetBytes(firstChar);
        int score = ascii[0] - 33;
        return score;
    }

    /// <summary>
    /// removes nucleotide data that has already been fed into the animation
    /// </summary>
    private void trimSequence(LinkedList<string> ll, string dummy_seq) 
    {
        if (ll != null && ll.First != null) ;
        {
            if (ll.First.Value.Length > 1)
            {
                string trimmedSeq = ll.First.Value.Remove(0, 1);

                ll.RemoveFirst();
                ll.AddFirst(trimmedSeq);
            }
            else
            {
                ll.RemoveFirst();
            }
        }
        
    }

    private int getNextSignal()
    {
        signalInd++;

        if (signalInd >= signals.First.Value.Length)
            signals.RemoveFirst();
            signalInd = 0;

        return signals.First.Value[signalInd];
    }

    public void RunIdleAnimation()
    {
        throw new NotImplementedException();
    }

    public void StartAnimations()
    {
        seqController.active = true;
    }

    public void EndAnimations()
    {
        seqController.active = false;
    }

    public void SubscribeForAnimationsFinished(Action cbAnimationFinishedFunc)
    {
        throw new NotImplementedException();
    }

    public void UnSubscribeForAnimationsFinished(Action cbAnimationFinishedFunc)
    {
        throw new NotImplementedException();
    }
}
