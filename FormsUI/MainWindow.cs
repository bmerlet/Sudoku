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
using Toolbox.UILogic.Dialogs;

namespace FormsUI
{
    public partial class MainWindow : Form, IUIProvider
    {
        #region Private members

        private readonly MainWindowLogic logic;
        private readonly BoardLogic boardLogic;
        private readonly FreeContextMenuStrip contextMenu;

        #endregion

        #region Initialization

        public MainWindow()
        {
            // Create logic
            logic = new MainWindowLogic(this);
            boardLogic = logic.BoardLogic;

            // Create winforms widgets
            InitializeComponent();

            // Get settings
            Load += (s, e) => OnLoad();

            // Save settings on closing
            Closing += (s, e) => SaveSettings();

            // Build context menu
            var numberPicker = new NumberPicker(boardLogic);
            contextMenu = new FreeContextMenuStrip(numberPicker);
            numberPicker.ParentContextMenuStrip = contextMenu;

            // Regain focus when context menu goes away
            contextMenu.Closed += (e, s) => Focus();

            // Create the cells
            CreateCells();

            // Initial state
            buttonUndo.Enabled = boardLogic.Undo.CanExecute();
            buttonRedo.Enabled = boardLogic.Redo.CanExecute();
            buttonReset.Enabled = boardLogic.Reset.CanExecute();

            // Listen to logic events
            boardLogic.Undo.CanExecuteChanged += (s, e) => buttonUndo.Enabled = boardLogic.Undo.CanExecute();
            boardLogic.Redo.CanExecuteChanged += (s, e) => buttonRedo.Enabled = boardLogic.Redo.CanExecute();
            boardLogic.Reset.CanExecuteChanged += (s, e) => buttonReset.Enabled = boardLogic.Reset.CanExecute();
        }

        private void CreateCells()
        {
            foreach (var pos in Position.AllPositions)
            {
                var cellLogic = boardLogic.UICells[pos.Cell];
                var cell = new UICell(cellLogic, boardLogic, contextMenu, pos);
                tableLayoutPanelBoard.Controls.Add(cell, (int)pos.Column, (int)pos.Row);
            }
        }

        #endregion

        #region Actions

        private void buttonNewPuzzle_Click(object sender, EventArgs e)
        {
            logic.NewGame.Execute();
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            boardLogic.Stats.Execute();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            logic.Help.Execute();
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ProcessKeyboardInput(keyData))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ProcessKeyboardInput(Keys keyData)
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

            if (keyData == (Keys.Control | Keys.Q) && boardLogic.Query.CanExecute())
            {
                boardLogic.Query.Execute();
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

            if (boardLogic.KbdNumber.CanExecute() && keyData >= Keys.F1 && keyData <= Keys.F9)
            {
                int num = keyData - Keys.F1 + 1;
                boardLogic.KbdPossible.Execute(num.ToString());
                return true;
            }

            if (keyData == Keys.Space || keyData == Keys.Delete)
            {
                boardLogic.KbdNumber.Execute("0");
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

        private void OnLoad()
        {
            // Get settings
            var settings = logic.SettingsManager.Load();
            if (settings != null)
            {
                DesktopLocation = new Point(settings.Left, settings.Top);
            }

            // Show the start dialog
            BeginInvoke((Action)(() => logic.OnLoaded()));
        }

        private void SaveSettings()
        {
            var settings = new Settings();
            settings.Left = DesktopLocation.X;
            settings.Top = DesktopLocation.Y;

            logic.SettingsManager.Save(settings);
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

        public bool DisplayDialog(LogicDialogBase logic)
        {
            Form dialog = null;

            if (logic is InfoDialogLogic infoDialogLogic)
            {
                dialog = new InfoDialog(infoDialogLogic);
            }
            else if (logic is NewGameDialogLogic newGameDialogLogic)
            {
                dialog = new NewGameDialog(newGameDialogLogic);
            }
            else if (logic is YesNoDialogLogic yesNoDialogLogic)
            {
                dialog = new YesNoDialog(yesNoDialogLogic);
            }
            else if (logic is StatsDialogLogic statsDialogLogic)
            {
                dialog = new StatsDialog(statsDialogLogic);
            }

            if (dialog == null)
            {
                throw new InvalidOperationException($"Unknown dialog {logic.GetType()}");
            }

            var result = dialog.ShowDialog();

            return  result == DialogResult.OK;
        }

        public void RunAsyncOnUIThread(Action action)
        {
            BeginInvoke(action);
        }

        public void Exit()
        {
            Application.Exit();
        }

        #endregion
    }
}
