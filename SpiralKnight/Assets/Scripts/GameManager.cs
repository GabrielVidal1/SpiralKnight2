using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer.Client;
using Multiplayer.Client.Entities;
using Multiplayer.Server.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    
    public static Dictionary<int, EntityManager> entities = new Dictionary<int, EntityManager>();

    public static Dictionary<int, DestroyableManager> destroyables = new Dictionary<int, DestroyableManager>();
    
    public PlayerManager playerPrefab;
    
    public EnemyManager basicEnemyPrefab;
    public EnemyManager turretEnemyPrefab;

    public ProjectileManager projectilePrefab;
    public DestructibleBlockManager destructibleBlock;

    public bool isServer;

    public static PlayerManager LocalPlayer { get; private set; }
    
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

        _enemyPrefabs[Enemy.Type.basic] = basicEnemyPrefab;
        _enemyPrefabs[Enemy.Type.turret] = turretEnemyPrefab;
    }

    private readonly Dictionary<Enemy.Type, EnemyManager> _enemyPrefabs = new Dictionary<Enemy.Type, EnemyManager>();


    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        Debug.Log($"Spawn player {_id}");
        
        PlayerManager _player = Instantiate(playerPrefab, _position, _rotation);
        
        if (_id == Client.instance.myId)
            LocalPlayer = _player;
        
        _player.Initialize(_id, _username, _id == Client.instance.myId);
        
        players.Add(_id, _player);
        entities.Add(_id, _player);
        destroyables.Add(_id, _player);
    }

    private void OnApplicationQuit()
    {
        if (isServer)
            Multiplayer.Server.Server.Stop();
    }

    public void SpawnEnemy(int _id, Enemy.Type _type, Vector3 _position, Quaternion _rotation, float _maxHealth)
    {
        EnemyManager _enemy = Instantiate(_enemyPrefabs[_type], _position, _rotation);
        _enemy.Initialize(_id, _maxHealth);
        entities.Add(_id, _enemy);
        destroyables.Add(_id, _enemy);
    }

    public void SpawnProjectile(int _id, Vector3 position, Vector3 direction, float speed)
    {
        ProjectileManager projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile.Initialize(_id, direction, speed);
        destroyables.Add(_id, projectile);
    }

    public void SpawnDestructibleBlock(int _id, Vector3 position)
    {
        DestructibleBlockManager _block = Instantiate(destructibleBlock, position, Quaternion.identity);
        destroyables.Add(_id, _block);
    }
}
