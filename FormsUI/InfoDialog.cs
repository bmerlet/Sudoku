using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sudoku.UILogic;

namespace FormsUI
{
    public partial class InfoDialog : Form
    {
        private readonly InfoDialogLogic logic;

        public InfoDialog(InfoDialogLogic logic)
        {
            this.logic = logic;

            InitializeComponent();

            Text = logic.Title;
            textBox.Lines = logic.Info.Split('\n');
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            logic.CommitCommand.Execute();
        }
    }
}
