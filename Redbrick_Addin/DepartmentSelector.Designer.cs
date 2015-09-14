namespace redbrick.csproj
{
    partial class DepartmentSelector
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
            this.rbDeptWood = new System.Windows.Forms.RadioButton();
            this.rbDeptMetal = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rbDeptWood
            // 
            this.rbDeptWood.AutoSize = true;
            this.rbDeptWood.Location = new System.Drawing.Point(3, 4);
            this.rbDeptWood.Name = "rbDeptWood";
            this.rbDeptWood.Size = new System.Drawing.Size(54, 17);
            this.rbDeptWood.TabIndex = 0;
            this.rbDeptWood.TabStop = true;
            this.rbDeptWood.Text = "Wood";
            this.rbDeptWood.UseVisualStyleBackColor = true;
            this.rbDeptWood.CheckedChanged += new System.EventHandler(this.rbDeptWood_CheckedChanged);
            // 
            // rbDeptMetal
            // 
            this.rbDeptMetal.AutoSize = true;
            this.rbDeptMetal.Location = new System.Drawing.Point(63, 4);
            this.rbDeptMetal.Name = "rbDeptMetal";
            this.rbDeptMetal.Size = new System.Drawing.Size(51, 17);
            this.rbDeptMetal.TabIndex = 1;
            this.rbDeptMetal.TabStop = true;
            this.rbDeptMetal.Text = "Metal";
            this.rbDeptMetal.UseVisualStyleBackColor = true;
            // 
            // DepartmentSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbDeptMetal);
            this.Controls.Add(this.rbDeptWood);
            this.Name = "DepartmentSelector";
            this.Size = new System.Drawing.Size(115, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbDeptWood;
        private System.Windows.Forms.RadioButton rbDeptMetal;
    }
}
