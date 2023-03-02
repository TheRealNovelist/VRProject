using UnityEngine;
using UnityEngine.AI;
using System;

namespace EnemySystem.Possessed
{
    public class Possessed : BaseEnemy
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent agent;

        [Header("Settings")]
        public float attackRange = 2f;

        public float attackCooldown = 5f;
        public float damageDealt = 10f;

        private bool isAlive = true;
        
        protected override void Awake()
        {
            base.Awake();
            if (!agent)
                agent = GetComponent<NavMeshAgent>();
            
            if (Target == null)
                SetTarget();
        }

        private void Start()
        {
            StartStateMachine(3f);
        }

   
        public override void StartStateMachine(float delay = 0f)
        {
            if (IsStateMachineStarted()) return;
            
            var MoveToPlayer = new MoveToPlayer(this, agent);
            var Attacking = new Attacking(this);
            var Waiting = new Blank();

            AddTransition(MoveToPlayer, Attacking, TargetInRange());
            AddAnyTransition(MoveToPlayer, TargetOutRange());
            AddAnyTransition(Waiting, () => !isAlive);
            AddTransition(Waiting, MoveToPlayer, () => isAlive);

            initialState = MoveToPlayer;
            
            Func<bool> TargetInRange() => () => Vector3.Distance(Target.position, transform.position) <= attackRange;
            Func<bool> TargetOutRange() => () => Vector3.Distance(Target.position, transform.position) > attackRange 
                                                 && isAlive;

            base.StartStateMachine(delay);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            isAlive = false;
        }

        public override void OnRespawn()
        {
            base.OnRespawn();
            isAlive = true;
        }
    }

    

    
}