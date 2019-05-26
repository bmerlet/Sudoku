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

        internal Statistics(List<LogEntry> logEntries)
        {
            // Count each type of event
            foreach (var log in logEntries)
            {
                switch(log.Type)
                {
                    case EMarkType.GIVEN:
                        NumGivens += 1;
                        break;
                    case EMarkType.SINGLE:
                        NumSingles += 1;
                        break;
                    case EMarkType.HIDDEN_SINGLE_COLUMN:
                    case EMarkType.HIDDEN_SINGLE_ROW:
                    case EMarkType.HIDDEN_SINGLE_SECTION:
                        NumHiddenSingles += 1;
                        break;
                    case EMarkType.NAKED_PAIR_COLUMN:
                    case EMarkType.NAKED_PAIR_ROW:
                    case EMarkType.NAKED_PAIR_SECTION:
                        NumNakedPairs += 1;
                        break;
                    case EMarkType.HIDDEN_PAIR_COLUMN:
                    case EMarkType.HIDDEN_PAIR_ROW:
                    case EMarkType.HIDDEN_PAIR_SECTION:
                        NumHiddenPairs += 1;
                        break;
                    case EMarkType.ROW_BOX:
                    case EMarkType.COLUMN_BOX:
                        NumBoxLineReduction += 1;
                        break;
                    case EMarkType.POINTING_PAIR_TRIPLE_COLUMN:
                    case EMarkType.POINTING_PAIR_TRIPLE_ROW:
                        NumPointingPairTriple += 1;
                        break;
                    case EMarkType.GUESS:
                        NumGuesses += 1;
                        break;
                    case EMarkType.ROLLBACK:
                        NumRollBacks += 1;
                        break;
                }
            }

            // And finally the difficulty
            if (NumGuesses > 0)
            {
                Difficulty = EDifficulty.EXPERT;
            }
            else if (NumBoxLineReduction > 0 || NumPointingPairTriple > 0 || NumNakedPairs > 0 || NumHiddenPairs > 0)
            {
                Difficulty = EDifficulty.INTERMEDIATE;
            }
            else if (NumHiddenSingles > 0)
            {
                Difficulty = EDifficulty.EASY;
            }
            else if (NumSingles > 0)
            {
                Difficulty = EDifficulty.SIMPLE;
            }
            else
            {
                Difficulty = EDifficulty.UNKNOWN;
            }
        }
    }
}
