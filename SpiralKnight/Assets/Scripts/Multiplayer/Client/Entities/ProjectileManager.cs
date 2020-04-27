using System;
using UnityEngine;

namespace Multiplayer.Client.Entities
{
    public class ProjectileManager : DestroyableManager
    {
        private Vector3 movement;
        
        public void Initialize(int _id, Vector3 direction, float speed)
        {
            base.Initialize(_id);
            transform.forward = direction;

            movement = direction * speed;
        }

        private void Update()
        {
            transform.position += movement * Time.deltaTime;
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}