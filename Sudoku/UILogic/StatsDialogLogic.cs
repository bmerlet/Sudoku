using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox.UILogic.Dialogs;

namespace Sudoku.UILogic
{
    public class StatsDialogLogic : LogicDialogBase
    {
        private readonly Statistics stats;

        public StatsDialogLogic(Statistics stats)
        {
            this.stats = stats;
        }

        public string NumGivens => stats.NumGivens.ToString();
        public string NumSingles => stats.NumSingles.ToString();
        public string NumHiddenSingles => stats.NumHiddenSingles.ToString();
        public string NumNakedPairs => stats.NumNakedPairs.ToString();
        public string NumHiddenPairs => stats.NumHiddenPairs.ToString();
        public string NumBoxLineReduction => stats.NumBoxLineReduction.ToString();
        public string NumPointingPairTriple => stats.NumPointingPairTriple.ToString();
        public string NumGuesses => (stats.NumGuesses + 1).ToString();
        public string NumRollBacks => stats.NumRollBacks.ToString();
        public string Difficulty => Toolbox.Attributes.EnumDescriptionAttribute.GetDescription(stats.Difficulty);


        protected override bool? Commit()
        {
            return true;
        }
    }
}
