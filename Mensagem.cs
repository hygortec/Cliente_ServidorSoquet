using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_ServidorSoquet
{
    public class Mensagem
    {
        public List<string> ComandosPermitidos = new List<string>(); 

        public string Endereco { get; set; }
        public string Comando { get; set; }
        public string MensagemTexto { get; set; }
        public string Checksum { get; set; }        

    }
}
