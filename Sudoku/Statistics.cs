using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    /// <summary>
    /// Statistics about a (solved) sudoku puzzle
    /// </summary>
    public class Statistics
    {
        public readonly uint NumGivens;
        public readonly uint NumSingles;
        public readonly uint NumHiddenSingles;
        public readonly uint NumNakedPairs;
        public readonly uint NumHiddenPairs;
        public readonly uint NumBoxLineReduction;
        public readonly uint NumPointingPairTriple;
        public readonly uint NumGuesses;
        public readonly uint NumRollBacks;
        public readonly EDifficulty Difficulty;

        public Statistics(List<LogEntry> logEntries)
        {
            // Count each type of event
            NumGivens = (uint)logEntries.Count(le =>
                le.Type == EMarkType.GIVEN);
            NumSingles = (uint)logEntries.Count(le => 
                le.Type == EMarkType.SINGLE);
            NumHiddenSingles = (uint)logEntries.Count(le =>
                le.Type == EMarkType.HIDDEN_SINGLE_COLUMN ||
                le.Type == EMarkType.HIDDEN_SINGLE_ROW ||
                le.Type == EMarkType.HIDDEN_SINGLE_SECTION);
            NumNakedPairs = (uint)logEntries.Count(le => 
                le.Type == EMarkType.NAKED_PAIR_COLUMN ||
                le.Type == EMarkType.NAKED_PAIR_ROW ||
                le.Type == EMarkType.NAKED_PAIR_SECTION);
            NumHiddenPairs = (uint)logEntries.Count(le =>
                le.Type == EMarkType.HIDDEN_PAIR_COLUMN ||
                le.Type == EMarkType.HIDDEN_PAIR_ROW ||
                le.Type == EMarkType.HIDDEN_PAIR_SECTION);
            NumBoxLineReduction = (uint)logEntries.Count(le =>
                le.Type == EMarkType.ROW_BOX ||
                le.Type == EMarkType.COLUMN_BOX);
            NumPointingPairTriple = (uint)logEntries.Count(le =>
                le.Type == EMarkType.POINTING_PAIR_TRIPLE_COLUMN ||
                le.Type == EMarkType.POINTING_PAIR_TRIPLE_ROW);
            NumGuesses = (uint)logEntries.Count(le =>
                le.Type == EMarkType.GUESS);
            NumRollBacks = (uint)logEntries.Count(le =>
                le.Type == EMarkType.ROLLBACK);

            // And finally the difficulty
            if (NumGuesses > 0)
            {
                Difficulty = EDifficulty.EXPERT;
            }
            else if (NumBoxLineReduction > 0 || NumPointingPairTriple > 0 || NumNakedPairs > 0 || NumHiddenPairs > 0)
            {
                Difficulty = EDifficulty.INTERMEDIATE;
            }
            else if (NumHiddenSingles > 0 || NumSingles > 0)
            {
                Difficulty = EDifficulty.EASY;
            }
            else
            {
                Difficulty = EDifficulty.UNKNOWN;
            }
        }
    }
}
