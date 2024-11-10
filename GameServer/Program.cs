using System;
using System.Threading;

namespace GameServer
{
    internal class Program
    {
        private static bool isRunnig = false;
        static void Main(string[] args)
        {
            Console.Title = "Gmae Server";

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(20, 26950); // 플레이어 최대 수, 포트 번호
            Console.ReadKey();
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main Thread Start. Running {Constarts.TICKS_PER_SEC} ticks Per Second");
            DateTime  nextLoop = DateTime.Now;

            while (isRunnig)
            {
                while(nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    nextLoop = nextLoop.AddMilliseconds(Constarts.MS_PER_TICK);

                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }

                }
            }
        }
    }
}
