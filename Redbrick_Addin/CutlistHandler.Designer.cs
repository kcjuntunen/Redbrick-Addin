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
            this.lbCutlistBox = new System.Windows.Forms.Label();
            this.tlpCutlist.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpCutlist
            // 
            this.tlpCutlist.ColumnCount = 1;
            this.tlpCutlist.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCutlist.Controls.Add(this.cbCutlist, 0, 1);
            this.tlpCutlist.Controls.Add(this.lbCutlistBox, 0, 0);
            this.tlpCutlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCutlist.Location = new System.Drawing.Point(0, 0);
            this.tlpCutlist.Name = "tlpCutlist";
            this.tlpCutlist.RowCount = 2;
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCutlist.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCutlist.Size = new System.Drawing.Size(258, 56);
            this.tlpCutlist.TabIndex = 2;
            // 
            // cbCutlist
            // 
            this.cbCutlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCutlist.FormattingEnabled = true;
            this.cbCutlist.Location = new System.Drawing.Point(3, 31);
            this.cbCutlist.Name = "cbCutlist";
            this.cbCutlist.Size = new System.Drawing.Size(252, 21);
            this.cbCutlist.TabIndex = 0;
            this.cbCutlist.SelectedIndexChanged += new System.EventHandler(this.cbCutlist_SelectedIndexChanged);
            // 
            // lbCutlistBox
            // 
            this.lbCutlistBox.AutoSize = true;
            this.lbCutlistBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCutlistBox.Location = new System.Drawing.Point(3, 0);
            this.lbCutlistBox.Name = "lbCutlistBox";
            this.lbCutlistBox.Size = new System.Drawing.Size(252, 28);
            this.lbCutlistBox.TabIndex = 1;
            this.lbCutlistBox.Text = "Cutlist";
            // 
            // CutlistHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpCutlist);
            this.Name = "CutlistHandler";
            this.Size = new System.Drawing.Size(258, 56);
            this.tlpCutlist.ResumeLayout(false);
            this.tlpCutlist.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpCutlist;
        private System.Windows.Forms.ComboBox cbCutlist;
        private System.Windows.Forms.Label lbCutlistBox;
    }
}
