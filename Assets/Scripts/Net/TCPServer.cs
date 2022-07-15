using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Net
{
    public class TCPServer
    {
        private TcpListener tcpListener;
        private Thread tcpListenerThread;
        private TcpClient connectedTcpClient;
        private IProcessNetworkMessages processNetworkMessages;
        private string ip;
        private int port;
        private bool shouldRun = true;
        
        public string[] informationArray;

        public TCPServer(TcpListener tcpListener, IProcessNetworkMessages processNetworkMessages)
        {
            this.tcpListener = tcpListener;
            this.processNetworkMessages = processNetworkMessages;
        }

        public Thread BuildServerThread()
        {
            tcpListenerThread = new Thread(new ThreadStart(ListenForIncomingRequest));
            tcpListenerThread.IsBackground = true;
            return tcpListenerThread;
        }

        public void StartThread()
        {
            tcpListenerThread.Start();
        }
        

        private void ListenForIncomingRequest()
        {
            try
            {
                tcpListener.Start();
                Debug.Log("Server is listening!");

                ReadStream();
            }
            catch (SocketException se)
            {
                Debug.Log("Socket exception - " + se.ToString());
            }
            finally{
                tcpListener.Stop();
            }
        }

        private void ReadStream()
        {   
            while(shouldRun){
                processNetworkMessages.ConnectionStatus(NetStatus.WaitForConnection);
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    processNetworkMessages.ConnectionStatus(NetStatus.ClientConnected);
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            while (shouldRun)
                            {
                                int counter = Int32.Parse(sr.ReadLine());
                                string command = sr.ReadLine();
                                ProcessCommand(command, sr);
                                
                                /*
                                int length;
                                Byte[] bytes = new byte[1024];
                                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    var incommingData = new byte[length];
                                    Array.Copy(bytes, 0, incommingData, 0, length);
                                    string clientMessage = Encoding.ASCII.GetString(incommingData);

                                    // send to subscriber
                                }
                                */
                            }
                        }

                    }
                }
            }
        }

        private void ProcessCommand(string command, StreamReader sr)
        {
            switch (command)
            {
                case "state":
                    ReadState(sr);
                    break;
                case "read":
                    ReadRead(sr);
                    break;
                case "phenotype":
                    ReadPhenotype(sr);
                    break;
            }
        }

        private void ReadRead(StreamReader sr)
        {
            ReadData rd = new ReadData();
            rd.id = Int32.Parse(sr.ReadLine());
            //Debug.Log("Read received with ID : " + rd.id);

            rd.quality = sr.ReadLine();
            rd.data = sr.ReadLine();
            int signalCount = Int32.Parse(sr.ReadLine());
            rd.signals = new int[signalCount];
            for (int i = 0; i < rd.signals.Length; i++)
            {
                rd.signals[i] = Int32.Parse(sr.ReadLine());
            }
            processNetworkMessages.ReadReceived(rd);
        }

        private void ReadPhenotype(StreamReader sr)
        {
            int count = Int32.Parse(sr.ReadLine());
            List<PhenotypeData> datas = new List<PhenotypeData>();

            for (int i = 0; i < count; i++)
            {
                PhenotypeData phenotypeData = new PhenotypeData();
                //var pheno = (Phenotype)Enum.Parse(typeof(PhenotypeData),sr.ReadLine());
                var pheno = (Phenotype)Int32.Parse(sr.ReadLine());
                //Debug.Log("Phenotype received: " + pheno);
                phenotypeData.phenotype = pheno;
                Color color = GetColor(sr.ReadLine());
                phenotypeData.color = color;
                phenotypeData.probability = float.Parse(sr.ReadLine());
                datas.Add(phenotypeData);
            }
            
            processNetworkMessages.PhenotypeReceived(datas);

        }

        private Color GetColor(string color)
        {
            Color toFind = Color.white;
            switch (color)
            {
                case "red":
                    toFind = Color.red;
                    break;
                case "blue":
                    toFind = Color.blue;
                    break;
                case "brown":
                    toFind = new Color(139,69,19);
                    break;
                case "green":
                    toFind = Color.green;
                    break;
                case "white":
                    toFind = Color.white;
                    break;
                case "yellow":
                    toFind = Color.yellow;
                    break;
                case "black":
                    toFind = Color.black;
                    break;
            }
            return toFind;
        }

        private void ReadState(StreamReader sr)
        {
            DataCollectorState state = (DataCollectorState)Enum.Parse(typeof(DataCollectorState),sr.ReadLine());
            if (state == DataCollectorState.End) shouldRun = false;
            processNetworkMessages.StateReceived(state);
        }
        
    }
}