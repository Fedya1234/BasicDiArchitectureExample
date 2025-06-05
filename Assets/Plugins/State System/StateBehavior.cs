using System;
using UnityEngine;

namespace State_System
{
    public class StateBehavior<T> : IStateBehavior where T : MonoBehaviour
    {
        public T Unit { get; private set; }

        protected Vector3 Position => Unit.transform.position;

        public event Action OnFinished;
        protected void InvokeOnFinished()
        {
            OnFinished?.Invoke();
        }

        public StateBehavior(T unit)
        {
            Unit = unit;
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnEnd()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public void SubscribeOnFinish(Action callback)
        {
            OnFinished += callback;
        }

        public void UnsubscribeFromFinish(Action callback)
        {
            OnFinished -= callback;
        }
    }

    public class StateTransition<T> where T : System.Enum
    {
        public StateTransitionType transitionType;
        public delegate bool EvaluateDelegate(out T nextState);
        public EvaluateDelegate Evaluate { get; set; }

        public StateTransition(EvaluateDelegate evaluate, StateTransitionType transitionType = StateTransitionType.Independent)
        {
            this.transitionType = transitionType;
            Evaluate = evaluate;
        }
    }

    public enum StateTransitionType
    {
        Independent,
        OnFinish,
    }

    public interface IStateBehavior
    {
        void OnUpdate();
        void OnEnd();
        void OnStart();

        void SubscribeOnFinish(Action callback);
        void UnsubscribeFromFinish(Action callback);
    }
}