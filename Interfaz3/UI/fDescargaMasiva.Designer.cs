namespace AdminDispositivosBiometricos
{
    partial class fDescargaMasiva
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvBitacora = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Evento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Equipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gv_Attlog = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Workcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUserinfo = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvNewMarcaciones = new System.Windows.Forms.DataGridView();
            this.badgenumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Attlog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserinfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewMarcaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBitacora
            // 
            this.dgvBitacora.AllowUserToAddRows = false;
            this.dgvBitacora.AllowUserToDeleteRows = false;
            this.dgvBitacora.AllowUserToResizeRows = false;
            this.dgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBitacora.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.Evento,
            this.Equipo,
            this.Hora});
            this.dgvBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBitacora.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBitacora.Location = new System.Drawing.Point(0, 0);
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvBitacora.Size = new System.Drawing.Size(800, 180);
            this.dgvBitacora.TabIndex = 2;
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.Width = 50;
            // 
            // Evento
            // 
            this.Evento.HeaderText = "Evento";
            this.Evento.Name = "Evento";
            this.Evento.Width = 250;
            // 
            // Equipo
            // 
            this.Equipo.HeaderText = "Equipo";
            this.Equipo.Name = "Equipo";
            // 
            // Hora
            // 
            this.Hora.HeaderText = "Hora";
            this.Hora.Name = "Hora";
            this.Hora.Width = 140;
            // 
            // gv_Attlog
            // 
            this.gv_Attlog.AllowUserToAddRows = false;
            this.gv_Attlog.AllowUserToOrderColumns = true;
            this.gv_Attlog.AllowUserToResizeRows = false;
            this.gv_Attlog.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gv_Attlog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gv_Attlog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_Attlog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn10,
            this.Date,
            this.VType,
            this.VState,
            this.Workcode});
            this.gv_Attlog.Location = new System.Drawing.Point(177, 246);
            this.gv_Attlog.MultiSelect = false;
            this.gv_Attlog.Name = "gv_Attlog";
            this.gv_Attlog.ReadOnly = true;
            this.gv_Attlog.RowHeadersWidth = 20;
            this.gv_Attlog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gv_Attlog.RowTemplate.Height = 23;
            this.gv_Attlog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gv_Attlog.Size = new System.Drawing.Size(477, 126);
            this.gv_Attlog.TabIndex = 112;
            this.gv_Attlog.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn10.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn10.FillWeight = 80F;
            this.dataGridViewTextBoxColumn10.HeaderText = "User ID";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Date.DataPropertyName = "Date";
            this.Date.HeaderText = "Verify Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // VType
            // 
            this.VType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VType.DataPropertyName = "VType";
            this.VType.FillWeight = 80F;
            this.VType.HeaderText = "Verify Type";
            this.VType.Name = "VType";
            this.VType.ReadOnly = true;
            // 
            // VState
            // 
            this.VState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VState.DataPropertyName = "VState";
            this.VState.FillWeight = 80F;
            this.VState.HeaderText = "Verify State";
            this.VState.Name = "VState";
            this.VState.ReadOnly = true;
            // 
            // Workcode
            // 
            this.Workcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Workcode.DataPropertyName = "Workcode";
            this.Workcode.FillWeight = 80F;
            this.Workcode.HeaderText = "WorkCode";
            this.Workcode.Name = "Workcode";
            this.Workcode.ReadOnly = true;
            // 
            // dgvUserinfo
            // 
            this.dgvUserinfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserinfo.Location = new System.Drawing.Point(177, 137);
            this.dgvUserinfo.Name = "dgvUserinfo";
            this.dgvUserinfo.Size = new System.Drawing.Size(477, 103);
            this.dgvUserinfo.TabIndex = 113;
            this.dgvUserinfo.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvNewMarcaciones);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvBitacora);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 114;
            // 
            // dgvNewMarcaciones
            // 
            this.dgvNewMarcaciones.AllowUserToAddRows = false;
            this.dgvNewMarcaciones.AllowUserToDeleteRows = false;
            this.dgvNewMarcaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNewMarcaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.badgenumber,
            this.nombre,
            this.FechaHora});
            this.dgvNewMarcaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNewMarcaciones.Location = new System.Drawing.Point(0, 0);
            this.dgvNewMarcaciones.Name = "dgvNewMarcaciones";
            this.dgvNewMarcaciones.ReadOnly = true;
            this.dgvNewMarcaciones.Size = new System.Drawing.Size(800, 266);
            this.dgvNewMarcaciones.TabIndex = 110;
            // 
            // badgenumber
            // 
            this.badgenumber.DataPropertyName = "badgenumber";
            this.badgenumber.HeaderText = "Codigo Empleado";
            this.badgenumber.Name = "badgenumber";
            this.badgenumber.ReadOnly = true;
            // 
            // nombre
            // 
            this.nombre.DataPropertyName = "nombre";
            this.nombre.HeaderText = "Nombre Empleado";
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            this.nombre.Width = 200;
            // 
            // FechaHora
            // 
            this.FechaHora.DataPropertyName = "FechaHora";
            dataGridViewCellStyle1.Format = "G";
            dataGridViewCellStyle1.NullValue = null;
            this.FechaHora.DefaultCellStyle = dataGridViewCellStyle1;
            this.FechaHora.HeaderText = "Fecha Hora";
            this.FechaHora.Name = "FechaHora";
            this.FechaHora.ReadOnly = true;
            this.FechaHora.Width = 140;
            // 
            // fDescargaMasiva
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvUserinfo);
            this.Controls.Add(this.gv_Attlog);
            this.Controls.Add(this.splitContainer1);
            this.Name = "fDescargaMasiva";
            this.Text = "Descarga De Marcaciones";
            this.Load += new System.EventHandler(this.fDescargaMasiva_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Attlog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserinfo)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewMarcaciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DataGridView dgvBitacora;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Evento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Equipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hora;
        private System.Windows.Forms.DataGridView gv_Attlog;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn VType;
        private System.Windows.Forms.DataGridViewTextBoxColumn VState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Workcode;
        private System.Windows.Forms.DataGridView dgvUserinfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvNewMarcaciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn badgenumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaHora;
    }
}