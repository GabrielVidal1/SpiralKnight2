using System;
using UnityEngine;
using UnityEngine.Events;

namespace Multiplayer.Server.Entities
{
    [RequireComponent(typeof(Collider))]
    public class TriggerZone : MonoBehaviour
    {
        [Serializable]
        private class TriggerEvent : UnityEvent<Entity> { }
        
        [SerializeField] private TriggerEvent toTrigger;
        private void OnTriggerEnter(Collider _other)
        {
            if (_other.CompareTag("Player"))
                toTrigger.Invoke(_other.GetComponent<Entity>());
        }
    }
}
