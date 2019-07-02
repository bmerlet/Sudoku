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
    public partial class NewGameDialog : Form
    {
        private NewGameDialogLogic logic;

        public NewGameDialog(NewGameDialogLogic logic)
        {
            this.logic = logic;
            logic.CloseView = result => DialogResult = result == true ? DialogResult.OK : DialogResult.Cancel;

            InitializeComponent();
        }

        private void buttonEasy_Click(object sender, EventArgs e)
        {
            logic.NewEasy.Execute();
        }

        private void buttonMedium_Click(object sender, EventArgs e)
        {
            logic.NewMedium.Execute();
        }

        private void buttonHard_Click(object sender, EventArgs e)
        {
            logic.NewHard.Execute();
        }

        private void buttonVeryHard_Click(object sender, EventArgs e)
        {
            logic.NewVeryHard.Execute();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            logic.CancelCommand.Execute();
        }
    }
}
