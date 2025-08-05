using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LogicaB;

namespace AdminDispositivosBiometricos
{
    public partial class fAdminDestinatariosCorreos : Form
    {
        DataTable dtCorreos = new DataTable();
        int idCorreo;
        public fAdminDestinatariosCorreos()
        {
            InitializeComponent();
        }

        private void fAdminDestinatariosCorreos_Load(object sender, EventArgs e)
        {
            CargaDatos();
        }

        private void CargaDatos()
        {
            dtCorreos = clsLogicaAdminCorreos.dtRetornaCorreos();
            Utilitarios.ClsDataTableDgv.LlenaGridConDataTable(dtCorreos, dgvCorreos, new List<string> { "ID" });
        }

        private void tsbAgregar_Click(object sender, EventArgs e)
        {
            clsLogicaAdminCorreos.AgregaCorreoNuevo();
            CargaDatos();
            dgvCorreos.Rows[dgvCorreos.Rows.Count - 1].Selected = true;
            tsbEditar_Click(null, null);
        }

        private void tsbEliminar_Click(object sender, EventArgs e)
        {
            idCorreo = FilaSeleccionada(dgvCorreos);
            if (idCorreo > -1)
            {
                clsLogicaAdminCorreos.EliminaCorreo(idCorreo);
                CargaDatos();
            }
        }

        private int FilaSeleccionada(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count <= 0)
                return -1;
            return (int)dgv.SelectedRows[0].Cells[0].Value;
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            idCorreo = FilaSeleccionada(dgvCorreos);
            if (idCorreo > -1)
            {
                ActivaEdicion(true);
                txtCorreo.Text = dgvCorreos.SelectedRows[0].Cells[1].Value.ToString();
            }
        }

        private void ActivaEdicion(bool on_off)
        {
            txtCorreo.Enabled = on_off;
            btnGuardar.Enabled = on_off;
            btnCancelar.Enabled = on_off;
            dgvCorreos.Enabled = !on_off;
            toolStrip1.Enabled = !on_off;
            if (on_off)
                btnGuardar.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ActivaEdicion(false);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if ( txtCorreo.Text.Contains(" ") || !txtCorreo.Text.Contains(".") ||
                !txtCorreo.Text.Contains("@") || String.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("El texto ingresado no corresponde a una dirección de correo válida");
                return;
            }
            clsLogicaAdminCorreos.EditaCorreo(idCorreo, txtCorreo.Text);
            CargaDatos();
            ActivaEdicion(false);
        }

        private void dgvCorreos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCorreos.CurrentRow == null)
                return;

            txtCorreo.Text = dgvCorreos.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
