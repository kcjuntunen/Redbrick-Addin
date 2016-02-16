namespace Redbrick_Addin {
  partial class MachineProgramManager {
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
      this.lbPri1 = new System.Windows.Forms.ListBox();
      this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
      this.lbPri2 = new System.Windows.Forms.ListBox();
      this.lbPri3 = new System.Windows.Forms.ListBox();
      this.lbAssocPart = new System.Windows.Forms.ListBox();
      this.lbAssocCutl = new System.Windows.Forms.ListBox();
      this.labAssocPrts = new System.Windows.Forms.Label();
      this.labAssocCtlst = new System.Windows.Forms.Label();
      this.labPri1 = new System.Windows.Forms.Label();
      this.labPri2 = new System.Windows.Forms.Label();
      this.labPri3 = new System.Windows.Forms.Label();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCncl = new System.Windows.Forms.Button();
      this.tlpMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // lbPri1
      // 
      this.lbPri1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbPri1.FormattingEnabled = true;
      this.lbPri1.Location = new System.Drawing.Point(149, 43);
      this.lbPri1.Name = "lbPri1";
      this.tlpMain.SetRowSpan(this.lbPri1, 3);
      this.lbPri1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lbPri1.Size = new System.Drawing.Size(140, 238);
      this.lbPri1.TabIndex = 1;
      this.lbPri1.SelectedIndexChanged += new System.EventHandler(this.lbPri1_SelectedIndexChanged);
      this.lbPri1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbPri1_MouseDown);
      // 
      // tlpMain
      // 
      this.tlpMain.ColumnCount = 4;
      this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tlpMain.Controls.Add(this.lbPri1, 1, 1);
      this.tlpMain.Controls.Add(this.lbPri2, 2, 1);
      this.tlpMain.Controls.Add(this.lbPri3, 3, 1);
      this.tlpMain.Controls.Add(this.lbAssocPart, 0, 1);
      this.tlpMain.Controls.Add(this.lbAssocCutl, 0, 3);
      this.tlpMain.Controls.Add(this.labAssocPrts, 0, 0);
      this.tlpMain.Controls.Add(this.labAssocCtlst, 0, 2);
      this.tlpMain.Controls.Add(this.labPri1, 1, 0);
      this.tlpMain.Controls.Add(this.labPri2, 2, 0);
      this.tlpMain.Controls.Add(this.labPri3, 3, 0);
      this.tlpMain.Controls.Add(this.btnOK, 0, 4);
      this.tlpMain.Controls.Add(this.btnCncl, 1, 4);
      this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tlpMain.Location = new System.Drawing.Point(0, 0);
      this.tlpMain.Name = "tlpMain";
      this.tlpMain.RowCount = 5;
      this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
      this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
      this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tlpMain.Size = new System.Drawing.Size(584, 320);
      this.tlpMain.TabIndex = 1;
      // 
      // lbPri2
      // 
      this.lbPri2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbPri2.FormattingEnabled = true;
      this.lbPri2.Location = new System.Drawing.Point(295, 43);
      this.lbPri2.Name = "lbPri2";
      this.tlpMain.SetRowSpan(this.lbPri2, 3);
      this.lbPri2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lbPri2.Size = new System.Drawing.Size(140, 238);
      this.lbPri2.TabIndex = 2;
      this.lbPri2.SelectedIndexChanged += new System.EventHandler(this.lbPri2_SelectedIndexChanged);
      this.lbPri2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbPri2_MouseDown);
      // 
      // lbPri3
      // 
      this.lbPri3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbPri3.FormattingEnabled = true;
      this.lbPri3.Location = new System.Drawing.Point(441, 43);
      this.lbPri3.Name = "lbPri3";
      this.tlpMain.SetRowSpan(this.lbPri3, 3);
      this.lbPri3.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lbPri3.Size = new System.Drawing.Size(140, 238);
      this.lbPri3.TabIndex = 3;
      this.lbPri3.SelectedIndexChanged += new System.EventHandler(this.lbPri3_SelectedIndexChanged);
      this.lbPri3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbPri3_MouseDown);
      // 
      // lbAssocPart
      // 
      this.lbAssocPart.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbAssocPart.FormattingEnabled = true;
      this.lbAssocPart.Location = new System.Drawing.Point(3, 43);
      this.lbAssocPart.Name = "lbAssocPart";
      this.lbAssocPart.Size = new System.Drawing.Size(140, 96);
      this.lbAssocPart.TabIndex = 4;
      // 
      // lbAssocCutl
      // 
      this.lbAssocCutl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbAssocCutl.FormattingEnabled = true;
      this.lbAssocCutl.Location = new System.Drawing.Point(3, 185);
      this.lbAssocCutl.Name = "lbAssocCutl";
      this.lbAssocCutl.Size = new System.Drawing.Size(140, 96);
      this.lbAssocCutl.TabIndex = 5;
      // 
      // labAssocPrts
      // 
      this.labAssocPrts.AutoSize = true;
      this.labAssocPrts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labAssocPrts.Location = new System.Drawing.Point(3, 0);
      this.labAssocPrts.Name = "labAssocPrts";
      this.labAssocPrts.Size = new System.Drawing.Size(140, 40);
      this.labAssocPrts.TabIndex = 5;
      this.labAssocPrts.Text = "Associated Parts";
      this.labAssocPrts.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // labAssocCtlst
      // 
      this.labAssocCtlst.AutoSize = true;
      this.labAssocCtlst.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labAssocCtlst.Location = new System.Drawing.Point(3, 142);
      this.labAssocCtlst.Name = "labAssocCtlst";
      this.labAssocCtlst.Size = new System.Drawing.Size(140, 40);
      this.labAssocCtlst.TabIndex = 6;
      this.labAssocCtlst.Text = "Associated Cutlists";
      this.labAssocCtlst.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // labPri1
      // 
      this.labPri1.AutoSize = true;
      this.labPri1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labPri1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labPri1.Location = new System.Drawing.Point(149, 0);
      this.labPri1.Name = "labPri1";
      this.labPri1.Size = new System.Drawing.Size(140, 40);
      this.labPri1.TabIndex = 7;
      this.labPri1.Text = "1";
      this.labPri1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // labPri2
      // 
      this.labPri2.AutoSize = true;
      this.labPri2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labPri2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labPri2.Location = new System.Drawing.Point(295, 0);
      this.labPri2.Name = "labPri2";
      this.labPri2.Size = new System.Drawing.Size(140, 40);
      this.labPri2.TabIndex = 8;
      this.labPri2.Text = "2";
      this.labPri2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // labPri3
      // 
      this.labPri3.AutoSize = true;
      this.labPri3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labPri3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labPri3.Location = new System.Drawing.Point(441, 0);
      this.labPri3.Name = "labPri3";
      this.labPri3.Size = new System.Drawing.Size(140, 40);
      this.labPri3.TabIndex = 9;
      this.labPri3.Text = "3";
      this.labPri3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      // 
      // btnOK
      // 
      this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnOK.Location = new System.Drawing.Point(3, 287);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(140, 30);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCncl
      // 
      this.btnCncl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnCncl.Location = new System.Drawing.Point(149, 287);
      this.btnCncl.Name = "btnCncl";
      this.btnCncl.Size = new System.Drawing.Size(140, 30);
      this.btnCncl.TabIndex = 7;
      this.btnCncl.Text = "Cancel";
      this.btnCncl.UseVisualStyleBackColor = true;
      this.btnCncl.Click += new System.EventHandler(this.btnCncl_Click);
      // 
      // MachineProgramManager
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(584, 320);
      this.Controls.Add(this.tlpMain);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "MachineProgramManager";
      this.Text = "Machine Priority Manager";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MachineProgramManager_FormClosing);
      this.Load += new System.EventHandler(this.MachineProgramManager_Load);
      this.tlpMain.ResumeLayout(false);
      this.tlpMain.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox lbPri1;
    private System.Windows.Forms.TableLayoutPanel tlpMain;
    private System.Windows.Forms.ListBox lbPri2;
    private System.Windows.Forms.ListBox lbPri3;
    private System.Windows.Forms.ListBox lbAssocPart;
    private System.Windows.Forms.ListBox lbAssocCutl;
    private System.Windows.Forms.Label labAssocPrts;
    private System.Windows.Forms.Label labAssocCtlst;
    private System.Windows.Forms.Label labPri1;
    private System.Windows.Forms.Label labPri2;
    private System.Windows.Forms.Label labPri3;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCncl;
  }
}