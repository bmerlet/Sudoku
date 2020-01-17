using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;

namespace Sudoku.UILogic
{
    public class StatsDialogLogic : LogicDialogBase
    {
        private readonly Statistics puzzleStats;
        private readonly Statistics currentStats;
        private Statistics stats;
        private static bool lastCurrentState = false;

        public StatsDialogLogic(Statistics puzzleStats, Statistics currentStats)
        {
            this.puzzleStats = puzzleStats;
            this.currentStats = currentStats;
            this.stats = lastCurrentState ? currentStats : puzzleStats;
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
        public string Difficulty => Toolbox.EnumDescriptionAttribute.GetDescription(stats.Difficulty);

        public bool CurrentState
        {
            get => lastCurrentState;
            set => SetCurrentState(value);
        }

        private void SetCurrentState(bool current)
        {
            if (current != lastCurrentState)
            {
                lastCurrentState = current;
                this.stats = lastCurrentState ? currentStats : puzzleStats;

                OnPropertyChanged(() => NumGivens);
                OnPropertyChanged(() => NumSingles);
                OnPropertyChanged(() => NumHiddenSingles);
                OnPropertyChanged(() => NumNakedPairs);
                OnPropertyChanged(() => NumHiddenPairs);
                OnPropertyChanged(() => NumBoxLineReduction);
                OnPropertyChanged(() => NumPointingPairTriple);
                OnPropertyChanged(() => NumGuesses);
                OnPropertyChanged(() => NumRollBacks);
                OnPropertyChanged(() => Difficulty);
            }

        }

        protected override bool? Commit()
        {
            return true;
        }
    }
}
