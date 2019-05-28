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

        // What possibles are on
        private bool[] possibles = new bool[Creator.ROW_SIZE];

        public UICellLogic(uint cellindex)
        {
            this.cellindex = cellindex;

            Number = "";
            Background = normalBackground;
            Foreground = normalForeground;

            Possibles = "";
            PossiblesForeground = normalForeground;
        }

        public String Number { get; private set; }
        public String Possibles { get; private set; }

        public Brush Background { get; private set; }
        public Brush Foreground { get; private set; }
        public Brush PossiblesForeground { get; private set; }

        public void SetNumber(uint number)
        {
            Number = number == 0 ? "" : number.ToString();
            OnPropertyChanged(() => Number);
        }

        public void UpdateGiven(bool given)
        {
            isGiven = given;
            UpdateBackground();
        }

        public void UpdateSelected(bool selected)
        {
            isSelected = selected;
            UpdateBackground();
        }

        public void UpdateNumberStatus(bool error)
        {
            Foreground = error ? errorForeground : normalForeground;
            OnPropertyChanged(() => Foreground);
        }

        public void UpdatePossibles(uint number)
        {
            possibles[number - 1] = !possibles[number - 1];
            Possibles = "";

            for(uint i = 0; i < possibles.Length; i++)
            {
                if (possibles[i])
                {
                    Possibles += (i + 1).ToString();
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
                Array.Clear(possibles, 0, possibles.Length);
                Possibles = "";
                OnPropertyChanged(() => Possibles);
            }

            UpdatePossiblesStatus(false);
        }

        public bool IsListedAsPossible(uint number)
        {
            return possibles[number - 1];
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
    }
}
