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
using Sudoku.UILogic;

namespace FormsUI
{
    public partial class MainWindow : Form, IUIProvider
    {
        #region Private members

        private readonly MainWindowLogic logic;
        private readonly BoardLogic boardLogic;

        #endregion

        #region Initialization

        public MainWindow()
        {
            // Create logic
            logic = new MainWindowLogic(this);
            boardLogic = logic.BoardLogic;

            // Crete winforms widgets
            InitializeComponent();

            // Create the cells
            CreateCells();

            // Initial state
            buttonNewEasy.Enabled = logic.NewEasy.CanExecute();
            buttonNewMedium.Enabled = logic.NewMedium.CanExecute();
            buttonNewHard.Enabled = logic.NewHard.CanExecute();
            buttonNewVeryHard.Enabled = logic.NewVeryHard.CanExecute();
            buttonUndo.Enabled = boardLogic.Undo.CanExecute();
            buttonRedo.Enabled = boardLogic.Redo.CanExecute();
            buttonReset.Enabled = boardLogic.Reset.CanExecute();

            // Listen to logic events
            logic.NewEasy.CanExecuteChanged += (s, e) => buttonNewEasy.Enabled = logic.NewEasy.CanExecute();
            logic.NewMedium.CanExecuteChanged += (s, e) => buttonNewMedium.Enabled = logic.NewMedium.CanExecute();
            logic.NewHard.CanExecuteChanged += (s, e) => buttonNewHard.Enabled = logic.NewHard.CanExecute();
            logic.NewVeryHard.CanExecuteChanged += (s, e) => buttonNewVeryHard.Enabled = logic.NewVeryHard.CanExecute();
            boardLogic.Undo.CanExecuteChanged += (s, e) => buttonUndo.Enabled = boardLogic.Undo.CanExecute();
            boardLogic.Redo.CanExecuteChanged += (s, e) => buttonRedo.Enabled = boardLogic.Redo.CanExecute();
            boardLogic.Reset.CanExecuteChanged += (s, e) => buttonReset.Enabled = boardLogic.Reset.CanExecute();

            boardLogic.PuzzleSolved += OnBoardLogicPuzzleSolved;
        }

        private void CreateCells()
        {
            foreach (var pos in Position.AllPositions)
            {
                var cellLogic = boardLogic.UICells[pos.Cell];
                var cell = new UICell(cellLogic, boardLogic, pos);
                tableLayoutPanelBoard.Controls.Add(cell, (int)pos.Column, (int)pos.Row);
            }
        }

        #endregion

        #region Logic events

        private void OnBoardLogicPuzzleSolved(object sender, EventArgs e)
        {
            MessageBox.Show("Congratulations, game won!");
        }

        #endregion

        #region Actions

        private void ButtonNewEasy_Click(object sender, EventArgs e)
        {
            logic.NewEasy.Execute();
        }

        private void ButtonNewMedium_Click(object sender, EventArgs e)
        {
            logic.NewMedium.Execute();
        }

        private void ButtonNewHard_Click(object sender, EventArgs e)
        {
            logic.NewHard.Execute();
        }

        private void ButtonNewVeryHard_Click(object sender, EventArgs e)
        {
            logic.NewVeryHard.Execute();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            boardLogic.Reset.Execute();
        }

        private void ButtonUndo_Click(object sender, EventArgs e)
        {
            boardLogic.Undo.Execute();
        }

        private void ButtonRedo_Click(object sender, EventArgs e)
        {
            boardLogic.Redo.Execute();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C) && boardLogic.Check.CanExecute())
            {
                boardLogic.Check.Execute();
                return true;
            }

            if (keyData == (Keys.Control | Keys.H) && boardLogic.Hint.CanExecute())
            {
                boardLogic.Hint.Execute();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Z) && boardLogic.Undo.CanExecute())
            {
                boardLogic.Undo.Execute();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Y) && boardLogic.Redo.CanExecute())
            {
                boardLogic.Redo.Execute();
                return true;
            }

            if (boardLogic.KbdNumber.CanExecute() && keyData >= Keys.D0 && keyData <= Keys.D9)
            {
                int num = keyData - Keys.D0;
                boardLogic.KbdNumber.Execute(num.ToString());
                return true;
            }

            if (boardLogic.KbdNumber.CanExecute() && keyData >= (Keys.D0 | Keys.Control) && keyData <= (Keys.D9 | Keys.Control))
            {
                int num = keyData - (Keys.D0 | Keys.Control);
                boardLogic.KbdPossible.Execute(num.ToString());
                return true;
            }

            if (keyData == Keys.Right || keyData == Keys.L)
            {
                boardLogic.MoveRight.Execute();
                return true;
            }

            if (keyData == Keys.Left || keyData == Keys.J)
            {
                boardLogic.MoveLeft.Execute();
                return true;
            }

            if (keyData == Keys.Up || keyData == Keys.I)
            {
                boardLogic.MoveUp.Execute();
                return true;
            }

            if (keyData == Keys.Down || keyData == Keys.K)
            {
                boardLogic.MoveDown.Execute();
                return true;
            }

            return false;
        }

        #endregion

        #region IUIProvider implementation

        public object GetBrush(EColors color)
        {
            switch (color)
            {
                case EColors.NormalBackground: return Color.Transparent;
                case EColors.GivenBackground: return Color.LightGray;
                case EColors.SelectedBackground: return Color.LightBlue;
                case EColors.NormalForeground: return Color.Black;
                case EColors.ErrorForeground: return Color.Red;
            }

            return Color.Turquoise;
        }

        #endregion
    }
}
