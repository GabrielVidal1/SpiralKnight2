using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Multiplayer.Client.Entities
{
    public class PlayerManager : EntityManager
    {
        public string username;
        
        public MeshRenderer model;
    
        [SerializeField] private Camera camera;
    
        public void Initialize(int _id, string _username, bool isLocalPlayer)
        {
            maxHealth = 100f;
            
            base.Initialize(_id);

            username = _username;

            if (!isLocalPlayer)
            {
                Destroy(GetComponent<PlayerController>());
                Destroy(camera.gameObject);
            }
            
            
        }

        protected override void HealthChangeHook()
        {
            UIManager.instance.UpdateHealth();
        }

        protected override void Die()
        {
            model.enabled = false;
        }

        public void Respawn()
        {
            model.enabled = true;
            SetHealth(maxHealth);
        }


        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }
    }
}
