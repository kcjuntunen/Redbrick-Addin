namespace Redbrick_Addin
{
    partial class CutlistHandler
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpCutlist = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbCutlist = new System.Windows.Forms.ComboBox();
            this.cbRev = new System.Windows.Forms.ComboBox();
            this.labItemNo = new System.Windows.Forms.Label();
            this.labRev = new System.Windows.Forms.Label();
            this.labDate = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.labCust = new System.Windows.Forms.Label();
            this.cbCustomer = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbL = new System.Windows.Forms.TextBox();
            this.tbW = new System.Windows.Forms.TextBox();
            this.tbH = new System.Windows.Forms.TextBox();
            this.labDescr = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOriginal = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRef = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbQty = new System.Windows.Forms.TextBox();
            this.tlpCutlist.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpCutlist
            // 
            this.tlpCutlist.ColumnCount = 1;
            this.tlpCutlist.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpCutlist.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tlpCutlist.Controls.Add(this.labDate, 0, 1);
            this.tlpCutlist.Controls.Add(this.dateTimePicker1, 0, 2);
            this.tlpCutlist.Controls.Add(this.labCust, 0, 3);
            this.tlpCutlist.Controls.Add(this.cbCustomer, 0, 4);
            this.tlpCutlist.Controls.Add(this.tableLayoutPanel2, 0, 7);
            this.tlpCutlist.Controls.Add(this.labDescr, 0, 5);
            this.tlpCutlist.Controls.Add(this.tbDescription, 0, 6);
            this.tlpCutlist.Controls.Add(this.tableLayoutPanel3, 0, 8);
            this.tlpCutlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCutlist.Location = new System.Drawing.Point(0, 0);
            this.tlpCutlist.Name = "tlpCutlist";
            this.tlpCutlist.RowCount = 9;
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpCutlist.Size = new System.Drawing.Size(258, 331);
            this.tlpCutlist.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.cbCutlist, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbRev, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labItemNo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labRev, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(252, 44);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // cbCutlist
            // 
            this.cbCutlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCutlist.FormattingEnabled = true;
            this.cbCutlist.Location = new System.Drawing.Point(3, 16);
            this.cbCutlist.Name = "cbCutlist";
            this.cbCutlist.Size = new System.Drawing.Size(157, 21);
            this.cbCutlist.TabIndex = 0;
            this.cbCutlist.SelectedIndexChanged += new System.EventHandler(this.cbCutlist_SelectedIndexChanged);
            // 
            // cbRev
            // 
            this.cbRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRev.FormattingEnabled = true;
            this.cbRev.Location = new System.Drawing.Point(166, 16);
            this.cbRev.Name = "cbRev";
            this.cbRev.Size = new System.Drawing.Size(83, 21);
            this.cbRev.TabIndex = 1;
            this.cbRev.Visible = false;
            // 
            // labItemNo
            // 
            this.labItemNo.AutoSize = true;
            this.labItemNo.Location = new System.Drawing.Point(3, 0);
            this.labItemNo.Name = "labItemNo";
            this.labItemNo.Size = new System.Drawing.Size(37, 13);
            this.labItemNo.TabIndex = 2;
            this.labItemNo.Text = "Item #";
            // 
            // labRev
            // 
            this.labRev.AutoSize = true;
            this.labRev.Location = new System.Drawing.Point(166, 0);
            this.labRev.Name = "labRev";
            this.labRev.Size = new System.Drawing.Size(27, 13);
            this.labRev.TabIndex = 3;
            this.labRev.Text = "Rev";
            this.labRev.Visible = false;
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(3, 50);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(30, 13);
            this.labDate.TabIndex = 4;
            this.labDate.Text = "Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePicker1.Location = new System.Drawing.Point(3, 66);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(252, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // labCust
            // 
            this.labCust.AutoSize = true;
            this.labCust.Location = new System.Drawing.Point(3, 89);
            this.labCust.Name = "labCust";
            this.labCust.Size = new System.Drawing.Size(51, 13);
            this.labCust.TabIndex = 6;
            this.labCust.Text = "Customer";
            // 
            // cbCustomer
            // 
            this.cbCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCustomer.FormattingEnabled = true;
            this.cbCustomer.Location = new System.Drawing.Point(3, 105);
            this.cbCustomer.Name = "cbCustomer";
            this.cbCustomer.Size = new System.Drawing.Size(252, 21);
            this.cbCustomer.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbL, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbW, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbH, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 171);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(252, 44);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Length";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(170, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Height";
            // 
            // tbL
            // 
            this.tbL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbL.Location = new System.Drawing.Point(3, 16);
            this.tbL.Name = "tbL";
            this.tbL.Size = new System.Drawing.Size(77, 20);
            this.tbL.TabIndex = 3;
            // 
            // tbW
            // 
            this.tbW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbW.Location = new System.Drawing.Point(86, 16);
            this.tbW.Name = "tbW";
            this.tbW.Size = new System.Drawing.Size(78, 20);
            this.tbW.TabIndex = 4;
            // 
            // tbH
            // 
            this.tbH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbH.Location = new System.Drawing.Point(170, 16);
            this.tbH.Name = "tbH";
            this.tbH.Size = new System.Drawing.Size(79, 20);
            this.tbH.TabIndex = 5;
            // 
            // labDescr
            // 
            this.labDescr.AutoSize = true;
            this.labDescr.Location = new System.Drawing.Point(3, 129);
            this.labDescr.Name = "labDescr";
            this.labDescr.Size = new System.Drawing.Size(60, 13);
            this.labDescr.TabIndex = 8;
            this.labDescr.Text = "Description";
            // 
            // tbDescription
            // 
            this.tbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDescription.Location = new System.Drawing.Point(3, 145);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(252, 20);
            this.tbDescription.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnOriginal, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnInsert, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbRef, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbQty, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 221);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(252, 107);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // btnOriginal
            // 
            this.btnOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOriginal.Location = new System.Drawing.Point(129, 58);
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(120, 46);
            this.btnOriginal.TabIndex = 0;
            this.btnOriginal.Text = global::Redbrick_Addin.Properties.Resources.MakeOriginalButtonText;
            this.btnOriginal.UseVisualStyleBackColor = true;
            this.btnOriginal.Click += new System.EventHandler(this.btnOriginal_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsert.Location = new System.Drawing.Point(3, 58);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(120, 46);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Text = global::Redbrick_Addin.Properties.Resources.InsertIntoCutlistButtonText;
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "DWG Reference";
            // 
            // tbRef
            // 
            this.tbRef.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRef.Location = new System.Drawing.Point(3, 23);
            this.tbRef.Name = "tbRef";
            this.tbRef.Size = new System.Drawing.Size(120, 20);
            this.tbRef.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "QTY";
            // 
            // tbQty
            // 
            this.tbQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbQty.Location = new System.Drawing.Point(129, 23);
            this.tbQty.Name = "tbQty";
            this.tbQty.Size = new System.Drawing.Size(120, 20);
            this.tbQty.TabIndex = 13;
            // 
            // CutlistHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpCutlist);
            this.Name = "CutlistHandler";
            this.Size = new System.Drawing.Size(258, 331);
            this.tlpCutlist.ResumeLayout(false);
            this.tlpCutlist.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpCutlist;
        private System.Windows.Forms.ComboBox cbCutlist;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cbRev;
        private System.Windows.Forms.Label labItemNo;
        private System.Windows.Forms.Label labRev;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labCust;
        private System.Windows.Forms.ComboBox cbCustomer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbL;
        private System.Windows.Forms.TextBox tbW;
        private System.Windows.Forms.TextBox tbH;
        private System.Windows.Forms.Label labDescr;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRef;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnOriginal;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbQty;
    }
}
