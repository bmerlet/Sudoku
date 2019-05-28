﻿using System;
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
    //  Find bug in creation algorithm
    //  Save/restore
    //  Hints
    //  Timing?
    //  Better show affected places when impossible
    //  Shortcuts
    //
    class BoardLogic : LogicBase
    {
        #region Private members

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

        // Log and current index in log
        private readonly List<LogEntry> logEntries = new List<LogEntry>();
        private int logIndex;

        #endregion

        #region Constructor

        public BoardLogic()
        {
            Undo = new CommandBase(OnUndo, false);
            Redo = new CommandBase(OnRedo, false);

            for (uint i = 0; i < UICells.Length; i++)
            {
                UICells[i] = new UICellLogic(i);
            }
        }

        #endregion

        #region Events

        // Activated when the puzzle is solved
        public event EventHandler PuzzleSolved;

        #endregion

        #region UI properties

        // The cell logic array
        public UICellLogic[] UICells { get; } = new UICellLogic[Creator.BOARD_SIZE];

        // Undo/redo commands
        public CommandBase Undo { get; private set; }
        public CommandBase Redo { get; private set; }

        #endregion

        #region Actions

        //
        // Generate a puzzle of a specific difficulty
        //
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
            logEntries.Clear();
            Undo.SetCanExecute(false);
            Redo.SetCanExecute(false);
            logIndex = 0;
        }

        //
        // ZZZ This is test code
        //
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

        //
        // Process a left mouse click
        //
        public bool OnMouseLeft(uint row, uint col)
        {
            // We are picking a number, not possibles
            pickPossibilities = false;

            return OnMouse(row, col);
        }

        //
        // Process a right mouse click
        //
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

        //
        // Process setting a number
        //
        public bool OnSetNumber(uint number)
        {
            if (puzzle != null && selectedCell != uint.MaxValue)
            {
                var cell = UICells[selectedCell];

                if (pickPossibilities)
                {
                    uint oldValue = number;
                    if (number == 0)
                    {
                        // Special case fir undo when clearing possibles
                        // We need to remember what possibles were there
                        oldValue = 0xf;
                        foreach(var cv in Cell.AllValidCellValues)
                        {
                            if (cell.IsListedAsPossible(cv))
                            {
                                oldValue |= (uint)(1 << ((int)cv + 4));
                            }
                        }
                    }

                    SetPossible(selectedCell, number);

                    // Log the change
                    LogAction(LogEntry.EType.PickPossibles, selectedCell, oldValue, number);

                    // Keep the context menu open
                    return true;
                }
                else
                {
                    uint oldValue = cell.Number == "" ? 0 : uint.Parse(cell.Number);

                    // Set the number in the cell, and evaluate
                    SetNumber(selectedCell, number);

                    // Log the change
                    LogAction(LogEntry.EType.PickNumber, selectedCell, oldValue, number);

                    // Unselect the cell
                    cell.UpdateSelected(false);
                    selectedCell = uint.MaxValue;

                    // close context menu
                    return false;
                }
            }

            return false;
        }

        //
        // Process Undo
        //
        private void OnUndo()
        {
            if (logIndex > 0)
            {
                var entryToUndo = logEntries[--logIndex];
                if (logIndex == 0)
                {
                    Undo.SetCanExecute(false);
                }

                if (entryToUndo.Type == LogEntry.EType.PickNumber)
                {
                    SetNumber(entryToUndo.Position, entryToUndo.OldValue);
                }
                else
                {
                    if (entryToUndo.OldValue > 0xf)
                    {
                        // Special case of undoing the clearing of possibles
                        foreach (var cv in Cell.AllValidCellValues)
                        {
                            if ((entryToUndo.OldValue & (uint)(1 << ((int)cv + 4))) != 0)
                            {
                                SetPossible(entryToUndo.Position, cv);
                            }
                        }
                    }
                    else
                    {
                        SetPossible(entryToUndo.Position, entryToUndo.OldValue);
                    }
                }

                Redo.SetCanExecute(true);
            }
        }

        //
        // Process Redo
        //
        private void OnRedo()
        {
            if (logIndex < logEntries.Count)
            {
                var entryToRedo = logEntries[logIndex++];
                if (logIndex == logEntries.Count)
                {
                    Redo.SetCanExecute(false);
                }

                if (entryToRedo.Type == LogEntry.EType.PickNumber)
                {
                    SetNumber(entryToRedo.Position, entryToRedo.NewValue);
                }
                else
                {
                    SetPossible(entryToRedo.Position, entryToRedo.NewValue);
                }

                Undo.SetCanExecute(true);
            }
        }

        private void LogAction(LogEntry.EType type, uint position, uint oldValue, uint newValue)
        {
            var entry = new LogEntry(type, position, oldValue, newValue);
            if (logIndex < logEntries.Count)
            {
                logEntries[logIndex] = entry;
            }
            else
            {
                logEntries.Add(entry);
            }

            logIndex++;
            Redo.SetCanExecute(false);
            Undo.SetCanExecute(true);
        }

        //
        // Set a number
        //
        private void SetNumber(uint pos, uint number)
        {
            var cell = UICells[pos];

            // Wipe out possibles
            cell.ResetPossibles();

            // memorize the number
            userSolution.Cells[pos] = number;

            // Evaluate the correctness
            userTable = creator.Evaluate(userSolution);
            if (userTable == null)
            {
                // User selection makes puzzle impossible!
                cell.UpdateNumberStatus(true);
            }
            else
            {
                cell.UpdateNumberStatus(false);
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
            cell.SetNumber(number);
        }

        private void SetPossible(uint position, uint number)
        {
            var cell = UICells[position];

            // Add/remove the number (except if it is 0)
            if (number != 0)
            {
                cell.UpdatePossibles(number);

                // If we have a table, check if this number is possible
                if (userTable != null)
                {
                    UpdateOnePossibleStatus(Position.GetCellFromCell(selectedCell));
                }
            }
            else
            {
                cell.ResetPossibles();
            }
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

        #endregion
    }
}
