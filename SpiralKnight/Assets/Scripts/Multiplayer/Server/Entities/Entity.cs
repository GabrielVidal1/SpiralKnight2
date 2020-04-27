using System;
using System.Collections;
using UnityEngine;

namespace Multiplayer.Server.Entities
{
    public abstract class Entity : Destroyable
    {
        public float health;
        public float maxHealth;

        public Vector3 direction;

        public virtual void Initialize(float _maxHealth)
        {
            maxHealth = _maxHealth;
            health = maxHealth;
        }
        
        public virtual void Attack(Vector3 _direction)
        {
            if (health <= 0f) return;
            
            ServerSend.EntityAttack(this);
            
            Collider[] _cols = new Collider[10];
            int _size = Physics.OverlapSphereNonAlloc(transform.position + _direction, 0.5f, _cols);
            for (int i = 0; i < _size; i++)
            {
                Collider _collider = _cols[i];
                if (_collider.CompareTag("Player"))
                {
                    _collider.GetComponent<Player>().TakeDamage(10);
                }
            }
        }
        
        public void TakeDamage(float _damage)
        {
            if (health <= 0f)
            {
                return;
            }

            health -= _damage;
            if (health <= 0f)
            {
                Die();
            }

            ServerSend.EntityHealth(this);
        }

        public abstract void Die();

        private void FixedUpdate()
        {
            EachTick();
            ServerSend.EntityTransform(this);
        }

        protected abstract void EachTick();
    }
}