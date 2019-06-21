using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sudoku.Game
{
    public class Creator : Solver
    {
        public const int BOARD_SIZE = Position.BOARD_SIZE;
        public const int ROW_SIZE = Position.ROW_COL_SEC_SIZE;

        public Puzzle GeneratePuzzle(EDifficulty difficulty)
        {
            Puzzle puzzle = null;
            //Severity = ESev.Interesting;

            if (difficulty == EDifficulty.UNKNOWN)
            {
                return GeneratePuzzle(difficulty, ESymmetry.RANDOM);
            }

            for (int retry = 0; retry < 1000; retry++)
            {
                puzzle = GeneratePuzzle(difficulty, ESymmetry.RANDOM);
                if (puzzle.Statistics.Difficulty == difficulty)
                {
                    Trace(ESev.Important, $"Got puzzle after {retry + 1} tries:");
                    Trace(ESev.Important, Print(puzzle, EPrintStyle.READABLE));
                    break;
                }

                Trace(ESev.Important, $"Got puzzle of difficulty {puzzle.Statistics.Difficulty} on try {retry} - retrying");
            }

            return puzzle;
        }

        public Puzzle GeneratePuzzle(EDifficulty difficulty = EDifficulty.UNKNOWN, ESymmetry symmetry = ESymmetry.NONE)
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

            Stopwatch stopwatch = TrackTiming ? new Stopwatch() : null;
            if (TrackTiming)
            {
                stopwatch.Start();
            }

            // Don't record history while generating.
            LogDecisions = false;

            // Create an empty puzzle
            var puzzle = new Puzzle();

            // Solve all the way. This invents a puzzle and returns it completely solved.
            puzzle = Solve(puzzle, false);

            var solveTime = TrackTiming ? stopwatch.Elapsed : TimeSpan.MinValue;

            Trace(ESev.Important, "Solved puzzle:");
            Trace(ESev.Important, Print(puzzle, EPrintStyle.READABLE));

            // Optimization: When not using symmetry to unmark multiple cells,
            // unmark all the cells that were found through logic instead of guessed
            if (symmetry == ESymmetry.NONE)
            {
            //    // Rollback any square for which it is obvious that
            //    // the square doesn't contribute to a unique solution
            //    // (ie, squares that were filled by logic rather
            //    // than by guess)
            //    rollbackNonGuesses();
            }

            // Create a position randomizer
            var randomizer = new PositionRandomizer(random);

            // If we want a specific difficulty, log the decisions we make in the solver so we can check
            // the difficulty after each hollowing out of the puzzle
            LogDecisions = difficulty != EDifficulty.UNKNOWN;

            // Try removing a 1,2 or 4 values at a time (depending on symmetry).
            // Put them back if the puzzle does not have a unique solution anymore.
            var positions = new List<Position>();
            var savedCells = new List<uint>();
            Puzzle solvedPuzzle = null;
            foreach(var posInOrder in Position.AllPositions)
            {
                // Get randomized position
                positions.Clear();
                savedCells.Clear();
                positions.Add(randomizer[posInOrder]);

                // Add whatever the symmetry requires
                switch(symmetry)
                {
                    case ESymmetry.ROTATE90:
                        positions.Add(positions[0].Opposite);
                        positions.Add(positions[0].VerticalFlip);
                        positions.Add(positions[0].HorizontalFlip);
                        break;

                    case ESymmetry.ROTATE180:
                        positions.Add(positions[0].Opposite);
                        break;

                    case ESymmetry.FLIP:
                        positions.Add(positions[0].VerticalFlip);
                        break;

                    case ESymmetry.MIRROR:
                        positions.Add(positions[0].HorizontalFlip);
                        break;
                }

                // Save the values and remove them from the puzzle
                foreach(var pos in positions)
                {
                    savedCells.Add(puzzle.Givens[pos.Cell]);
                    puzzle.Givens[pos.Cell] = 0;
                }

                // See if we can solve in one
                bool putValuesBack = false;
                var tmpSolvedPuzzle = Solve(puzzle, true);
                if (tmpSolvedPuzzle == null)
                {
                    // Nope, put the values back
                    putValuesBack = true;
                    Trace(ESev.Interesting, $"Put back {savedCells.Count} values");
                }
                else if (difficulty != EDifficulty.UNKNOWN)
                {
                    Trace(ESev.Interesting, $"Desired difficulty {difficulty}, after hollowing out: {tmpSolvedPuzzle.Statistics.Difficulty} pos {posInOrder}");

                    // See if we made the puzzle too difficult
                    if (tmpSolvedPuzzle.Statistics.Difficulty > difficulty)
                    {
                        // Yes, put the values back
                        putValuesBack = true;
                        Trace(ESev.Interesting, "Put back {savedCells.Count} values");
                    }
                }

                if (putValuesBack)
                {
                    // put back in reverse order, as some cells may be identical
                    for (int p = positions.Count - 1; p >= 0 ; p--)
                    {
                        puzzle.Givens[positions[p].Cell] = savedCells[p];
                    }
                }
                else
                {
                    solvedPuzzle = tmpSolvedPuzzle;
                }
            }

            var hollowOutTime = TrackTiming ? stopwatch.Elapsed : TimeSpan.MinValue;

            // Solve one last time to get the stats if we did not want a specific difficulty
            if (difficulty == EDifficulty.UNKNOWN)
            {
                LogDecisions = true;
                solvedPuzzle = Solve(puzzle, true);
            }

            if (TrackTiming)
            {
                var statsTime = stopwatch.Elapsed;
                stopwatch.Stop();
                Console.WriteLine($"Solve time: {solveTime.Milliseconds}ms, Hollow out time: {hollowOutTime.Milliseconds}ms, stat time: {statsTime.Milliseconds}, symmetry {symmetry}, difficulty {solvedPuzzle.Statistics.Difficulty}");
            }

            Trace(ESev.Important, "Final puzzle:");
            Trace(ESev.Important, Print(puzzle, EPrintStyle.READABLE));

            // Update stats
            puzzle.Statistics = solvedPuzzle.Statistics;

            return puzzle;
        }

        private uint[] GetRandomizer()
        {
            var randomizer = new List<uint>(Position.BOARD_SIZE);

            for (uint i = 0; i < randomizer.Count; i++)
            {
                randomizer.Add(i);
            }

            Shuffle(randomizer);

            return randomizer.ToArray();
        }
    }
}
