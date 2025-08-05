using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicaB;
using Utilitarios;

namespace AdminDispositivosBiometricos
{
    public partial class fLogsDescargas : Form
    {
        public fLogsDescargas()
        {
            InitializeComponent();
        }

        private void fLogsDescargas_Load(object sender, EventArgs e)
        {
            CargaDatos();
        }

        private void CargaDatos()
        {
            DataTable dt = clsLogicaLogsDescargas.retornaLogs(100);
            Utilitarios.ClsDataTableDgv.LlenaGridConDataTable(dt, dgvLogs, null);
        }


    }
}
