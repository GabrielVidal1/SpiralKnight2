using System;
using UnityEngine;

namespace Multiplayer.Server.Entities
{
    public class Destroyable : MonoBehaviour
    {
        public int id;

        private void Awake()
        {
            id = Server.EntityCount++;
        }

        public void Destroy()
        {
            ServerSend.Destroy(this);
            Destroy(gameObject);
        }
    }
}