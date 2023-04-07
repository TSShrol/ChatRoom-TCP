using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ChatRoom_Server
{
    public class ServerObj
    {
        static TcpListener tcpListener=new TcpListener(IPAddress.Any,8008); //server for listener
        List<ClientObj> clients = new List<ClientObj>(); // all connections

        //listening entered connection
        protected internal async Task ListenAsync() {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Server is running. Waiting for connections...");
                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally {

                Disconnect();
            }

        }
        //diconnection all clients
        private void Disconnect()
        {
           
        }
    }
}
