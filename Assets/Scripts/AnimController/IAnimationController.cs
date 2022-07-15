using System;
namespace AnimController
{
    public interface IAnimationController
    {
        void RunIdleAnimation();

        void StartAnimations();

        void EndAnimations();

        void SubscribeForAnimationsFinished(Action cbAnimationFinishedFunc);
        void UnSubscribeForAnimationsFinished(Action cbAnimationFinishedFunc);
    }
}