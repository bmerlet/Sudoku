using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    public class Creator : Solver
    {
        // The puzzle
        private Table puzzle = new Table();


        public bool GeneratePuzzle(ESymmetry symmetry = ESymmetry.NONE)
        {
            // Random symmetry if so desired
            if (symmetry == ESymmetry.RANDOM)
            {
                switch(random.Next(5))
                {
                    case 0: symmetry = ESymmetry.NONE; break;
                    case 1: symmetry = ESymmetry.ROTATE90; break;
                    case 2: symmetry = ESymmetry.ROTATE180; break;
                    case 3: symmetry = ESymmetry.MIRROR; break;
                    case 4: symmetry = ESymmetry.FLIP; break;
                }
            }

            // Don't record history while generating. ZZZ
            //bool recHistory = recordHistory;
            //setRecordHistory(false);
            //bool lHistory = logHistory;
            //setLogHistory(false);

            // Clear the puzzle and the solution area
            puzzle.Reset();
            SetPuzzle(puzzle);

            // ZZZ
            // Start by getting the randomness in order so that
            // each puzzle will be different from the last.
            // shuffleRandomArrays();

            // Solve all the way. This invents a puzzle in 'solution'.
            var statistics = Solve();

            // ZZZZ
            Console.WriteLine(solution.Print(EPrintStyle.READABLE));

            // ZZZ Why do that only for symmetry NONE???
            //if (symmetry == SudokuBoard::NONE)
            //{
            //    // Rollback any square for which it is obvious that
            //    // the square doesn't contribute to a unique solution
            //    // (ie, squares that were filled by logic rather
            //    // than by guess)
            //    rollbackNonGuesses();
            //}

            // ZZZ
            return true;
        }
#if false


		if (symmetry == SudokuBoard::NONE){
			// Rollback any square for which it is obvious that
			// the square doesn't contribute to a unique solution
			// (ie, squares that were filled by logic rather
			// than by guess)
			rollbackNonGuesses();
		}

		// Record all marked squares as the puzzle so
		// that we can call countSolutions without losing it.
		{for (int i=0; i<BOARD_SIZE; i++){
			puzzle[i] = solution[i];
		}}

		// Rerandomize everything so that we test squares
		// in a different order than they were added.
		shuffleRandomArrays();

		// Remove one value at a time and see if
		// the puzzle still has only one solution.
		// If it does, leave it out the point because
		// it is not needed.
		{for (int i=0; i<BOARD_SIZE; i++){
			// check all the positions, but in shuffled order
			int position = randomBoardArray[i];
			if (puzzle[position] > 0){
				int positionsym1 = -1;
				int positionsym2 = -1;
				int positionsym3 = -1;
				switch (symmetry){
					case ROTATE90:
						positionsym2 = rowColumnToCell(ROW_COL_SEC_SIZE-1-cellToColumn(position),cellToRow(position));
						positionsym3 = rowColumnToCell(cellToColumn(position),ROW_COL_SEC_SIZE-1-cellToRow(position));
					case ROTATE180:
						positionsym1 = rowColumnToCell(ROW_COL_SEC_SIZE-1-cellToRow(position),ROW_COL_SEC_SIZE-1-cellToColumn(position));
					break;
					case MIRROR:
						positionsym1 = rowColumnToCell(cellToRow(position),ROW_COL_SEC_SIZE-1-cellToColumn(position));
					break;
					case FLIP:
						positionsym1 = rowColumnToCell(ROW_COL_SEC_SIZE-1-cellToRow(position),cellToColumn(position));
					break;
					case RANDOM: // NOTE: Should never happen
					break;
					case NONE: // NOTE: No need to do anything
					break;
				}
				// try backing out the value and
				// counting solutions to the puzzle
				int savedValue = puzzle[position];
				puzzle[position] = 0;
				int savedSym1 = 0;
				if (positionsym1 >= 0){
					savedSym1 = puzzle[positionsym1];
					puzzle[positionsym1] = 0;
				}
				int savedSym2 = 0;
				if (positionsym2 >= 0){
					savedSym2 = puzzle[positionsym2];
					puzzle[positionsym2] = 0;
				}
				int savedSym3 = 0;
				if (positionsym3 >= 0){
					savedSym3 = puzzle[positionsym3];
					puzzle[positionsym3] = 0;
				}
				reset();
				if (countSolutions(2, true) > 1){
					// Put it back in, it is needed
					puzzle[position] = savedValue;
					if (positionsym1 >= 0 && savedSym1 != 0) puzzle[positionsym1] = savedSym1;
					if (positionsym2 >= 0 && savedSym2 != 0) puzzle[positionsym2] = savedSym2;
					if (positionsym3 >= 0 && savedSym3 != 0) puzzle[positionsym3] = savedSym3;
				}
			}
		}}

		// Clear all solution info, leaving just the puzzle.
		reset();

		// Restore recording history.
		setRecordHistory(recHistory);
		setLogHistory(lHistory);

		return true;

	}
#endif

    }
}
