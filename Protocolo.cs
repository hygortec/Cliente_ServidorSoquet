using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_ServidorSoquet
{
    public class Protocolo
    {
   

        public List<string> ComandosPermitidos = new List<string>();

        public List<string> Restornos = new List<string>();

        public static Encoding Encoding = Encoding.UTF8;
        public Protocolo()
        {
            ComandosPermitidos.Add(" CADASTRARCONTA");
            ComandosPermitidos.Add("          SACAR");
            ComandosPermitidos.Add("      DEPOSITAR");
            ComandosPermitidos.Add("     TRANSFERIR");
            ComandosPermitidos.Add("     EMPRESTIMO");

            Restornos.Add("ERRO400");//Solicitação incorreta
            Restornos.Add("ERRO404");//Não encontrado
            Restornos.Add("ERRO411");//Comprimento Obrigatório
            Restornos.Add("ERRO500");//Erro interno do servidor
            Restornos.Add("ERRO501");//Não Implementado
            Restornos.Add("ERRO503");//Serviço indisponível
            Restornos.Add("RESP200");//Ok

        }

        public string ValidaMensagem(string _Mensagem)
        {
            if (_Mensagem.Length != 87)
                return "ERRO411";

            Mensagem mgs = new Mensagem();

            mgs.Endereco = _Mensagem.Substring(0, 20);
            mgs.Comando = _Mensagem.Substring(20, 15);
            mgs.MensagemTexto = _Mensagem.Substring(35, 50);
            mgs.Checksum = _Mensagem.Substring(85, 2);

            try
            {
                if (Convert.ToInt32(mgs.Checksum) != GetCheckSum(_Mensagem.Substring(0, 85)))
                    return "ERRO400";
            }
            catch
            {
                return "ERRO400";
            }

            if (!ComandosPermitidos.Contains(mgs.Comando.ToUpper()))
                return "ERRO404";


            return "RESP200";
        }


        public int GetCheckSum(string str)
        {
            try
            {
                int xor = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    xor ^= Protocolo.Encoding.GetBytes(str)[i];
                }

                return xor;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int GetCheckSum_2(string str)
        {
            try
            {               
                int xor = 0;

                for (int i = 0; i < str.Length; i++)
                    xor ^= Convert.ToInt32((int)Char.GetNumericValue(str[i]));

                return xor;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetKey(string _Mensagem)
        {
            return _Mensagem.Substring(20, 15);
        }
        
    }
}
