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
            Vector3 pos = GameManager.players.Values.ToList()[0].transform.position;
            _agent.SetDestination(pos);
            direction = transform.forward;
        }
    }
}