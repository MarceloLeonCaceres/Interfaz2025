namespace AdminDispositivosBiometricos
{
    partial class fEmpleado
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tsfEmpleado = new System.Windows.Forms.ToolStrip();
            this.tsTxtBusqueda = new System.Windows.Forms.ToolStripTextBox();
            this.dgvEmpleados = new System.Windows.Forms.DataGridView();
            this.gbDatosEmpleado = new System.Windows.Forms.GroupBox();
            this.txtSsn = new System.Windows.Forms.TextBox();
            this.lblCodigoAlterno = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtBadgenumber = new System.Windows.Forms.TextBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tsbAgregar = new System.Windows.Forms.ToolStripButton();
            this.tsbEliminar = new System.Windows.Forms.ToolStripButton();
            this.tsbEditar = new System.Windows.Forms.ToolStripButton();
            this.tsbImportarEmpleado = new System.Windows.Forms.ToolStripButton();
            this.tsbBuscar = new System.Windows.Forms.ToolStripButton();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tsfEmpleado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados)).BeginInit();
            this.gbDatosEmpleado.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsfEmpleado
            // 
            this.tsfEmpleado.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAgregar,
            this.tsbEliminar,
            this.tsbEditar,
            this.tsbImportarEmpleado,
            this.tsbBuscar,
            this.tsTxtBusqueda});
            this.tsfEmpleado.Location = new System.Drawing.Point(0, 0);
            this.tsfEmpleado.Name = "tsfEmpleado";
            this.tsfEmpleado.Size = new System.Drawing.Size(592, 54);
            this.tsfEmpleado.TabIndex = 0;
            this.tsfEmpleado.Text = "toolStrip1";
            // 
            // tsTxtBusqueda
            // 
            this.tsTxtBusqueda.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsTxtBusqueda.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsTxtBusqueda.Name = "tsTxtBusqueda";
            this.tsTxtBusqueda.Size = new System.Drawing.Size(100, 23);
            this.tsTxtBusqueda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tsTxtBusqueda_KeyDown);
            // 
            // dgvEmpleados
            // 
            this.dgvEmpleados.AllowUserToAddRows = false;
            this.dgvEmpleados.AllowUserToDeleteRows = false;
            this.dgvEmpleados.AllowUserToOrderColumns = true;
            this.dgvEmpleados.AllowUserToResizeRows = false;
            this.dgvEmpleados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEmpleados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmpleados.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvEmpleados.Location = new System.Drawing.Point(6, 57);
            this.dgvEmpleados.Name = "dgvEmpleados";
            this.dgvEmpleados.ReadOnly = true;
            this.dgvEmpleados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmpleados.Size = new System.Drawing.Size(580, 337);
            this.dgvEmpleados.TabIndex = 1;
            this.dgvEmpleados.SelectionChanged += new System.EventHandler(this.dgvEmpleados_SelectionChanged);
            // 
            // gbDatosEmpleado
            // 
            this.gbDatosEmpleado.Controls.Add(this.txtSsn);
            this.gbDatosEmpleado.Controls.Add(this.lblCodigoAlterno);
            this.gbDatosEmpleado.Controls.Add(this.txtNombre);
            this.gbDatosEmpleado.Controls.Add(this.lblNombre);
            this.gbDatosEmpleado.Controls.Add(this.txtBadgenumber);
            this.gbDatosEmpleado.Controls.Add(this.lblCodigo);
            this.gbDatosEmpleado.Location = new System.Drawing.Point(12, 400);
            this.gbDatosEmpleado.Name = "gbDatosEmpleado";
            this.gbDatosEmpleado.Size = new System.Drawing.Size(481, 77);
            this.gbDatosEmpleado.TabIndex = 2;
            this.gbDatosEmpleado.TabStop = false;
            this.gbDatosEmpleado.Text = "Edición Datos de Empleado";
            // 
            // txtSsn
            // 
            this.txtSsn.Enabled = false;
            this.txtSsn.Location = new System.Drawing.Point(308, 43);
            this.txtSsn.MaxLength = 20;
            this.txtSsn.Name = "txtSsn";
            this.txtSsn.Size = new System.Drawing.Size(164, 20);
            this.txtSsn.TabIndex = 8;
            // 
            // lblCodigoAlterno
            // 
            this.lblCodigoAlterno.AutoSize = true;
            this.lblCodigoAlterno.Location = new System.Drawing.Point(305, 27);
            this.lblCodigoAlterno.Name = "lblCodigoAlterno";
            this.lblCodigoAlterno.Size = new System.Drawing.Size(76, 13);
            this.lblCodigoAlterno.TabIndex = 7;
            this.lblCodigoAlterno.Text = "Código Alterno";
            // 
            // txtNombre
            // 
            this.txtNombre.Enabled = false;
            this.txtNombre.Location = new System.Drawing.Point(118, 43);
            this.txtNombre.MaxLength = 24;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(164, 20);
            this.txtNombre.TabIndex = 6;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(115, 27);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 5;
            this.lblNombre.Text = "Nombre";
            // 
            // txtBadgenumber
            // 
            this.txtBadgenumber.Enabled = false;
            this.txtBadgenumber.Location = new System.Drawing.Point(6, 43);
            this.txtBadgenumber.MaxLength = 9;
            this.txtBadgenumber.Name = "txtBadgenumber";
            this.txtBadgenumber.Size = new System.Drawing.Size(86, 20);
            this.txtBadgenumber.TabIndex = 1;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(6, 27);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(40, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.csv";
            // 
            // tsbAgregar
            // 
            this.tsbAgregar.Image = global::AdminDispositivosBiometricos.Properties.Resources.AgregarUsuario_32x32;
            this.tsbAgregar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAgregar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAgregar.Name = "tsbAgregar";
            this.tsbAgregar.Size = new System.Drawing.Size(109, 51);
            this.tsbAgregar.Text = "Agregar Empleado";
            this.tsbAgregar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbAgregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAgregar.Click += new System.EventHandler(this.tsbAgregar_Click);
            // 
            // tsbEliminar
            // 
            this.tsbEliminar.Image = global::AdminDispositivosBiometricos.Properties.Resources.EliminarUsuario_32x32;
            this.tsbEliminar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEliminar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEliminar.Name = "tsbEliminar";
            this.tsbEliminar.Size = new System.Drawing.Size(110, 51);
            this.tsbEliminar.Text = "Eliminar Empleado";
            this.tsbEliminar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEliminar.Click += new System.EventHandler(this.tsbEliminar_Click);
            // 
            // tsbEditar
            // 
            this.tsbEditar.Image = global::AdminDispositivosBiometricos.Properties.Resources.EditarUsuario_32x32;
            this.tsbEditar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEditar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(97, 51);
            this.tsbEditar.Text = "Editar Empleado";
            this.tsbEditar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEditar.Click += new System.EventHandler(this.tsbEditar_Click);
            // 
            // tsbImportarEmpleado
            // 
            this.tsbImportarEmpleado.Image = global::AdminDispositivosBiometricos.Properties.Resources.ImportarUsuarios_32x32;
            this.tsbImportarEmpleado.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbImportarEmpleado.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportarEmpleado.Name = "tsbImportarEmpleado";
            this.tsbImportarEmpleado.Size = new System.Drawing.Size(118, 51);
            this.tsbImportarEmpleado.Text = "Importar Empleados";
            this.tsbImportarEmpleado.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbImportarEmpleado.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbImportarEmpleado.Click += new System.EventHandler(this.tsbImportarEmpleado_Click);
            // 
            // tsbBuscar
            // 
            this.tsbBuscar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbBuscar.Image = global::AdminDispositivosBiometricos.Properties.Resources.BusquedaUsuario_32x32;
            this.tsbBuscar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbBuscar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBuscar.Name = "tsbBuscar";
            this.tsbBuscar.Size = new System.Drawing.Size(46, 51);
            this.tsbBuscar.Text = "Buscar";
            this.tsbBuscar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbBuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbBuscar.Click += new System.EventHandler(this.tsbBuscar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Enabled = false;
            this.btnGuardar.Image = global::AdminDispositivosBiometricos.Properties.Resources.CheckCircled_16x16;
            this.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.Location = new System.Drawing.Point(499, 417);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Enabled = false;
            this.btnCancelar.Image = global::AdminDispositivosBiometricos.Properties.Resources.DeletedCircled_16x16;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(499, 454);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // fEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 489);
            this.Controls.Add(this.gbDatosEmpleado);
            this.Controls.Add(this.dgvEmpleados);
            this.Controls.Add(this.tsfEmpleado);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fEmpleado";
            this.Text = "Empleados Registrados En Base De Datos";
            this.Load += new System.EventHandler(this.fEmpleado_Load);
            this.tsfEmpleado.ResumeLayout(false);
            this.tsfEmpleado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados)).EndInit();
            this.gbDatosEmpleado.ResumeLayout(false);
            this.gbDatosEmpleado.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsfEmpleado;
        private System.Windows.Forms.ToolStripButton tsbAgregar;
        private System.Windows.Forms.ToolStripButton tsbEliminar;
        private System.Windows.Forms.ToolStripButton tsbEditar;
        private System.Windows.Forms.ToolStripButton tsbBuscar;
        private System.Windows.Forms.ToolStripTextBox tsTxtBusqueda;
        private System.Windows.Forms.DataGridView dgvEmpleados;
        private System.Windows.Forms.GroupBox gbDatosEmpleado;
        private System.Windows.Forms.TextBox txtBadgenumber;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtSsn;
        private System.Windows.Forms.Label lblCodigoAlterno;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.ToolStripButton tsbImportarEmpleado;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}