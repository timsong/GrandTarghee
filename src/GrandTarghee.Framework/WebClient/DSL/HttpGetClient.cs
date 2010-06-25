﻿using System;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace GrandTarghee.Framework.WebClient.DSL
{
    public class HttpGetClient : IHttpGetClient
    {
        private Action<HttpWebResponse, Stream> _accessResponseStreamMethod;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public HttpWebRequest Request { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public HttpGetClient(HttpWebRequest request)
        {
            this.Request = request;
            this.Request.Method = "GET";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Access the response stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public IHttpGetClient AccessResponseStream(Action<HttpWebResponse, Stream> access)
        {
            this._accessResponseStreamMethod = access;

            return this;
        }

        /// <summary>
        /// Gets the response from the get call in the form of a string.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        public IHttpClientResponse GetResponse(Action<string> callback)
        {
            var response = this.GetResponse(this.Request, callback);
            
            return new HttpClientResponse((HttpWebResponse)response);
        }

        /// <summary>
        /// Gets the response from the get call in the form of an XElement.
        /// The format of the response need to in xml.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        public IHttpClientResponse GetResponse(Action<XElement> callback)
        {
            var response = this.GetResponse(this.Request, callback);

            return new HttpClientResponse((HttpWebResponse)response);
        }

        /// <summary>
        /// Gets the response from the asychronous get call in the form of a string.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        public void GetResponseAsync(Action<string> callback)
        {
            var asyncState = new AsyncState<string>()
            {
                Request = this.Request,
                Callback = callback
            };

            this.Request.BeginGetResponse(new AsyncCallback(result => this.GetStringResponseAsync(result)), asyncState);
        }

        /// <summary>
        /// Gets the response from the asychronous get call in the form of an XElement.
        /// The format of the response need to in xml.
        /// </summary>
        /// <param name="callback">Callback to handle the response.</param>
        /// <returns></returns>
        public void GetResponseAsync(Action<XElement> callback)
        {
            var asyncState = new AsyncState<XElement>()
            {
                Request = this.Request,
                Callback = callback
            };

            this.Request.BeginGetResponse(new AsyncCallback(result => this.GetXElementResponseAsync(result)), asyncState);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private HttpWebResponse GetResponse(HttpWebRequest request, Action<string> callback)
        {
            var response = this.Request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                var result = this.ProcessStream(response, stream);

                // Run the callback to handle the response.

                callback(result);
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private HttpWebResponse GetResponse(HttpWebRequest request, Action<XElement> callback)
        {
            var response = this.Request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                var result = this.ProcessStream(response, stream);

                // Run the callback to handle the response.

                var xelement = XElement.Parse(result);
                callback(xelement);
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        private void GetStringResponseAsync(IAsyncResult asyncResult) 
        {
            var state = asyncResult.AsyncState as AsyncState<string>;

            if (state != null)
            {
                var response = state.Request.EndGetResponse(asyncResult) as HttpWebResponse;

                using (var stream = response.GetResponseStream())
                {
                    var result = this.ProcessStream(response, stream);

                    // Run the callback to handle the response.

                    state.Callback(result);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        private void GetXElementResponseAsync(IAsyncResult asyncResult)
        {
            var state = asyncResult.AsyncState as AsyncState<XElement>;

            if (state != null)
            {
                var response = state.Request.EndGetResponse(asyncResult) as HttpWebResponse;

                using (var stream = response.GetResponseStream())
                {
                    var result = this.ProcessStream(response, stream);

                    // Run the callback to handle the response.

                    var xelement = XElement.Parse(result);
                    state.Callback(xelement);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private string ProcessStream(HttpWebResponse response, Stream stream)
        {
            var result = string.Empty;

            // Allow the caller to access the stream if they've specified.

            if (this._accessResponseStreamMethod != null)
            {
                this._accessResponseStreamMethod(response, stream);
            }

            // Read from the response stream.

            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
                reader.Close();
            }

            return result;
        }

        #endregion
    }
}
