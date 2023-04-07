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

        protected internal void AddConnection(ClientObj clientObj) {
            clients.Add(clientObj);
        }

        protected internal void RemoveConnection(string id)
        {
            //find by id connection for closing

            ClientObj client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null) {
                clients.Remove(client);
            }
        }

            //listening entered connection
            protected internal async Task ListenAsync() {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Server is running. Waiting for connections...");
                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    ClientObj clientObj = new ClientObj(tcpClient, this);
                    AddConnection(clientObj);
                    Task.Run(clientObj.ProcessAsync);

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
            foreach (var client in clients)
            {
                client.Close();
            }
        }

        //broadcast messages all cleint
        protected internal async Task BroadCastMessageAsync(string message, string id) {
            foreach (var client in clients)
            {
                if (client.Id != id) {

                    await client.Writer.WriteAsync(message);
                    await client.Writer.FlushAsync();
                }
            }
        }

    }
}
