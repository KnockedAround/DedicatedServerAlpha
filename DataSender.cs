namespace DedicatedServer.Alpha
{
    public enum ServerPackets
    {
        SWelcomeMessage = 1,
        SPlayerData,
    }
    static class DataSender
    {
        public static void SendWelcomeMessage(int connectionId)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SWelcomeMessage);
            buffer.WriteString("Client connection to server established...");
            ClientManager.SendDataTo(connectionId, buffer.ToByteArray());
            buffer.Dispose();
        }

        public static void SendPlayerInstantiated(int index, int connectionId)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerData);
            buffer.WriteInteger(index);
            ClientManager.SendDataTo(connectionId, buffer.ToByteArray());
            buffer.Dispose();
        }
    }
}
