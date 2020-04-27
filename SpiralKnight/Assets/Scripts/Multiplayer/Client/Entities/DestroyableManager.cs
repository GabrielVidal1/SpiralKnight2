using UnityEngine;

namespace Multiplayer.Client.Entities
{
    public abstract class DestroyableManager : MonoBehaviour
    {
        public int id;

        public virtual void Initialize(int _id)
        {
            id = _id;
        }

        public abstract void Destroy();
    }
}