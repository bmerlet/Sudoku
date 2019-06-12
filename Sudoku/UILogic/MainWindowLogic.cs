using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox.UILogic;

namespace Sudoku.UILogic
{
    public class MainWindowLogic : LogicBase
    {
        public MainWindowLogic(IUIProvider uiProvider)
        {
            BoardLogic = new BoardLogic(uiProvider);

            NewEasy = new CommandBase(() => OnGeneratePuzzle(EDifficulty.SIMPLE));
            NewMedium = new CommandBase(() => OnGeneratePuzzle(EDifficulty.EASY));
            NewHard = new CommandBase(() => OnGeneratePuzzle(EDifficulty.INTERMEDIATE));
            NewVeryHard = new CommandBase(() => OnGeneratePuzzle(EDifficulty.EXPERT));

            BoardLogic.PuzzleSolved += (s, e) => OnPuzzleSolved();
        }

        public CommandBase NewEasy { get; }
        public CommandBase NewMedium { get; }
        public CommandBase NewHard { get; }
        public CommandBase NewVeryHard { get; }

        public BoardLogic BoardLogic { get; }

        public void OnLoaded()
        {
        }

        private void OnGeneratePuzzle(EDifficulty difficulty)
        {
            if (NewEasy.CanExecute())
            {
                BoardLogic.OnGeneratePuzzle(difficulty);
                NewEasy.SetCanExecute(false);
                NewMedium.SetCanExecute(false);
                NewHard.SetCanExecute(false);
                NewVeryHard.SetCanExecute(false);
            }
        }

        private void OnPuzzleSolved()
        {
            NewEasy.SetCanExecute(true);
            NewMedium.SetCanExecute(true);
            NewHard.SetCanExecute(true);
            NewVeryHard.SetCanExecute(true);
        }
    }
}
