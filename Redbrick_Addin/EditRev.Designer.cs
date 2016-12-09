namespace Redbrick_Addin
{
    partial class EditRev
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRev));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.cbRev = new System.Windows.Forms.ComboBox();
      this.cbBy = new System.Windows.Forms.ComboBox();
      this.dtpDate = new System.Windows.Forms.DateTimePicker();
      this.tbECO = new System.Windows.Forms.TextBox();
      this.tbDesc = new System.Windows.Forms.TextBox();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel3.SuspendLayout();
      this.tableLayoutPanel4.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(25, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Rev";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 40);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(36, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "ECO#";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 81);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(66, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Description";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 122);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(19, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "By";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 162);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(31, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Date";
      // 
      // cbRev
      // 
      this.cbRev.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbRev.FormattingEnabled = true;
      this.cbRev.Location = new System.Drawing.Point(3, 16);
      this.cbRev.Name = "cbRev";
      this.cbRev.Size = new System.Drawing.Size(196, 21);
      this.cbRev.TabIndex = 1;
      // 
      // cbBy
      // 
      this.cbBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.cbBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cbBy.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbBy.FormattingEnabled = true;
      this.cbBy.Location = new System.Drawing.Point(3, 138);
      this.cbBy.Name = "cbBy";
      this.cbBy.Size = new System.Drawing.Size(196, 21);
      this.cbBy.TabIndex = 4;
      this.cbBy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.combobox_KeyDown);
      // 
      // dtpDate
      // 
      this.dtpDate.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dtpDate.Location = new System.Drawing.Point(3, 178);
      this.dtpDate.Name = "dtpDate";
      this.dtpDate.Size = new System.Drawing.Size(196, 22);
      this.dtpDate.TabIndex = 5;
      // 
      // tbECO
      // 
      this.tbECO.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbECO.Location = new System.Drawing.Point(3, 56);
      this.tbECO.Name = "tbECO";
      this.tbECO.Size = new System.Drawing.Size(196, 22);
      this.tbECO.TabIndex = 2;
      // 
      // tbDesc
      // 
      this.tbDesc.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbDesc.Location = new System.Drawing.Point(3, 97);
      this.tbDesc.Name = "tbDesc";
      this.tbDesc.Size = new System.Drawing.Size(196, 22);
      this.tbDesc.TabIndex = 3;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.Controls.Add(this.btnOK, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 0);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 206);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel3.Size = new System.Drawing.Size(196, 71);
      this.tableLayoutPanel3.TabIndex = 0;
      // 
      // btnOK
      // 
      this.btnOK.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnOK.Location = new System.Drawing.Point(3, 3);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(92, 28);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnCancel.Location = new System.Drawing.Point(101, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(92, 28);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // tableLayoutPanel4
      // 
      this.tableLayoutPanel4.ColumnCount = 1;
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel4.Controls.Add(this.dtpDate, 0, 9);
      this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 10);
      this.tableLayoutPanel4.Controls.Add(this.label5, 0, 8);
      this.tableLayoutPanel4.Controls.Add(this.label4, 0, 6);
      this.tableLayoutPanel4.Controls.Add(this.cbBy, 0, 7);
      this.tableLayoutPanel4.Controls.Add(this.label3, 0, 4);
      this.tableLayoutPanel4.Controls.Add(this.label2, 0, 2);
      this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel4.Controls.Add(this.cbRev, 0, 1);
      this.tableLayoutPanel4.Controls.Add(this.tbDesc, 0, 5);
      this.tableLayoutPanel4.Controls.Add(this.tbECO, 0, 3);
      this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      this.tableLayoutPanel4.RowCount = 11;
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel4.Size = new System.Drawing.Size(202, 280);
      this.tableLayoutPanel4.TabIndex = 1;
      // 
      // EditRev
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(202, 280);
      this.Controls.Add(this.tableLayoutPanel4);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = global::Redbrick_Addin.Properties.Settings.Default.EditRevLocation;
      this.MaximumSize = new System.Drawing.Size(500, 310);
      this.MinimumSize = new System.Drawing.Size(210, 310);
      this.Name = "EditRev";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "EditRev";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditRev_FormClosing);
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel4.ResumeLayout(false);
      this.tableLayoutPanel4.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbRev;
        private System.Windows.Forms.ComboBox cbBy;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox tbECO;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}