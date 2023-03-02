using System;
using UnityEngine.AI;

namespace EnemySystem.Possessed
{
    internal class MoveToPlayer : IState
    {
        private readonly BaseEnemy _root;
        private readonly NavMeshAgent _navMeshAgent;
        
        public MoveToPlayer(BaseEnemy root, NavMeshAgent agent)
        {
            _root = root;
            _navMeshAgent = agent;
        }

        public void Update()
        {
            _navMeshAgent.SetDestination(_root.Target.position);
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
        }
    }
}