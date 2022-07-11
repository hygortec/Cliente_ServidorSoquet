using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_ServidorSoquet
{
    public class Node
    {
        public string Key { get; set; }
        public string HostName { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public SynchronousSocketClient SocketClient { get; set; }

        public bool IsConnected { get; set; }
    }
}
