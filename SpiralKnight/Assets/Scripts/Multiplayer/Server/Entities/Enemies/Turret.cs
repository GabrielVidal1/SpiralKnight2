using UnityEngine;

namespace Multiplayer.Server.Entities.Enemies
{
    public class Turret : Enemy
    {
        protected override float Range => 10;
        public override Type enemyType => Type.turret;

        [SerializeField] private Transform projectileSpawnPoint;
        
        public override void Attack(Vector3 _direction)
        {
            NetworkManager.instance.SpawnProjectile(projectileSpawnPoint.position, _direction, 10, 4, false);
        }
    }
}