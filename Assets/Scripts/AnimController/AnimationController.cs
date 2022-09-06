using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AnimController
{ 
    public class AnimationController: MonoBehaviour, IAnimationController
    {
        // Sound Manager Stuff
        public SoundManager sound;

        private enum AnimationState
        {
            Init, StartChaos, StartPheno, RunChaos, StopChaos, RunPheno, StopPheno
        }
        private IDataManager dataManager;
        private Action cbAnimationFinished;
        
        public GameObject chaosAnimationPrefab;
        public GameObject phenoAnimationPrefab;

        private IAnimationConnector currentlyRunningAnimation = null;

        private float maxRuntimeChaos = 380f; //380f; // 3min //
        private float maxRuntimePheno = 94f; // 90s + 4s
        private float currAnimationRuntime = 0f;
        
        private float maxRuntimeChaosFadingOut = 40f; //40s
        private float maxRuntimePhenoFadingOut = 0f; //4s
        private float currFadeoutRuntime = 0f;

        private AnimationState animationState = AnimationState.Init;

        private void Start()
        {
            dataManager = new DataManager();
        }

        void Update()
        {
            RunState();
        }

        public void RunState()
        {
            switch (animationState)
            {
                case AnimationState.RunChaos:
                    OnCaseRunChaos();
                    break;
                case AnimationState.RunPheno:
                    OnCaseRunPheno();
                    break;
                case AnimationState.StartChaos:
                    OnCaseStartChaos();
                    break;
                case AnimationState.StartPheno:
                    OnCaseStartPheno();
                    break;
                case AnimationState.StopChaos:
                    OnCaseStopChaos();
                    break;
                case AnimationState.StopPheno:
                    OnCaseStopPheno();
                    break;
                case AnimationState.Init:
                    animationState = AnimationState.StartChaos;
                    break;
            }
        }
        
        /// <summary>
        /// instantiates chaos prefab and starts chaos animation and sets runchaos
        /// </summary>
        private void OnCaseStartChaos()
        {
            GameObject newAnimation = Instantiate(chaosAnimationPrefab, transform);
            IAnimationConnector animationConnector = newAnimation.GetComponent<IAnimationConnector>();
            currentlyRunningAnimation = animationConnector;
            currentlyRunningAnimation.SetDataManager(dataManager);
            currentlyRunningAnimation.StartAnimation();
            
            animationState = AnimationState.RunChaos;
            currAnimationRuntime = 0f;
            currFadeoutRuntime = 0f;

            // Sound Stuff
            sound.Play("chaos");
        }
        
        /// <summary>
        /// instantiates pheno prefab and starts pheno animation and sets runpheno
        /// </summary>
        private void OnCaseStartPheno()
        {
            GameObject newAnimation = Instantiate(phenoAnimationPrefab, transform);
            IAnimationConnector animationConnector = newAnimation.GetComponent<IAnimationConnector>();
            currentlyRunningAnimation = animationConnector;
            currentlyRunningAnimation.SetDataManager(dataManager);
            currentlyRunningAnimation.StartAnimation();
            
            animationState = AnimationState.RunPheno;
            currAnimationRuntime = 0f;
            currFadeoutRuntime = 0f;

            // Sound Stuff
            sound.Play("phenotypes");
        }
        private void OnCaseRunChaos()
        {
            if (currAnimationRuntime >= maxRuntimeChaos)
            {
                animationState = AnimationState.StopChaos;
                currentlyRunningAnimation.FadeOutAnimation();
                return;
            }
            currAnimationRuntime += Time.deltaTime;
        }
        
        private void OnCaseRunPheno()
        {
            if (currAnimationRuntime >= maxRuntimePheno)
            {
                animationState = AnimationState.StopPheno;
                //Todo currentlyRunningAnimation.FadeOutAnimation();
                return;
            }
            currAnimationRuntime += Time.deltaTime;
        }
        
        private void OnCaseStopChaos()
        {
            if (currFadeoutRuntime >= maxRuntimeChaosFadingOut)
            {
                animationState = AnimationState.StartPheno;
                currentlyRunningAnimation.EndAnimation();
                return;
            }
            currFadeoutRuntime += Time.deltaTime;
        }
        
        private void OnCaseStopPheno()
        {
            if (currFadeoutRuntime >= maxRuntimePhenoFadingOut)
            {
                animationState = AnimationState.StartChaos;
                currentlyRunningAnimation.EndAnimation();
                return;
            }
            currFadeoutRuntime += Time.deltaTime;
        }

        public void SetDataManager(IDataManager iDataManager)
        {
            dataManager = iDataManager;
        }
        
       

        private void CurrentAnimationFinished()
        {
            
        }

        public void RunIdleAnimation()
        {
            throw new NotImplementedException();
        }

        public void StartAnimations()
        {
            /*
            RunAnimation(animations[0]);
            currentAnimationIndex = 0;
            */
            Debug.Log("AnimationController::StartAnimations");
            
        }

        public void EndAnimations()
        {
            throw new System.NotImplementedException();
        }

        

        private void OnAnimationsFinished()
        {
            cbAnimationFinished?.Invoke();
        }

        public void SubscribeForAnimationsFinished(Action cbAnimationFinishedFunc)
        {
            cbAnimationFinished += cbAnimationFinishedFunc;
        }

        public void UnSubscribeForAnimationsFinished(Action cbAnimationFinishedFunc)
        {
            cbAnimationFinished -= cbAnimationFinishedFunc;
        }

        public void OnAnimationFinished()
        {
            //SwitchAnimations();
        }
    }
}