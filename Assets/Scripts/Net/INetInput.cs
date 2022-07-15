using System;
using System.Collections;
using System.Collections.Generic;
using Net;
using UnityEngine;

public interface INetInput
{
    void StartServer();

    void EndServer();

    void SubscribeForServerStatus(Action<NetStatus> callbackFunc);
    void UnSubscribeForServerStatus(Action<NetStatus> callbackFunc);

    void SubscribeForReads(Action<ReadData> cbReadReceivedFunc);
    void UnSubscribeForReads(Action<ReadData> cbReadReceivedFunc);
    
    void SubscribeForPhenotypes(Action<List<PhenotypeData>> cbPhenotypeReceivedFunc);
    void UnSubscribeForPhenotypes(Action<List<PhenotypeData>> cbPhenotypeReceivedFunc);

}
