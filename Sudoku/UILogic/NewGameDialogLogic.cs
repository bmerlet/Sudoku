using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox.UILogic;
using Toolbox.UILogic.Dialogs;

namespace Sudoku.UILogic
{
    public class NewGameDialogLogic : LogicDialogBase
    {
        #region Dialog result

        public EDifficulty Difficulty { get; private set; }

        #endregion

        #region UI Properties

        public CommandBase NewEasy { get; }
        public CommandBase NewMedium { get; }
        public CommandBase NewHard { get; }
        public CommandBase NewVeryHard { get; }
        public CommandBase Exit { get; }

        #endregion

        #region Actions

        public NewGameDialogLogic()
        {
            NewEasy = new CommandBase(() => OnGeneratePuzzle(EDifficulty.SIMPLE));
            NewMedium = new CommandBase(() => OnGeneratePuzzle(EDifficulty.EASY));
            NewHard = new CommandBase(() => OnGeneratePuzzle(EDifficulty.INTERMEDIATE));
            NewVeryHard = new CommandBase(() => OnGeneratePuzzle(EDifficulty.EXPERT));
        }

        private void OnGeneratePuzzle(EDifficulty difficulty)
        {
            // memorize difficulty
            Difficulty = difficulty;

            // Close dialog
            CommitCommand.Execute();
        }

        protected override bool? Commit()
        {
            // Always return OK status
            return true;
        }

        #endregion
    }
}
