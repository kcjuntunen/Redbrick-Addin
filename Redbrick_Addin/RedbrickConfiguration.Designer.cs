namespace Redbrick_Addin {
  partial class RedbrickConfiguration {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RedbrickConfiguration));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.chbDBEnabled = new System.Windows.Forms.CheckBox();
      this.chbTestingMode = new System.Windows.Forms.CheckBox();
      this.cbDept = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.cbRevLimit = new System.Windows.Forms.ComboBox();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.cbDefaultMaterial = new System.Windows.Forms.ComboBox();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.chbDBEnabled, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.chbTestingMode, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.cbDept, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.label1, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.cbRevLimit, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.cbDefaultMaterial, 0, 5);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 6;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(296, 88);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // chbDBEnabled
      // 
      this.chbDBEnabled.AutoSize = true;
      this.chbDBEnabled.Location = new System.Drawing.Point(3, 3);
      this.chbDBEnabled.Name = "chbDBEnabled";
      this.chbDBEnabled.Size = new System.Drawing.Size(144, 17);
      this.chbDBEnabled.TabIndex = 1;
      this.chbDBEnabled.Text = "Enable Database Writing";
      this.chbDBEnabled.UseVisualStyleBackColor = true;
      this.chbDBEnabled.CheckedChanged += new System.EventHandler(this.chbDBEnabled_CheckedChanged);
      // 
      // chbTestingMode
      // 
      this.chbTestingMode.AutoSize = true;
      this.chbTestingMode.Location = new System.Drawing.Point(3, 26);
      this.chbTestingMode.Name = "chbTestingMode";
      this.chbTestingMode.Size = new System.Drawing.Size(91, 17);
      this.chbTestingMode.TabIndex = 2;
      this.chbTestingMode.Text = "Testing Mode";
      this.chbTestingMode.UseVisualStyleBackColor = true;
      this.chbTestingMode.CheckedChanged += new System.EventHandler(this.chbTestingMode_CheckedChanged);
      // 
      // cbDept
      // 
      this.cbDept.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbDept.FormattingEnabled = true;
      this.cbDept.Location = new System.Drawing.Point(153, 66);
      this.cbDept.Name = "cbDept";
      this.cbDept.Size = new System.Drawing.Size(140, 21);
      this.cbDept.TabIndex = 3;
      this.cbDept.SelectedIndexChanged += new System.EventHandler(this.cbDept_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(153, 50);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(87, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "User Department";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 50);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(53, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "REV Limit";
      // 
      // cbRevLimit
      // 
      this.cbRevLimit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbRevLimit.FormattingEnabled = true;
      this.cbRevLimit.Location = new System.Drawing.Point(3, 66);
      this.cbRevLimit.Name = "cbRevLimit";
      this.cbRevLimit.Size = new System.Drawing.Size(144, 21);
      this.cbRevLimit.TabIndex = 7;
      this.cbRevLimit.SelectedIndexChanged += new System.EventHandler(this.cbRevLimit_SelectedIndexChanged);
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(296, 219);
      this.tableLayoutPanel2.TabIndex = 1;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.Controls.Add(this.btnCancel, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.btnOK, 1, 0);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 97);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel3.Size = new System.Drawing.Size(296, 119);
      this.tableLayoutPanel3.TabIndex = 1;
      // 
      // btnCancel
      // 
      this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnCancel.Location = new System.Drawing.Point(3, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 113);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnOK.Location = new System.Drawing.Point(84, 3);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(209, 113);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(153, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(81, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Default Material";
      // 
      // cbDefaultMaterial
      // 
      this.cbDefaultMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbDefaultMaterial.FormattingEnabled = true;
      this.cbDefaultMaterial.Location = new System.Drawing.Point(153, 26);
      this.cbDefaultMaterial.Name = "cbDefaultMaterial";
      this.cbDefaultMaterial.Size = new System.Drawing.Size(140, 21);
      this.cbDefaultMaterial.TabIndex = 9;
      // 
      // RedbrickConfiguration
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(296, 219);
      this.Controls.Add(this.tableLayoutPanel2);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(304, 161);
      this.Name = "RedbrickConfiguration";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Redbrick Configuration";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RedbrickConfiguration_FormClosing);
      this.Load += new System.EventHandler(this.RedbrickConfiguration_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel3.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.CheckBox chbDBEnabled;
    private System.Windows.Forms.CheckBox chbTestingMode;
    private System.Windows.Forms.ComboBox cbDept;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbRevLimit;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox cbDefaultMaterial;
  }
}