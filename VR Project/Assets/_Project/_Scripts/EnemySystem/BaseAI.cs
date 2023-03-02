using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemySystem
{
    public abstract class BaseAI : MonoBehaviour
    {
        protected StateMachine _stateMachine;

        protected IState initialState;
        
        protected virtual void Awake()
        {
            _stateMachine = new StateMachine();
        }

        [Button]
        public virtual void StartStateMachine(float delay = 0f)
        {
            StartCoroutine(StartWithDelay(initialState, delay));
        }

        private IEnumerator StartWithDelay(IState state, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            _stateMachine.SetState(state);
        }
        
        private void Update() => _stateMachine.Update();

        protected void AddAnyTransition(IState to, Func<bool> condition) =>
            _stateMachine.AddAnyTransition(to, condition);
        
        protected void AddTransition(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        public void Pause(bool isPaused) => _stateMachine.Pause(isPaused);

        public void Stop() => _stateMachine.Stop();

        public bool IsStateMachineStarted() { return _stateMachine.isStarted; }
    }
}
