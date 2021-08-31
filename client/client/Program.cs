using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
            again:
                int port = 11000;
                // Буфер для входящих данных
                byte[] bytes = new byte[1024];

                //соединяемся с удаленным устройством
                //устанавливаем удаленную точку для сокета

                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                //соединяем сокет с удаленной точкой
                sender.Connect(ipEndPoint);

                Console.WriteLine("Enter message (byebye for the loss connection)");
                string message = Console.ReadLine();

                Console.WriteLine("Socket conecting... ", sender.RemoteEndPoint.ToString());
                byte[] msg = Encoding.UTF8.GetBytes(message);

                //отправляем данные через сокет
                int bytesSent = sender.Send(msg);

                //получаем ответ от сервера
                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("\nServer response : {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

                //отправляемся на метку для неоднократного прогона участка программы
                if (message.IndexOf("byebye") == -1)
                    goto again;

                //oсвобождаем сокет
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
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