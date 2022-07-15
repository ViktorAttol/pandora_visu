using System.Collections;
using System.Collections.Generic;
using AnimController;
using Net;
using State;
using UnityEngine;

public class StateMachine: IAnimationsFinishedReceiver
{

    private static StateMachine instance = null;

    private INetInput netInput;
    private IAnimationController animationController;
    private IDataManager dataManager;
    
    private StateMachine() {}

    public static StateMachine Instance => instance ??= new StateMachine();

    

    private VisuState state = VisuState.Init;

    public void SwitchState()
    {
        switch (state)
        {
            case VisuState.Init:
                CaseInit();
                break;
            case VisuState.Idle:
                CaseIdle();
                break;
            case VisuState.Connect:
                CaseConnect();
                break;
            case VisuState.Run:
                CaseRun();
                break;
            case VisuState.End:
                CaseEnd();
                break;
            case VisuState.Error:
                CaseError();
                break;
            case VisuState.Exit:
                CaseExit();
                break;
        }
    }

    // software is startet
    // starts server and switches to idle
    private void CaseInit()
    {
        Debug.Log("StateMachine - Init");

        netInput.StartServer();
        state = VisuState.Idle;
        SwitchState();
    }

    // after connect
    // show idle animations till client startet sending reads
    private void CaseIdle()
    {
        Debug.Log("StateMachine - Idle");
        animationController.RunIdleAnimation();
    }

    // after idle. is set when client is connected to server
    // starts animation an set state to start
    private void CaseConnect()
    {
        Debug.Log("StateMachine - Connect");

        animationController.StartAnimations();
        state = VisuState.Run;
        SwitchState();
    }

    //after connected. animation is running. all is fine nothing has to be done
    private void CaseRun()
    {
        Debug.Log("StateMachine - Run");

    }
    

    // after caseRun clear all lists and prepare software for new connection and idle state
    private void CaseEnd()
    {
        Debug.Log("StateMachine - End");
        netInput.EndServer();
        dataManager.ClearLists();
        state = VisuState.Init;
        SwitchState();
    }

    // can happen every time -> error animation?
    private void CaseError()
    {
        Debug.Log("StateMachine - Error Case!!!!!");
        state = VisuState.Exit;
        SwitchState();
    }

    // should shut down sockets and exit application
    private void CaseExit()
    {
        Debug.Log("StateMachine - Exit State");
    }

    public void SetNetInput(INetInput netInput)
    {
        this.netInput = netInput;
    }

    public void SetAnimationController(IAnimationController animationController)
    {
        this.animationController = animationController;
    }

    public void SetDataManager(IDataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void AnimationsFinished()
    {
        state = state == VisuState.Run ? VisuState.End : VisuState.Error;
        SwitchState();
    }

    public void ReceiveNetStatusChanged(NetStatus netStatus)
    {
        Debug.Log("StateMachine::NetworkStatusReceived: " + netStatus);
        if (state == VisuState.Idle && netStatus == NetStatus.ClientConnected)
        {
            state = VisuState.Connect;
            SwitchState();
        } 
    }
}
