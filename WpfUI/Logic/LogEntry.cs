using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.Logic
{
    class LogEntry
    {
        public readonly uint Position;
        public readonly uint OldNumber;
        public readonly uint OldPossibles;
        public readonly uint NewNumber;
        public readonly uint NewPossibles;

        public LogEntry(uint position, uint oldNumber, uint newNumber, uint oldPossibles, uint newPossibles)
        {
            Position = position;
            OldNumber = oldNumber;
            NewNumber = newNumber;
            OldPossibles = oldPossibles;
            NewPossibles = newPossibles;
        }
    }
}
