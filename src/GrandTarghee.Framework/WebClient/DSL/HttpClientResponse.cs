using System;
using System.Net;

namespace GrandTarghee.Framework.WebClient.DSL
{
    public class HttpClientResponse : IHttpClientResponse, IDisposable
    {
        private bool _disposed;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public HttpWebResponse Response { get; private set; }
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public HttpClientResponse(HttpWebResponse response)
        {
            this.Response = response;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public void AccessHttpResponse(Action<HttpWebResponse> access)
        {
            access(this.Response);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing && this.Response != null)
                {
                    this.Response.Close();
                }

                this._disposed = true;
            }
        }

        #endregion
    }
}
