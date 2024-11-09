using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Client
    {
        public int clientId;
        public TCP tcp;
        public static int bufferSize = 4096;

        public Client(int _clientId)
        {
            clientId = _clientId;
            tcp = new TCP(clientId);
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = bufferSize;
                socket.SendBufferSize = bufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[bufferSize];

                stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallBack, null);
            }

            private void ReceiveCallBack(IAsyncResult _result)
            {
                try
                {
                    int byteLength = stream.EndRead(_result);
                    if (byteLength <= 0)
                    {
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);

                    stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallBack, null);
                }
                catch (Exception m)
                {
                        Console.WriteLine($"Error : {m}");
                }
            }

        }
    }
}
