using System;
using System.Runtime.Serialization;

namespace Sudoku.UILogic
{
    [Serializable]
    public class Settings
    {
        // Position on screen
        public int Left;
        public int Top;

        public Settings()
        {
        }
    }
}
