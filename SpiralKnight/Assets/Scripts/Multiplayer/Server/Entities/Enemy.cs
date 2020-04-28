using System;
using System.Collections;
using System.Linq;
using Multiplayer.Client.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Multiplayer.Server.Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Entity
    {
        private const float Range = 2f;

        private NavMeshAgent _agent;
        private Transform _target;

        private bool _attacking;

        private float _speed;
        
        public override void Initialize(float _maxHealth)
        {
            base.Initialize(_maxHealth);
            _agent = GetComponent<NavMeshAgent>();
            _speed = _agent.speed;
        }

        public override void Die()
        {
            Destroy();
        }

        protected override void EachTick()
        {
            if (_target == null) return;

            if (_attacking) return;
            
            if ((_target.position - transform.position).sqrMagnitude < Range * Range)
            {
                StartCoroutine(Attack());
            }
            else
            {
                _agent.SetDestination(_target.position);
                direction = transform.forward;
            }
        }

        IEnumerator Attack()
        {
            _attacking = true;
            _agent.speed = 0;
            
            
            yield return new WaitForSeconds(0.5f);
            base.Attack(transform.forward);
            yield return new WaitForSeconds(0.5f);
            _agent.speed = _speed;
            _attacking = false;
        }

        public void SetTarget(Entity _player)
        {
            _target = _player.transform;
        }
    }
}