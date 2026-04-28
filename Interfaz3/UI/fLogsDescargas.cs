using LogicaB;
using System;
using System.Windows.Forms;
using DataGridViewAutoFilter;

namespace AdminDispositivosBiometricos
{
    public partial class fLogsDescargas : Form
    {
        public fLogsDescargas()
        {
            InitializeComponent();
        }
        private void FLogsDescargas_Load(object sender, EventArgs e)
        {
            dtpDesde.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpHasta.Value = DateTime.Today;

            CargaDatos();
            foreach(DataGridViewColumn columna in dgvLogs.Columns)
            {
                columna.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(columna.HeaderCell);
            }
        }
        private void CargaDatos()
        {
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = ClsLogicaLogsDescargas.RetornaLogs(dtpDesde.Value, dtpHasta.Value);
            dgvLogs.DataSource = bindingSource;
            dgvLogs.Columns["LogTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss.fff";
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            CargaDatos();
        }
    }
}
