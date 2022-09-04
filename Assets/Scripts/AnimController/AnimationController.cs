using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimController
{
    
    
    
    public class AnimationController: MonoBehaviour, IAnimationController
    {
        private enum AnimationType
        {
            Chaos, Pheno, None
        }
        public GameObject prefabTest;
        
        private IDataManager dataManager;
        private Action cbAnimationFinished;
        public GameObject animationConnector;
        public GameObject idleAnimationPrefab;

        private GameObject currentAnimationConnector;
        private GameObject nextAnimationConnector;

        private GameObject idleAnimation;
        [SerializeField] public List<GameObject> animations = new List<GameObject>();
        private int currentAnimationIndex;

        public GameObject chaosAnimationPrefab;
        public GameObject phenoAnimationPrefab;

        private AnimationType currentlyRunninganimationType = AnimationType.None;
        private IAnimationConnector currentlyRunningAnimation = null;

        private bool animationIsRunning = false;
        public bool isFadingOut = false;
        private float runnTimeChaos = 25f; // 3min
        private float runnTimePheno = 25f; // 90s + 4s // edit: 180s + 4s
        private float currRunnTimeChaos = 0f;
        private float currRunnTimePheno = 0f;
        
        private float runnTimeChaosFadingOut = 25f; //
        private float runnTimePhenoFadingOut = 4f; //4s
        private float currRunnTimeChaosFadingOut = 0f;
        private float currRunnTimePhenoFadingOut = 0f;
        
        private void Start()
        {
            dataManager = new DataManager();
        }

        void Update()
        {
            ReadInput();
        }

        public void SetDataManager(IDataManager iDataManager)
        {
            dataManager = iDataManager;
        }

        private GameObject BuildAnimationWithConnector(GameObject anim)
        {
            GameObject connector = Instantiate(animationConnector, transform);
            AnimationConnector controllerComponent = connector.GetComponent<AnimationConnector>();
            controllerComponent.SetAnimationPrefab(anim);
            return connector;
        }

        private void RunAnimation(GameObject prefab)
        {
            nextAnimationConnector = BuildAnimationWithConnector(prefab);
            AnimationConnector controllerComponent = nextAnimationConnector.GetComponent<AnimationConnector>();
            controllerComponent.SubscribeForAnimationFinished(OnAnimationFinished);
            if (currentAnimationConnector != null)
            {
                AnimationConnector currentControllerComponent = currentAnimationConnector.GetComponent<AnimationConnector>();
                currentControllerComponent.EndAnimation();
                Destroy(currentAnimationConnector);
            }
            controllerComponent.StartAnimation();
            currentAnimationConnector = nextAnimationConnector;
        }

        private void SwitchAnimation()
        {
            if (currentlyRunningAnimation == null || currentlyRunninganimationType == AnimationType.Pheno)
            {
                DestroyAndStartAnimation(AnimationType.Chaos);
            }

            else if (currentlyRunninganimationType == AnimationType.Chaos)
            {
                DestroyAndStartAnimation(AnimationType.Pheno);
            }
        }

        private void DestroyAndStartAnimation(AnimationType typeToStart)
        {
            if(currentlyRunningAnimation != null) currentlyRunningAnimation.EndAnimation();
            
            GameObject newAnimation = null;
            if (typeToStart == AnimationType.Chaos)
            {
                newAnimation = Instantiate(chaosAnimationPrefab, transform);
                currentlyRunninganimationType = AnimationType.Chaos;
            }
            
            if (typeToStart == AnimationType.Pheno)
            {
                newAnimation = Instantiate(phenoAnimationPrefab, transform);
                currentlyRunninganimationType = AnimationType.Pheno;
            }
            
            IAnimationConnector animationConnector = newAnimation.GetComponent<IAnimationConnector>();
            currentlyRunningAnimation = animationConnector;
            currentlyRunningAnimation.SetDataManager(dataManager);
            currentlyRunningAnimation.StartAnimation();
        }

        private void ReadInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchAnimation();

                //SwitchAnimations();
            }
        }
        
        public void RunIdleAnimation()
        {
            IAnimationConnector connector = prefabTest.GetComponent<IAnimationConnector>();
            connector.SetDataManager(dataManager);
            connector.SubscribeForAnimationFinished(CurrentAnimationFinished);
            connector.StartAnimation();
            
            //RunAnimation(idleAnimationPrefab);
            //urrentAnimationIndex = -1;
        }

        private void CurrentAnimationFinished()
        {
            
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

        private void SwitchAnimations()
        {
            currentAnimationIndex++;
            if (currentAnimationIndex > animations.Count)
            {
                return;
            }
            if (currentAnimationIndex == animations.Count)
            {
                RunAnimation(animations[0]);
            }
            else
            {
                RunAnimation(animations[currentAnimationIndex]);
            }
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