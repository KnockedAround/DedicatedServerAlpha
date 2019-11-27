using System;

namespace DedicatedServer.Alpha
{
    public enum ClientPackets
    {
        CHelloServer = 1,
    }

    static class DataReceiver
    {
        public static void HandleHelloServer(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetId = buffer.ReadInteger();

            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
        }
    }
}
