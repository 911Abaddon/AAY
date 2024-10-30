namespace AAY
{
    partial class frmCustomers
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
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lbll1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(304, 83);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(296, 20);
            this.txtLastName.TabIndex = 1;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnConfirm.Location = new System.Drawing.Point(304, 277);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(141, 62);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click_1);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.btnExit.Location = new System.Drawing.Point(459, 277);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(141, 62);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbl3
            // 
            this.lbl3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lbl3.Location = new System.Drawing.Point(12, 77);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(175, 32);
            this.lbl3.TabIndex = 6;
            this.lbl3.Text = "Επίθετο";
            // 
            // lbl4
            // 
            this.lbl4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lbl4.Location = new System.Drawing.Point(12, 20);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(175, 32);
            this.lbl4.TabIndex = 7;
            this.lbl4.Text = "Όνομα";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(304, 26);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(296, 20);
            this.txtFirstName.TabIndex = 9;
            // 
            // lbl
            // 
            this.lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lbl.Location = new System.Drawing.Point(12, 149);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(209, 32);
            this.lbl.TabIndex = 11;
            this.lbl.Text = "Μέθοδος Πληρωμής";
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.Location = new System.Drawing.Point(304, 159);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(296, 21);
            this.cmbPaymentMethod.TabIndex = 12;
            this.cmbPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentMethod_SelectedIndexChanged);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Location = new System.Drawing.Point(304, 228);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(296, 20);
            this.txtCardNumber.TabIndex = 13;
            this.txtCardNumber.Visible = false;
            // 
            // lbll1
            // 
            this.lbll1.AutoSize = true;
            this.lbll1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lbll1.Location = new System.Drawing.Point(13, 223);
            this.lbll1.Name = "lbll1";
            this.lbll1.Size = new System.Drawing.Size(158, 24);
            this.lbll1.TabIndex = 14;
            this.lbll1.Text = "Αριθμός Κάρτας.";
            // 
            // frmCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 353);
            this.Controls.Add(this.lbll1);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.cmbPaymentMethod);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lbl4);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtLastName);
            this.Name = "frmCustomers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCostumers";
            this.Load += new System.EventHandler(this.frmCustomers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl4;
        public System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lbll1;
    }
}