using System;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace GrandTarghee.Framework.WebClient
{
    public interface IHttpGetClient
    {
        #region Properties

        /// <summary>
        /// The request of the http client.
        /// </summary>
        HttpWebRequest Request { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Access the response stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        IHttpGetClient AccessResponseStream(Action<HttpWebResponse, Stream> access);

        /// <summary>
        /// Gets the response from the get call in the form of a string.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        IHttpClientResponse GetResponse(Action<string> callback);

        /// <summary>
        /// Gets the response from the get call in the form of an XElement.
        /// The format of the response need to in xml.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        IHttpClientResponse GetResponse(Action<XElement> callback);

        /// <summary>
        /// Gets the response from the asychronous get call in the form of a string.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        void GetResponseAsync(Action<string> callback);

        /// <summary>
        /// Gets the response from the asychronous get call in the form of an XElement.
        /// The format of the response need to in xml.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        void GetResponseAsync(Action<XElement> callback);

        #endregion
    }
}
