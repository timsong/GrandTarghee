using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace GrandTarghee.Framework.WebClient.DSL
{
    public class HttpPostClient : IHttpPostClient
    {
        private Action<HttpWebRequest, Stream> _accessRequestStreamMethod;
        private Action<HttpWebResponse, Stream> _accessResponseStreamMethod;

        private string _data;

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
        public HttpPostClient(HttpWebRequest request)
        {
            this.Request = request;
            this.Request.Method = "POST";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the method of the http client to put.
        /// </summary>
        /// <returns></returns>
        public IHttpPostClient Put()
        {
            this.Request.Method = "PUT";
            return this;
        }

        /// <summary>
        /// Writes the data to the http client request stream as is.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IHttpPostClient Data(string data)
        {
            this.Request.ContentLength = data.Length;
            this._data = data;

            return this;
        }

        /// <summary>
        /// Writes the data to the http client request stream name value format (i.e. key1=value1&key2=value2&...).
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IHttpPostClient Data(IDictionary<string, string> data)
        {
            var stringBuilder = new StringBuilder();

            // Generate the request data as name value pairs, url encoding the value.

            foreach (var item in data)
            {
                if (!item.Key.IsNullOrEmpty() && !item.Value.IsNullOrEmpty())
                {
                    if (stringBuilder.Length == 0)
                    {
                        stringBuilder.AppendFormat("{0}={1}", item.Key, Uri.EscapeDataString(item.Value));
                    }
                    else
                    {
                        stringBuilder.AppendFormat("&{0}={1}", item.Key, Uri.EscapeDataString(item.Value));
                    }
                }
            }

            return this.Data(stringBuilder.ToString());
        }

        /// <summary>
        /// Writes the data to the http client request stream as xml.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IHttpPostClient Data(XElement data)
        {
            return this.Data(data.ToString());
        }

        /// <summary>
        /// Access the response stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public IHttpPostClient AccessRequestStream(Action<HttpWebRequest, Stream> access)
        {
            this._accessRequestStreamMethod = access;

            return this;
        }

        /// <summary>
        /// Access the response stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public IHttpPostClient AccessResponseStream(Action<HttpWebResponse, Stream> access)
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
            // Write the data to the request stream.

            using (var stream = this.Request.GetRequestStream())
            {
                this.ProcessRequestStream(stream);
            }

            // Get the response back.

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
            // Write the data to the request stream.

            using (var stream = this.Request.GetRequestStream())
            {
                this.ProcessRequestStream(stream);
            }

            // Get the response back.

            var response = this.GetResponse(this.Request, callback);

            return new HttpClientResponse((HttpWebResponse)response);
        }

        /// <summary>
        /// Gets the response from the asynchronous post call in the form of a string.
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
        /// Gets the response from the asynchronous post call in the form of an XElement.
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
            var result = string.Empty;
            var response = this.Request.GetResponse() as HttpWebResponse;

            // Get the response stream.

            using (var stream = response.GetResponseStream())
            {
                result = this.ProcessResponseStream(response, stream);

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
            var result = string.Empty;
            var response = this.Request.GetResponse() as HttpWebResponse;

            // Get the response stream.

            using (var stream = response.GetResponseStream())
            {
                result = this.ProcessResponseStream(response, stream);

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
                    var result = this.ProcessResponseStream(response, stream);

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
                    var result = this.ProcessResponseStream(response, stream);

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
        /// <param name="data"></param>
        private void ProcessRequestStream(Stream stream)
        {
            if (this._data.IsNullOrEmpty())
            {
                return;
            }

            // Allow the client to access the request stream.

            if (this._accessRequestStreamMethod != null)
            {
                this._accessRequestStreamMethod(this.Request, stream);
            }

            // Write the data to the stream.

            using (var writer = new StreamWriter(stream))
            {
                writer.Write(this._data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private string ProcessResponseStream(HttpWebResponse response, Stream stream)
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
