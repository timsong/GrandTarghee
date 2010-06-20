using System.Windows.Input;

namespace GrandTarghee.Framework.MVVM
{
    public static class CommandExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public static void NotifyCanExecuteChanged(this ICommand command)
        {
            var delegateCommand = command as DelegateCommand;

            if (delegateCommand != null)
            {
                delegateCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        public static void NotifyCanExecuteChanged<T>(this ICommand command)
        {
            var delegateCommand = command as DelegateCommand<T>;

            if (delegateCommand != null)
            {
                delegateCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
