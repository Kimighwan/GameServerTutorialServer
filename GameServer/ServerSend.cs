using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    internal class ServerSend // 네트워크에 보낼 패킷을 생성
    {
        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        private static void SendUDPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].udp.SendData(packet);
        }

        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for(int i = 1; i <= Server.maxPlayer; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        private static void SendTCPDataToAll(int exceptClinet, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayer; i++)
            {
                if(i != exceptClinet)
                {
                    Server.clients[i].tcp.SendData(packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayer; i++)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }

        private static void SendUDPDataToAll(int exceptClinet, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxPlayer; i++)
            {
                if (i != exceptClinet)
                {
                    Server.clients[i].udp.SendData(packet);
                }
            }
        }

        #region Packet

        public static void Welcome(int toClient, string m) // 매개변수 => 어떤 클라이언트, 메세지 
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome)) 
            {
                packet.Write(m);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }

        public static void SpawnPlayer(int toClient, Player player)
        {
            using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                packet.Write(player.id);
                packet.Write(player.userName);
                packet.Write(player.position);
                packet.Write(player.rotation);

                SendTCPData(toClient, packet);
            }
        }

        public static void PlayerPosition(Player player)
        {
            using (Packet packet = new Packet((int)ServerPackets.playerPosition))
            {
                packet.Write(player.id);
                packet.Write(player.position);

                SendUDPDataToAll(packet);
            }
        }

        public static void PlayerRotation(Player player)
        {
            using (Packet packet = new Packet((int)ServerPackets.playerRotation))
            {
                packet.Write(player.id);
                packet.Write(player.rotation);

                SendUDPDataToAll(player.id, packet);
            }
        }

        #endregion
    }
}
