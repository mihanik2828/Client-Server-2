using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Слушаем соединение
                while (true)
                {
                    Console.WriteLine("Waiting for connect", ipEndPoint);


                    Socket handler = sListener.Accept();
                    string data = null;

                    // Увидели клиента, который к нам подключился

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Вывод данных в консоль
                    Console.Write("Getted message: " + data + "\n\n");

                    // Отправка ответа клиенту
                    string reply = "Get contact";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("byebye") > -1)
                    {
                        Console.WriteLine("Server has copmleted connetion with client");
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}