using UnityEngine;

namespace Multiplayer.Client
{
    public class ClientSend : MonoBehaviour
    {
        
        #region Send
        /// <summary>Sends a packet to the server via TCP.</summary>
        /// <param name="_packet">The packet to send to the sever.</param>
        private static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        /// <summary>Sends a packet to the server via UDP.</summary>
        /// <param name="_packet">The packet to send to the sever.</param>
        private static void SendUDPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #endregion
        
        #region Packets

        /// <summary>Lets the server know that the welcome message was received.</summary>
        public static void WelcomeReceived()
        {
            using (Packet _packet = new Packet((int) ClientPackets.welcomeReceived))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(UIManager.instance.usernameField.text);

                SendTCPData(_packet);
            }
        }

        /// <summary>Sends player input to the server.</summary>
        /// <param name="_inputs"></param>
        public static void PlayerMovement(bool[] _inputs, Vector3 direction)
        {
            using (Packet _packet = new Packet((int) ClientPackets.playerTransform))
            {
                _packet.Write(_inputs.Length);
                foreach (bool _input in _inputs)
                {
                    _packet.Write(_input);
                }

                _packet.Write(direction);

                SendUDPData(_packet);
            }
        }

        public static void PlayerAttack(Vector3 _direction)
        {
            using (Packet _packet = new Packet((int) ClientPackets.playerAttack))
            {
                _packet.Write(_direction);
                SendTCPData(_packet);
            }
        }

        #endregion
    }
}