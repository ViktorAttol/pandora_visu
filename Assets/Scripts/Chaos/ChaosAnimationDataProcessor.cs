using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using AnimController;
using System;

public class ChaosAnimationDataProcessor : MonoBehaviour
{
    public enum DataState
    {
        Idle, WaitingForData, ProcessingRead, SwitchRead
    }

    private LinkedList<string> reads = new LinkedList<string>();
    private LinkedList<string> qualities = new LinkedList<string>();
    private LinkedList<int[]> signals = new LinkedList<int[]>();

    private IDataManager dataManager;
    private ChaosController chaosController;
    [SerializeField]private DataState state = DataState.WaitingForData;
    
    private int signalInd = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateLoop();
    }

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void SetChaosController(ChaosController chaosController)
    {
        this.chaosController = chaosController;
    }

    public void StateLoop()
    {
        switch (state)
        {
            case (DataState.Idle):
                OnCaseIdle();
                break;
            case (DataState.WaitingForData):
                OnCaseWaitingForData();
                break;
            case (DataState.ProcessingRead):
                OnCaseProcessingRead();
                break;
            case (DataState.SwitchRead):
                OnCaseSwitchRead();
                break;
        }
    }

    private void OnCaseIdle()
    {
        if(dataManager != null) chaosController.state = ChaosController.AnimationState.Idle;
    }

    private void OnCaseWaitingForData()
    {
        ReadData read = dataManager.GetRead();
        reads.AddLast(read.data);
        qualities.AddLast(read.quality);
        signals.AddLast(read.signals);

        chaosController.setQualityScore(getQualityMean(qualities.First.Value));
        state = DataState.ProcessingRead;
    }

    private void OnCaseProcessingRead()
    {
        if (reads.First == null || qualities.First == null)
        {
            chaosController.setNucleotide(-1);
            state = DataState.WaitingForData;
        } else
        {
            int nextNuc = getNextNucleotide(reads.First.Value);
            chaosController.setNucleotide(nextNuc);
            trimSequence(reads, reads.First.Value);
        }

        if (signals.First != null)
        {
            int nextSignal = getNextSignal();
            chaosController.setSignal(nextSignal);
        }
    }

    private void OnCaseSwitchRead()
    {
        if (qualities.First != null)
            qualities.RemoveFirst();

        if (qualities.First != null)
        {
            chaosController.setQualityScore(getQualityMean(qualities.First.Value));
            state = DataState.ProcessingRead;
        } else
        {
            state = DataState.WaitingForData;
        }
    }

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


    /// <summary>
    /// removes nucleotide data that has already been fed into the animation
    /// </summary>
    private void trimSequence(LinkedList<string> ll, string dummy_seq) 
    {
        if (ll.First != null)
        {
            if (ll.First.Value.Length > 1)
            {
                string trimmedSeq = ll.First.Value.Remove(0, 1);

                ll.RemoveFirst();
                ll.AddFirst(trimmedSeq);
            }
            else
                state = DataState.SwitchRead;

        } else
            state = DataState.WaitingForData;
    }


    private int getQualityMean(string quality_seq)
    {
        int sum = 0;

        foreach (char c in quality_seq)
        {
            sum += getPhredScore(c);
        }

        return sum / quality_seq.Length;
    }

    private int getPhredScore(char c)
    {
        return (int)c - 33;
    }

    private int getNextSignal()
    {
        signalInd++;

        if (signalInd >= signals.First.Value.Length)
            signals.RemoveFirst();
            signalInd = 0;

        return signals.First.Value[signalInd];
    }


}
