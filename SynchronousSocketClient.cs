
using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;
namespace Cliente_ServidorSoquet
{
    public class SynchronousSocketClient
    {
        private Socket sender;
        TcpClient client;
        NetworkStream stream;

        public string StartClient(string _Host, int _Porta)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(_Host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, _Porta);

                // Create a TCP/IP  socket.  
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    return $"Socket connected to {sender.RemoteEndPoint.ToString()}";                       
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    return null;
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    return null;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public void StopClient()
        {
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                if (sender != null ? sender.Connected : false)
                {
                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                else
                    sender = null;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }

        public void StopClient_v2()
        {
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                // Close everything.
                stream.Close();
                client.Close();
                stream = null;
                client = null;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }

       

        public string SendMesssage(string _Messsage)
        {
            try
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];
                if (!sender.Connected)
                    throw new Exception("Socket não conectado!");

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.UTF8.GetBytes(_Messsage);

                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine("Echoed test = {0}", Encoding.UTF8.GetString(bytes, 0, bytesRec));

                return Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return null;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return null;
            }
        }

        public string SendMesssage_v2(string _Messsage)
        {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Protocolo.Encoding.GetBytes(_Messsage);
                               

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", _Messsage);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Protocolo.Encoding.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                return responseData;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return null;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return null;
            }
        }

        public void StartClient_v2(string _Host, int _Porta)
        {
            try
            {
                String message = "teste";
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.                
                client = new TcpClient(_Host, _Porta);



                stream = client.GetStream();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        //public static int Main(String[] args)
        //{
        //    StartClient();
        //    return 0;
        //}
    }
}