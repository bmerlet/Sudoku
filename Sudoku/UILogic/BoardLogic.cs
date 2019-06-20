using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox.UILogic;

namespace Sudoku.UILogic
{
    //
    // TODO:
    //  Save/restore
    //  ^P for possibles?
    //  Timing?
    //
    public class BoardLogic : LogicBase
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
        private uint lastSelectedCell = uint.MaxValue;

        // If we are picking numbers or possibles
        private bool pickPossibilities;

        // If we are showing the number picker
        private bool showingPicker;

        // Log and current index in log
        private readonly List<LogEntry> logEntries = new List<LogEntry>();
        private int logIndex;

        #endregion

        #region Constructor

        public BoardLogic(IUIProvider uiProvider)
        {
            Undo = new CommandBase(OnUndo, false);
            Redo = new CommandBase(OnRedo, false);
            Reset = new CommandBase(OnReset, false);
            Check = new CommandBase(OnCheck, false);
            Hint = new CommandBase(OnHint, false);
            KbdNumber = new CommandBase(OnKbdNumber, true);
            KbdPossible = new CommandBase(OnKbdPossible, true);
            MoveRight = new CommandBase(OnMoveRight, true);
            MoveLeft = new CommandBase(OnMoveLeft, true);
            MoveUp = new CommandBase(OnMoveUp, true);
            MoveDown = new CommandBase(OnMoveDown, true);

            for (uint i = 0; i < UICells.Length; i++)
            {
                UICells[i] = new UICellLogic(uiProvider, i);
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

        // Reset command
        public CommandBase Reset { get; private set; }

        // Undo/redo commands
        public CommandBase Undo { get; private set; }
        public CommandBase Redo { get; private set; }

        // Check and Hint command (^C and ^H)
        public CommandBase Check { get; private set; }
        public CommandBase Hint { get; private set; }

        // User typed a number
        public CommandBase KbdNumber { get; private set; }
        public CommandBase KbdPossible { get; private set; }

        // User pressed an arrow key
        public CommandBase MoveRight { get; private set; }
        public CommandBase MoveLeft { get; private set; }
        public CommandBase MoveUp { get; private set; }
        public CommandBase MoveDown { get; private set; }

        #endregion

        #region Actions

        //
        // Generate a puzzle of a specific difficulty
        //
        public void OnGeneratePuzzle(EDifficulty difficulty)
        {
            puzzle = creator.GeneratePuzzle(difficulty);

            Check.SetCanExecute(true);
            Hint.SetCanExecute(true);

            OnReset();
        }

        //
        // Process reset
        //
        private void OnReset()
        {
            userSolution = new Puzzle(puzzle, null);
            userTable = creator.Evaluate(userSolution);

            for (uint i = 0; i < puzzle.Givens.Length; i++)
            {
                uint val = puzzle.Givens[i];
                UICells[i].SetNumber(val);
                UICells[i].SetGiven(val != 0);
                UICells[i].SetSelected(false);
                UICells[i].ResetPossibles();
            }

            selectedCell = uint.MaxValue;
            lastSelectedCell = uint.MaxValue;
            logEntries.Clear();
            Undo.SetCanExecute(false);
            Redo.SetCanExecute(false);
            Reset.SetCanExecute(false);
            logIndex = 0;
        }

        //
        // Process check
        //
        private void OnCheck()
        {
            // Flag any value that the user guessed and is different in the solution
            for(uint pos = 0; pos < puzzle.Solutions.Length; pos++)
            {
                if (userSolution.Givens[pos] != 0 &&
                    userSolution.Givens[pos] != puzzle.Solutions[pos])
                {
                    UICells[pos].SetNumberStatus(true);
                }
            }
        }

        //
        // Process hint
        //
        private void OnHint()
        {
            if (userTable != null)
            {
                var tmpPuzzle = new Puzzle(userSolution, null);
                var pos = creator.GetHint(tmpPuzzle);
                if (pos != null)
                {
                    SelectCell(pos.Cell);
                    showingPicker = false;
                }
            }
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
                if (selectedCell == pos && puzzle.Givens[pos] == 0 && !showingPicker)
                {
                    showPicker = true;
                }

                // Flip value for next time
                showingPicker = !showingPicker;
            }

            return showPicker;
        }

        //
        // Process user setting a number from keyoard
        //
        public void OnKbdNumber(object arg)
        {
            OnKbdNumberOrPossible(arg, false);
        }

        public void OnKbdPossible(object arg)
        {
            OnKbdNumberOrPossible(arg, true);
        }

        private void OnKbdNumberOrPossible(object arg, bool possible)
        {
            if (puzzle != null && selectedCell != uint.MaxValue && arg is string numString)
            {
                pickPossibilities = possible;
                OnSetNumber(uint.Parse(numString));
            }
        }

        //
        // Process user using the arrow keys
        //
        public void OnMoveRight()
        {
            var curPos = GetSelectedPosition();
            if (curPos.Column != Position.ROW_COL_SEC_SIZE - 1)
            {
                var newPos = Position.GetCell(curPos.Row, curPos.Column + 1);
                SelectCell(newPos.Cell);
            }
        }

        public void OnMoveLeft()
        {
            var curPos = GetSelectedPosition();
            if (curPos.Column != 0)
            {
                var newPos = Position.GetCell(curPos.Row, curPos.Column - 1);
                SelectCell(newPos.Cell);
            }
        }

        public void OnMoveDown()
        {
            var curPos = GetSelectedPosition();
            if (curPos.Row != Position.ROW_COL_SEC_SIZE - 1)
            {
                var newPos = Position.GetCell(curPos.Row + 1, curPos.Column);
                SelectCell(newPos.Cell);
            }
        }

        public void OnMoveUp()
        {
            var curPos = GetSelectedPosition();
            if (curPos.Row != 0)
            {
                var newPos = Position.GetCell(curPos.Row - 1, curPos.Column);
                SelectCell(newPos.Cell);
            }
        }

        private Position GetSelectedPosition()
        {
            uint cell = selectedCell;
            if (cell == uint.MaxValue)
            {
                cell = lastSelectedCell;
            }
            if (cell == uint.MaxValue)
            {
                cell = Position.BOARD_SIZE / 2;
            }

            return Position.GetCellFromCell(cell);
        }

        //
        // Process user setting a number via the picker
        //
        public bool OnSetNumber(uint number)
        {
            bool keepContextMenuOpen = false;

            if (puzzle != null &&
                selectedCell != uint.MaxValue &&
                puzzle.Givens[selectedCell] == 0)
            {
                var cell = UICells[selectedCell];
                uint position = selectedCell;
                uint oldNumber = cell.Number == "" ? 0 : uint.Parse(cell.Number);
                uint oldPossibles = cell.GetPossiblesAsBitList();
                uint newNumber;
                uint newPossibles;

                if (pickPossibilities)
                {
                    // update possibles
                    SetPossible(selectedCell, number);

                    // For undo/redo
                    newNumber = oldNumber;
                    newPossibles = cell.GetPossiblesAsBitList();

                    // Keep the context menu open
                    keepContextMenuOpen = true;
                }
                else
                {
                    // Set the number in the cell, and evaluate
                    SetNumber(selectedCell, number);

                    // For undo/redo
                    newNumber = number;
                    newPossibles = cell.GetPossiblesAsBitList();

                    // Unselect the cell
                    cell.SetSelected(false);
                    lastSelectedCell = selectedCell;
                    selectedCell = uint.MaxValue;

                    // close context menu
                    keepContextMenuOpen = false;
                }

                // Log the change
                LogAction(position, oldNumber, newNumber, oldPossibles, newPossibles);
            }

            return keepContextMenuOpen;
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
                    Reset.SetCanExecute(false);
                }

                SetNumber(entryToUndo.Position, entryToUndo.OldNumber);
                UICells[entryToUndo.Position].SetPossiblesAsBitList(entryToUndo.OldPossibles);
                UpdateAllNumberAndPossibleStatus();

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

                SetNumber(entryToRedo.Position, entryToRedo.NewNumber);
                UICells[entryToRedo.Position].SetPossiblesAsBitList(entryToRedo.NewPossibles);
                UpdateAllNumberAndPossibleStatus();

                Undo.SetCanExecute(true);
                Reset.SetCanExecute(true);
            }
        }

        private void LogAction(uint position, uint oldNumber, uint newNumber, uint oldPossibles, uint newPossibles)
        {
            var entry = new LogEntry(position, oldNumber, newNumber, oldPossibles, newPossibles);
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
            Reset.SetCanExecute(true);
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
            userSolution.Givens[pos] = number;

            // Evaluate the correctness
            userTable = creator.Evaluate(userSolution);
            if (userTable == null)
            {
                // User selection makes puzzle impossible!
                cell.SetNumberStatus(true);
            }
            else
            {
                cell.SetNumberStatus(false);

                // Update all number and all possibles
                UpdateAllNumberAndPossibleStatus();

                if (userTable.IsSolved)
                {
                    // Finished
                    PuzzleSolved?.Invoke(this, EventArgs.Empty);
                }
                else
                {
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
                cell.SetPossibles(number);

                // If we have a table, check if this number is possible
                if (userTable != null)
                {
                    UpdateOnePossibleStatus(Position.GetCellFromCell(position));
                }
            }
            else
            {
                cell.ResetPossibles();
            }
        }

        private void UpdateAllNumberAndPossibleStatus()
        {
            if (userTable != null)
            {
                foreach (var pos in Position.AllPositions)
                {
                    UpdateOnePossibleStatus(pos);
                    UICells[pos.Cell].SetNumberStatus(false);
                }
            }
        }

        private void UpdateOnePossibleStatus(Position pos)
        {
            if (userTable != null)
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
        }

        private void SelectCell(uint pos)
        {
            if (selectedCell != uint.MaxValue && selectedCell != pos)
            {
                // Unselect previously selected cell
                lastSelectedCell = selectedCell;
                UICells[selectedCell].SetSelected(false);
            }

            // Select this cell
            selectedCell = pos;
            UICells[pos].SetSelected(true);
        }

        #endregion
    }
}
