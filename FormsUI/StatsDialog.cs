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
    public partial class StatsDialog : Form
    {
        private readonly StatsDialogLogic logic;

        public StatsDialog(StatsDialogLogic logic)
        {
            this.logic = logic;
            logic.CloseView = result => DialogResult = result == true ? DialogResult.OK : DialogResult.Cancel;

            InitializeComponent();

            labelGivensVal.Text = logic.NumGivens;
            labelSinglesVal.Text = logic.NumSingles;
            labelHiddenSinglesVal.Text = logic.NumHiddenSingles;
            labelNakedPairsVal.Text = logic.NumNakedPairs;
            labelHiddenPairsVal.Text = logic.NumHiddenPairs;
            labelBoxLineReductionsVal.Text = logic.NumBoxLineReduction;
            labelPairPointingTripletsVal.Text = logic.NumPointingPairTriple;
            labelSolutionsVal.Text = logic.NumGuesses;
            labelDifficultyVal.Text = logic.Difficulty;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            logic.CommitCommand.Execute();
        }
    }
}
