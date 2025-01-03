﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using static GameServer.Server;

namespace GameServer
{
    internal class Server
    {
        public static int maxPlayer {  get; private set; }
        public static int port {  get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient tudpListener;

        public static void Start(int _maxPlayer, int _port) // Server Start // 매개변수로 최대 플레이어와 내부 포트 번호 넣는다
        {
            maxPlayer = _maxPlayer;
            port = _port;

            Console.WriteLine("Start Server...");
            ServerInit();

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TcpConnectCallBack, null);

            tudpListener = new UdpClient(port);
            tudpListener.BeginReceive(UDPReceiveCallback, null);


            Console.WriteLine($"Server Start / Port: {port}");
        }

        private static void TcpConnectCallBack(IAsyncResult _result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TcpConnectCallBack, null);
            Console.WriteLine($"Incoming Connetion from {client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= maxPlayer; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);
                    return;
                }

                Console.WriteLine($"{client.Client.RemoteEndPoint} Connect Failed(Server Full :("); 
            }
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = tudpListener.EndReceive(_result, ref clientEndPoint);
                tudpListener.BeginReceive(UDPReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                using (Packet packet = new Packet(data))
                {
                    int clientId = packet.ReadInt();

                    if (clientId == 0)
                    {
                        return;
                    }

                    if (clients[clientId].udp.endPoint == null)
                    {
                        clients[clientId].udp.Connect(clientEndPoint);
                        return;
                    }

                    if (clients[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
                    {
                        clients[clientId].udp.HandleData(packet);
                    }
                }
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Error receiving UDP Data : {e}");
            }
        }

        public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if(clientEndPoint != null)
                {
                    tudpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }    
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error sending Data to {clientEndPoint} using UDP : {e}");
            }
        }

        private static void ServerInit()
        {
            for (int i = 1; i <= maxPlayer; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
            };
            Console.WriteLine("Init Packet");
        }
    }
}
