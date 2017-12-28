using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib
{
    public class Server : Messanger
    {
        Socket serverSocket;

        public Server(IPEndPoint localEndPoint)
        {
            serverSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);
        }

        public override void Start()
        {
            serverSocket.Listen(1);
            serverSocket.BeginAccept(ConnectionAccepted, null);
        }

        protected override void CloseSockets()
        {
            if(_socket != null) _socket.Close();
            serverSocket.Close();
        }

        void ConnectionAccepted(IAsyncResult result)
        {
            //try
            //{
                _socket = serverSocket.EndAccept(result);
                WaitForMessages();
            //}
            /*catch(ObjectDisposedException e)
            {

            }*/
        }

        protected override void ConnectionEndedByRemoteEndPoint()
        {
            Start();
        }
    }
}
