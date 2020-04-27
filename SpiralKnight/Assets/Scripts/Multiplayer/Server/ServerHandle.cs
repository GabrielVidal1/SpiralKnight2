using UnityEngine;

namespace Multiplayer.Server
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Debug.Log(
                $"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Debug.Log(
                    $"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }

            Vector3 _direction = _packet.ReadVector3();

            Server.clients[_fromClient].player.SetInput(_inputs, _direction);
        }

        public static void PlayerAttack(int _fromClient, Packet _packet)
        {
            Vector3 _direction = _packet.ReadVector3();

            Vector3 playerPosition = Server.clients[_fromClient].player.transform.position;
            
            NetworkManager.instance.SpawnProjectile(playerPosition  + _direction, _direction, 10, 1, true);
            
            //Server.clients[_fromClient].player.Attack(_direction);
        }
    }
}