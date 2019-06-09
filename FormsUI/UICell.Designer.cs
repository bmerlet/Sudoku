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
            this.tableLayoutPanelBorders = new System.Windows.Forms.TableLayoutPanel();
            this.panelCell = new System.Windows.Forms.Panel();
            this.tableLayoutPanelBorders.SuspendLayout();
            this.panelCell.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPossibles
            // 
            this.labelPossibles.AutoSize = true;
            this.labelPossibles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPossibles.Location = new System.Drawing.Point(0, 0);
            this.labelPossibles.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelPossibles.Name = "labelPossibles";
            this.labelPossibles.Size = new System.Drawing.Size(56, 18);
            this.labelPossibles.TabIndex = 1;
            this.labelPossibles.Text = "123456";
            // 
            // labelNumber
            // 
            this.labelNumber.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelNumber.AutoSize = false;
            this.labelNumber.BackColor = System.Drawing.Color.Transparent;
            this.labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumber.Location = new System.Drawing.Point(0, 0);
            this.labelNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Padding = new System.Windows.Forms.Padding(0);
            this.labelNumber.Size = new System.Drawing.Size(48, 48);
            this.labelNumber.TabIndex = 0;
            this.labelNumber.Text = "8";
            this.labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelBorders
            // 
            this.tableLayoutPanelBorders.ColumnCount = 3;
            this.tableLayoutPanelBorders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelBorders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBorders.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelBorders.Controls.Add(this.panelCell, 1, 1);
            this.tableLayoutPanelBorders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBorders.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBorders.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelBorders.Name = "tableLayoutPanelBorders";
            this.tableLayoutPanelBorders.RowCount = 3;
            this.tableLayoutPanelBorders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelBorders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBorders.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelBorders.Size = new System.Drawing.Size(671, 407);
            this.tableLayoutPanelBorders.TabIndex = 2;
            // 
            // panelCell
            // 
            this.panelCell.Controls.Add(this.labelPossibles);
            this.panelCell.Controls.Add(this.labelNumber);
            this.panelCell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCell.Location = new System.Drawing.Point(4, 4);
            this.panelCell.Margin = new System.Windows.Forms.Padding(0);
            this.panelCell.Name = "panelCell";
            this.panelCell.Size = new System.Drawing.Size(663, 399);
            this.panelCell.TabIndex = 0;
            // 
            // UICell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelBorders);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UICell";
            this.Size = new System.Drawing.Size(671, 407);
            this.tableLayoutPanelBorders.ResumeLayout(false);
            this.panelCell.ResumeLayout(false);
            this.panelCell.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelPossibles;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBorders;
        private System.Windows.Forms.Panel panelCell;
    }
}
