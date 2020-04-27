using System.Net;
using UnityEngine;

namespace Multiplayer.Client
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Debug.Log($"Message from server: {_msg} + {_myId}");
            Client.instance.myId = _myId;
            ClientSend.WelcomeReceived();

            // Now that we have the client's id, connect UDP
            Client.instance.udp.Connect(((IPEndPoint) Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        #region Spawning

        public static void SpawnPlayer(Packet _packet)
        {
            int _id = _packet.ReadInt();
            string _username = _packet.ReadString();
            Vector3 _position = _packet.ReadVector3();
            Quaternion _rotation = _packet.ReadQuaternion();

            GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        }

        public static void SpawnEnemy(Packet _packet)
        {
            int _id = _packet.ReadInt();
            Vector3 _position = _packet.ReadVector3();
            Quaternion _rotation = _packet.ReadQuaternion();

            GameManager.instance.SpawnEnemy(_id, _position, _rotation);
        }

        public static void ProjectileSpawn(Packet _packet)
        {
            int _id = _packet.ReadInt();

            Vector3 _position = _packet.ReadVector3();
            Vector3 _direction = _packet.ReadVector3();

            float _speed = _packet.ReadFloat();
            
            GameManager.instance.SpawnProjectile(_id, _position, _direction, _speed);
        }

        public static void SpawnBlock(Packet _packet)
        {
            int _id = _packet.ReadInt();
            Vector3 _position = _packet.ReadVector3();


            GameManager.instance.SpawnDestructibleBlock(_id, _position);
        }

        #endregion


        public static void EntityTransform(Packet _packet)
        {
            int _id = _packet.ReadInt();

            Vector3 _position = _packet.ReadVector3();
            Vector3 _direction = _packet.ReadVector3();

            GameManager.entities[_id].SetPosition(_position);
            GameManager.entities[_id].SetDirection(_direction);
        }

        public static void EntityHealth(Packet _packet)
        {
            int _id = _packet.ReadInt();
            float _health = _packet.ReadFloat();

            GameManager.entities[_id].SetHealth(_health);
        }

        public static void EntityAttack(Packet _packet)
        {
            int _id = _packet.ReadInt();
            Vector3 _direction = _packet.ReadVector3();

            GameManager.entities[_id].SetDirection(_direction);
            GameManager.entities[_id].Attack();
        }


        public static void PlayerDisconnected(Packet _packet)
        {
            int _id = _packet.ReadInt();

            Destroy(GameManager.players[_id].gameObject);
            GameManager.players.Remove(_id);
        }


        public static void PlayerRespawned(Packet _packet)
        {
            int _id = _packet.ReadInt();

            GameManager.players[_id].Respawn();
        }


        public static void Destroy(Packet _packet)
        {
            int _id = _packet.ReadInt();

            GameManager.destroyables[_id].Destroy();

            GameManager.destroyables.Remove(_id);
        }
    }
}