using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatRoom_Server
{
    public class ClientObj
    {
        protected internal string Id { get; set; }
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }
        TcpClient client;
        ServerObj server;
        DateTime enterTime;
        DateTime exitTime;

        public ClientObj(TcpClient tcpClient, ServerObj serverObj) {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObj;
            var stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
            enterTime = DateTime.Now;

            //server.AddConnection(this); ;
        }

        public async Task ProcessAsync() {
            try
            {
                //get username
                string userName = await Reader.ReadLineAsync();
                string message = $"{userName} entered in chat";
                await server.BroadCastMessageAsync(message, Id);
                Console.WriteLine(message);
                //reading in cicle infinity message from client
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        //TimeSpan interval=DateTime.Now- enterTime;
                        TimeSpan interval = DateTime.Now.Subtract(enterTime);
                        message = $"{userName} ({interval.Minutes} min {interval.Seconds} in chat) : {message}";
                        Console.WriteLine(message);
                        await server.BroadCastMessageAsync(message, Id);

                    }
                    catch (Exception ex)
                    {
                        exitTime = DateTime.Now;
                        message = $"{userName} exit in chat at {exitTime.Hour}:{exitTime.Minute}:{exitTime.Second}";
                        Console.WriteLine(message);
                        await server.BroadCastMessageAsync(message, Id);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                server.RemoveConnection(Id);
            }


           
        }

        protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}