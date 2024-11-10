using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameServer
{
    internal class ServerHandler
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientChk = packet.ReadInt();
            string userName = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} Connected Successfully {fromClient}");
            if(fromClient != clientChk)
            {
                Console.WriteLine($"Player \"{userName}\" (ID :{fromClient}) wrong client ID ({clientChk})");
            }
        }
    }
}
