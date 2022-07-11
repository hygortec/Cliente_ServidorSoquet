using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Cliente_ServidorSoquet
{

    public class SynchronousSocketListener
    {
        private List<Node> ListaServidores = new List<Node>();

        private Socket listener;

        private TcpListener listener_v2;

        public int Porta = 8080;

        public delegate void MensagemRecebida(string _Mensagem);
        public event MensagemRecebida _MensagemRecebida;

        public Protocolo Protocolo = new Protocolo();

        // Incoming data from the client.  
        public static string data = null;

        public SynchronousSocketListener()
        {
            this.CriaListaServidores();
        }

        public void StartListening()
        {   
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Porta);

            // Create a TCP/IP socket.  
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    if (_MensagemRecebida != null)
                        _MensagemRecebida("Aguardando conexão...");

                    Console.WriteLine("Aguardando conexão...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;
                    
                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        bytes = new Byte[1024];

                        int bytesRec = handler.Receive(bytes);

                        data = Protocolo.Encoding.GetString(bytes, 0, bytesRec);

                        if (_MensagemRecebida != null)
                            _MensagemRecebida(data);

                        string retorno = Protocolo.ValidaMensagem(data);

                        // Echo the data back to the client.  
                        byte[] msg = Protocolo.Encoding.GetBytes(retorno);

                        handler.Send(msg);
                       
                        //if (data.IndexOf("<EOF>") > -1)
                        //{
                        //    break;
                        //}
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", data);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void StartListening_v2()
        {          
            TcpListener listener_v2 = new TcpListener(IPAddress.Any, Porta);
            listener_v2.Start();

            while (true)
            {
               
                TcpClient clientSocket = listener_v2.AcceptTcpClient();
               
                string IPCliente = ((System.Net.IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString();
                IPHostEntry ipHostInfoClient = Dns.GetHostEntry(IPCliente);
                string  ConexaoCliente = $"| Hotsname: {ipHostInfoClient.HostName} ";
                ConexaoCliente += $"| IP: {IPCliente} ";                                
               
               RespMessage("Cliente conectado: "+ ConexaoCliente);
               
                int requestCount = 0;
                byte[] bytesFrom = new byte[10000];
                string dataFromClient = null;
                Byte[] sendBytes = null;
                string serverResponse = null;
                string rCount = null;
                requestCount = 0;
                while ((true))
                {
                    try
                    {
                        data = null;
                        requestCount = requestCount + 1;
                        NetworkStream networkStream = clientSocket.GetStream();
                        Console.WriteLine(" >> " + (int)clientSocket.ReceiveBufferSize);

                        int read = networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                        data = Protocolo.Encoding.GetString(bytesFrom, 0, read);

                        Console.WriteLine(" >> " + "From client - " + data);

                        rCount = Convert.ToString(requestCount);
                        serverResponse = "Server to clinet() " + rCount;

                        RespMessage(data);

                        string retorno = Protocolo.ValidaMensagem(data);
                       
                        byte[] msgRetorno = Protocolo.Encoding.GetBytes(retorno);
                        Console.WriteLine(" >> " + serverResponse);

                        networkStream.Write(msgRetorno, 0, msgRetorno.Length);
                        networkStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146232800)
                        {
                            RespMessage("Cliente desconectado:" + ConexaoCliente);
                        }
                        Console.WriteLine(" >> " + ex.ToString());
                        break;
                    }
                }
            }            
        }
        public void StartListening_v3()
        {
            TcpListener listener_v2 = new TcpListener(IPAddress.Any, Porta);
            listener_v2.Start();

            RespMessage("Aguardando conexão...");
            while (true)
            {   
                TcpClient clientSocket = listener_v2.AcceptTcpClient();

                Thread thread = new Thread(() => TratarConexao(clientSocket));
                thread.Start();
            }
        }
        
        public void TratarConexao(TcpClient clientSocket)
        {
            try
            {
                string IPCliente = ((System.Net.IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString();
                IPHostEntry ipHostInfoClient = null;
                try {
                    ipHostInfoClient = Dns.GetHostEntry(IPCliente);
                }
                catch
                {

                }
                string ConexaoCliente = "";
               if (ipHostInfoClient != null)
                    ConexaoCliente = $"| Hotsname: {ipHostInfoClient.HostName} ";

                ConexaoCliente += $"| IP: {IPCliente} ";

                if(IPCliente == "")
                {

                }

                RespMessage("Cliente conectado: " + ConexaoCliente);

                int requestCount = 0;
                byte[] bytesFrom = new byte[10000];
                string dataFromClient = null;
                Byte[] sendBytes = null;
                string serverResponse = null;
                string rCount = null;
                requestCount = 0;
                while ((true))
                {
                    try
                    {
                        data = null;
                        requestCount = requestCount + 1;
                        NetworkStream networkStream = clientSocket.GetStream();
                        Console.WriteLine(" >> " + (int)clientSocket.ReceiveBufferSize);

                        int read = networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                        data = Protocolo.Encoding.GetString(bytesFrom, 0, read);

                        Console.WriteLine(" >> " + "From client - " + data);

                        rCount = Convert.ToString(requestCount);
                        serverResponse = "Server to clinet() " + rCount;

                        RespMessage(data);


                        var key = Protocolo.GetKey(data).Replace(" ", "");

                        var retornoServior = this.EnviaMensagemServidor(key, data);
                        byte[] msgRetorno = Protocolo.Encoding.GetBytes(retornoServior);
                        Console.WriteLine(" >> " + serverResponse);

                        networkStream.Write(msgRetorno, 0, msgRetorno.Length);
                        networkStream.Flush();

                        //string retorno = Protocolo.ValidaMensagem(data);

                        //byte[] msgRetorno = Protocolo.Encoding.GetBytes(retorno);
                        //Console.WriteLine(" >> " + serverResponse);

                        //networkStream.Write(msgRetorno, 0, msgRetorno.Length);
                        //networkStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146232800)
                        {
                            RespMessage("Cliente desconectado:" + ConexaoCliente);
                        }
                        Console.WriteLine(" >> " + ex.ToString());
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public void StopListening(int _Porta)
        {
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                if (listener != null ? listener.Connected : false)
                {
                    // Release the socket.  
                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }
                else
                    listener = null;
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

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public void StopListening_v2(int _Porta)
        {
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                if (listener_v2 != null)
                {
                    // Release the socket.
                    listener_v2.Stop();
                }
                else
                    listener_v2 = null;
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

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public void RespMessage(string _MSG)
        {
            try
            {  
                Console.WriteLine(_MSG);
                if (_MensagemRecebida != null)
                    _MensagemRecebida(_MSG);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }  
        }

        //public static int Main(String[] args)
        //{
        //    StartListening();
        //    return 0;
        //}

        public string EnviaMensagemServidor(string Key, string _Mensagem)
        {
            string retorno = "";
            var tcpClient = ConectaServidorRemoto(Key);
            if (tcpClient != null)
            {
                retorno = tcpClient.SocketClient.SendMesssage_v2(_Mensagem);
            }
            return retorno;
        }

        public Node ConectaServidorRemoto(string Key)
        {
            try
            {
                var Node = ListaServidores.FirstOrDefault(p => p.Key == Key);
                if (Node != null)
                {
                    var SocketClient = new SynchronousSocketClient();
                    Node.SocketClient = SocketClient;
                    Node.SocketClient.StartClient_v2(Node.IP, Node.Port);
                    Node.IsConnected = true;

                    return Node;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return null;
        }

        public void CriaListaServidores()
        {
            #region + DEPOSITAR
            var newNode = new Node();
            newNode.Key = "DEPOSITAR";
            newNode.IP = "192.168.137.18";
            newNode.HostName = "";
            newNode.Port = 8888;            

            ListaServidores.Add(newNode);

            newNode = new Node();
            newNode.Key = "EXTRATO";
            newNode.IP = "192.168.137.18";
            newNode.HostName = "";
            newNode.Port = 8888;

            ListaServidores.Add(newNode);

            #endregion

            #region + TRANSFERIR
            newNode = new Node();
            newNode.Key = "TRANSFERIR";
            newNode.IP = "192.168.137.31";
            newNode.HostName = "";
            newNode.Port = 5555;

            ListaServidores.Add(newNode);

            newNode = new Node();
            newNode.Key = "TRANSFERIR";
            newNode.IP = "192.168.137.31";
            newNode.HostName = "";
            newNode.Port = 5555;

            ListaServidores.Add(newNode);

            #endregion

            #region + SACAR
            newNode = new Node();
            newNode.Key = "SACAR";
            newNode.IP = "192.168.137.18";
            newNode.HostName = "";
            newNode.Port = 8888;

            ListaServidores.Add(newNode);

            newNode = new Node();
            newNode.Key = "SACAR";
            newNode.IP = "192.168.137.18";
            newNode.HostName = "";
            newNode.Port = 8888;

            ListaServidores.Add(newNode);

            #endregion
        }
    }

   

}