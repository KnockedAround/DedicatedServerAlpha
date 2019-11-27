using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DedicatedServer.Alpha
{
    static class ClientManager
    {
        public static Dictionary<int, Client> Clients = new Dictionary<int, Client>();
        public static void CreateNewConnection(TcpClient tempClient)
        {
            Client newClient = new Client();
            newClient.socket = tempClient;
            newClient.connectionId = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
            newClient.StartConnection();
            Clients.Add(newClient.connectionId, newClient);

            DataSender.SendWelcomeMessage(newClient.connectionId);
            InstantiatePlayer(newClient.connectionId);
        }

        public static void SendDataTo(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            Clients[connectionId].stream.BeginWrite(buffer.ToByteArray(), 0, buffer.ToByteArray().Length, null, null);
            buffer.Dispose();
        }

        public static void InstantiatePlayer(int connectionId)
        {

            // Send everyone who is already online to the new connection
            foreach(var client in Clients)
            {
                if(client.Key != connectionId)
                {
                    DataSender.SendPlayerInstantiated(client.Key, connectionId);
                }
            }

            // Send new connection to all others, including new player
            foreach(var client in Clients)
            {
                DataSender.SendPlayerInstantiated(connectionId, client.Key);
            }
        }
    }
}
