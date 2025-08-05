using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using LogicaB;
using Utilitarios;

namespace AdminDispositivosBiometricos
{
    public partial class fEditaReloj : Form
    {

        public int iNumero;
        public IPAddress IP;
        public int iPuerto;

        public Utilitarios.ClsReloj datosReloj = new ClsReloj();

        public fEditaReloj()
        {
            InitializeComponent();
            this.Name = datosReloj.sNombreReloj;
        }

        private void fEditaReloj_Load(object sender, EventArgs e)
        {
            cargaDatos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cargaDatos()
        {
            txtNombre.Text = datosReloj.sNombreReloj;
            txtNumero.Text = datosReloj.iNumero.ToString();
            txtIP.Text = datosReloj.sIP.ToString();
            txtPuerto.Text = datosReloj.iPuerto.ToString();
            txtSn.Text = datosReloj.sSN;

            IP = datosReloj.sIP;

        }

        private bool validaDatos()
        {

            if (!IPAddress.TryParse(txtIP.Text, out IP))
            {
                MessageBox.Show("La dirección IP ingresada no es válida", "Error en ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtNumero.Text, out iNumero))
            {
                MessageBox.Show("El número de dispositivo no es válido", "Error en ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtPuerto.Text, out iPuerto))
            {
                MessageBox.Show("El número de puerto no es válido", "Error en ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtSn.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios, no pueden quedar campos vacíos", "Error en ingreso de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validaDatos())
                return;

            clsLogicaDispositivos oLog = new clsLogicaDispositivos();
            oLog.ActualizarReloj(datosReloj.idReloj, txtNombre.Text, iNumero, IP, iPuerto, txtSn.Text);
            MessageBox.Show("Los cambios han sido guardados.", "Modificación realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
