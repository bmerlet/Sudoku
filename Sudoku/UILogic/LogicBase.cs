//
// Copyright 2019 Benoit J. Merlet
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.UILogic
{
    /// <summary>
    /// UI Logic (i.e. View Model) base class 
    /// </summary>
    public class LogicBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyLambda)
        {
            string propertyName = GetPropertyName(propertyLambda);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Property name from property lambda expression

        /// <summary>
        /// Get the name of a static or instance property from a property access lambda.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyLambda">lambda expression of the form: '() => Class.Property' or '() => object.Property'</param>
        /// <returns>The name of the property</returns>
        public string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        #endregion
    }

    /// <summary>
    /// UI Logic (i.e. View Model) base class for all dialogs
    /// </summary>
    public abstract class LogicDialogBase : LogicBase
    {
        #region Window closure delegation

        // For the dialog to let us know how to close it
        public delegate void CloseViewDelegate(bool? dialogResult);
        public CloseViewDelegate CloseView;

        #endregion

        #region OK/Cancel actions

        // private commit action
        private CommandBase commitAction;
        private CommandBase cancelAction;

        // Commit action UI property
        public CommandBase CommitCommand => GetCommitAction();

        // Cancel action UI property
        public CommandBase CancelCommand => GetCancelAction();

        // Action creation
        private CommandBase GetCommitAction()
        {
            if (commitAction == null)
            {
                commitAction = new CommandBase(OnCommit);
            }
            return commitAction;
        }

        private CommandBase GetCancelAction()
        {
            if (cancelAction == null)
            {
                cancelAction = new CommandBase(OnCancel);
            }
            return cancelAction;
        }

        // Commit action execution: Call Commit() on derived class.
        private void OnCommit()
        {
            var result = Commit();

            CloseView(result);
        }

        // Implement in derived classes
        protected abstract bool? Commit();

        // Cancel action execution: Call cancel, which can be overridden in derived classes.
        private void OnCancel()
        {
            var result = Cancel();

            CloseView(result);
        }

        // Override in derived classes if needed
        protected virtual bool? Cancel()
        {
            return false;
        }

        #endregion
    }
}
