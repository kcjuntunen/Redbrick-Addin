namespace redbrick.csproj
{
    partial class tvRevs
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
            this.tvRevisions = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewRev = new System.Windows.Forms.Button();
            this.btnEditRev = new System.Windows.Forms.Button();
            this.btnDelRev = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvRevisions
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tvRevisions, 3);
            this.tvRevisions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRevisions.FullRowSelect = true;
            this.tvRevisions.Location = new System.Drawing.Point(3, 3);
            this.tvRevisions.Name = "tvRevisions";
            this.tvRevisions.Size = new System.Drawing.Size(291, 91);
            this.tvRevisions.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.tvRevisions, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNewRev, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEditRev, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDelRev, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(297, 127);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnNewRev
            // 
            this.btnNewRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewRev.Location = new System.Drawing.Point(3, 100);
            this.btnNewRev.Name = "btnNewRev";
            this.btnNewRev.Size = new System.Drawing.Size(93, 24);
            this.btnNewRev.TabIndex = 1;
            this.btnNewRev.Text = "New...";
            this.btnNewRev.UseVisualStyleBackColor = true;
            this.btnNewRev.Click += new System.EventHandler(this.btnNewRev_Click);
            // 
            // btnEditRev
            // 
            this.btnEditRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditRev.Location = new System.Drawing.Point(102, 100);
            this.btnEditRev.Name = "btnEditRev";
            this.btnEditRev.Size = new System.Drawing.Size(93, 24);
            this.btnEditRev.TabIndex = 2;
            this.btnEditRev.Text = "Edit...";
            this.btnEditRev.UseVisualStyleBackColor = true;
            this.btnEditRev.Click += new System.EventHandler(this.btnEditRev_Click);
            // 
            // btnDelRev
            // 
            this.btnDelRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelRev.Location = new System.Drawing.Point(201, 100);
            this.btnDelRev.Name = "btnDelRev";
            this.btnDelRev.Size = new System.Drawing.Size(93, 24);
            this.btnDelRev.TabIndex = 3;
            this.btnDelRev.Text = "Delete...";
            this.btnDelRev.UseVisualStyleBackColor = true;
            this.btnDelRev.Click += new System.EventHandler(this.btnDelRev_Click);
            // 
            // tvRevs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "tvRevs";
            this.Size = new System.Drawing.Size(297, 127);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvRevisions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNewRev;
        private System.Windows.Forms.Button btnEditRev;
        private System.Windows.Forms.Button btnDelRev;
    }
}
