using Domain.Administradores;
using LogicaB;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Utilitarios;

namespace AdminDispositivosBiometricos.UI
{
    public partial class FLogin : Form
    {
        private DataTable _dtAdministradores;
        public FLogin()
        {
            InitializeComponent();
        }

        private void FLogin_Load(object sender, EventArgs e)
        {
            try
            {
                var oLogAdmin = new ClsLogicaAdministradores();
                _dtAdministradores = oLogAdmin.RetornaAdministradores();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!ValidaInput()) return;

            var user = Autentica(txtUser.Text, txtPass.Text);

            if (user == null)
            {
                MessageBox.Show("Usuario y/o contraseña incorrecta", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            PoneVariablesGlobalesSession(user);

            fPrincipal frmMain = new fPrincipal();

            this.Hide();
            frmMain.Show();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit(); // O cerrar el form principal si ya existe
        }
        private AdminUser Autentica(string username, string password)
        {
            if (_dtAdministradores == null || _dtAdministradores.Rows.Count == 0) return null;

            ClsEncriptacion util = new ClsEncriptacion();

            // Uso de LINQ para buscar directamente la fila que coincide
            var row = _dtAdministradores.AsEnumerable().FirstOrDefault(r =>
                (r.Field<string>("badgenumber") == username || r.Field<string>("ZIP") == username) &&
                util.Desencripta(r.Field<string>("OTPassword")) == password);

            if (row == null) return null;

            return new AdminUser
            {
                Codigo = row.Field<string>("badgenumber"),
                Nombre = row.Field<string>("name"),
                OTAdmin = row.Field<int>("OTAdmin"),
                Privilegios = util.Desencripta(row.Field<string>("OTPrivAdmin"))
            };
        }
        private bool ValidaInput()
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("El nombre de usuario/contraseña no puede estar vacío.", "Error de ingreso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }
        private void PoneVariablesGlobalesSession(AdminUser user)
        {
            Globales.CodAdminLog = user.Codigo;
            Globales.AdminNom = user.Nombre;
            Globales.TAdminLogeado = user.OTAdmin;
            Globales.AdminPass = txtPass.Text;
            Globales.PrivAdmin = user.Privilegios;
        }
    }
}
