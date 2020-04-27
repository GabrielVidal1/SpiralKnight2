using System.Collections;
using UnityEngine;

namespace Multiplayer.Client.Entities
{
    public abstract class EntityManager : DestroyableManager
    {
        public float health;
        public float maxHealth;

        
        [SerializeField] private MeshRenderer cylinder;
        [SerializeField] private Material cylAttack;
        [SerializeField] private Material cylDefault;

        
        [SerializeField] private Transform mesh;

        public void Initialize(int _id, float _maxHealth)
        {
            maxHealth = _maxHealth;
            
            base.Initialize(_id);
            
            health = maxHealth;
            HealthChangeHook();
        }
        
        
        public void SetDirection(Vector3 _direction)
        {
            mesh.forward = _direction;
        }

        public void SetPosition(Vector3 _position)
        {
            transform.position = _position;
        }

        public void SetHealth(float _health)
        {
            health = _health;
            HealthChangeHook();
            if (health <= 0f)
            {
                Die();
            }
        }

        protected virtual void HealthChangeHook()
        {
            
        }

        protected virtual void Die()
        {
            Destroy();
        }

        public void Attack()
        {
            StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            cylinder.material = cylAttack;
            yield return new WaitForSeconds(0.5f);
            cylinder.material = cylDefault;
        }
    }
}