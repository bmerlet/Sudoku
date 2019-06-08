using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.UILogic
{
    public enum EColors { NormalBackground, GivenBackground, SelectedBackground, NormalForeground, ErrorForeground }

    public interface IUIProvider
    {
        object GetBrush(EColors color);
    }
}
