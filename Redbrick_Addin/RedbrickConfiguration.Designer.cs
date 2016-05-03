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
      this.label3 = new System.Windows.Forms.Label();
      this.cbDefaultMaterial = new System.Windows.Forms.ComboBox();
      this.chbFlameWar = new System.Windows.Forms.CheckBox();
      this.chbWarnings = new System.Windows.Forms.CheckBox();
      this.chbSounds = new System.Windows.Forms.CheckBox();
      this.chbIdiotLight = new System.Windows.Forms.CheckBox();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.chbOnlyActive = new System.Windows.Forms.CheckBox();
      this.chbOnlyActiveCustomers = new System.Windows.Forms.CheckBox();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.chbDBEnabled, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.chbTestingMode, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.cbDept, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.label1, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.cbRevLimit, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.cbDefaultMaterial, 0, 5);
      this.tableLayoutPanel1.Controls.Add(this.chbFlameWar, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.chbWarnings, 1, 4);
      this.tableLayoutPanel1.Controls.Add(this.chbSounds, 1, 5);
      this.tableLayoutPanel1.Controls.Add(this.chbIdiotLight, 1, 6);
      this.tableLayoutPanel1.Controls.Add(this.chbOnlyActive, 0, 6);
      this.tableLayoutPanel1.Controls.Add(this.chbOnlyActiveCustomers, 0, 7);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 8;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(412, 206);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // chbDBEnabled
      // 
      this.chbDBEnabled.AutoSize = true;
      this.chbDBEnabled.Location = new System.Drawing.Point(3, 3);
      this.chbDBEnabled.Name = "chbDBEnabled";
      this.chbDBEnabled.Size = new System.Drawing.Size(154, 17);
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
      this.chbTestingMode.Size = new System.Drawing.Size(96, 17);
      this.chbTestingMode.TabIndex = 2;
      this.chbTestingMode.Text = "Testing Mode";
      this.chbTestingMode.UseVisualStyleBackColor = true;
      this.chbTestingMode.CheckedChanged += new System.EventHandler(this.chbTestingMode_CheckedChanged);
      // 
      // cbDept
      // 
      this.cbDept.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbDept.FormattingEnabled = true;
      this.cbDept.Location = new System.Drawing.Point(209, 62);
      this.cbDept.Name = "cbDept";
      this.cbDept.Size = new System.Drawing.Size(200, 21);
      this.cbDept.TabIndex = 3;
      this.cbDept.SelectedIndexChanged += new System.EventHandler(this.cbDept_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(209, 46);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(94, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "User Department";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 46);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(54, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "REV Limit";
      // 
      // cbRevLimit
      // 
      this.cbRevLimit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbRevLimit.FormattingEnabled = true;
      this.cbRevLimit.Location = new System.Drawing.Point(3, 62);
      this.cbRevLimit.Name = "cbRevLimit";
      this.cbRevLimit.Size = new System.Drawing.Size(200, 21);
      this.cbRevLimit.TabIndex = 7;
      this.cbRevLimit.SelectedIndexChanged += new System.EventHandler(this.cbRevLimit_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 86);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(90, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Default Material";
      // 
      // cbDefaultMaterial
      // 
      this.cbDefaultMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbDefaultMaterial.FormattingEnabled = true;
      this.cbDefaultMaterial.Location = new System.Drawing.Point(3, 112);
      this.cbDefaultMaterial.Name = "cbDefaultMaterial";
      this.cbDefaultMaterial.Size = new System.Drawing.Size(200, 21);
      this.cbDefaultMaterial.TabIndex = 9;
      this.cbDefaultMaterial.SelectedIndexChanged += new System.EventHandler(this.cbDefaultMaterial_SelectedIndexChanged);
      // 
      // chbFlameWar
      // 
      this.chbFlameWar.AutoSize = true;
      this.chbFlameWar.Checked = true;
      this.chbFlameWar.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chbFlameWar.Location = new System.Drawing.Point(209, 3);
      this.chbFlameWar.Name = "chbFlameWar";
      this.chbFlameWar.Size = new System.Drawing.Size(162, 17);
      this.chbFlameWar.TabIndex = 10;
      this.chbFlameWar.Text = "Filter comments to all caps";
      this.chbFlameWar.UseVisualStyleBackColor = true;
      this.chbFlameWar.CheckedChanged += new System.EventHandler(this.chbFlameWar_CheckedChanged);
      // 
      // chbWarnings
      // 
      this.chbWarnings.AutoSize = true;
      this.chbWarnings.Checked = true;
      this.chbWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chbWarnings.Location = new System.Drawing.Point(209, 89);
      this.chbWarnings.Name = "chbWarnings";
      this.chbWarnings.Size = new System.Drawing.Size(76, 17);
      this.chbWarnings.TabIndex = 11;
      this.chbWarnings.Text = "Warnings";
      this.chbWarnings.UseVisualStyleBackColor = true;
      this.chbWarnings.CheckedChanged += new System.EventHandler(this.chbWarnings_CheckedChanged);
      // 
      // chbSounds
      // 
      this.chbSounds.AutoSize = true;
      this.chbSounds.Checked = true;
      this.chbSounds.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chbSounds.Location = new System.Drawing.Point(209, 112);
      this.chbSounds.Name = "chbSounds";
      this.chbSounds.Size = new System.Drawing.Size(65, 17);
      this.chbSounds.TabIndex = 12;
      this.chbSounds.Text = "Sounds";
      this.chbSounds.UseVisualStyleBackColor = true;
      this.chbSounds.CheckedChanged += new System.EventHandler(this.chbSounds_CheckedChanged);
      this.chbSounds.Click += new System.EventHandler(this.chbSounds_Click);
      // 
      // chbIdiotLight
      // 
      this.chbIdiotLight.AutoSize = true;
      this.chbIdiotLight.Location = new System.Drawing.Point(209, 139);
      this.chbIdiotLight.Name = "chbIdiotLight";
      this.chbIdiotLight.Size = new System.Drawing.Size(135, 17);
      this.chbIdiotLight.TabIndex = 13;
      this.chbIdiotLight.Text = "Green check warning";
      this.chbIdiotLight.UseVisualStyleBackColor = true;
      this.chbIdiotLight.Visible = false;
      this.chbIdiotLight.CheckedChanged += new System.EventHandler(this.chbIdiotLight_CheckedChanged);
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
      this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 12);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(418, 246);
      this.tableLayoutPanel2.TabIndex = 1;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.Controls.Add(this.btnCancel, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.btnOK, 1, 0);
      this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 215);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(412, 28);
      this.tableLayoutPanel3.TabIndex = 1;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(3, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.Location = new System.Drawing.Point(84, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(325, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // chbOnlyActive
      // 
      this.chbOnlyActive.AutoSize = true;
      this.chbOnlyActive.Location = new System.Drawing.Point(3, 139);
      this.chbOnlyActive.Name = "chbOnlyActive";
      this.chbOnlyActive.Size = new System.Drawing.Size(156, 17);
      this.chbOnlyActive.TabIndex = 14;
      this.chbOnlyActive.Text = "Only show active authors";
      this.chbOnlyActive.UseVisualStyleBackColor = true;
      this.chbOnlyActive.CheckedChanged += new System.EventHandler(this.chbOnlyActive_CheckedChanged);
      // 
      // chbOnlyActiveCustomers
      // 
      this.chbOnlyActiveCustomers.AutoSize = true;
      this.chbOnlyActiveCustomers.Location = new System.Drawing.Point(3, 162);
      this.chbOnlyActiveCustomers.Name = "chbOnlyActiveCustomers";
      this.chbOnlyActiveCustomers.Size = new System.Drawing.Size(168, 17);
      this.chbOnlyActiveCustomers.TabIndex = 15;
      this.chbOnlyActiveCustomers.Text = "Only show active customers";
      this.chbOnlyActiveCustomers.UseVisualStyleBackColor = true;
      this.chbOnlyActiveCustomers.CheckedChanged += new System.EventHandler(this.chbOnlyActiveCustomers_CheckedChanged);
      // 
      // RedbrickConfiguration
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(442, 270);
      this.Controls.Add(this.tableLayoutPanel2);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(450, 300);
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
    private System.Windows.Forms.CheckBox chbFlameWar;
    private System.Windows.Forms.CheckBox chbWarnings;
    private System.Windows.Forms.CheckBox chbSounds;
    private System.Windows.Forms.CheckBox chbIdiotLight;
    private System.Windows.Forms.CheckBox chbOnlyActive;
    private System.Windows.Forms.CheckBox chbOnlyActiveCustomers;
  }
}