using System;
using System.Windows.Input;

namespace GrandTarghee.Framework.MVVM
{
    public interface IDelegateCommand : ICommand
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        void RaiseCanExecuteChanged();

        #endregion
    }
}
