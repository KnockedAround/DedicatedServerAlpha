using System.Collections.Generic;

namespace DedicatedServer.Alpha
{
    static class ServerHandleData
    {
        public delegate void Packet(int connectionId, byte[] data);
        public static Dictionary<int, Packet> Packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            Packets.Add((int)ClientPackets.CHelloServer, DataReceiver.HandleHelloServer);
        }

        public static void HandleData(int connectionId, byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int packetLength = 0;

            if (ClientManager.Clients[connectionId].buffer == null)
                ClientManager.Clients[connectionId].buffer = new ByteBuffer();

            ClientManager.Clients[connectionId].buffer.WriteBytes(buffer);

            if(ClientManager.Clients[connectionId].buffer.Count() == 0)
            {
                ClientManager.Clients[connectionId].buffer.Clear();
                return;
            }

            if(ClientManager.Clients[connectionId].buffer.GetRemainingLength() >= 4)
            {
                packetLength = ClientManager.Clients[connectionId].buffer.ReadInteger(false);
                if(packetLength <= 0)
                {
                    ClientManager.Clients[connectionId].buffer.Clear();
                    return;
                }
            }

            while(packetLength > 0 & packetLength <= ClientManager.Clients[connectionId].buffer.GetRemainingLength() - 4)
            {
                if(packetLength <= ClientManager.Clients[connectionId].buffer.GetRemainingLength() - 4)
                {
                    ClientManager.Clients[connectionId].buffer.ReadInteger();
                    data = ClientManager.Clients[connectionId].buffer.ReadBytes(packetLength);
                    HandleDataPackets(connectionId, data);
                }

                packetLength = 0;
                if(ClientManager.Clients[connectionId].buffer.GetRemainingLength() >= 4)
                {
                    packetLength = ClientManager.Clients[connectionId].buffer.ReadInteger(false);
                    if (packetLength <= 0)
                    {
                        ClientManager.Clients[connectionId].buffer.Clear();
                        return;
                    }
                }
            }

            if(packetLength <= 1)
            {
                ClientManager.Clients[connectionId].buffer.Clear();
            }
        }

        private static void HandleDataPackets(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetId = buffer.ReadInteger();
            buffer.Dispose();
            if(Packets.TryGetValue(packetId, out Packet packet))
            {
                packet.Invoke(connectionId, data);
            }
        }
    }
}
