namespace Redbrick_Addin
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
      this.cbDepartment = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // cbDepartment
      // 
      this.cbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.cbDepartment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cbDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cbDepartment.FormattingEnabled = true;
      this.cbDepartment.Location = new System.Drawing.Point(0, 0);
      this.cbDepartment.Name = "cbDepartment";
      this.cbDepartment.Size = new System.Drawing.Size(153, 21);
      this.cbDepartment.TabIndex = 26;
      this.cbDepartment.DropDown += new System.EventHandler(this.cbDepartment_DropDown);
      this.cbDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbDepartment_KeyDown);
      // 
      // DepartmentSelector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.cbDepartment);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "DepartmentSelector";
      this.Size = new System.Drawing.Size(153, 24);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbDepartment;
    }
}
