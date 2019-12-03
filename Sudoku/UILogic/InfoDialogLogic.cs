using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Toolbox.UILogic.Dialogs;

namespace Sudoku.UILogic
{
    public class InfoDialogLogic : LogicDialogBase
    {
        public InfoDialogLogic(string title, string info)
        {
            Title = title;
            Info = info;
        }

        public string Title { get; private set; }
        public string Info { get; private set; }


        protected override bool? Commit()
        {
            return true;
        }
    }
}
