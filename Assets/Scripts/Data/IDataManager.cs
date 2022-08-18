using System;
using System.Collections;
using System.Collections.Generic;
using Net;
using UnityEngine;

public interface IDataManager: IReadReceiver, IPhenotypeReceiver
{
    ReadData GetRead();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Empty list if there are no phenotype data currently. If there is data returns list with data.</returns>
    List<PhenotypeData> GetPhenotypes();

    void ClearLists();
}

//fastq + fast5
public struct ReadData
{
    public int id;
    public string quality;
    public string data;
    public int[] signals; //mabye empty
}