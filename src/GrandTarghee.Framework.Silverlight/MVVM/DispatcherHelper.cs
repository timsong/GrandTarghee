using System;
using System.Windows;

namespace GrandTarghee.Framework.MVVM
{
    public class DispatcherHelper
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void DispatchOnUIThread(Action action)
        {
            var dispatcher = Deployment.Current.Dispatcher;

            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion
    }
}
