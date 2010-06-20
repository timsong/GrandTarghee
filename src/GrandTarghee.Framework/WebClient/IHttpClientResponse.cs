using System;
using System.Net;

namespace GrandTarghee.Framework.WebClient
{
    public interface IHttpClientResponse : IDisposable
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        HttpWebResponse Response { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Accesses the http web response.
        /// </summary>
        /// <param name="modifyRequest"></param>
        /// <returns></returns>
        void AccessResponse(Action<HttpWebResponse> access);

        #endregion
    }
}
