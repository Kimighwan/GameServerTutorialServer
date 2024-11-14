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

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected sucessfully and is now Player {fromClient}.");
            if (fromClient != clientChk)
            {
                Console.WriteLine($"Player \"{userName}\" (ID: {fromClient}) has wrong client ID ({clientChk})");
            }
        }

        public static void UDPTestReceived(int fromClient, Packet packet)
        {
            string m = packet.ReadString();

            Console.WriteLine($"Received packet using UDP. Contains message : {m}");
        }
    }
}
