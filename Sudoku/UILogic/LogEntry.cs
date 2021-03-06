﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.UILogic
{
    class LogEntry
    {
        public readonly uint Position;
        public readonly uint OldNumber;
        public readonly uint OldPossibles;
        public readonly uint NewNumber;
        public readonly uint NewPossibles;
        public readonly uint SelectedCell;

        public LogEntry(uint position, uint oldNumber, uint newNumber, uint oldPossibles, uint newPossibles, uint selectedCell)
        {
            Position = position;
            OldNumber = oldNumber;
            NewNumber = newNumber;
            OldPossibles = oldPossibles;
            NewPossibles = newPossibles;
            SelectedCell = selectedCell;
        }
    }
}
