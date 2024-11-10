using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    internal class GameLogic
    {
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
