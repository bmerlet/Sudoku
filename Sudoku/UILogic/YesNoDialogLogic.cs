using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.UILogic
{
    public class YesNoDialogLogic : LogicDialogBase
    {
        public YesNoDialogLogic(string title, string question, bool yesNo)
        {
            Title = title;
            Question = question;
            LeftButtonText = yesNo ? "Yes" : "OK";
            RightButtonText = yesNo ? "No" : "Cancel";
        }
        
        public string Title { get; }
        public string Question { get; }
        public string LeftButtonText { get; }
        public string RightButtonText { get; }

        protected override bool? Commit()
        {
            return true;
        }
    }
}
