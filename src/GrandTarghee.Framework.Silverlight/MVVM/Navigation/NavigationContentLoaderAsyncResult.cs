using System;
using System.Threading;

namespace GrandTarghee.Framework.MVVM
{
    public class NavigationContentLoaderAsyncResult : IAsyncResult
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object AsyncState { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public WaitHandle AsyncWaitHandle { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CompletedSynchronously { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompleted { get { return true; } }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncState"></param>
        public NavigationContentLoaderAsyncResult(object asyncState)
        {
            this.AsyncState = asyncState;
            this.AsyncWaitHandle = new ManualResetEvent(true);
        }

        #endregion
    }
}
