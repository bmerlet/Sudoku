namespace FormsUI
{
    partial class NewGameDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonVeryHard = new System.Windows.Forms.Button();
            this.buttonHard = new System.Windows.Forms.Button();
            this.buttonMedium = new System.Windows.Forms.Button();
            this.buttonEasy = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonVeryHard, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonHard, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonMedium, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonEasy, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(129, 215);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(27, 182);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonVeryHard
            // 
            this.buttonVeryHard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonVeryHard.AutoSize = true;
            this.buttonVeryHard.Location = new System.Drawing.Point(27, 139);
            this.buttonVeryHard.Name = "buttonVeryHard";
            this.buttonVeryHard.Size = new System.Drawing.Size(75, 23);
            this.buttonVeryHard.TabIndex = 4;
            this.buttonVeryHard.Text = "Very hard";
            this.buttonVeryHard.UseVisualStyleBackColor = true;
            this.buttonVeryHard.Click += new System.EventHandler(this.buttonVeryHard_Click);
            // 
            // buttonHard
            // 
            this.buttonHard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonHard.AutoSize = true;
            this.buttonHard.Location = new System.Drawing.Point(27, 96);
            this.buttonHard.Name = "buttonHard";
            this.buttonHard.Size = new System.Drawing.Size(75, 23);
            this.buttonHard.TabIndex = 3;
            this.buttonHard.Text = "Hard";
            this.buttonHard.UseVisualStyleBackColor = true;
            this.buttonHard.Click += new System.EventHandler(this.buttonHard_Click);
            // 
            // buttonMedium
            // 
            this.buttonMedium.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonMedium.AutoSize = true;
            this.buttonMedium.Location = new System.Drawing.Point(27, 53);
            this.buttonMedium.Name = "buttonMedium";
            this.buttonMedium.Size = new System.Drawing.Size(75, 23);
            this.buttonMedium.TabIndex = 2;
            this.buttonMedium.Text = "Medium";
            this.buttonMedium.UseVisualStyleBackColor = true;
            this.buttonMedium.Click += new System.EventHandler(this.buttonMedium_Click);
            // 
            // buttonEasy
            // 
            this.buttonEasy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonEasy.AutoSize = true;
            this.buttonEasy.Location = new System.Drawing.Point(27, 10);
            this.buttonEasy.Name = "buttonEasy";
            this.buttonEasy.Size = new System.Drawing.Size(75, 23);
            this.buttonEasy.TabIndex = 1;
            this.buttonEasy.Text = "Easy";
            this.buttonEasy.UseVisualStyleBackColor = true;
            this.buttonEasy.Click += new System.EventHandler(this.buttonEasy_Click);
            // 
            // NewGameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(129, 215);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewGameDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Puzzle";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonVeryHard;
        private System.Windows.Forms.Button buttonHard;
        private System.Windows.Forms.Button buttonMedium;
        private System.Windows.Forms.Button buttonEasy;
    }
}