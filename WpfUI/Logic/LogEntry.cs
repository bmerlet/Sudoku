using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.Logic
{
    class LogEntry
    {
        public enum EType { PickNumber, PickPossibles }

        public readonly EType Type;
        public readonly uint Position;
        public readonly uint OldValue;
        public readonly uint NewValue;

        public LogEntry(EType type, uint position, uint oldValue, uint newValue)
        {
            Type = type;
            Position = position;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
