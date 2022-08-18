using System;
using System.Collections.Generic;
using System.Linq;
using Net;


    public class DataManager: IDataManager, IReadReceiver, IPhenotypeReceiver
    {
        private List<ReadData> reads;
        private List<PhenotypeData> phenotypeDatas;

        public DataManager()
        {
            reads = new List<ReadData>();
            phenotypeDatas = new List<PhenotypeData>();
            SetDummyPhenoData();
        }

        private void SetDummyPhenoData()
        {
            PhenotypeData data1 = new PhenotypeData();
            data1.phenotype = Phenotype.Eye;
            data1.color = "brown";
            data1.probability = 1f;
            
            PhenotypeData data2 = new PhenotypeData();
            data2.phenotype = Phenotype.Skin;
            data2.color = "white";
            data2.probability = 1f;
            
            PhenotypeData data3 = new PhenotypeData();
            data3.phenotype = Phenotype.Hair;
            data3.color = "brown";
            data3.probability = 1f;
            
            phenotypeDatas.Add(data1);
            phenotypeDatas.Add(data2);
            phenotypeDatas.Add(data3);
        }
        
        public ReadData GetRead()
        {
            //if (reads.Count <= 0) return new ReadData();
            //return reads.First();
            ReadData read = new ReadData();
            read.data =  "CATTGTACTTCGTTCAATTTTTCGAATTTGAGTGTTTAACCGTTTTCGCATTTATCGTGAAACGCTTTCGCGTTTTTCGTGCACCGCTTCAATATACCAAATGTCATATCTATAATCTGGTTTTGTTTTTTTGAATAATAAATATTTTCATTCTTGCGGTTTGGAGGAATTGATTCAAATTCAAGCAGAAATAATTCCAGGAGTCCAAAATATGTATCAATGCAGCATTTGAGCAAGTGCGATAAATCTTTAAGTGCTTCTTTCCCATGGTTTTAGTCATAAAACTCTCCATTTTGATAGGTTGCATGCTAGATGCTGAAGTATATTTTTGAAAATTTGTCGATGCTACTTAACTGTCAATATGGCCACAAGTTGTTTGATCTTTGCAATGATTTATATCAGAAACCATATAGTAAATTAGTTACACAGGAAATTTTTATATGTCCTTATTATCATTCATTATGTATTAAAATTAGAGTTGTGGCTTGGCTCTGCTAACACGTTGCTCATAGGAGATATGGTAGAGCCGCAGACACGTCGTATGCAGGAACGTGCTGCGGCTGGCTGGTGAACTTCCGATAGTGCGGGTGTTAGACGTTGATTCTTATACCGATTTTACATATTTTTTGCATGAGAA";
            return read;
        }

        public List<PhenotypeData> GetPhenotypes()
        {
            if (phenotypeDatas.Count <= 0) return null;
            return phenotypeDatas;
        }

        public void ClearLists()
        {
            reads.Clear();
            phenotypeDatas.Clear();
        }

        public void ReceiveNewRead(ReadData read)
        {
            reads.Add(read);
        }

        public void ReceiveNewPhenotype(List<PhenotypeData> phenotypeData)
        {
            phenotypeDatas.AddRange(phenotypeData);
        }
    }
