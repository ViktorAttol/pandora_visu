using System;
namespace AnimController
{
    /// <summary>
    /// Interface to control the Animation Controller
    /// Should be called by the state machine
    /// </summary>
    public interface IAnimationController
    {
        void RunIdleAnimation();

        void StartAnimations();

        void EndAnimations();

        void SubscribeForAnimationsFinished(Action cbAnimationFinishedFunc);
        void UnSubscribeForAnimationsFinished(Action cbAnimationFinishedFunc);
    }
}