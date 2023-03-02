using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySystem
{
    public class StateMachine
    {
        private IState _currentState;
        
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();

        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        public bool isStarted;
        public bool isPaused;

        //Ticking the state machine every frame
        public void Update()
        {
            if (isPaused || !isStarted)
                return;
            
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);
            
            Debug.Log("Current State: " + _currentState);
            _currentState?.Update();
        }

        //Allow to pause the state machine, cease operation and continue
        public void Pause(bool pause)
        {
            if (pause)
                _currentState.OnExit();
            else
                _currentState.OnEnter();
            
            isPaused = pause;
        }
        
        //Stop the state machine, clear any transitions and reset
        public void Stop()
        {
            isStarted = false;

            _currentState = null;
            
            _transitions = new Dictionary<Type, List<Transition>>();
            _currentTransitions = new List<Transition>();
            _anyTransitions = new List<Transition>();
        }
        
        //Transfer to next state
        public void SetState(IState state)
        {
            if (isPaused) return;

            if (!isStarted) isStarted = true;
            
            if (state == _currentState)
                return;
            
            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;
            
            _currentState.OnEnter();
        }

        //Add new transition between state
        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }
            
            transitions.Add(new Transition(to, condition));
        }

        //Add transition from any state
        public void AddAnyTransition(IState state, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(state, condition));
        }
        
        //Private class for transition storage
        private class Transition
        {
            public IState To { get; }
            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        //Get a transition. In order of any state transition first, then individual transition.
        private Transition GetTransition()
        {
            foreach(var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;
            
            foreach(var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }
}
