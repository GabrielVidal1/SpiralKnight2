using Multiplayer.Server.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Multiplayer.Client.Entities
{
    public class EnemyManager : EntityManager
    {
        [SerializeField] private Slider HealthBar;

        private NavMeshAgent _agent;

        protected override void HealthChangeHook()
        {
            HealthBar.value = health / maxHealth;
        }

        public override void Destroy()
        {
            GameManager.entities.Remove(id);

            Destroy(gameObject);
        }
    }
}