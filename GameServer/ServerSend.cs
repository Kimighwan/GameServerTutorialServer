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


        public static void Welcome(int toClient, string m) // 매개변수 => 어떤 클라이언트, 메세지 
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome)) 
            {
                packet.Write(m);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }
    }
}
