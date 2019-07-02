using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox.UILogic;
using Toolbox.Models;

namespace Sudoku.UILogic
{
    public class MainWindowLogic : LogicBase
    {
        private readonly IUIProvider uiProvider;

        public MainWindowLogic(IUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;

            BoardLogic = new BoardLogic(uiProvider);
            SettingsManager = new SettingsManager<Settings>("Sarabande, Inc.", "Sudoku");
            BoardLogic.PuzzleSolved += OnPuzzleSolved;

            NewGame = new CommandBase(OnNewGame);
        }

        public CommandBase NewGame { get; }

        public BoardLogic BoardLogic { get; }
        public SettingsManager<Settings> SettingsManager { get; }

        public void OnLoaded()
        {
            GenerateNewGame(true);
        }

        private void OnNewGame()
        {
            var logic = new YesNoDialogLogic("New puzzle", "Quit current puzzle and start a new one?");
            bool result = uiProvider.DisplayDialog(logic);
            if (result)
            {
                GenerateNewGame(false);
            }
        }

        private void GenerateNewGame(bool exitOnCancel)
        {
            var newGameDialogLogic = new NewGameDialogLogic();
            bool result = uiProvider.DisplayDialog(newGameDialogLogic);
            if (result)
            {
                BoardLogic.OnGeneratePuzzle(newGameDialogLogic.Difficulty);
            }
            else if (exitOnCancel)
            {
                uiProvider.Exit();
            }
        }

        private void OnPuzzleSolved(object sender, EventArgs e)
        {
            uiProvider.RunAsyncOnUIThread(OnPuzzleSolvedAsync);
        }

        private void OnPuzzleSolvedAsync()
        {
            var logic = new YesNoDialogLogic("Solved", "Congratulations, you solved the puzzle!\nPlay again?");
            bool result = uiProvider.DisplayDialog(logic);
            if (result)
            {
                GenerateNewGame(false);
            }
            else
            {
                uiProvider.Exit();
            }
        }
    }
}
