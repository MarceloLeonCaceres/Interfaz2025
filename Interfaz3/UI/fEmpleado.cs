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
    public enum Modo
    {
        Creacion,
        Edicion
    }
    public partial class fEmpleado : Form
    {

        DataTable dtEmpleados = new DataTable();
        int idUsuario;
        Modo modo = Modo.Creacion;        

        public fEmpleado()
        {
            InitializeComponent();
        }

        private void fEmpleado_Load(object sender, EventArgs e)
        {
            CargaDatos();
        }

        private void CargaDatos(string sBusqueda = "")
        {
            dtEmpleados = clsLogicaEmpleados.RetornaIdBadgeNombreSsn(sBusqueda);
            Utilitarios.ClsDataTableDgv.LlenaGridConDataTable(dtEmpleados, dgvEmpleados, new List<string> { "Userid" });
            foreach (DataGridViewColumn col in dgvEmpleados.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
        }

        private void tsbAgregar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            clsLogicaEmpleados.AgregaNuevoEmpleado();
            CargaDatos();
            dgvEmpleados.Rows[dgvEmpleados.Rows.Count - 1].Selected = true;
            dgvEmpleados.Refresh();
            modo = Modo.Creacion;
            Editar();
            this.Cursor = Cursors.Default;
        }

        private void tsbEliminar_Click(object sender, EventArgs e)
        {
            DialogResult confirma = MessageBox.Show("Confirma que desea eliminar los empleados seleccionados de la base de datos?\n\nTambién se eliminarán las huellas, rostro, marcaciones y demás información del empleado", "Borrado de la base de datos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.No)
                return;
            
            this.Cursor = Cursors.WaitCursor;
            idUsuario = FilaSeleccionada(dgvEmpleados);
            if(idUsuario > -1)
            {
                clsLogicaEmpleados.EliminaUsuario(idUsuario);
                CargaDatos();
            }
            this.Cursor = Cursors.Default;
        }

        private int FilaSeleccionada(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count <= 0)
                return -1;
            return (int)dgv.SelectedRows[0].Cells[0].Value;
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            modo = Modo.Edicion;
            this.Cursor = Cursors.WaitCursor;
            Editar();
            this.Cursor = Cursors.Default;
        }

        private void Editar()
        {
            idUsuario = FilaSeleccionada(dgvEmpleados);
            if (idUsuario > -1)
            {
                ActivaEdicion(true);
                txtBadgenumber.Text = dgvEmpleados.SelectedRows[0].Cells[1].Value.ToString();
                txtNombre.Text = dgvEmpleados.SelectedRows[0].Cells[2].Value.ToString();
                txtSsn.Text = dgvEmpleados.SelectedRows[0].Cells[3].Value.ToString();
                txtBadgenumber.Focus();
            }
        }

        private void ActivaEdicion(bool on_off)
        {
            txtBadgenumber.Enabled = on_off;
            txtNombre.Enabled = on_off;
            txtSsn.Enabled = on_off;
            btnGuardar.Enabled = on_off;
            btnCancelar.Enabled = on_off;
            dgvEmpleados.Enabled = !on_off;
            tsfEmpleado.Enabled = !on_off;
            if (on_off)
                btnGuardar.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (modo == Modo.Creacion)
            {
                this.Cursor = Cursors.WaitCursor;
                clsLogicaEmpleados.EliminaUsuario(idUsuario);
                CargaDatos();
                this.Cursor = Cursors.Default;
            }
            ActivaEdicion(false);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtBadgenumber.Text) || String.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El Código y el Nombre del empleado no pueden quedar en blanco.", "Datos no válidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long lCodigo;
            if (!long.TryParse(txtBadgenumber.Text, out lCodigo))
            {
                MessageBox.Show("El código debe ser número de hasta 9 dígitos y el primer dígito debe ser diferente de cero.", "Datos no válidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            int caso = clsLogicaEmpleados.EditaUsuario(idUsuario, txtBadgenumber.Text, txtNombre.Text, txtSsn.Text);
            switch (caso)
            {
                case -1:
                    MessageBox.Show("Ya existe otro empleado con ese Código", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                case -2:
                    MessageBox.Show("Ya existe otro empleado con ese Nombre", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                case -3:
                    MessageBox.Show("Ya existe otro empleado con ese Código Alterno", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                case -4:
                    MessageBox.Show("Se presentó un error al ingresar los datos indicados\n\nPor favor revise que los datos sean correctos y no duplique la información de otra persona", "Error al ingresar datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
            }
            tsTxtBusqueda.Text = "";
            CargaDatos();
            ActivaEdicion(false);
            this.Cursor = Cursors.Default;
            return;
        }

        private void dgvEmpleados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null)
                return;

            txtBadgenumber.Text = dgvEmpleados.CurrentRow.Cells[1].Value.ToString();
            txtNombre.Text = dgvEmpleados.CurrentRow.Cells[2].Value.ToString();
            txtSsn.Text = dgvEmpleados.CurrentRow.Cells[3].Value.ToString();
            idUsuario = (int)dgvEmpleados.CurrentRow.Cells[0].Value;
        }

        private void tsTxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                CargaDatos(tsTxtBusqueda.Text);
        }

        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            CargaDatos(tsTxtBusqueda.Text);
        }

        private void tsbImportarEmpleado_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivo de datos de empleados|*.csv";
            openFileDialog1.Title = "Seleccione un archivo de datos de empleados";
            openFileDialog1.Multiselect = false;

            DialogResult respuesta = openFileDialog1.ShowDialog();
            switch (respuesta)
            {
                case DialogResult.OK:
                    this.Cursor = Cursors.WaitCursor;

                    try
                    {
                        // ingresa a todos los empleados en 1 solo proceso, por eso no ocupa counter
                        // int counter = 0;
                        string[] lines = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                        char[] delimiterChars = { ',', '.', ';' };
                        var lstRegistros = new List<Tuple<string, string, string >>();
                    
                        clsLogicaEmpleados.EnceraRegEmpleadosCsv();
                        for (int i = 1; i < lines.Length; i++)
                        {
                            // procesa la línea line
                            string[] param = lines[i].Split(delimiterChars);
                            lstRegistros.Add(Tuple.Create(param[0], param[1], param[2]));                            
                        }
                        
                        if (lstRegistros.Count > 0)
                            clsLogicaEmpleados.guardaEmpleadosCsv(lstRegistros);

                        int num = clsLogicaEmpleados.IngresaEmpleadosCsv();
                        string mensaje = "";
                        switch (num)
                        {
                            case -1:
                                mensaje = "Hay empleados con código principal duplicado";
                                break;
                            case -2:
                                mensaje = "Hay empleados con nombre duplicado";
                                break;
                            case -3:
                                mensaje = "Hay empleados con código alterno duplicado";
                                break;
                            case -4:
                                mensaje = "El formato de los datos no es válido";
                                break;
                            default:
                                mensaje = "Se ingresaron " + num.ToString() + " usuarios nuevos.";
                                break;
                        }
                        clsLogicaEmpleados.EnceraRegEmpleadosCsv();
                        if (num < 0)
                        {
                            MessageBox.Show(mensaje + "\nPor favor revise los datos y el formato, e intente nuevamente.", "Datos inválidos o duplicados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(mensaje, "Ingreso de empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (num > 0)
                        {
                            tsTxtBusqueda.Text = "";
                            CargaDatos();
                        }
                    }
                    catch (clsLogicaException errBdd)
                    {
                        MessageBox.Show(errBdd.logErrorDescription, "Error con Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("El formato del archivo no es el correcto.\n" + err.Message + "\n" + err.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    MessageBox.Show("El formato del archivo seleccionado no corresponde a un archivo de marcaciones de reloj biométrico", "Archivo inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }

            this.Cursor = Cursors.Default;
            return;
        }
    }
}
