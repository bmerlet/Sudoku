using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    /// <summary>
    /// Sudoku puzzle, in raw form
    /// </summary>
    public class Puzzle : IPrintSource
    {
        // The puzzle itself
        public readonly uint[] Cells = new uint[Position.BOARD_SIZE];

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
                Cells[pos.Cell] = solution[pos].Value;
            }

            // memorize stats
            Statistics = statistics;
        }

        // Build a puzzle from another puzzle
        public Puzzle(Puzzle puzzle, Statistics statistics)
        {
            // Memorize values
            Array.Copy(puzzle.Cells, Cells, Cells.Length);

            // memorize stats
            Statistics = statistics;
        }

        // Build a puzzle from an array
        internal Puzzle(uint[] cells)
        {
            Array.Copy(cells, Cells, Cells.Length);
        }

        // IPrintSource interface
        public uint GetCellValue(uint cellPosition)
        {
            return Cells[cellPosition];
        }
    }
}
