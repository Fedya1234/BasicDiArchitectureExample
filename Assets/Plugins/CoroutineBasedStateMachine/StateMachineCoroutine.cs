using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// var idleState = new IdleState();
// var walkingState = new WalkingState();
// var jumpState = new JumpState();
//         
// AddState(idleState);
// AddState(walkingState);
// AddState(jumpState);
//         
// SetAutomaticTransition<IdleState, WalkingState>();
// SetAutomaticTransition<WalkingState, IdleState>();
// SetAutomaticTransition<JumpState, WalkingState>();
//         
// SetConditionalTransition<WalkingState, JumpState>(() => Input.GetKeyDown(KeyCode.Space));
//         
// SetState<IdleState>();

namespace StateMachines.CoroutineBasedStateMachine
{
  public abstract class UnitState<T> : State where T : MonoBehaviour
  {
    protected T Unit { get; private set; }

    protected UnitState(T unit)
    {
      Unit = unit;
    }
  }
  
  public class AbstractState : State
  {
    private readonly Func<IEnumerator> _onExplosionCoroutine;
    private Action _onStopAction;

    public AbstractState(Func<IEnumerator> onExplosionCoroutine, Action onStopAction = null)
    {
      _onExplosionCoroutine = onExplosionCoroutine;
      _onStopAction = onStopAction;
    }

    public override IEnumerator Execute()
    {
      yield return _onExplosionCoroutine.Invoke();
    }

    public override void OnStop()
    {
      _onStopAction?.Invoke();
    }
  }
  
  public abstract class CoolDownUnitState<T> : UnitState<T> where T : MonoBehaviour
  {
    private readonly float _cooldown;
    private float _lastCastTime;
    public virtual bool IsCanCast => _lastCastTime + _cooldown < Time.time;
    protected CoolDownUnitState(T unit, float cooldown) : base(unit)
    {
      _cooldown = cooldown;
    }
    
    protected void SetLastCastTime() => 
      _lastCastTime = Time.time;
  }

  public abstract class State
  {
    public abstract IEnumerator Execute();
    public abstract void OnStop();
  }

  public class StateMachineCoroutine : MonoBehaviour
  {
    [SerializeField] private bool _debug;
    [SerializeField][ReadOnly] private string _currentStateName;
    private readonly Dictionary<Type, State> _states = new();
    private readonly Dictionary<State, State> _automaticTransitions = new();
    private readonly Dictionary<State, List<(State to, Func<bool> condition)>> _conditionalTransitions = new();
    private State _currentState;
    private State _startState;
    private Coroutine _stateRoutine;

    private void OnDisable()
    {
      if (_currentState == null)
        return;
      
      StopCoroutine(_stateRoutine);
      _currentState.OnStop();
    }

    protected virtual void OnEnable()
    {
      if (_startState != null)
      {
        SetStateInstance(_startState);
      }
    }

    private void Update()
    {
      if (_currentState == null || !_conditionalTransitions.TryGetValue(_currentState, out var transitions))
        return;
      
      foreach (var (to, condition) in transitions)
      {
        if (!condition.Invoke())
          continue;
        
        SetStateInstance(to);
        break;
      }
    }
    
    public void AddState(State state)
    {
      _states[state.GetType()] = state;
    }

    // Legacy method for backward compatibility
    public void SetAutomaticTransition<TFrom, TTo>() where TFrom : State where TTo : State
    {
      State fromState = _states[typeof(TFrom)];
      State toState = _states[typeof(TTo)];
      SetAutomaticTransition(fromState, toState);
    }

    // New instance-based method
    public void SetAutomaticTransition(State fromState, State toState)
    {
      _automaticTransitions[fromState] = toState;
    }

    // Legacy method for backward compatibility
    public void SetConditionalTransition<TFrom, TTo>(Func<bool> condition) where TFrom : State where TTo : State
    {
      State fromState = _states[typeof(TFrom)];
      State toState = _states[typeof(TTo)];
      SetConditionalTransition(fromState, toState, condition);
    }

    // New instance-based method
    public void SetConditionalTransition(State fromState, State toState, Func<bool> condition)
    {
      if (!_conditionalTransitions.ContainsKey(fromState))
      {
        _conditionalTransitions[fromState] = new List<(State, Func<bool>)>();
      }

      _conditionalTransitions[fromState].Add((toState, condition));
    }

    public void SetState(Type stateType)
    {
      if (_states.TryGetValue(stateType, out State newState))
      {
        SetStateInstance(newState);
      }
    }
    
    public void SetState<T>() where T : State => 
      SetState(typeof(T));

    // Get a state instance by type
    protected T GetState<T>() where T : State
    {
      return (T)_states[typeof(T)];
    }

    // New method to set state by instance
    public void SetStateInstance(State state)
    {
      if (_currentState != null)
      {
        StopCoroutine(_stateRoutine);
        _currentState.OnStop();
      }

      if (_debug)
        Debug.Log($"Set state: from {_currentState?.GetType().Name} to {state.GetType().Name}");
      
      _currentStateName = state.GetType().Name;
      
      _currentState = state;
      _stateRoutine = StartCoroutine(RunState());
    }

    public void SetStartState(State state)
    {
      _startState = state;
    }

    public void SetStartState<T>() where T : State
    {
      _startState = GetState<T>();
    }

    public void ResetToStartState()
    {
      if (_startState != null)
      {
        SetStateInstance(_startState);
      }
    }

    private IEnumerator RunState()
    {
      yield return _currentState.Execute();

      if (_automaticTransitions.TryGetValue(_currentState, out State nextState))
      {
        _currentState = null;
        SetStateInstance(nextState);
      }
    }
  }
}