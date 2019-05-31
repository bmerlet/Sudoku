using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Toolbox.UILogic;

namespace WpfUI.Logic
{
    class UICellLogic : LogicBase
    {
        #region Private members

        // Color scheme
        private readonly Brush normalBackground = Brushes.Transparent;
        private readonly Brush givenBackground = Brushes.LightGray;
        private readonly Brush selectedBackground = Brushes.LightBlue;
        private readonly Brush normalForeground = Brushes.Black;
        private readonly Brush errorForeground = Brushes.Red;

        // my position
        private readonly uint cellindex;

        // If selected
        private bool isSelected;

        // If given
        private bool isGiven;

        // What possibles are on (bit-list)
        private int possibles;

        #endregion

        #region Constructor

        public UICellLogic(uint cellindex)
        {
            this.cellindex = cellindex;

            Number = "";
            Background = normalBackground;
            Foreground = normalForeground;

            Possibles = "";
            PossiblesForeground = normalForeground;
        }

        #endregion

        #region UI properties

        public String Number { get; private set; }
        public String Possibles { get; private set; }

        public Brush Background { get; private set; }
        public Brush Foreground { get; private set; }
        public Brush PossiblesForeground { get; private set; }

        #endregion

        #region Actions

        public void SetNumber(uint number)
        {
            Number = number == 0 ? "" : number.ToString();
            OnPropertyChanged(() => Number);
        }

        public void SetGiven(bool given)
        {
            isGiven = given;
            UpdateBackground();
        }

        public void SetSelected(bool selected)
        {
            isSelected = selected;
            UpdateBackground();
        }

        public void SetNumberStatus(bool error)
        {
            Foreground = error ? errorForeground : normalForeground;
            OnPropertyChanged(() => Foreground);
        }

        public void SetPossibles(uint number)
        {
            // Flip bit corresponding to number
            int bit = 1 << (int)number;
            if ((possibles & bit) == 0)
            {
                possibles |= bit;
            }
            else
            {
                possibles &= ~bit;
            }

            // Rebuild string
            BuildPossiblesString();
            UpdatePossiblesStatus(false);
        }

        private void BuildPossiblesString()
        { 
            Possibles = "";

            foreach(var cv in Cell.AllValidCellValues)
            {
                if ((possibles & (1 << (int)cv)) != 0)
                {
                    Possibles += cv.ToString();
                }
            }

            OnPropertyChanged(() => Possibles);
        }

        public void UpdatePossiblesStatus(bool error)
        {
            var possiblesForeground = error ? errorForeground : normalForeground;

            if (PossiblesForeground != possiblesForeground)
            {
                PossiblesForeground = possiblesForeground;
                OnPropertyChanged(() => PossiblesForeground);
            }
        }

        public void ResetPossibles()
        {
            if (Possibles != "")
            {
                possibles = 0;
                Possibles = "";
                OnPropertyChanged(() => Possibles);
            }

            UpdatePossiblesStatus(false);
        }

        public bool IsListedAsPossible(uint number)
        {
            return ((possibles & (1 << (int)number)) != 0);
        }

        public uint GetPossiblesAsBitList()
        {
            return (uint)possibles;
        }

        public void SetPossiblesAsBitList(uint possibles)
        {
            this.possibles = (int)possibles;

            BuildPossiblesString();
        }

        private void UpdateBackground()
        {
            Brush background;

            if (isSelected)
            {
                background = selectedBackground;
            }
            else if (isGiven)
            {
                background = givenBackground;
            }
            else
            {
                background = normalBackground;
            }

            if (Background != background)
            {
                Background = background;
                OnPropertyChanged(() => Background);
            }
        }

        #endregion
    }
}
