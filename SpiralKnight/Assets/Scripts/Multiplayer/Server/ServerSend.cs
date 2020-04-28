using Multiplayer.Server.Entities;
using UnityEngine;

namespace Multiplayer.Server
{
    public class ServerSend
    {
        #region Send
        
        /// <summary>Sends a packet to a client via TCP.</summary>
        /// <param name="_toClient">The client to send the packet the packet to.</param>
        /// <param name="_packet">The packet to send to the client.</param>
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            global::Multiplayer.Server.Server.clients[_toClient].tcp.SendData(_packet);
        }

        /// <summary>Sends a packet to a client via UDP.</summary>
        /// <param name="_toClient">The client to send the packet the packet to.</param>
        /// <param name="_packet">The packet to send to the client.</param>
        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            global::Multiplayer.Server.Server.clients[_toClient].udp.SendData(_packet);
        }

        /// <summary>Sends a packet to all clients via TCP.</summary>
        /// <param name="_packet">The packet to send.</param>
        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= global::Multiplayer.Server.Server.MaxPlayers; i++)
            {
                global::Multiplayer.Server.Server.clients[i].tcp.SendData(_packet);
            }
        }

        /// <summary>Sends a packet to all clients except one via TCP.</summary>
        /// <param name="_exceptClient">The client to NOT send the data to.</param>
        /// <param name="_packet">The packet to send.</param>
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= global::Multiplayer.Server.Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    global::Multiplayer.Server.Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        /// <summary>Sends a packet to all clients via UDP.</summary>
        /// <param name="_packet">The packet to send.</param>
        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= global::Multiplayer.Server.Server.MaxPlayers; i++)
            {
                global::Multiplayer.Server.Server.clients[i].udp.SendData(_packet);
            }
        }

        /// <summary>Sends a packet to all clients except one via UDP.</summary>
        /// <param name="_exceptClient">The client to NOT send the data to.</param>
        /// <param name="_packet">The packet to send.</param>
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= global::Multiplayer.Server.Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    global::Multiplayer.Server.Server.clients[i].udp.SendData(_packet);
                }
            }
        }
        
        #endregion

        #region Packets

        /// <summary>Sends a welcome message to the given client.</summary>
        /// <param name="_toClient">The client to send the packet to.</param>
        /// <param name="_msg">The message to send.</param>
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int) ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        #region Spawning
        
        /// <summary>Tells a client to spawn a player.</summary>
        /// <param name="_toClient">The client that should spawn the player.</param>
        /// <param name="_player">The player to spawn.</param>
        public static void Spawn(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int) ServerPackets.spawnPlayer))
            {
                Debug.Log($"id before send : {_player.id}");
                
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.transform.position);
                _packet.Write(_player.transform.rotation);

                SendTCPData(_toClient, _packet);
            }
        }
        
        
        public static void Spawn(Enemy _enemy)
        {
            using (Packet _packet = new Packet((int) ServerPackets.spawnEnemy))
            {
                _packet.Write(_enemy.id);
                _packet.Write((int)_enemy.enemyType);
                _packet.Write(_enemy.transform.position);
                _packet.Write(_enemy.transform.rotation);
                _packet.Write(_enemy.maxHealth);

                SendTCPDataToAll(_packet);
            }
        }
        
        public static void Spawn(Projectile _projectile)
        {
            using (Packet _packet = new Packet((int) ServerPackets.projectileSpawn))
            {
                _packet.Write(_projectile.id);
                
                _packet.Write(_projectile.transform.position);
                _packet.Write(_projectile.transform.forward);
                
                _packet.Write(_projectile.speed);
                
                SendTCPDataToAll(_packet);
            }
        }

        public static void Spawn(DestructibleBlock _destructibleBlock)
        {
            using (Packet _packet = new Packet((int) ServerPackets.blockSpawn))
            {
                _packet.Write(_destructibleBlock.id);
                _packet.Write(_destructibleBlock.transform.position);
                SendTCPDataToAll(_packet);
            }
        }
        
        #endregion

        /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
        /// <param name="_entity">The player whose rotation to update.</param>
        public static void EntityTransform(Entity _entity)
        {
            using (Packet _packet = new Packet((int) ServerPackets.entityTransform))
            {
                _packet.Write(_entity.id);
                _packet.Write(_entity.transform.position);
                _packet.Write(_entity.direction);

                SendUDPDataToAll(_packet);
            }
        }

        public static void EntityHealth(Entity _entity)
        {
            using (Packet _packet = new Packet((int) ServerPackets.entityHealth))
            {
                _packet.Write(_entity.id);
                _packet.Write(_entity.health);

                SendTCPDataToAll(_packet);
            }
        }
        
        public static void EntityAttack(Entity _entity)
        {
            using (Packet _packet = new Packet((int) ServerPackets.entityAttack))
            {
                _packet.Write(_entity.id);
                _packet.Write(_entity.direction);
                
                SendTCPDataToAll(_packet);
            }
        }
        
        public static void PlayerDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int) ServerPackets.playerDisconnected))
            {
                _packet.Write(_playerId);

                SendTCPDataToAll(_packet);
            }
        }
        
        public static void PlayerRespawned(Player _player)
        {
            using (Packet _packet = new Packet((int) ServerPackets.playerRespawned))
            {
                _packet.Write(_player.id);

                SendTCPDataToAll(_packet);
            }
        }

        public static void Destroy(Destroyable _destroyable)
        {
            using (Packet _packet = new Packet((int) ServerPackets.destroy))
            {
                _packet.Write(_destroyable.id);
                SendTCPDataToAll(_packet);
            }
        }
        
        #endregion
    }
}