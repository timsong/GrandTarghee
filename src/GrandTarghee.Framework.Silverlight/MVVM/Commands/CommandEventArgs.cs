using System;

namespace GrandTarghee.Framework.MVVM
{
    public class CommandEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public object CommandParameter { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandParameter"></param>
        public CommandEventArgs(object commandParameter)
        {
            this.CommandParameter = commandParameter;
        }

        /// <summary>
        /// 
        /// </summary>
        public CommandEventArgs()
        {
        }

        #endregion
    }
}
