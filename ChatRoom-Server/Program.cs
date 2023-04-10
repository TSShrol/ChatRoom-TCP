using System;

namespace ChatRoom_Server
{
    class Program
    {
        static ServerObj server = new ServerObj();
        static void Main(string[] args)
        {
            server.ListenAsync();
            Console.ReadLine();

        }
    }
}
