using UnityEngine;

namespace Multiplayer.Server.Entities
{
    public class Destroyable : MonoBehaviour
    {
        public int id;

        public void Initialize(int _id)
        {
            id = _id;
        }
        
        public void Destroy()
        {
            ServerSend.Destroy(this);
            Destroy(gameObject);
        }
    }
}