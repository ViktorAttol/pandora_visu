using System;
using UnityEngine;

namespace AnimController
{
    public class AnimationConnectorImpl : MonoBehaviour, IAnimationConnector
    {
        private Action cbAnimFinished;
       
        public void SetDataManager(IDataManager dataManager)
        {
            throw new NotImplementedException();
        }

        public void StartAnimation()
        {
            throw new NotImplementedException();
        }

        public void FadeOutAnimation()
        {
            throw new NotImplementedException();
        }

        public void EndAnimation()
        {
            throw new NotImplementedException();
        }
        

        private void OnAnimationFinished()
        {
            cbAnimFinished?.Invoke();
        }

        public void SubscribeForAnimationFinished(Action finished)
        {
            cbAnimFinished += finished;
        }
        public void UnSubscribeForAnimationFinished(Action finished)
        {
            cbAnimFinished -= finished;
        }
    }
}