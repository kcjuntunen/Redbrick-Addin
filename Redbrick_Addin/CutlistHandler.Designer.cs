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
            this.cbCutlist = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbRev = new System.Windows.Forms.ComboBox();
            this.labItemNo = new System.Windows.Forms.Label();
            this.labRev = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labDate = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.labCust = new System.Windows.Forms.Label();
            this.cbCustomer = new System.Windows.Forms.ComboBox();
            this.labDescr = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tlpCutlist.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.tlpCutlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCutlist.Location = new System.Drawing.Point(0, 0);
            this.tlpCutlist.Name = "tlpCutlist";
            this.tlpCutlist.RowCount = 8;
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpCutlist.Size = new System.Drawing.Size(258, 371);
            this.tlpCutlist.TabIndex = 2;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(252, 39);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // cbRev
            // 
            this.cbRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRev.FormattingEnabled = true;
            this.cbRev.Location = new System.Drawing.Point(166, 16);
            this.cbRev.Name = "cbRev";
            this.cbRev.Size = new System.Drawing.Size(83, 21);
            this.cbRev.TabIndex = 1;
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
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox2, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox3, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 166);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(252, 202);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(3, 45);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(30, 13);
            this.labDate.TabIndex = 4;
            this.labDate.Text = "Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePicker1.Location = new System.Drawing.Point(3, 61);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(252, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // labCust
            // 
            this.labCust.AutoSize = true;
            this.labCust.Location = new System.Drawing.Point(3, 84);
            this.labCust.Name = "labCust";
            this.labCust.Size = new System.Drawing.Size(51, 13);
            this.labCust.TabIndex = 6;
            this.labCust.Text = "Customer";
            // 
            // cbCustomer
            // 
            this.cbCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCustomer.FormattingEnabled = true;
            this.cbCustomer.Location = new System.Drawing.Point(3, 100);
            this.cbCustomer.Name = "cbCustomer";
            this.cbCustomer.Size = new System.Drawing.Size(252, 21);
            this.cbCustomer.TabIndex = 7;
            // 
            // labDescr
            // 
            this.labDescr.AutoSize = true;
            this.labDescr.Location = new System.Drawing.Point(3, 124);
            this.labDescr.Name = "labDescr";
            this.labDescr.Size = new System.Drawing.Size(60, 13);
            this.labDescr.TabIndex = 8;
            this.labDescr.Text = "Description";
            // 
            // tbDescription
            // 
            this.tbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDescription.Location = new System.Drawing.Point(3, 140);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(252, 20);
            this.tbDescription.TabIndex = 9;
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
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(77, 20);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(86, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(78, 20);
            this.textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(170, 16);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(79, 20);
            this.textBox3.TabIndex = 5;
            // 
            // CutlistHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpCutlist);
            this.Name = "CutlistHandler";
            this.Size = new System.Drawing.Size(258, 371);
            this.tlpCutlist.ResumeLayout(false);
            this.tlpCutlist.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label labDescr;
        private System.Windows.Forms.TextBox tbDescription;
    }
}
