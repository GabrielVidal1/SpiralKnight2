namespace Multiplayer.Server.Entities
{
    public class DestructibleBlock : Destructible
    {
        public override void Hit()
        {
            Destroy();
        }
    }
}
