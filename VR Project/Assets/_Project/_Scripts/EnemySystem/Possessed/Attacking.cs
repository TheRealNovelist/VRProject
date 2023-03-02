using BNG;
using UnityEngine;

namespace EnemySystem.Possessed
{
    internal class Attacking : IState
    {
        private readonly Possessed _possessed;

        private readonly Damageable _targetDamage;
        
        private float cooldown = 0f;
        
        public Attacking(Possessed possessed)
        {
            _possessed = possessed;

            _targetDamage = _possessed.Target.GetComponent<Damageable>();
        }

        public void Update()
        {
            if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
                return;
            }
            
            //Attack animation
            _targetDamage?.DealDamage(_possessed.damageDealt);
            cooldown = _possessed.attackCooldown;
            
            Debug.DrawLine(_possessed.transform.position, _possessed.transform.position + Vector3.forward * _possessed.attackRange);
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}