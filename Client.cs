using System;
using System.Net.Sockets;

namespace DedicatedServer.Alpha
{
    public class Client
    {
        public int connectionId;
        public TcpClient socket;
        public NetworkStream stream;
        public ByteBuffer buffer;

        private byte[] _receiveBuffer;

        public void StartConnection()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            _receiveBuffer = new byte[4096];
            stream.BeginRead(_receiveBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            Console.WriteLine("Incoming Connection from '{0}'...", socket.Client.RemoteEndPoint.ToString());
        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int length = stream.EndRead(result);
                if(length <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(_receiveBuffer, newBytes, length);
                ServerHandleData.HandleData(connectionId, newBytes);
                stream.BeginRead(_receiveBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch (Exception)
            {
                CloseConnection();
                return;
            }
        }

        private void CloseConnection()
        {
            Console.WriteLine("Connection from '{0}' has been terminated...", socket.Client.RemoteEndPoint.ToString());
            socket.Close();
        }
    }
}
