using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente_ServidorSoquet
{
    public partial class frmPrincipal : Form
    {
        SynchronousSocketClient SocketClient = new SynchronousSocketClient();
        SynchronousSocketListener SocketServer = new SynchronousSocketListener();
        Protocolo Protocolo = new Protocolo();

        private string Mensagem = "";
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(!rbCliente.Checked && !rbServidor.Checked)
            {
                MessageBox.Show("Selecione uma opção");
                return;
            }

            groupBox1.Enabled = false;
            if (rbCliente.Checked)
            {            
                //SocketClient.StartClient(txtHost.Text, Convert.ToInt32(txtPorta.Text));

                SocketClient.StartClient_v2(txtHost.Text, Convert.ToInt32(txtPorta.Text));
            }                
            else
            {
                SocketServer.Porta = Convert.ToInt32(txtPorta.Text);
                SocketServer._MensagemRecebida += this.MensagemRecebida;
                Thread InstanceCaller = new Thread(new ThreadStart(SocketServer.StartListening_v3));
                InstanceCaller.Start();
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rbCliente_Click(object sender, EventArgs e)
        {
            this.lblHost.Enabled = true;
            this.txtHost.Enabled = true;
        }

        private void rbCliente_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCliente.Checked)
            {
                this.Text = "Cliente";
                this.lblHost.Enabled = true;
                this.txtHost.Enabled = true;               
            }
        }

        private void rbServidor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbServidor.Checked)
            {
                this.Text = "Servidor";
                this.txtHost.Text = "127.0.0.1";
                this.lblHost.Enabled = false;
                this.txtHost.Enabled = false;

                this.btnEnviar.Enabled = false;       
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string old = txtMsgResp.Text + Environment.NewLine;
            txtMsgResp.Text = old + SocketClient.SendMesssage_v2(txtMsgSend.Text);
            txtMsgSend.Text = "";
        }

        public void MensagemRecebida(string _Msg)
        {            
            ReceberMensagem("\r\n" + _Msg);            
        }
       
        private void btnStop_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
        }

        delegate void ReceberMensagemCallback(string msg);
        void ReceberMensagem(string msg)
        {
            if (InvokeRequired)
            {
                ReceberMensagemCallback callback = ReceberMensagem;
                Invoke(callback, msg);
            }
            else
            {   
                txtMsgSend.Text += msg;
            }
        }
    }
}
