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

            NewEasy = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.SIMPLE));
            NewMedium = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.EASY));
            NewHard = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.INTERMEDIATE));
            NewVeryHard = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.EXPERT));
        }

        public CommandBase NewEasy { get; }
        public CommandBase NewMedium { get; }
        public CommandBase NewHard { get; }
        public CommandBase NewVeryHard { get; }

        public BoardLogic BoardLogic { get; }

        public void OnLoaded()
        {
        }
    }
}
