using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Net;
using UnityEngine;

public class NetInput: INetInput, IProcessNetworkMessages
{
    private Action<NetStatus> cbServerStatusChanged;
    private Action<ReadData> cbNewReadReceived;
    private Action<List<PhenotypeData>> cbNewPhenotypeReceived;
    private Thread serverThread;
    private string ip = "127.0.0.1";
    private int port = 8080;
    private TcpListener tcpListener;

    
    public void StartServer()
    {
        //throw new System.NotImplementedException();
        // build class
        tcpListener = new TcpListener(IPAddress.Parse(ip), port);
        TCPServer server = new TCPServer(tcpListener, this);
        // build thread
        server.BuildServerThread();
        server.StartThread();
        // start thread
    }

    public void EndServer()
    {
        serverThread.Abort();
        tcpListener.Stop();
        throw new System.NotImplementedException();
        //end thread
        //free socket
    }

    private void OnServerStatusChanged(NetStatus netStatus)
    {
        cbServerStatusChanged?.Invoke(netStatus);
    }
    
    public void SubscribeForServerStatus(Action<NetStatus> callbackFunc)
    {
        cbServerStatusChanged += callbackFunc;
    }

    public void UnSubscribeForServerStatus(Action<NetStatus> callbackFunc)
    {
        cbServerStatusChanged -= callbackFunc;
    }

    private void OnNewReadReceived(ReadData read)
    {
        cbNewReadReceived?.Invoke(read);
    }
    
    public void SubscribeForReads(Action<ReadData> cbReadReceivedFunc)
    {
        cbNewReadReceived += cbReadReceivedFunc;
    }

    public void UnSubscribeForReads(Action<ReadData> cbReadReceivedFunc)
    {
        cbNewReadReceived -= cbReadReceivedFunc;

    }

    private void OnNewPhenotypeReceived(List<PhenotypeData> phenotypeData)
    {
        cbNewPhenotypeReceived?.Invoke(phenotypeData);
    }
    
    public void SubscribeForPhenotypes(Action<List<PhenotypeData>> cbPhenotypeReceivedFunc)
    {
        cbNewPhenotypeReceived += cbPhenotypeReceivedFunc;
    }

    public void UnSubscribeForPhenotypes(Action<List<PhenotypeData>> cbPhenotypeReceivedFunc)
    {
        cbNewPhenotypeReceived -= cbPhenotypeReceivedFunc;
    }

    public void ReadReceived(ReadData read)
    {
        OnNewReadReceived(read);
    }

    public void PhenotypeReceived(List<PhenotypeData> phenotypeData)
    {
        OnNewPhenotypeReceived(phenotypeData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void StateReceived(DataCollectorState state)
    {
        if(state == DataCollectorState.End) OnServerStatusChanged(NetStatus.ConnectionClosed);
    }

    public void ConnectionStatus(NetStatus status)
    {
        OnServerStatusChanged(status);
    }
}


