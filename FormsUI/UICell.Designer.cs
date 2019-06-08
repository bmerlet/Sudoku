namespace FormsUI
{
    partial class UICell
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
            this.labelPossibles = new System.Windows.Forms.Label();
            this.labelNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPossibles
            // 
            this.labelPossibles.AutoSize = true;
            this.labelPossibles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPossibles.Location = new System.Drawing.Point(0, 0);
            this.labelPossibles.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.labelPossibles.Name = "labelPossibles";
            this.labelPossibles.Size = new System.Drawing.Size(56, 18);
            this.labelPossibles.TabIndex = 1;
            this.labelPossibles.Text = "123456";
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.BackColor = System.Drawing.Color.Transparent;
            this.labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumber.Location = new System.Drawing.Point(0, 0);
            this.labelNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Padding = new System.Windows.Forms.Padding(12, 12, 0, 0);
            this.labelNumber.Size = new System.Drawing.Size(47, 49);
            this.labelNumber.TabIndex = 0;
            this.labelNumber.Text = "8";
            // 
            // UICell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPossibles);
            this.Controls.Add(this.labelNumber);
            this.Name = "UICell";
            this.Size = new System.Drawing.Size(350, 278);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelPossibles;
        private System.Windows.Forms.Label labelNumber;
    }
}
