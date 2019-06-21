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

        // The user guesses
        public readonly uint[] Guesses = new uint[Position.BOARD_SIZE];

        // The puzzle solution
        public readonly uint[] Solutions = new uint[Position.BOARD_SIZE];

        // Associated stats
        public Statistics Statistics { get; internal set; }

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
        public Puzzle(Puzzle src)
        {
            // Copy values
            Array.Copy(src.Givens, Givens, Givens.Length);
            Array.Copy(src.Guesses, Guesses, Guesses.Length);
            Array.Copy(src.Solutions, Solutions, Solutions.Length);
            Statistics = src.Statistics;
        }

        // IPrintSource interface
        public uint GetCellValue(uint cellPosition)
        {
            return Givens[cellPosition];
        }
    }
}
