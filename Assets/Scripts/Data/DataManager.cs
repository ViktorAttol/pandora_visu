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
        }
        
        public ReadData? GetRead()
        {
            if (reads.Count <= 0) return new ReadData?();
            return reads.First();
        }

        public List<PhenotypeData> GetPhenotype()
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
