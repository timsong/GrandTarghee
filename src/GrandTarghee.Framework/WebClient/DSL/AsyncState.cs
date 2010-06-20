using System;
using System.Net;

namespace GrandTarghee.Framework.WebClient.DSL
{
    public class AsyncState<T>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public HttpWebRequest Request { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<T> Callback { get; set; }

        #endregion
    }
}
