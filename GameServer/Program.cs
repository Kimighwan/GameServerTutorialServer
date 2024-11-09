using System;

namespace GameServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Gmae Server";

            Server.Start(20, 26950); // 플레이어 최대 수, 포트 번호
            Console.ReadKey();
        }
    }
}
