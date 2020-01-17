//
// Copyright 2019 Benoit J. Merlet
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sudoku.UILogic
{
    /// <summary>
    /// Generic implementation of ICommand
    /// </summary>
    public class CommandBase : ICommand
    {
        #region private members

        private readonly Action actionWithoutParam;
        private readonly Action<object> actionWithParam;
        private bool canExecute;

        #endregion

        #region Constructors

        public CommandBase(Action action, bool canExecute = true)
        {
            this.actionWithoutParam = action;
            this.actionWithParam = null;
            this.canExecute = canExecute;
        }

        public CommandBase(Action<object> action, bool canExecute = true)
        {
            this.actionWithoutParam = null;
            this.actionWithParam = action;
            this.canExecute = canExecute;
        }

        #endregion

        #region Execution availability change

        public void SetCanExecute(bool value)
        {
            if (canExecute != value)
            {
                canExecute = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region parameter override

        public object ParameterOverride { get; set; }

        #endregion

        #region ICommand implementation

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter = null)
        {
            return canExecute;
        }

        public void Execute(object parameter = null)
        {
            // Override parameter
            if (ParameterOverride != null)
            {
                parameter = ParameterOverride;
            }

            actionWithoutParam?.Invoke();
            actionWithParam?.Invoke(parameter);
        }

        #endregion
    }
}
