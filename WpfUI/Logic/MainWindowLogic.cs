using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Sudoku;
using Toolbox.UILogic;

namespace WpfUI.Logic
{
    class MainWindowLogic : LogicBase
    {
        public MainWindowLogic()
        {
            BoardLogic = new BoardLogic();

            Pause = new CommandBase(BoardLogic.OnPause);
            NewEasy = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.SIMPLE));
            NewMedium = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.EASY));
            NewHard = new CommandBase(() => BoardLogic.OnGeneratePuzzle(EDifficulty.INTERMEDIATE));
        }

        public CommandBase Pause { get; }
        public CommandBase NewEasy { get; }
        public CommandBase NewMedium { get; }
        public CommandBase NewHard { get; }

        public BoardLogic BoardLogic { get; }

        public void OnLoaded()
        {
        }
    }
}
