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
    class BoardLogic : LogicBase
    {
        private Creator creator = new Creator();
        private Puzzle puzzle;

        public BoardLogic()
        {
            Cells = new string[Creator.BOARD_SIZE];
            Background = new Brush[Creator.BOARD_SIZE];
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = "";
                Background[i] = Brushes.Transparent;
            }
        }
        public string[] Cells { get; private set; }
        public Brush[] Background { get; private set; }

        public void OnGeneratePuzzle(EDifficulty difficulty)
        {
            creator.Verbose = false;
            puzzle = creator.GeneratePuzzle(difficulty);

            for (int i = 0; i < puzzle.Cells.Length; i++)
            {
                uint val = puzzle.Cells[i];
                Cells[i] = (val == 0) ? "" : val.ToString();
                Background[i] = (val == 0) ? Brushes.Transparent : Brushes.LightGray;
            }

            OnPropertyChanged(() => Cells);
            OnPropertyChanged(() => Background);
        }
        public void OnPause()
        {
            uint numEasy = 0;
            uint numSimple = 0;
            uint numInter = 0;
            uint numHard = 0;

            creator.TrackTiming = true;

            for (int i = 0; i < 100; i++)
            {
                var puzzle = creator.GeneratePuzzle(EDifficulty.UNKNOWN, ESymmetry.RANDOM);
                switch (puzzle.Statistics.Difficulty)
                {
                    case EDifficulty.SIMPLE: numSimple++; break;
                    case EDifficulty.EASY: numEasy++; break;
                    case EDifficulty.INTERMEDIATE: numInter++; break;
                    case EDifficulty.EXPERT: numHard++; break;
                }
            }

            Console.WriteLine($"Simple: {numSimple}, Easy: {numEasy}, Inter: {numInter}, Hard: {numHard}");
        }

        public void OnMouseLeft(uint row, uint col)
        {

        }
        public void OnMouseRight(uint row, uint col)
        {

        }
    }
}
