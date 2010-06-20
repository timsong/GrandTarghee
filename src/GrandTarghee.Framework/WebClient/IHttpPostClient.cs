﻿using System;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace GrandTarghee.Framework.WebClient
{
    public interface IHttpPostClient
    {
        #region Properties

        /// <summary>
        /// The request of the http client.
        /// </summary>
        HttpWebRequest Request { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the method of the http client to post.
        /// </summary>
        /// <returns></returns>
        IHttpPostClient Post();

        /// <summary>
        /// Sets the method of the http client to put.
        /// </summary>
        /// <returns></returns>
        IHttpPostClient Put();

        /// <summary>
        /// Writes data the http client request.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IHttpPostClient Data(string data);

        /// <summary>
        /// Writes data to the http client request using the name value format (ie. name1=value1&name2=value2...).
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IHttpPostClient Data(IDictionary<string, string> data);

        /// <summary>
        /// Writes data to the http client request .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IHttpPostClient Data(XElement data);

        /// <summary>
        /// Access the request stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        IHttpPostClient AccessRequestStream(Action<Stream> access);

        /// <summary>
        /// Access the response stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        IHttpPostClient AccessResponseStream(Action<Stream> access);

        /// <summary>
        /// Gets the response from the get call in the form of a string.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        IHttpClientResponse GetResponse(Action<string> callback);

        /// <summary>
        /// Gets the response from the asynchronous post call in the form of a string.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        void GetResponseAsync(Action<string> callback);

        /// <summary>
        /// Gets the response from the get call in the form of an XElement.
        /// The format of the response need to in xml.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        IHttpClientResponse GetResponse(Action<XElement> callback);

        /// <summary>
        /// Gets the response from the asynchronous post call in the form of an XElement.
        /// The format of the response need to in xml.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        void GetResponseAsync(Action<XElement> callback);

        #endregion
    }
}
