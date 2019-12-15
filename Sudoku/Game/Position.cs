using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.Game
{
    /// <summary>
    /// Position of cell in a sudoku puzzle - immutable
    /// </summary>
    public class Position
    {
        public const int GRID_SIZE = 3;
        public const int ROW_COL_SEC_SIZE = GRID_SIZE * GRID_SIZE;
        public const int SEC_GROUP_SIZE = ROW_COL_SEC_SIZE * GRID_SIZE;
        public const int BOARD_SIZE = ROW_COL_SEC_SIZE * ROW_COL_SEC_SIZE;
        public const int POSSIBILITY_SIZE = BOARD_SIZE * ROW_COL_SEC_SIZE;

        // Cell index (0-80)
        public readonly uint Cell;

        // Row (0-8)
        public readonly uint Row;

        // Column (0-8)
        public readonly uint Column;

        // Section (0-8)
        public readonly uint Section;

        // Section start (upper left) cell (0-80)
        public readonly uint StartSectionCell;

        // Constructors
        // Cell -> position
        private Position(uint cell)
        {
            Cell = cell;
            Column = cell % ROW_COL_SEC_SIZE;
            Row = cell / ROW_COL_SEC_SIZE;
            Section = 
                ((cell / SEC_GROUP_SIZE) * GRID_SIZE) + // Row index * 3
                (Column / GRID_SIZE);   // Column index
            StartSectionCell =
                ((Cell / SEC_GROUP_SIZE) * SEC_GROUP_SIZE) + // Row
                ((Column) / GRID_SIZE) * GRID_SIZE; // column
        }

        // cell (0-80)
        static public Position GetCellFromCell(uint cell)
        {
            return PositionCache[cell];
        }

        // Row (0-8) and column (0-8) => cell (0-80)
        static public Position GetCell(uint row, uint column)
        {
            return PositionCache[ROW_COL_SEC_SIZE * row + column];
        }

        // Row (0-8) -> First cell of the row (0-80)
        static public Position GetCellFromRow(uint row)
        {
            return PositionCache[ROW_COL_SEC_SIZE * row];
        }

        // Column (0-8) -> First cell of the column (0-80)
        static public Position GetCellFromColumn(uint column)
        {
            return PositionCache[column];
        }

        // Section (0-8) and offset into the section (0-8) -> cell (0-80)
        static public Position GetCellFromSection(uint section, uint offset = 0)
        {
            uint sectionStartCell =
                ((section % GRID_SIZE) * GRID_SIZE) +
                ((section / GRID_SIZE) * SEC_GROUP_SIZE);

            uint cell =
                sectionStartCell +
                ((offset / GRID_SIZE) * ROW_COL_SEC_SIZE) +
                (offset % GRID_SIZE);

            return PositionCache[cell];
        }

        static public IEnumerable<Position> AllPositions
        {
            get
            {
                for (uint cell = 0; cell < BOARD_SIZE; cell++)
                {
                    yield return PositionCache[cell];
                }
            }
        }

        static public IEnumerable<Position> AllSectionStarts
        {
            get
            {
                for (uint section = 0; section < ROW_COL_SEC_SIZE; section++)
                {
                    yield return GetCellFromSection(section);
                }
            }
        }

        static public IEnumerable<Position> AllColumnStarts
        {
            get
            {
                for (uint col = 0; col < ROW_COL_SEC_SIZE; col++)
                {
                    yield return GetCellFromColumn(col);
                }
            }
        }
        static public IEnumerable<Position> AllRowStarts
        {
            get
            {
                for (uint row = 0; row < ROW_COL_SEC_SIZE; row++)
                {
                    yield return GetCellFromRow(row);
                }
            }
        }


        public IEnumerable<Position> AllColumnCells
        {
            get
            {
                for (uint col = 0; col < ROW_COL_SEC_SIZE; col++)
                {
                    yield return GetCell(Row, col);
                }
            }
        }

        public IEnumerable<Position> AllRowCells
        {
            get
            {
                for (uint row = 0; row < ROW_COL_SEC_SIZE; row++)
                {
                    yield return GetCell(row, Column);
                }
            }
        }

        public IEnumerable<Position> AllSectionCells
        {
            get
            {
                for (uint offset = 0; offset < ROW_COL_SEC_SIZE; offset++)
                {
                    yield return GetCellFromSection(Section, offset);
                }
            }
        }

        public Position Opposite => GetCell(ROW_COL_SEC_SIZE - 1 - Row, ROW_COL_SEC_SIZE - 1 - Column);
        public Position HorizontalFlip => GetCell(Row, ROW_COL_SEC_SIZE - 1 - Column);
        public Position VerticalFlip => GetCell(ROW_COL_SEC_SIZE - 1 - Row, Column);

        public override bool Equals(object obj)
        {
            return
                obj is Position p &&
                p.Cell == this.Cell;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Cell} Row:{Row} Col:{Column} Sec:{Section}";
        }

        public string ToReadableString()
        {
            return $"row {Row + 1}, col {Column + 1}";
        }

        // Create all the position instances that we will need once and for all
        static private Position[] PositionCache;
        static Position()
        {
            PositionCache = new Position[BOARD_SIZE];
            for (uint i = 0; i < PositionCache.Length; i++)
            {
                PositionCache[i] = new Position(i);
            }
        }

    }
}
