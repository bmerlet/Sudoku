using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Toolbox.UILogic.Dialogs;

namespace Sudoku.UILogic
{
    public class YesNoDialogLogic : LogicDialogBase
    {
        public YesNoDialogLogic(string title, string question)
        {
            Title = title;
            Question = question;
        }
        
        public string Title { get; }
        public string Question { get; }

        protected override bool? Commit()
        {
            return true;
        }
    }
}
