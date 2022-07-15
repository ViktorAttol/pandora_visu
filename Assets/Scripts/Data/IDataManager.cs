using System;
using System.Collections;
using System.Collections.Generic;
using Net;

public interface IDataManager: IReadReceiver, IPhenotypeReceiver
{
    ReadData? GetRead();

    List<PhenotypeData> GetPhenotype();

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