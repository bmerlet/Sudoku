using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox.UILogic;

namespace Sudoku.UILogic
{
    public class UICellLogic : LogicBase
    {
        #region Private members

        // my position
        private readonly uint cellindex;

        // UI provider
        private readonly IUIProvider uiProvider;

        // If selected
        private bool isSelected;

        // If given
        private bool isGiven;

        // What possibles are on (bit-list)
        private int possibles;

        #endregion

        #region Constructor

        public UICellLogic(IUIProvider uiProvider, uint cellindex)
        {
            this.cellindex = cellindex;
            this.uiProvider = uiProvider;

            Number = "";
            Background = uiProvider.GetBrush(EColors.NormalBackground);
            Foreground = uiProvider.GetBrush(EColors.NormalForeground);

            Possibles = "";
            PossiblesForeground = uiProvider.GetBrush(EColors.NormalForeground);
        }

        #endregion

        #region UI properties

        public String Number { get; private set; }
        public String Possibles { get; private set; }

        public object Background { get; private set; }
        public object Foreground { get; private set; }
        public object PossiblesForeground { get; private set; }

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
            Foreground = uiProvider.GetBrush(error ? EColors.ErrorForeground : EColors.NormalForeground);
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
            var possiblesForeground = uiProvider.GetBrush(error ? EColors.ErrorForeground : EColors.NormalForeground);

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
            EColors background;

            if (isSelected)
            {
                background =  EColors.SelectedBackground;
            }
            else if (isGiven)
            {
                background = EColors.GivenBackground;
            }
            else
            {
                background = EColors.NormalBackground;
            }

            var backgroundBrush = uiProvider.GetBrush(background);

            if (Background != backgroundBrush)
            {
                Background = backgroundBrush;
                OnPropertyChanged(() => Background);
            }
        }

        #endregion
    }
}
