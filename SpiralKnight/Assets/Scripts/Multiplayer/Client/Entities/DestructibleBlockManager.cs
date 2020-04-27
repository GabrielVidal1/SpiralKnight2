namespace Multiplayer.Client.Entities
{
    public class DestructibleBlockManager : DestroyableManager
    {
        public override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}