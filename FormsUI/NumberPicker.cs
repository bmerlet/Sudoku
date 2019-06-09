using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sudoku.UILogic;

namespace FormsUI
{
    public partial class NumberPicker : UserControl
    {
        private readonly BoardLogic boardLogic;
        public ContextMenuStrip ParentContextMenuStrip { get; set; }

        public NumberPicker(BoardLogic boardLogic)
        {
            this.boardLogic = boardLogic;

            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ButtonClick(1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ButtonClick(2);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ButtonClick(3);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            ButtonClick(4);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            ButtonClick(5);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            ButtonClick(6);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            ButtonClick(7);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            ButtonClick(8);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            ButtonClick(9);
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            ButtonClick(0);
        }

        private void ButtonClick(uint number)
        {
            bool keepOpen = boardLogic.OnSetNumber(number);
            
            if (!keepOpen && ParentContextMenuStrip != null)
            {
                ParentContextMenuStrip.Close();
            }
        }
    }
}
