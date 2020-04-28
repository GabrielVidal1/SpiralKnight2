using System;
using System.Linq;
using Multiplayer.Client.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Multiplayer.Server.Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Entity
    {
        private NavMeshAgent _agent;
        private Transform _target;
        
        public override void Initialize(float _maxHealth)
        {
            base.Initialize(_maxHealth);
            _agent = GetComponent<NavMeshAgent>();
        }

        public override void Die()
        {
            Destroy();
        }

        protected override void EachTick()
        {
            if (_target == null) return;
            
            _agent.SetDestination(_target.position);
            direction = transform.forward;
        }

        public void SetTarget(Entity _player)
        {
            _target = _player.transform;
        }
    }
}