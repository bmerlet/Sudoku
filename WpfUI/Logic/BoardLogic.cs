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
    //
    // TODO:
    //  Undo/Redo
    //  Find bug in creation algorithm
    //  Save/restore
    //  Hints
    //  Timing?
    //  Better show affected places when impossible
    //
    class BoardLogic : LogicBase
    {
        // Creator/solver of sudoku puzzle
        private readonly Creator creator = new Creator();

        // The initial puzzle
        private Puzzle puzzle;

        // The puzzle, as currently completed by the user
        private Puzzle userSolution;

        // The information from the solver wrt the user solution
        private Table userTable;

        // Currently selected cell
        private uint selectedCell = uint.MaxValue;

        // If we are picking numbers or possibles
        private bool pickPossibilities;

        // If we are showing the number picker
        private bool showingPicker;

        public BoardLogic()
        {
            UICells = new UICellLogic[Creator.BOARD_SIZE];

            for (uint i = 0; i < UICells.Length; i++)
            {
                UICells[i] = new UICellLogic(i);
            }
        }

        public event EventHandler PuzzleSolved;

        public UICellLogic[] UICells { get; private set; }

        public void OnGeneratePuzzle(EDifficulty difficulty)
        {
            creator.Verbose = false;
            puzzle = creator.GeneratePuzzle(difficulty);
            userSolution = new Puzzle(puzzle, null);
            userTable = creator.Evaluate(userSolution);

            for (uint i = 0; i < puzzle.Cells.Length; i++)
            {
                uint val = puzzle.Cells[i];
                UICells[i].SetNumber(val);
                UICells[i].UpdateGiven(val != 0);
                UICells[i].UpdateSelected(false);
                UICells[i].ResetPossibles();
            }

            selectedCell = uint.MaxValue;
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

        public bool OnMouseLeft(uint row, uint col)
        {
            // We are picking a number, not possibles
            pickPossibilities = false;

            return OnMouse(row, col);
        }

        public bool OnMouseRight(uint row, uint col)
        {
            // We are picking possibilities, not numbers
            pickPossibilities = true;

            return OnMouse(row, col);
        }

        private bool OnMouse(uint row, uint col)
        {
            bool showPicker = false;

            if (puzzle != null)
            {
                // Compute index
                uint pos = row * Creator.ROW_SIZE + col;

                // Reset the showing picker state if changed cell
                if (pos != selectedCell)
                {
                    showingPicker = false;
                }

                // Select cell
                SelectCell(pos);


                // Show picker if selected and not a given and picker not showing
                if (selectedCell == pos && puzzle.Cells[pos] == 0 && !showingPicker)
                {
                    showPicker = true;
                }

                // Flip value for next time
                showingPicker = !showingPicker;
            }

            return showPicker;
        }

        public bool OnSetNumber(uint number)
        {
            if (puzzle != null && selectedCell != uint.MaxValue)
            {
                if (pickPossibilities)
                {
                    // Add/remove the number (except if it is 0)
                    if (number != 0)
                    {
                        UICells[selectedCell].UpdatePossibles(number);

                        // If we have a table, check if this number is possible
                        if (userTable != null)
                        {
                            UpdateOnePossibleStatus(Position.GetCellFromCell(selectedCell));
                        }
                    }
                    else
                    {
                        UICells[selectedCell].ResetPossibles();
                    }

                    // Keep the context menu open
                    return true;
                }
                else
                {
                    // Wipe out possibles
                    UICells[selectedCell].ResetPossibles();

                    // memorize the number
                    userSolution.Cells[selectedCell] = number;

                    // Evaluate the correctness
                    userTable = creator.Evaluate(userSolution);
                    if (userTable == null)
                    {
                        // User selection makes puzzle impossible!
                        UICells[selectedCell].UpdateNumberStatus(true);
                    }
                    else
                    {
                        UICells[selectedCell].UpdateNumberStatus(false);
                        if (userTable.IsSolved)
                        {
                            // Finished
                            PuzzleSolved?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            // Update all possibles
                            UpdateAllPossibleStatus();
                        }
                    }

                    // Display it
                    UICells[selectedCell].SetNumber(number);

                    // Unselect the cell
                    UICells[selectedCell].UpdateSelected(false);
                    selectedCell = uint.MaxValue;

                    // close context menu
                    return false;
                }
            }

            return false;
        }

        private void UpdateAllPossibleStatus()
        {
            foreach(var pos in Position.AllPositions)
            {
                UpdateOnePossibleStatus(pos);
            }
        }

        private void UpdateOnePossibleStatus(Position pos)
        {
            bool error = false;

            var tableCell = userTable[pos];
            foreach (var cellValue in Cell.AllValidCellValues)
            {
                if (UICells[pos.Cell].IsListedAsPossible(cellValue))
                {
                    if (!tableCell.IsPossible(cellValue))
                    {
                        error = true;
                        break;
                    }
                }
            }

            UICells[pos.Cell].UpdatePossiblesStatus(error);
        }

        private void SelectCell(uint pos)
        {
            if (selectedCell != uint.MaxValue && selectedCell != pos)
            {
                // Unselect previously selected cell
                UICells[selectedCell].UpdateSelected(false);
            }

            // Select this cell
            selectedCell = pos;
            UICells[pos].UpdateSelected(true);
        }
    }
}
