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
        public enum Type
        {
            basic,
            turret
        }

        public Type enemyType;
        
        protected virtual float Range => 2f;

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
                _agent.enabled = false;

                Vector3 _targetPosition = _target.position - transform.position;
                _targetPosition.y = 0;

                float _angle = Vector3.SignedAngle(_targetPosition, transform.forward, Vector3.up);
                
                if (Mathf.Abs(_angle) > 5)
                {
                    transform.Rotate(Vector3.up, - Math.Sign(_angle) * Time.deltaTime * _agent.angularSpeed);
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                _agent.enabled = true;
                _agent.SetDestination(_target.position);
            }

            direction = transform.forward;
        }

        IEnumerator Attack()
        {
            _attacking = true;
            yield return new WaitForSeconds(0.5f);
            base.Attack(direction);
            yield return new WaitForSeconds(0.5f);
            _attacking = false;
        }

        public void SetTarget(Entity _player)
        {
            _target = _player.transform;
        }
    }
}