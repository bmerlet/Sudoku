using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sudoku.Game;
//using Sudoku.UILogic;

namespace FormsUI
{
    public partial class MainWindow : Form
    {
        //private readonly MainWindowLogic logic = new 
        public MainWindow()
        {
            InitializeComponent();

            foreach(var pos in Position.AllPositions)
            {
                var cell = new UICell();
                tableLayoutPanelBoard.Controls.Add(cell, (int)pos.Column, (int)pos.Row);
            }
        }
    }
}
