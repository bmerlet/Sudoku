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
    public partial class YesNoDialog : Form
    {
        public YesNoDialog(YesNoDialogLogic logic)
        {
            logic.CloseView = result => DialogResult = result == true ? DialogResult.OK : DialogResult.Cancel;

            InitializeComponent();

            Text = logic.Title;
            labelString.Text = logic.Question;
            buttonOK.Text = logic.LeftButtonText;
            buttonCancel.Text = logic.RightButtonText;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
