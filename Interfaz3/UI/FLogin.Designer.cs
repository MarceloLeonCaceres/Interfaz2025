namespace AdminDispositivosBiometricos.UI
{
    partial class FLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLogin));
            this.Usernamelabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.LogoPictureBoxA = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBoxA)).BeginInit();
            this.SuspendLayout();
            // 
            // Usernamelabel
            // 
            this.Usernamelabel.AutoSize = true;
            this.Usernamelabel.Location = new System.Drawing.Point(170, 12);
            this.Usernamelabel.Name = "Usernamelabel";
            this.Usernamelabel.Size = new System.Drawing.Size(99, 13);
            this.Usernamelabel.TabIndex = 0;
            this.Usernamelabel.Text = "Nombre de usuario:";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(170, 58);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(56, 13);
            this.PasswordLabel.TabIndex = 1;
            this.PasswordLabel.Text = "Password:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(170, 31);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(240, 20);
            this.txtUser.TabIndex = 2;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(170, 75);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(240, 20);
            this.txtPass.TabIndex = 3;
            // 
            // OK
            // 
            this.OK.Image = global::AdminDispositivosBiometricos.Properties.Resources.CheckCircled_16x16;
            this.OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OK.Location = new System.Drawing.Point(170, 111);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(113, 26);
            this.OK.TabIndex = 4;
            this.OK.Text = "Aceptar";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Image = global::AdminDispositivosBiometricos.Properties.Resources.DeletedCircled_16x16;
            this.Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Cancel.Location = new System.Drawing.Point(297, 111);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(113, 26);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancelar";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // LogoPictureBoxA
            // 
            this.LogoPictureBoxA.Image = global::AdminDispositivosBiometricos.Properties.Resources.HuellaAmarillaRelojRojo_128x128;
            this.LogoPictureBoxA.Location = new System.Drawing.Point(29, 25);
            this.LogoPictureBoxA.Name = "LogoPictureBoxA";
            this.LogoPictureBoxA.Size = new System.Drawing.Size(101, 98);
            this.LogoPictureBoxA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogoPictureBoxA.TabIndex = 6;
            this.LogoPictureBoxA.TabStop = false;
            // 
            // FLogin
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(427, 150);
            this.Controls.Add(this.LogoPictureBoxA);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.Usernamelabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FLogin";
            this.Text = "Ingreso Administrador de Dispositivos";
            this.Load += new System.EventHandler(this.FLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBoxA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Usernamelabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.PictureBox LogoPictureBoxA;
    }
}