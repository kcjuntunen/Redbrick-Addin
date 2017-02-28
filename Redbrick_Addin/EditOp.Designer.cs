namespace Redbrick_Addin {
  partial class EditOp {
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
      this.components = new System.ComponentModel.Container();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.fRIENDLYCUTOPSBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.eNGINEERINGDataSet = new Redbrick_Addin.ENGINEERINGDataSet();
      this.label4 = new System.Windows.Forms.Label();
      this.comboBox2 = new System.Windows.Forms.ComboBox();
      this.cUTPARTTYPESBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.comboBox3 = new System.Windows.Forms.ComboBox();
      this.cUTOPSMETHODSBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.label5 = new System.Windows.Forms.Label();
      this.cUT_OPSBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.cUTOPSBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.cUT_OPSTableAdapter = new Redbrick_Addin.ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();
      this.tableAdapterManager = new Redbrick_Addin.ENGINEERINGDataSetTableAdapters.TableAdapterManager();
      this.cUT_PART_TYPESTableAdapter = new Redbrick_Addin.ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
      this.fRIENDLY_CUT_OPSTableAdapter = new Redbrick_Addin.ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter();
      this.cUT_OPS_METHODSTableAdapter = new Redbrick_Addin.ENGINEERINGDataSetTableAdapters.CUT_OPS_METHODSTableAdapter();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fRIENDLYCUTOPSBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSMETHODSBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUT_OPSBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(60, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Operation";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(91, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Setup Time (min)";
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.textBox2, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.comboBox1, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.label4, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.comboBox2, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.button1, 0, 6);
      this.tableLayoutPanel1.Controls.Add(this.button2, 1, 6);
      this.tableLayoutPanel1.Controls.Add(this.comboBox3, 0, 5);
      this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 7;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(309, 156);
      this.tableLayoutPanel1.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(157, 41);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(82, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Run Time (min)";
      // 
      // textBox1
      // 
      this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox1.Location = new System.Drawing.Point(3, 59);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(148, 22);
      this.textBox1.TabIndex = 4;
      // 
      // textBox2
      // 
      this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox2.Location = new System.Drawing.Point(157, 59);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(149, 22);
      this.textBox2.TabIndex = 5;
      // 
      // comboBox1
      // 
      this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBox1.DataSource = this.fRIENDLYCUTOPSBindingSource;
      this.comboBox1.DisplayMember = "FRIENDLYDESCR";
      this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(3, 18);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(148, 21);
      this.comboBox1.TabIndex = 6;
      this.comboBox1.ValueMember = "OPID";
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
      // 
      // fRIENDLYCUTOPSBindingSource
      // 
      this.fRIENDLYCUTOPSBindingSource.DataMember = "FRIENDLY_CUT_OPS";
      this.fRIENDLYCUTOPSBindingSource.DataSource = this.eNGINEERINGDataSet;
      this.fRIENDLYCUTOPSBindingSource.Filter = "TYPEID = 1";
      // 
      // eNGINEERINGDataSet
      // 
      this.eNGINEERINGDataSet.DataSetName = "ENGINEERINGDataSet";
      this.eNGINEERINGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(157, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(86, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Operation Type";
      // 
      // comboBox2
      // 
      this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBox2.DataSource = this.cUTPARTTYPESBindingSource;
      this.comboBox2.DisplayMember = "TYPEDESC";
      this.comboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new System.Drawing.Point(157, 18);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new System.Drawing.Size(149, 21);
      this.comboBox2.TabIndex = 8;
      this.comboBox2.ValueMember = "TYPEID";
      this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
      this.comboBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
      // 
      // cUTPARTTYPESBindingSource
      // 
      this.cUTPARTTYPESBindingSource.DataMember = "CUT_PART_TYPES";
      this.cUTPARTTYPESBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(3, 128);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(148, 24);
      this.button1.TabIndex = 9;
      this.button1.Text = "Cancel";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(157, 128);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(149, 24);
      this.button2.TabIndex = 10;
      this.button2.Text = "OK";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // comboBox3
      // 
      this.tableLayoutPanel1.SetColumnSpan(this.comboBox3, 2);
      this.comboBox3.DataSource = this.cUTOPSMETHODSBindingSource;
      this.comboBox3.DisplayMember = "METHNAME";
      this.comboBox3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.comboBox3.Enabled = false;
      this.comboBox3.FormattingEnabled = true;
      this.comboBox3.Location = new System.Drawing.Point(3, 98);
      this.comboBox3.Name = "comboBox3";
      this.comboBox3.Size = new System.Drawing.Size(303, 21);
      this.comboBox3.TabIndex = 11;
      this.comboBox3.ValueMember = "METHID";
      // 
      // cUTOPSMETHODSBindingSource
      // 
      this.cUTOPSMETHODSBindingSource.DataMember = "CUT_OPS_METHODS";
      this.cUTOPSMETHODSBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 82);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(48, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Method";
      // 
      // cUT_OPSBindingSource
      // 
      this.cUT_OPSBindingSource.DataMember = "CUT_OPS";
      this.cUT_OPSBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // cUTOPSBindingSource
      // 
      this.cUTOPSBindingSource.DataMember = "CUT_OPS";
      this.cUTOPSBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // cUT_OPSTableAdapter
      // 
      this.cUT_OPSTableAdapter.ClearBeforeFill = true;
      // 
      // tableAdapterManager
      // 
      this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
      this.tableAdapterManager.CUT_CUTLIST_PARTSTableAdapter = null;
      this.tableAdapterManager.CUT_CUTLISTS_TIMETableAdapter = null;
      this.tableAdapterManager.CUT_OPS_METHODSTableAdapter = null;
      this.tableAdapterManager.CUT_OPS_TYPESTableAdapter = null;
      this.tableAdapterManager.CUT_OPSTableAdapter = this.cUT_OPSTableAdapter;
      this.tableAdapterManager.CUT_PART_OPSTableAdapter = null;
      this.tableAdapterManager.CUT_PART_TYPESTableAdapter = this.cUT_PART_TYPESTableAdapter;
      this.tableAdapterManager.OpDataTableAdapter = null;
      this.tableAdapterManager.UpdateOrder = Redbrick_Addin.ENGINEERINGDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
      // 
      // cUT_PART_TYPESTableAdapter
      // 
      this.cUT_PART_TYPESTableAdapter.ClearBeforeFill = true;
      // 
      // fRIENDLY_CUT_OPSTableAdapter
      // 
      this.fRIENDLY_CUT_OPSTableAdapter.ClearBeforeFill = true;
      // 
      // cUT_OPS_METHODSTableAdapter
      // 
      this.cUT_OPS_METHODSTableAdapter.ClearBeforeFill = true;
      // 
      // EditOp
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(309, 156);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "EditOp";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "EditOp";
      this.Load += new System.EventHandler(this.EditOp_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fRIENDLYCUTOPSBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSMETHODSBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUT_OPSBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private ENGINEERINGDataSet eNGINEERINGDataSet;
    private System.Windows.Forms.BindingSource cUT_OPSBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cUT_OPSTableAdapter;
    private ENGINEERINGDataSetTableAdapters.TableAdapterManager tableAdapterManager;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.BindingSource cUTOPSBindingSource;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox comboBox2;
    private ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cUT_PART_TYPESTableAdapter;
    private System.Windows.Forms.BindingSource cUTPARTTYPESBindingSource;
    private System.Windows.Forms.BindingSource fRIENDLYCUTOPSBindingSource;
    private ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter fRIENDLY_CUT_OPSTableAdapter;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.ComboBox comboBox3;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.BindingSource cUTOPSMETHODSBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_OPS_METHODSTableAdapter cUT_OPS_METHODSTableAdapter;
  }
}