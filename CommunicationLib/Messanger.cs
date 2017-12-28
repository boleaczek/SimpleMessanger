using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib
{
    public abstract class Messanger
    {
        protected Socket _socket;
        protected IPEndPoint endPoint;
        byte[] buffer = new byte[1024];
        public delegate void MessageRecievedHanlder(object sender, MessageRecievedEventArgs e);
        public event MessageRecievedHanlder OnMesageRecieved;
        

        protected abstract void ConnectionEndedByRemoteEndPoint();
        public abstract void Start();

        public Messanger(IPEndPoint localEndPoint)
        {
            _socket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            endPoint = localEndPoint;
        }

        public Messanger()
        {
            
        }

        protected void WaitForMessages()
        {
            _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, MessageRecieved, _socket);
        }

        protected void MessageRecieved(IAsyncResult result)
        {
            try
            {
                if (_socket.EndReceive(result) > 0)
                {
                    MessageRecievedEventArgs eventArgs = new MessageRecievedEventArgs() { Text = Encoding.UTF8.GetString(buffer).Trim() };
                    OnMesageRecieved?.Invoke(this, eventArgs);
                    WaitForMessages();
                }
                else
                {
                    ConnectionEndedByRemoteEndPoint();
                }
            }
            catch(ObjectDisposedException e)
            {
                CloseSockets();
            }
        }

        public void Send(string Text)
        {
            byte[] message = new byte[100];
            message = Encoding.UTF8.GetBytes(Text);
            _socket.Send(message);
        }

        protected virtual void CloseSockets()
        {
            _socket.Close();
        }

        public void Stop()
        {
            if (_socket != null  && _socket.Connected == true)
            {
                _socket.Shutdown(SocketShutdown.Both);
                CloseSockets();
            }
            else
            {
                CloseSockets();
            }
        }

        public class MessageRecievedEventArgs
        {
            public string Text { get; set; }
        }

        public class ConnectionEndedByRemoteEndPointEventArgs
        {

        }
    }
}
