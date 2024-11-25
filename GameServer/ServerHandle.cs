using System;
using System.Collections.Generic;
using System.Numerics;
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
            Server.clients[fromClient].SendIntToGame(userName);
        }

        public static void PlayerMovement(int fromClient, Packet packet)
        {
            bool[] inputs = new bool[packet.ReadInt()];
            for(int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = packet.ReadBool();
            }

            Quaternion rotation = packet.ReadQuaternion();

            Server.clients[fromClient].player.SetInput(inputs, rotation);
        }
    }
}
