namespace FormsUI
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonNewVeryHard = new System.Windows.Forms.Button();
            this.buttonNewHard = new System.Windows.Forms.Button();
            this.buttonNewMedium = new System.Windows.Forms.Button();
            this.buttonNewEasy = new System.Windows.Forms.Button();
            this.buttonRedo = new System.Windows.Forms.Button();
            this.tableLayoutPanelBoard = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelButtons, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelBoard, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(735, 555);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.ColumnCount = 2;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Controls.Add(this.buttonUndo, 0, 6);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonReset, 0, 5);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonNewVeryHard, 0, 4);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonNewHard, 0, 3);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonNewMedium, 0, 2);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonNewEasy, 0, 1);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonRedo, 1, 6);
            this.tableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(608, 3);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 7;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.71795F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.54701F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.54701F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.54701F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.54701F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.54701F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.54701F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(124, 549);
            this.tableLayoutPanelButtons.TabIndex = 0;
            // 
            // buttonUndo
            // 
            this.buttonUndo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonUndo.Location = new System.Drawing.Point(4, 509);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(55, 28);
            this.buttonUndo.TabIndex = 5;
            this.buttonUndo.Text = "<-";
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.ButtonUndo_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelButtons.SetColumnSpan(this.buttonReset, 2);
            this.buttonReset.Location = new System.Drawing.Point(9, 460);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(105, 28);
            this.buttonReset.TabIndex = 4;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // buttonNewVeryHard
            // 
            this.buttonNewVeryHard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelButtons.SetColumnSpan(this.buttonNewVeryHard, 2);
            this.buttonNewVeryHard.Location = new System.Drawing.Point(9, 414);
            this.buttonNewVeryHard.Name = "buttonNewVeryHard";
            this.buttonNewVeryHard.Size = new System.Drawing.Size(105, 28);
            this.buttonNewVeryHard.TabIndex = 3;
            this.buttonNewVeryHard.Text = "New very hard";
            this.buttonNewVeryHard.UseVisualStyleBackColor = true;
            this.buttonNewVeryHard.Click += new System.EventHandler(this.ButtonNewVeryHard_Click);
            // 
            // buttonNewHard
            // 
            this.buttonNewHard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelButtons.SetColumnSpan(this.buttonNewHard, 2);
            this.buttonNewHard.Location = new System.Drawing.Point(9, 368);
            this.buttonNewHard.Name = "buttonNewHard";
            this.buttonNewHard.Size = new System.Drawing.Size(105, 28);
            this.buttonNewHard.TabIndex = 2;
            this.buttonNewHard.Text = "New hard";
            this.buttonNewHard.UseVisualStyleBackColor = true;
            this.buttonNewHard.Click += new System.EventHandler(this.ButtonNewHard_Click);
            // 
            // buttonNewMedium
            // 
            this.buttonNewMedium.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelButtons.SetColumnSpan(this.buttonNewMedium, 2);
            this.buttonNewMedium.Location = new System.Drawing.Point(9, 322);
            this.buttonNewMedium.Name = "buttonNewMedium";
            this.buttonNewMedium.Size = new System.Drawing.Size(105, 28);
            this.buttonNewMedium.TabIndex = 1;
            this.buttonNewMedium.Text = "New medium";
            this.buttonNewMedium.UseVisualStyleBackColor = true;
            this.buttonNewMedium.Click += new System.EventHandler(this.ButtonNewMedium_Click);
            // 
            // buttonNewEasy
            // 
            this.buttonNewEasy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelButtons.SetColumnSpan(this.buttonNewEasy, 2);
            this.buttonNewEasy.Location = new System.Drawing.Point(9, 276);
            this.buttonNewEasy.Name = "buttonNewEasy";
            this.buttonNewEasy.Size = new System.Drawing.Size(105, 28);
            this.buttonNewEasy.TabIndex = 0;
            this.buttonNewEasy.Text = "New easy";
            this.buttonNewEasy.UseVisualStyleBackColor = true;
            this.buttonNewEasy.Click += new System.EventHandler(this.ButtonNewEasy_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonRedo.Location = new System.Drawing.Point(65, 509);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(54, 28);
            this.buttonRedo.TabIndex = 6;
            this.buttonRedo.Text = "->";
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.ButtonRedo_Click);
            // 
            // tableLayoutPanelBoard
            // 
            this.tableLayoutPanelBoard.ColumnCount = 9;
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBoard.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelBoard.Name = "tableLayoutPanelBoard";
            this.tableLayoutPanelBoard.RowCount = 9;
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanelBoard.Size = new System.Drawing.Size(599, 549);
            this.tableLayoutPanelBoard.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 555);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Sudoku";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonNewVeryHard;
        private System.Windows.Forms.Button buttonNewHard;
        private System.Windows.Forms.Button buttonNewMedium;
        private System.Windows.Forms.Button buttonNewEasy;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBoard;
    }
}

