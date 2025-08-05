namespace AdminDispositivosBiometricos
{
    partial class fPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fPrincipal));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbEditar = new System.Windows.Forms.ToolStripButton();
            this.tsbConectar = new System.Windows.Forms.ToolStripButton();
            this.tsbUSB = new System.Windows.Forms.ToolStripButton();
            this.tsbUsuarios = new System.Windows.Forms.ToolStripButton();
            this.tsbCorreos = new System.Windows.Forms.ToolStripButton();
            this.spBtnDescargaMasivaRelojes = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiLecturaLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvRelojes = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelojes)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEditar,
            this.tsbConectar,
            this.tsbUSB,
            this.tsbUsuarios,
            this.tsbCorreos,
            this.spBtnDescargaMasivaRelojes});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 54);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbEditar
            // 
            this.tsbEditar.Image = global::AdminDispositivosBiometricos.Properties.Resources.Editar_32x32;
            this.tsbEditar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEditar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(70, 51);
            this.tsbEditar.Text = "Editar Reloj";
            this.tsbEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEditar.Click += new System.EventHandler(this.tsbEditar_Click);
            // 
            // tsbConectar
            // 
            this.tsbConectar.Image = global::AdminDispositivosBiometricos.Properties.Resources.Play_32x32;
            this.tsbConectar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbConectar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConectar.Name = "tsbConectar";
            this.tsbConectar.Size = new System.Drawing.Size(88, 51);
            this.tsbConectar.Text = "Conectar Reloj";
            this.tsbConectar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbConectar.Click += new System.EventHandler(this.tsbConectar_Click);
            // 
            // tsbUSB
            // 
            this.tsbUSB.Image = global::AdminDispositivosBiometricos.Properties.Resources.icons8_usb_2_26;
            this.tsbUSB.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbUSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbUSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUSB.Name = "tsbUSB";
            this.tsbUSB.Size = new System.Drawing.Size(138, 51);
            this.tsbUSB.Text = "Descargar Registros USB";
            this.tsbUSB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbUSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbUSB.Click += new System.EventHandler(this.tsbUSB_Click);
            // 
            // tsbUsuarios
            // 
            this.tsbUsuarios.Image = global::AdminDispositivosBiometricos.Properties.Resources.PersonalIdAzulRojo_32x32;
            this.tsbUsuarios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbUsuarios.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUsuarios.Name = "tsbUsuarios";
            this.tsbUsuarios.Size = new System.Drawing.Size(89, 51);
            this.tsbUsuarios.Text = "Datos Usuarios";
            this.tsbUsuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbUsuarios.Visible = false;
            this.tsbUsuarios.Click += new System.EventHandler(this.tsbUsuarios_Click);
            // 
            // tsbCorreos
            // 
            this.tsbCorreos.Image = global::AdminDispositivosBiometricos.Properties.Resources.Correo_32x32;
            this.tsbCorreos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCorreos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCorreos.Name = "tsbCorreos";
            this.tsbCorreos.Size = new System.Drawing.Size(109, 51);
            this.tsbCorreos.Text = "Cuentas de Correo";
            this.tsbCorreos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbCorreos.Click += new System.EventHandler(this.tsbCorreos_Click);
            // 
            // spBtnDescargaMasivaRelojes
            // 
            this.spBtnDescargaMasivaRelojes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLecturaLogs});
            this.spBtnDescargaMasivaRelojes.Image = global::AdminDispositivosBiometricos.Properties.Resources.Descarga_32x32;
            this.spBtnDescargaMasivaRelojes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.spBtnDescargaMasivaRelojes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.spBtnDescargaMasivaRelojes.Name = "spBtnDescargaMasivaRelojes";
            this.spBtnDescargaMasivaRelojes.Size = new System.Drawing.Size(167, 51);
            this.spBtnDescargaMasivaRelojes.Text = "Descargar Todos los Relojes";
            this.spBtnDescargaMasivaRelojes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.spBtnDescargaMasivaRelojes.ButtonClick += new System.EventHandler(this.spBtnDescargaMasivaRelojes_ButtonClick);
            // 
            // tsmiLecturaLogs
            // 
            this.tsmiLecturaLogs.Name = "tsmiLecturaLogs";
            this.tsmiLecturaLogs.Size = new System.Drawing.Size(205, 22);
            this.tsmiLecturaLogs.Text = "Consulta Logs Descargas";
            this.tsmiLecturaLogs.Click += new System.EventHandler(this.tsmiLecturaLogs_Click);
            // 
            // dgvRelojes
            // 
            this.dgvRelojes.AllowUserToAddRows = false;
            this.dgvRelojes.AllowUserToDeleteRows = false;
            this.dgvRelojes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRelojes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelojes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRelojes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRelojes.Location = new System.Drawing.Point(0, 54);
            this.dgvRelojes.MultiSelect = false;
            this.dgvRelojes.Name = "dgvRelojes";
            this.dgvRelojes.ReadOnly = true;
            this.dgvRelojes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRelojes.Size = new System.Drawing.Size(800, 396);
            this.dgvRelojes.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.dat";
            // 
            // fPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvRelojes);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fPrincipal";
            this.Text = "Administración Equipos Biométricos";
            this.Load += new System.EventHandler(this.fPrincipal_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRelojes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbEditar;
        private System.Windows.Forms.ToolStripButton tsbConectar;
        private System.Windows.Forms.DataGridView dgvRelojes;
        private System.Windows.Forms.ToolStripButton tsbCorreos;
        private System.Windows.Forms.ToolStripSplitButton spBtnDescargaMasivaRelojes;
        private System.Windows.Forms.ToolStripMenuItem tsmiLecturaLogs;
        private System.Windows.Forms.ToolStripButton tsbUSB;
        private System.Windows.Forms.ToolStripButton tsbUsuarios;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

