using System;
using System.Collections.Generic;
using System.Text;

using Toolbox.UILogic.Dialogs;

namespace Sudoku.UILogic
{
    // Color scheme
    public enum EColors { NormalBackground, GivenBackground, SelectedBackground, NormalForeground, ErrorForeground }

    public interface IUIProvider
    {
        // Return a brush corresponding to a color
        object GetBrush(EColors color);

        // Display a dialog, return status (false means cancel or no change)
        bool DisplayDialog(LogicDialogBase logic);

        // Run async on UI thread
        void RunAsyncOnUIThread(Action action);

        // Exit the application
        void Exit();
    }
}
