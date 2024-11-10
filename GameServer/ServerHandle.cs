using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    internal class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientChk = packet.ReadInt();
            string userName = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected sucessfully {fromClient}");
            if (fromClient != clientChk)
            {
                Console.WriteLine($"Player \"{userName}\" (ID: {fromClient}) has wrong client ID ({clientChk})");
            }
        }
    }
}
