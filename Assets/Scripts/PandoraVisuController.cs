using System;
using AnimController;
using UnityEngine;

namespace DefaultNamespace
{
    public class PandoraVisuController: MonoBehaviour
    {

        private IDataManager dataManager;
        private INetInput netInput;
        public AnimationController animationController;
        private StateMachine stateMachine;
        
        
        private void Start()
        {
            InitialiseSetup();
        }

        /// <summary>
        /// Connects components and starts state machine
        /// here is the place where other implementations of interfaces should be connected if wished
        /// </summary>
        private void InitialiseSetup()
        {
            dataManager = new DataManager();
            netInput = new NetInput();
            netInput.SubscribeForReads(dataManager.ReceiveNewRead);
            netInput.SubscribeForPhenotypes(dataManager.ReceiveNewPhenotype);
            animationController.SetDataManager(dataManager);
            stateMachine = StateMachine.Instance;
            stateMachine.SetDataManager(dataManager);
            stateMachine.SetNetInput(netInput);
            netInput.SubscribeForServerStatus(stateMachine.ReceiveNetStatusChanged);
            stateMachine.SetAnimationController(animationController);
            animationController.SubscribeForAnimationsFinished(stateMachine.AnimationsFinished);
            stateMachine.SwitchState();
        }
    }
}