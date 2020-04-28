using System;
using System.Collections.Generic;
using Multiplayer.Server.Entities;
using UnityEngine;

namespace Multiplayer.Server
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager instance;

        public Player playerPrefab;
        public Enemy basicEnemyPrefab;
        public Enemy turretEnemyPrefab;
        public Projectile projectilePrefab;
        public DestructibleBlock destructibleBlockPrefab;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }

            enemyPrefabs[Enemy.Type.basic] = basicEnemyPrefab;
            enemyPrefabs[Enemy.Type.turret] = turretEnemyPrefab;
        }
        
        private Dictionary<Enemy.Type, Enemy> enemyPrefabs = new Dictionary<Enemy.Type, Enemy>();

        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.M))
            {
                SpawnEnemy(Vector3.zero, Input.GetKeyDown(KeyCode.P) ? Enemy.Type.basic : Enemy.Type.turret);
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                SpawnDestructibleBlock(Vector3.up);
            }
        }

        public void SpawnEnemy(Vector3 _position, Enemy.Type _type)
        {
            Enemy _enemy = Instantiate(enemyPrefabs[_type], _position, Quaternion.identity);
            _enemy.Initialize(50);
            ServerSend.Spawn(_enemy);
        }

        public Player InstantiatePlayer()
        {
            return Instantiate(playerPrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        }

        public void SpawnProjectile(Vector3 _position, Vector3 _direction, float _damage, float _speed, bool _ofPlayer)
        {
            Projectile _projectile = Instantiate(projectilePrefab);
            _projectile.Initialize(_position, _direction, _damage, _speed, _ofPlayer);
            ServerSend.Spawn(_projectile);
        }

        public void SpawnDestructibleBlock(Vector3 position)
        {
            DestructibleBlock _destructibleBlock = Instantiate(destructibleBlockPrefab, position, Quaternion.identity);
            ServerSend.Spawn(_destructibleBlock);
        }
    }
}