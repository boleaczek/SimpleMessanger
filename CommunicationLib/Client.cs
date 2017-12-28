using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib
{
    public class Client : Messanger
    {
        public delegate void ConnectionEndedByRemoteEndPointHandler(object sender, ConnectionEndedByRemoteEndPointEventArgs e);
        public event ConnectionEndedByRemoteEndPointHandler OnConnectionEndedByRemoteEndPoint;

        public Client(IPEndPoint remoteEndPoint) : base(remoteEndPoint)
        {

        }

        public override void Start()
        {
            _socket.BeginConnect(endPoint, Connected, null);
        }

        void Connected(IAsyncResult result)
        {
            try
            {
                _socket.EndConnect(result);
                WaitForMessages();
            }
            catch(SocketException e)
            {
                Stop();
            }
        }

        protected override void ConnectionEndedByRemoteEndPoint()
        {
            Stop();
            ConnectionEndedByRemoteEndPointEventArgs eventArgs = new ConnectionEndedByRemoteEndPointEventArgs();
            OnConnectionEndedByRemoteEndPoint?.Invoke(this, eventArgs);
        }
    }
}
