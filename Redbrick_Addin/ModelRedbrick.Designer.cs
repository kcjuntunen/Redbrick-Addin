namespace Redbrick_Addin
{
    partial class ModelRedbrick
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.gbOp = new System.Windows.Forms.GroupBox();
            this.gbMachProp = new System.Windows.Forms.GroupBox();
            this.gbGlobProp = new System.Windows.Forms.GroupBox();
            this.gbSpecProp = new System.Windows.Forms.GroupBox();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.gbOp, 0, 4);
            this.tlpMain.Controls.Add(this.gbMachProp, 0, 3);
            this.tlpMain.Controls.Add(this.gbGlobProp, 0, 2);
            this.tlpMain.Controls.Add(this.gbSpecProp, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 5;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.Size = new System.Drawing.Size(208, 256);
            this.tlpMain.TabIndex = 0;
            // 
            // gbOp
            // 
            this.gbOp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOp.Location = new System.Drawing.Point(3, 207);
            this.gbOp.Name = "gbOp";
            this.gbOp.Size = new System.Drawing.Size(202, 46);
            this.gbOp.TabIndex = 4;
            this.gbOp.TabStop = false;
            this.gbOp.Text = "Ops";
            // 
            // gbMachProp
            // 
            this.gbMachProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMachProp.Location = new System.Drawing.Point(3, 156);
            this.gbMachProp.Name = "gbMachProp";
            this.gbMachProp.Size = new System.Drawing.Size(202, 45);
            this.gbMachProp.TabIndex = 2;
            this.gbMachProp.TabStop = false;
            this.gbMachProp.Text = "Machine Properties";
            // 
            // gbGlobProp
            // 
            this.gbGlobProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGlobProp.Location = new System.Drawing.Point(3, 105);
            this.gbGlobProp.Name = "gbGlobProp";
            this.gbGlobProp.Size = new System.Drawing.Size(202, 45);
            this.gbGlobProp.TabIndex = 1;
            this.gbGlobProp.TabStop = false;
            this.gbGlobProp.Text = "Global Properties";
            // 
            // gbSpecProp
            // 
            this.gbSpecProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSpecProp.Location = new System.Drawing.Point(3, 54);
            this.gbSpecProp.Name = "gbSpecProp";
            this.gbSpecProp.Size = new System.Drawing.Size(202, 45);
            this.gbSpecProp.TabIndex = 0;
            this.gbSpecProp.TabStop = false;
            this.gbSpecProp.Text = "Configuration Specific";
            // 
            // ModelRedbrick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "ModelRedbrick";
            this.Size = new System.Drawing.Size(208, 256);
            this.tlpMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbOp;
        private System.Windows.Forms.GroupBox gbMachProp;
        private System.Windows.Forms.GroupBox gbGlobProp;
        private System.Windows.Forms.GroupBox gbSpecProp;
    }
}
