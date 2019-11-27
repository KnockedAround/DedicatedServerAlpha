using System;

namespace DedicatedServer.Alpha
{
    static class General
    {
        public static void InitializeServer()
        {
            ServerTCP.InitializeNetwork();
            Console.WriteLine("Server has started...");
        }
    }
}
