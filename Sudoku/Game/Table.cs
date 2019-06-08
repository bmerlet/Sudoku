using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.Game
{
    /// <summary>
    /// Holds a sudoku puzzle with all the solution information
    /// </summary>
    public class Table : IPrintSource
    {
        // The values themselves
        private readonly Cell[] values = new Cell[Position.BOARD_SIZE];

        // If puzzle solved
        public bool IsSolved => Array.TrueForAll(values, cv => cv.IsMarked);

        // Access values indexed by a position
        public Cell this[Position position]
        {
            get => values[position.Cell];
            set => values[position.Cell] = value;
        }

        // Constructor
        public Table()
        {
            Reset();
        }

        // Clone
        public Table(Table src)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] =  new Cell(src.values[i]);
            }
        }

        // Reset to all unmarked
        public void Reset()
        {
            for(int i = 0; i < values.Length; i++)
            {
                values[i] = new Cell();
            }

        }

        // Clone from another table
        public void Clone(Table src)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i].Clone(src.values[i]);
            }
        }

        // IPrintSource interface
        public uint GetCellValue(uint cellPosition)
        {
            return values[cellPosition].Value;
        }
    }

}
