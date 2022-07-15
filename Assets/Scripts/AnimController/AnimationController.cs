using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimController
{
    public class AnimationController: MonoBehaviour, IAnimationController
    {
        private IDataManager dataManager;
        private Action cbAnimationFinished;
        public GameObject animationConnector;
        public GameObject idleAnimationPrefab;

        private GameObject currentAnimationConnector;
        private GameObject nextAnimationConnector;

        private GameObject idleAnimation;
        [SerializeField] public List<GameObject> animations = new List<GameObject>();
        private int currentAnimationIndex;

        private void Update()
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

        

        private void ReadInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchAnimations();
            }
        }
        
        public void RunIdleAnimation()
        {
            RunAnimation(idleAnimationPrefab);
            currentAnimationIndex = -1;
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