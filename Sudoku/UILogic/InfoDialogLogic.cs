using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.UILogic
{
    public class InfoDialogLogic : LogicDialogBase
    {
        public InfoDialogLogic(string title, string info)
        {
            Title = title;
            Info = info;
            InfoDoubleUnderlines = info.Replace("_", "__");
        }

        public string Title { get; private set; }
        public string Info { get; private set; }
        public string InfoDoubleUnderlines { get; private set; }


        protected override bool? Commit()
        {
            return true;
        }
    }
}
