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
            this.gbMachProp = new System.Windows.Forms.GroupBox();
            this.gbGlobProp = new System.Windows.Forms.GroupBox();
            this.gbSpecProp = new System.Windows.Forms.GroupBox();
            this.gbOp = new System.Windows.Forms.GroupBox();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoScroll = true;
            this.tlpMain.AutoSize = true;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.gbMachProp, 0, 2);
            this.tlpMain.Controls.Add(this.gbGlobProp, 0, 1);
            this.tlpMain.Controls.Add(this.gbSpecProp, 0, 0);
            this.tlpMain.Controls.Add(this.gbOp, 0, 4);
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 5;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 370F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpMain.Size = new System.Drawing.Size(222, 1150);
            this.tlpMain.TabIndex = 0;
            // 
            // gbMachProp
            // 
            this.gbMachProp.AutoSize = true;
            this.gbMachProp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbMachProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMachProp.Location = new System.Drawing.Point(3, 653);
            this.gbMachProp.Name = "gbMachProp";
            this.gbMachProp.Size = new System.Drawing.Size(216, 194);
            this.gbMachProp.TabIndex = 2;
            this.gbMachProp.TabStop = false;
            this.gbMachProp.Text = "Machine Properties";
            // 
            // gbGlobProp
            // 
            this.gbGlobProp.AutoSize = true;
            this.gbGlobProp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbGlobProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGlobProp.Location = new System.Drawing.Point(3, 283);
            this.gbGlobProp.Name = "gbGlobProp";
            this.gbGlobProp.Size = new System.Drawing.Size(216, 364);
            this.gbGlobProp.TabIndex = 1;
            this.gbGlobProp.TabStop = false;
            this.gbGlobProp.Text = "Global Properties";
            // 
            // gbSpecProp
            // 
            this.gbSpecProp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbSpecProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSpecProp.Location = new System.Drawing.Point(3, 3);
            this.gbSpecProp.Name = "gbSpecProp";
            this.gbSpecProp.Size = new System.Drawing.Size(216, 274);
            this.gbSpecProp.TabIndex = 0;
            this.gbSpecProp.TabStop = false;
            this.gbSpecProp.Text = "Configuration Specific";
            // 
            // gbOp
            // 
            this.gbOp.AutoSize = true;
            this.gbOp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbOp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOp.Location = new System.Drawing.Point(3, 853);
            this.gbOp.Name = "gbOp";
            this.gbOp.Size = new System.Drawing.Size(216, 294);
            this.gbOp.TabIndex = 4;
            this.gbOp.TabStop = false;
            this.gbOp.Text = "Ops";
            // 
            // ModelRedbrick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tlpMain);
            this.Name = "ModelRedbrick";
            this.Size = new System.Drawing.Size(1160, 668);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbOp;
        private System.Windows.Forms.GroupBox gbMachProp;
        private System.Windows.Forms.GroupBox gbGlobProp;
        private System.Windows.Forms.GroupBox gbSpecProp;
    }
}
