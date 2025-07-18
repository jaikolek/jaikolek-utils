using System;
using UnityEngine;

namespace JaikolekUtils.Anim
{
    public class DynamicAnimation : MonoBehaviour
    {
        [SerializeField] protected bool disabledAction = false;
        [SerializeField] protected bool useUnscaledTime = true;
        [SerializeField] protected bool isLooping = false;
        [SerializeField] protected string[] strAnims;

        protected Animation thisAnimation;
        protected AnimationState animationState;
        protected float deltaTime = 0f;
        protected bool tickAnimation;

        public bool IsPlaying => tickAnimation;

        [SerializeField] protected float actionFrame = -1f;
        [SerializeField] protected bool actionInvoked;
        public event Action OnAction;

        public event Action OnAnimationPlayCompleted;

        protected virtual void Awake()
        {
            if (this.gameObject.TryGetComponent(out Animation thisAnimation))
            {
                this.thisAnimation = thisAnimation;
                this.animationState = this.thisAnimation[strAnims.Length > 0 ? strAnims[0] : null];
            }
        }

        protected virtual void Update()
        {
            if (!tickAnimation || animationState == null) return;

            deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            animationState.time += deltaTime * animationState.speed;

            if (actionFrame >= 0f)
            {
                if (animationState.time >= actionFrame.ToSecond() && !actionInvoked)
                {
                    actionInvoked = true;
                    ExecutedAction();
                }
            }

            if (animationState.time >= animationState.length)
            {
                OnAnimationCompleted();

                if (isLooping)
                {
                    animationState.time = 0f;

                    if (actionFrame >= 0f) actionInvoked = false;
                }
                else
                {
                    Stop();
                }
            }

            thisAnimation.Sample();
        }

        public void Play()
        {
            if (animationState == null)
            {
                Debug.LogError($"Err:: Null animation state");
                return;
            }

            animationState.time = 0f;
            animationState.speed = 1f;
            animationState.weight = 1f;
            animationState.enabled = true;
            actionInvoked = false;
            tickAnimation = true;
        }

        public void Play(int index)
        {
            this.animationState = this.thisAnimation[strAnims.Length > 0 ? strAnims[index] : null];
            Play();
        }

        public void Stop()
        {
            if (animationState == null)
            {
                Debug.LogError($"Err:: Null animation state");
                return;
            }

            animationState.time = 0f;
            animationState.speed = 0f;
            animationState.weight = 0f;
            animationState.enabled = false;
            thisAnimation.Sample();

            tickAnimation = false;
        }

        protected void ExecutedAction()
        {
            if (disabledAction) return;
            OnAction?.Invoke();
            OnAction = null;
        }

        protected void OnAnimationCompleted()
        {
            if (disabledAction) return;
            OnAnimationPlayCompleted?.Invoke();
            OnAnimationPlayCompleted = null;
        }
    }
}
