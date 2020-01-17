using System;
using System.Collections.Generic;
using System.Text;

using Toolbox;

namespace Sudoku.Game
{
    public enum EDifficulty
    {
        [EnumDescription("Easy")]
        SIMPLE,

        [EnumDescription("Medium")]
        EASY,

        [EnumDescription("Hard")]
        INTERMEDIATE,

        [EnumDescription("Very hard")]
        EXPERT
    }

    public enum ESymmetry
    {
        NONE,
        ROTATE90,
        ROTATE180,
        MIRROR,
        FLIP,
        RANDOM
    }

    public enum EMarkType
    {
        [EnumDescription("Given number")]
        GIVEN,

        [EnumDescription("Mark only possibility for cell")]
        SINGLE,

        [EnumDescription("Mark single possibility for value in row")]
        HIDDEN_SINGLE_ROW,

        [EnumDescription("Mark single possibility for value in column")]
        HIDDEN_SINGLE_COLUMN,

        [EnumDescription("Mark single possibility for value in section")]
        HIDDEN_SINGLE_SECTION,

        [EnumDescription("Guess (start branch)")]
        GUESS,

        [EnumDescription("Undo")]
        ROLLBACK,

        [EnumDescription("Remove possibilities for naked pair in row")]
        NAKED_PAIR_ROW,

        [EnumDescription("Remove possibilities for naked pair in column")]
        NAKED_PAIR_COLUMN,

        [EnumDescription("Remove possibilities for naked pair in section")]
        NAKED_PAIR_SECTION,

        [EnumDescription("Remove possibilities for row because all values are in one section")]
        POINTING_PAIR_TRIPLE_ROW,

        [EnumDescription("Remove possibilities for column because all values are in one section")]
        POINTING_PAIR_TRIPLE_COLUMN,

        [EnumDescription("Remove possibilities for section because all values are in one row")]
        ROW_BOX,

        [EnumDescription("Remove possibilities for section because all values are in one column")]
        COLUMN_BOX,

        [EnumDescription("Remove possibilities from hidden pair in row")]
        HIDDEN_PAIR_ROW,

        [EnumDescription("Remove possibilities from hidden pair in column")]
        HIDDEN_PAIR_COLUMN,

        [EnumDescription("Remove possibilities from hidden pair in section")]
        HIDDEN_PAIR_SECTION
    }

    public enum EPrintStyle
    {
        ONE_LINE,
        COMPACT,
        READABLE,
        CSV
    };

    public interface IPrintSource
    {
        uint GetCellValue(uint cellPosition);
    }
}
