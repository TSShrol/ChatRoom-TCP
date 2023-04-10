using System;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

namespace ChatRoom_ConsoleClient
{
    class Program
    {
        static string host = "10.7.150.203";
        static int port = 8008;
        static TcpClient client=new TcpClient();
        static StreamReader reader = null;
        static StreamWriter writer = null;

        static void Main(string[] args)
        {
            Start();

        }

        static void Start() {
            try
            {
                client.Connect(host, port);
                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                Task.Run(() => ReceiveMessageAsync(reader));
                SendMessageAsync(writer);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally {
                reader?.Close();
                writer?.Close();
                client?.Close();
            }
            
            
        }

        static async Task ReceiveMessageAsync(StreamReader reader) {
            while (true) {
                try
                {
                    //reading response by line
                    string message = await reader.ReadLineAsync();
                    
                    if (!string.IsNullOrEmpty(message)) {
                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }

        }

        static async Task SendMessageAsync(StreamWriter writer) {
            Console.WriteLine("Input login:");
            string loginUser = Console.ReadLine();
            await writer.WriteLineAsync(loginUser);
            await writer.FlushAsync(); //push data and clear buffer
            while (true) {
                Console.WriteLine($"{loginUser}: ");
                string message = Console.ReadLine();
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        }
    }
}
