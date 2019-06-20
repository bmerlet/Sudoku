using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.Game
{
    /// <summary>
    /// Sudoku puzzle, in raw form
    /// </summary>
    public class Puzzle : IPrintSource
    {
        // The puzzle itself
        public readonly uint[] Givens = new uint[Position.BOARD_SIZE];

        // The puzzle solution
        public readonly uint[] Solutions = new uint[Position.BOARD_SIZE];

        // Associated stats
        public readonly Statistics Statistics;

        // Build an empty puzzle
        public Puzzle()
        {

        }

        // Build a puzzle from a solution
        internal Puzzle(Table solution, Statistics statistics)
        {
            // Memorize values
            foreach (var pos in Position.AllPositions)
            {
                Givens[pos.Cell] = solution[pos].Value;
                Solutions[pos.Cell] = solution[pos].Value;
            }

            // memorize stats
            Statistics = statistics;
        }

        // Build a puzzle from another puzzle
        public Puzzle(Puzzle puzzle, Statistics statistics)
        {
            // Copy values
            Array.Copy(puzzle.Givens, Givens, Givens.Length);
            Array.Copy(puzzle.Solutions, Solutions, Solutions.Length);

            // memorize stats
            Statistics = statistics;
        }

        // IPrintSource interface
        public uint GetCellValue(uint cellPosition)
        {
            return Givens[cellPosition];
        }
    }
}
