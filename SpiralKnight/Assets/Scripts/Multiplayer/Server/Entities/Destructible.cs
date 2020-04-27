namespace Multiplayer.Server.Entities
{
    public abstract class Destructible : Destroyable
    {
        public abstract void Hit();
    }
}