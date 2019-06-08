using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sudoku.Game
{
    public class Solver
    {
        #region private members

        private Table solution = new Table();
        private List<LogEntry> logEntries = new List<LogEntry>();
        protected Random random = new Random();

        private bool findFirstMark;
        private Position firstMarkPosition;

        #endregion

        #region Debug properties

        public bool Verbose { get; set; }
        public bool TrackTiming { get; set; }
        protected bool LogDecisions;

        #endregion

        #region Solver

        /// <summary>
        /// Entry point to solve a sudoku puzzle
        /// </summary>
        /// <returns>statistics if solved, null otherwise</returns>
        public Puzzle Solve(Puzzle puzzle, bool withoutGuesses)
        {
            // Copy the puzzle into "solution"
            if (!CopyPuzzleToSolution(puzzle))
            {
                // Impossible puzzle
                return null;
            }

            // Now Solve the puzzle
            if (!Solve(2, withoutGuesses))
            {
                // Imppossible puzzle
                return null;
            }

            // Make a new puzzle from the solution
            var statistics = LogDecisions ? new Statistics(logEntries) : null;
            var solvedPuzzle = new Puzzle(solution, statistics);

            return solvedPuzzle;
        }

        //
        // Take a puzzle and evaluate the possiblities
        //
        public Table Evaluate(Puzzle puzzle)
        {
            // Copy the puzzle into "solution"
            if (!CopyPuzzleToSolution(puzzle))
            {
                // Impossible puzzle
                return null;
            }

            return new Table(solution);
        }

        public Position GetHint(Puzzle puzzle)
        {
            if (!CopyPuzzleToSolution(puzzle))
            {
                // Impossible puzzle
                return null;
            }

            for (findFirstMark = true; findFirstMark;)
            {
                if (!SingleSolveMove(2))
                {
                    // Unsolvable puzzle
                    return null;
                }
            }

            return firstMarkPosition;
        }

        //
        // Copy a naked puzzle into a solution, removing possiblities as the cells are marked
        //
        private bool CopyPuzzleToSolution(Puzzle puzzle)
        {
            uint round = 1;

            // Reset our internal state
            Reset();

            // Copy the puzzle into "solution"
            foreach (var pos in Position.AllPositions)
            {
                uint val = puzzle.Cells[pos.Cell];
                if (val != 0)
                {
                    if (!solution[pos].IsPossible(val))
                    {
                        // Impossible puzzle right off the start
                        return false;
                    }

                    Mark(pos, val, round, EMarkType.GIVEN);
                }
            }

            return true;
        }

        //
        // Recursively solve a puzzle
        //
        private bool Solve(uint round, bool withoutGuesses)
        {
            // Exhaust all deductions
            while (SingleSolveMove(round))
            {
                if (solution.IsSolved)
                {
                    // Puzzle is solved
                    return true;
                }

                if (IsImpossible)
                {
                    // If we are hollowing out a puzzle that we know is possible, we have a bug
                    if (withoutGuesses)
                    {
                        throw new InvalidOperationException("Known possible puzzle found impossible");
                    }
                    return false;
                }
            }

            // We can't solve without guessing, meaning the puzzle has
            // more than one solution.
            if (withoutGuesses)
            {
                // Indicate that the puzzle has more than one solution
                return false;
            }

            // Get all guesses
            var guesses = Guesses;

            // Randomize the guesses
            Shuffle(guesses);

            uint guessRound = round + 1;
            uint nextRound = round + 2;

            // Try all guesses
            foreach(var guess in guesses)
            {
                // Make a guess
                Mark(guess.Key, guess.Value, guessRound, EMarkType.GUESS);

                // Not sure this actually happens
                if (IsImpossible)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    Rollback(round);
                    continue;
                }

                // Solve recursively
                if (Solve(nextRound, withoutGuesses))
                {
                    return true;
                }

                // That didn't work. Roll back and try another guess
                Rollback(nextRound);
                Rollback(guessRound);

            }

            return false;
        }

        //
        // Return an array of guesses
        //
        private List<KeyValuePair<Position,uint>> Guesses
        {
            get
            {
                // Compute all the guesses
                var guesses = new List<KeyValuePair<Position, uint>>();
                foreach(var pos in PositionsWithFewestPossibilities)
                {
                    foreach(var cellValue in Cell.AllValidCellValues)
                    {
                        if (solution[pos].IsPossible(cellValue))
                        {
                            guesses.Add(new KeyValuePair<Position, uint>(pos, cellValue));
                        }
                    }
                }

                return guesses;
            }
        }

        //
        // Build a list of the positions with fewest possibilities
        //
        private List<Position> PositionsWithFewestPossibilities
        {
            get
            {
                uint minPossibilities = uint.MaxValue;
                var bestPositions = new List<Position>();

                foreach (var pos in Position.AllPositions)
                {
                    if (!solution[pos].IsMarked)
                    {
                        uint count = solution[pos].CountPossibilities();
                        if (count < minPossibilities)
                        {
                            minPossibilities = count;
                            bestPositions.Clear();
                            bestPositions.Add(pos);
                        }
                        else if (count == minPossibilities)
                        {
                            bestPositions.Add(pos);
                        }
                    }
                }

                return bestPositions;
            }
        }

        // Check if solution has become impossible
        private bool IsImpossible
        {
            get
            {
                foreach (var pos in Position.AllPositions)
                {
                    // Look for unmarked cells with no possibilities left
                    if (!solution[pos].IsMarked && solution[pos].CountPossibilities() == 0)
                    {
                        if (Verbose)
                        {
                            Console.WriteLine("Impossible at pos " + pos.ToString());
                            Console.Write(Print(solution, EPrintStyle.READABLE));
                        }
                        return true;
                    }
                }

                return false;
            }
        }

        // Reinstate all the possibilities removed by a round, and also unmark all cells marked during the round
        private void Rollback(uint round)
        {
            Log(round, EMarkType.ROLLBACK, null, uint.MaxValue);

            foreach(var pos in Position.AllPositions)
            {
                solution[pos].Unmark(round);
            }
        }

        #endregion

        #region Sieves

        private bool SingleSolveMove(uint round)
        {
            if (OnlyPossibilityForCell(round)) return true;
            if (OnlyValue(round, EMarkType.HIDDEN_SINGLE_SECTION)) return true;
            if (OnlyValue(round, EMarkType.HIDDEN_SINGLE_ROW)) return true;
            if (OnlyValue(round, EMarkType.HIDDEN_SINGLE_COLUMN)) return true;
            if (HandleNakedPairs(round)) return true;
            if (PointingRowAndColumnReduction(round)) return true;
            if (RowBoxReduction(round)) return true;
            if (ColumnBoxReduction(round)) return true;
            if (HiddenPairInRow(round)) return true;
            if (HiddenPairInColumn(round)) return true;
            if (HiddenPairInSection(round)) return true;
            return false;
        }

        //
        // Mark exactly one cell that has a single possibility, if such a cell exists.
        // This type of cell is often called a "single"
        //
        private bool OnlyPossibilityForCell(uint round)
        {
            // go through the whole board
            foreach (var position in Position.AllPositions)
            {
                var cell = solution[position];

                // Find unsolved locations
                if (!cell.IsMarked)
                {
                    // Go through all possible values
                    uint count = 0;
                    uint cellVal = uint.MaxValue;
                    foreach (var cv in Cell.AllValidCellValues)
                    {
                        // See how many possibilities are left
                        if (cell.IsPossible(cv))
                        {
                            // This cell value is possible
                            count += 1;
                            cellVal = cv;
                        }
                    }

                    // If we found exactly one possible solution, then mark it
                    if (count == 1)
                    {
                        Mark(position, cellVal, round, EMarkType.SINGLE);
                        return true;
                    }
                }
            }
            return false;
        }

        //
        // Mark exactly one cell which is the only possible value for a row, column, or section,
        // if such cell exists. This type of cell is often called a section "hidden single"
        //
        private bool OnlyValue(uint round, EMarkType type)
        {
            // Decide what to scan based on type
            IEnumerable<Position> mainIterator = null;
            switch(type)
            {
                case EMarkType.HIDDEN_SINGLE_ROW: mainIterator = Position.AllRowStarts; break;
                case EMarkType.HIDDEN_SINGLE_COLUMN: mainIterator = Position.AllColumnStarts; break;
                case EMarkType.HIDDEN_SINGLE_SECTION: mainIterator = Position.AllSectionStarts; break;
            }

            // Scan all row/column/sections
            foreach (var main in mainIterator)
            {
                // Decide what positions to scan next
                IEnumerable<Position> subIterator = null;
                switch (type)
                {
                    case EMarkType.HIDDEN_SINGLE_ROW: subIterator = main.AllColumnCells; break;
                    case EMarkType.HIDDEN_SINGLE_COLUMN: subIterator = main.AllRowCells; break;
                    case EMarkType.HIDDEN_SINGLE_SECTION: subIterator = main.AllSectionCells; break;
                }

                // scan all possible values
                foreach (var cellValue in Cell.AllValidCellValues)
                {
                    int count = 0;
                    Position position = null;

                    // Scan the row/column/section
                    foreach (var pos in subIterator)
                    {
                        // Look for possibles
                        if (solution[pos].IsPossible(cellValue))
                        {
                            position = pos;
                            count += 1;
                        }
                    }

                    // If this value is possible for only one position in the row/column/section,
                    // we have found our hidden single
                    if (count == 1)
                    {
                        Mark(position, cellValue, round, type);
                        return true;
                    }
                }
            }
            return false;
        }

        //
        // When 2 cells can only contain the same 2 numbers, and those cells
        // are in the same row or column or section, then no other cell
        // in the row/column/section can have those numbers
        //
        private bool HandleNakedPairs(uint round)
        {
            // Iterate over the whole board
            foreach (var position1 in Position.AllPositions)
            {
                // Count the number of open possiblities for this position
                uint numPossibilities1 = solution[position1].CountPossibilities();

                // We might have a naked pair if the possiblities are exactly 2
                if (numPossibilities1 == 2)
                {
                    // Scan the whole board trying to find a location that has the exact same 2 open possibilities
                    foreach (var position2 in Position.AllPositions)
                    {
                        if (!position1.Equals(position2))
                        {
                            uint numPossibilities2 = solution[position2].CountPossibilities();
                            if (numPossibilities2 == 2 && solution[position1].SamePossibilities(solution[position2]))
                            {
                                // Found another cell with the same 2 possibilities

                                // See if in the same row as the other
                                if (position1.Row == position2.Row &&
                                    HandleNakedPairs(round, position1, position2, EMarkType.NAKED_PAIR_ROW))
                                {
                                    return true;
                                }

                                // See if in the same column as the other
                                if (position1.Column == position2.Column &&
                                    HandleNakedPairs(round, position1, position2, EMarkType.NAKED_PAIR_COLUMN))
                                {
                                    return true;
                                }

                                // See if in the same section as the other
                                if (position1.Section == position2.Section &&
                                    HandleNakedPairs(round, position1, position2, EMarkType.NAKED_PAIR_SECTION))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        //
        // Naked pair processing: Remove possibilities from row or column or section
        //
        private bool HandleNakedPairs(uint round, Position position1, Position position2, EMarkType type)
        {
            bool doneSomething = false;
            IEnumerable<Position> enumerator = null;
            switch(type)
            {
                case EMarkType.NAKED_PAIR_ROW: enumerator = position1.AllColumnCells; break;
                case EMarkType.NAKED_PAIR_COLUMN: enumerator = position1.AllRowCells; break;
                case EMarkType.NAKED_PAIR_SECTION: enumerator = position1.AllSectionCells; break;
            }

            // Remove the possibilities present in position1 and position2 from the row or column or section
            foreach (var position3 in enumerator)
            {
                if (!position1.Equals(position3) && !position2.Equals(position3))
                {
                    doneSomething |= RemovePossibilitiesInOneFromTwo(position1, position3, round);
                }
            }

            if (doneSomething)
            {
                Log(round, type, position1);
            }

            return doneSomething;
        }

        //
        // Remove the possibilities at position1 from position2
        //
        private bool RemovePossibilitiesInOneFromTwo(Position position1, Position position2, uint round)
        {
            bool doneSomething = false;

            foreach (var cv in Cell.AllValidCellValues)
            {
                if (solution[position1].IsPossible(cv))
                {
                    doneSomething |= RemovePossiblity(position2, cv, round);
                }
            }

            return doneSomething;
        }

        //
        // If a value can only live in a specific row/column of a section, then it necessarily lives in
        // another row/column for the 2 other sections that share this row/column. 
        //
        private bool PointingRowAndColumnReduction(uint round)
        {
            // Iterate on all values
            foreach (var cellValue in Cell.AllValidCellValues)
            {
                // Iterate on all sections
                foreach (var sectionPos in Position.AllSectionStarts)
                {
                    uint row = uint.MaxValue;
                    bool possibleInDifferentRow = false;
                    uint column = uint.MaxValue;
                    bool possibleInDifferentColumn = false;

                    // Iterate on all cells of the section
                    foreach (var pos in sectionPos.AllSectionCells)
                    {
                        if (solution[pos].IsPossible(cellValue))
                        {
                            if (row == uint.MaxValue)
                            {
                                // value is possible in this row
                                row = pos.Row;
                            }
                            else if (row == pos.Row)
                            {
                                // value is possible in the same row - do nothing
                            }
                            else
                            {
                                // value is possible but in a different row - failed
                                possibleInDifferentRow = true;
                            }

                            if (column == uint.MaxValue)
                            {
                                // value is possible in this column
                                column = pos.Column;
                            }
                            else if (column == pos.Column)
                            {
                                // value is possible in the same column - do nothing
                            }
                            else
                            {
                                // value is possible but in a different column - failed
                                possibleInDifferentColumn = true;
                            }
                        }
                    }

                    // See if all the possibles for the current value are in the same row
                    if (row != uint.MaxValue && !possibleInDifferentRow)
                    {
                        // Yes. So the value lives somewhere on this row in this section, therefore
                        // it does not live in this row in the sections sharing the same row
                        bool doneSomething = false;
                        foreach(var rowPos in Position.GetCellFromRow(row).AllColumnCells)
                        {
                            if (rowPos.Section != sectionPos.Section)
                            {
                                doneSomething |= RemovePossiblity(rowPos, cellValue, round);
                            }
                        }

                        if (doneSomething)
                        {
                            Log(round, EMarkType.POINTING_PAIR_TRIPLE_ROW, sectionPos, cellValue);
                            return true;
                        }
                    }

                    // See if all the possibles for the current value are in the same column
                    if (column != uint.MaxValue && !possibleInDifferentColumn)
                    {
                        // Yes. So the value lives somewhere on this column in this section, therefore
                        // it does not live in this column in the sections sharing the same column
                        bool doneSomething = false;
                        foreach (var columnPos in Position.GetCellFromColumn(column).AllRowCells)
                        {
                            if (columnPos.Section != sectionPos.Section)
                            {
                                doneSomething |= RemovePossiblity(columnPos, cellValue, round);
                            }
                        }

                        if (doneSomething)
                        {
                            Log(round, EMarkType.POINTING_PAIR_TRIPLE_COLUMN, sectionPos, cellValue);
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        //
        // If a value can only live in a specific section of a row, then it cannot live in
        // the 2 other rows of this section. 
        //
        private bool RowBoxReduction(uint round)
        {
            // Iterate on all values
            foreach (var cellValue in Cell.AllValidCellValues)
            {
                // Iterate on all rows
                foreach (var rowPosition in Position.AllRowStarts)
                {
                    uint rowBox = uint.MaxValue;

                    // Iterate on the column
                    foreach (var pos in rowPosition.AllColumnCells)
                    {
                        if (solution[pos].IsPossible(cellValue))
                        {
                            if (rowBox == uint.MaxValue)
                            {
                                // value is possible in this section
                                rowBox = pos.Section;
                            }
                            else if (rowBox == pos.Section)
                            {
                                // value is possible in the same section - do nothing
                            }
                            else
                            {
                                // value is possible but in a different section
                                rowBox = uint.MaxValue;
                                break;
                            }
                        }
                    }

                    // See if all the possibles for the current value are in the same section
                    if (rowBox != uint.MaxValue)
                    {
                        // Yes. So the value lives somewhere in this section and row, therefore
                        // it does not live in the other rows of this section
                        bool doneSomething = false;
                        foreach (var sectionPos in Position.GetCellFromSection(rowBox).AllSectionCells)
                        {
                            if (rowPosition.Row != sectionPos.Row)
                            {
                                doneSomething |= RemovePossiblity(sectionPos, cellValue, round);
                            }
                        }

                        if (doneSomething)
                        {
                            Log(round, EMarkType.ROW_BOX, rowPosition, cellValue);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //
        // If a value can only live in a specific section of a column, then it cannot live in
        // the 2 other columns of this section. 
        //
        private bool ColumnBoxReduction(uint round)
        {
            // Iterate on all values
            foreach (var cellValue in Cell.AllValidCellValues)
            {
                // Iterate on all columns
                foreach (var columnPosition in Position.AllColumnStarts)
                {
                    uint columnBox = uint.MaxValue;

                    // Iterate on the row
                    foreach (var pos in columnPosition.AllRowCells)
                    {
                        if (solution[pos].IsPossible(cellValue))
                        {
                            if (columnBox == uint.MaxValue)
                            {
                                // value is possible in this section
                                columnBox = pos.Section;
                            }
                            else if (columnBox == pos.Section)
                            {
                                // value is possible in the same section - do nothing
                            }
                            else
                            {
                                // value is possible but in a different section
                                columnBox = uint.MaxValue;
                                break;
                            }
                        }
                    }

                    // See if all the possibles for the current value are in the same section
                    if (columnBox != uint.MaxValue)
                    {
                        // Yes. So the value lives somewhere in this section and column, therefore
                        // it does not live in the other columns of this section
                        bool doneSomething = false;
                        foreach (var sectionPos in Position.GetCellFromSection(columnBox).AllSectionCells)
                        {
                            if (columnPosition.Column != sectionPos.Column)
                            {
                                doneSomething |= RemovePossiblity(sectionPos, cellValue, round);
                            }
                        }

                        if (doneSomething)
                        {
                            Log(round, EMarkType.COLUMN_BOX, columnPosition, cellValue);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //
        // If 2 values are possible exactly twice and at the same spots in a row,
        // then no other values are possible at those spots
        //
        private bool HiddenPairInRow(uint round)
        {
            // Iterate on all rows
            foreach(var rowPosition in Position.AllRowStarts)
            {
                // Iterate on all values
                foreach (var cellValue1 in Cell.AllValidCellValues)
                {
                    // Check if the current value is possible exactly twice on this row
                    Position pos1_1 = null;
                    Position pos1_2 = null;

                    // Iterate on all column
                    foreach (var pos in rowPosition.AllColumnCells)
                    {
                        if (solution[pos].IsPossible(cellValue1))
                        {
                            if (pos1_1 == null)
                            {
                                // Possible a first time
                                pos1_1 = pos;
                            }
                            else if (pos1_2 == null)
                            {
                                // Possible a second time
                                pos1_2 = pos;
                            }
                            else
                            {
                                // Possible more than twice - abort 
                                pos1_1 = null;
                                pos1_2 = null;
                                break;
                            }
                        }
                    }

                    if (pos1_1 != null && pos1_2 != null)
                    {
                        // Try to find another value in the row that is possible at exactly the same positions
                        foreach (var cellValue2 in Cell.AllValidCellValues)
                        {
                            if (cellValue2 > cellValue1)
                            {
                                Position pos2_1 = null;
                                Position pos2_2 = null;

                                // Iterate on all column
                                foreach (var pos in rowPosition.AllColumnCells)
                                {
                                    if (solution[pos].IsPossible(cellValue2))
                                    {
                                        if (pos2_1 == null)
                                        {
                                            // Possible a first time
                                            pos2_1 = pos;
                                        }
                                        else if (pos2_2 == null)
                                        {
                                            // Possible a second time
                                            pos2_2 = pos;
                                        }
                                        else
                                        {
                                            // Possible more than twice - abort 
                                            pos2_1 = null;
                                            pos2_2 = null;
                                            break;
                                        }
                                    }
                                }

                                // Check if we actually got a pair
                                if (pos1_1.Equals(pos2_1) && pos1_2.Equals(pos2_2))
                                {
                                    // This is a pair, eliminate all other possibilites from those 2 locations
                                    bool doneSomething = false;
                                    foreach (var cellValue3 in Cell.AllValidCellValues)
                                    {
                                        if (cellValue3 != cellValue1 && cellValue3 != cellValue2)
                                        {
                                            doneSomething |= RemovePossiblity(pos1_1, cellValue3, round);
                                            doneSomething |= RemovePossiblity(pos1_2, cellValue3, round);
                                        }
                                    }

                                    if (doneSomething)
                                    {
                                        Log(round, EMarkType.HIDDEN_PAIR_ROW, pos1_1, cellValue1);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        //
        // If 2 values are possible exactly twice and at the same spots in a column,
        // then no other values are possible at those spots
        //
        private bool HiddenPairInColumn(uint round)
        {
            // Iterate on all rows
            foreach (var columnPosition in Position.AllColumnStarts)
            {
                // Iterate on all values
                foreach (var cellValue1 in Cell.AllValidCellValues)
                {
                    // Check if the current value is possible exactly twice on this column
                    Position pos1_1 = null;
                    Position pos1_2 = null;

                    // Iterate on all rows
                    foreach (var pos in columnPosition.AllRowCells)
                    {
                        if (solution[pos].IsPossible(cellValue1))
                        {
                            if (pos1_1 == null)
                            {
                                // Possible a first time
                                pos1_1 = pos;
                            }
                            else if (pos1_2 == null)
                            {
                                // Possible a second time
                                pos1_2 = pos;
                            }
                            else
                            {
                                // Possible more than twice - abort 
                                pos1_1 = null;
                                pos1_2 = null;
                                break;
                            }
                        }
                    }

                    if (pos1_1 != null && pos1_2 != null)
                    {
                        // Try to find another value in the column that is possible at exactly the same positions
                        foreach (var cellValue2 in Cell.AllValidCellValues)
                        {
                            if (cellValue2 > cellValue1)
                            {
                                Position pos2_1 = null;
                                Position pos2_2 = null;

                                // Iterate on all rows
                                foreach (var pos in columnPosition.AllRowCells)
                                {
                                    if (solution[pos].IsPossible(cellValue2))
                                    {
                                        if (pos2_1 == null)
                                        {
                                            // Possible a first time
                                            pos2_1 = pos;
                                        }
                                        else if (pos2_2 == null)
                                        {
                                            // Possible a second time
                                            pos2_2 = pos;
                                        }
                                        else
                                        {
                                            // Possible more than twice - abort 
                                            pos2_1 = null;
                                            pos2_2 = null;
                                            break;
                                        }
                                    }
                                }

                                // Check if we actually got a pair
                                if (pos1_1.Equals(pos2_1) && pos1_2.Equals(pos2_2))
                                {
                                    // This is a pair, eliminate all other possibilites from those 2 locations
                                    bool doneSomething = false;
                                    foreach (var cellValue3 in Cell.AllValidCellValues)
                                    {
                                        if (cellValue3 != cellValue1 && cellValue3 != cellValue2)
                                        {
                                            doneSomething |= RemovePossiblity(pos1_1, cellValue3, round);
                                            doneSomething |= RemovePossiblity(pos1_2, cellValue3, round);
                                        }
                                    }

                                    if (doneSomething)
                                    {
                                        Log(round, EMarkType.HIDDEN_PAIR_COLUMN, pos1_1, cellValue1);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        //
        // If 2 values are possible exactly twice and at the same spots in a section,
        // then no other values are possible at those spots
        //
        private bool HiddenPairInSection(uint round)
        {
            // Iterate on all sections
            foreach (var sectionPosition in Position.AllSectionStarts)
            {
                // Iterate on all values
                foreach (var cellValue1 in Cell.AllValidCellValues)
                {
                    // Check if the current value is possible exactly twice on this section
                    Position pos1_1 = null;
                    Position pos1_2 = null;

                    // Iterate on all cells of the section
                    foreach (var pos in sectionPosition.AllSectionCells)
                    {
                        if (solution[pos].IsPossible(cellValue1))
                        {
                            if (pos1_1 == null)
                            {
                                // Possible a first time
                                pos1_1 = pos;
                            }
                            else if (pos1_2 == null)
                            {
                                // Possible a second time
                                pos1_2 = pos;
                            }
                            else
                            {
                                // Possible more than twice - abort 
                                pos1_1 = null;
                                pos1_2 = null;
                                break;
                            }
                        }
                    }

                    if (pos1_1 != null && pos1_2 != null)
                    {
                        // Try to find another value in the section that is possible at exactly the same positions
                        foreach (var cellValue2 in Cell.AllValidCellValues)
                        {
                            if (cellValue2 > cellValue1)
                            {
                                Position pos2_1 = null;
                                Position pos2_2 = null;

                                // Iterate on all rows
                                foreach (var pos in sectionPosition.AllSectionCells)
                                {
                                    if (solution[pos].IsPossible(cellValue2))
                                    {
                                        if (pos2_1 == null)
                                        {
                                            // Possible a first time
                                            pos2_1 = pos;
                                        }
                                        else if (pos2_2 == null)
                                        {
                                            // Possible a second time
                                            pos2_2 = pos;
                                        }
                                        else
                                        {
                                            // Possible more than twice - abort 
                                            pos2_1 = null;
                                            pos2_2 = null;
                                            break;
                                        }
                                    }
                                }

                                // Check if we actually got a pair
                                if (pos1_1.Equals(pos2_1) && pos1_2.Equals(pos2_2))
                                {
                                    // This is a pair, eliminate all other possibilites from those 2 locations
                                    bool doneSomething = false;
                                    foreach (var cellValue3 in Cell.AllValidCellValues)
                                    {
                                        if (cellValue3 != cellValue1 && cellValue3 != cellValue2)
                                        {
                                            doneSomething |= RemovePossiblity(pos1_1, cellValue3, round);
                                            doneSomething |= RemovePossiblity(pos1_2, cellValue3, round);
                                        }
                                    }

                                    if (doneSomething)
                                    {
                                        Log(round, EMarkType.HIDDEN_PAIR_SECTION, pos1_1, cellValue1);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region Utilities

        //
        // Mark the given position with the given value
        //
        private void Mark(Position position, uint val, uint round, EMarkType type)
        {
            // Consistency checks
            if (solution[position].IsMarked)
            {
                throw new InvalidOperationException("Position already marked");
            }

            if (!solution[position].IsPossible(val))
            {
                throw new InvalidOperationException("Position not possible");
            }

            if (findFirstMark)
            {
                findFirstMark = false;
                firstMarkPosition = position;
            }

            // Enter the value in the solution, adorned with the round number (for rollback)
            solution[position].Mark(val, round, type);

            // Make this value impossible anywhere in the row
            foreach (var pos in position.AllRowCells)
            {
                solution[pos].SetImpossible(val, round);
            }

            // Make this value impossible anywhere in the column
            foreach (var pos in position.AllColumnCells)
            {
                solution[pos].SetImpossible(val, round);
            }

            // Make this value impossible anywhere in the section
            foreach (var pos in position.AllSectionCells)
            {
                solution[pos].SetImpossible(val, round);
            }

            Log(round, type, position, val);
        }

        private void Log(uint round, EMarkType type, Position position = null, uint value = uint.MaxValue)
        {
            if (LogDecisions || Verbose)
            {
                var logEntry = new LogEntry(round, type, position, value);

                if (LogDecisions)
                {
                    logEntries.Add(logEntry);
                }

                if (Verbose)
                {
                    Console.WriteLine(logEntry);
                }
            }
        }

        private bool RemovePossiblity(Position position, uint value, uint round)
        {
            if (solution[position].SetImpossible(value, round))
            {
                if (Verbose)
                {
                    Console.WriteLine($"Round {round}: Set value {value} impossible for {position}");
                }

                return true;
            }

            return false;
        }

        //
        // Reset everything
        //
        private void Reset()
        {
            solution.Reset();
            logEntries.Clear();
        }

        // Randomize a list
        protected void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                var value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }

        // Print a puzzle
        protected string Print(IPrintSource source, EPrintStyle style)
        {
            string str = "";

            foreach (var pos in Position.AllPositions)
            {
                uint val = source.GetCellValue(pos.Cell);

                str += style == EPrintStyle.READABLE ? " " : "";
                str += val == 0 ? "." : val.ToString();

                if (pos.Column == Position.ROW_COL_SEC_SIZE - 1)
                {
                    str += style == EPrintStyle.READABLE || style == EPrintStyle.COMPACT ? Environment.NewLine : "";

                    if (pos.Row % Position.GRID_SIZE == Position.GRID_SIZE - 1 && style == EPrintStyle.READABLE)
                        str += "------|-----|-----" + Environment.NewLine;
                }
            }

            str += style == EPrintStyle.CSV ? "," : "";
            str += Environment.NewLine;

            return str;
        }


        #endregion

    }
}
