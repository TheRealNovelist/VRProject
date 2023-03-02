using System;
using BNG;
using UnityEngine;

namespace EnemySystem
{
    public abstract class BaseEnemy : BaseAI
    {
        public Transform Target { get; private set; }
        
        public virtual void OnDamaged()
        {
            
        }

        public virtual void OnDeath()
        {
            
        }

        public virtual void OnRespawn()
        {
            
        }
        
        public virtual void SetTarget()
        {
            Target = GameObject.FindWithTag("Player").transform;
        }
    }
}
