using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    /// <summary>
    /// Holds a sudoku puzzle
    /// </summary>
    public class Table
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

        // Reset to all unmarked
        public void Reset()
        {
            for(int i = 0; i < values.Length; i++)
            {
                values[i] = new Cell();
            }

        }

        // Print the table
        public string Print(EPrintStyle style)
        {
            string str = "";

            foreach(var pos in Position.AllPositions)
            {
                str += style == EPrintStyle.READABLE ? " " : "";
                str += this[pos].Value == 0 ? "." : this[pos].Value.ToString();

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

    }

}
