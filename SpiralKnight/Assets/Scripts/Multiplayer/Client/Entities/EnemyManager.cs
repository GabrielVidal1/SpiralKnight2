﻿using Multiplayer.Server.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Multiplayer.Client.Entities
{
    public class EnemyManager : EntityManager
    {
        [SerializeField] private Slider HealthBar;

        private NavMeshAgent _agent;

        public void Spawn(Vector3 _position)
        {
        
        }

        protected override void HealthChangeHook()
        {
            HealthBar.value = health / maxHealth;
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}