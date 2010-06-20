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
        private Action<Stream> _accessRequestStreamMethod;
        private Action<Stream> _accessResponseStreamMethod;

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
        /// Sets the method of the http client to post.
        /// </summary>
        /// <returns></returns>
        public IHttpPostClient Post()
        {
            this.Request.Method = "POST";
            return this;
        }

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
        /// Sets the data to write to the http client.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IHttpPostClient Data(string data)
        {
            using (var stream = this.Request.GetRequestStream())
            {
                ProcessRequestStream(stream, data);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IHttpPostClient Data(IDictionary<string, string> data)
        {
            var stringBuilder = new StringBuilder();

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (!item.Key.IsNullOrEmpty() && !item.Value.IsNullOrEmpty())
                    {
                        if (stringBuilder.Length == 0)
                        {
                            stringBuilder.AppendFormat("{0}={1}", item.Key, item.Value);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("&{0}={1}", item.Key, item.Value);
                        }
                    }
                }
            }

            return this.Data(stringBuilder.ToString());
        }

        /// <summary>
        /// Sets the data to write to the http client.
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
        public IHttpPostClient AccessRequestStream(Action<Stream> access)
        {
            this._accessRequestStreamMethod = access;

            return this;
        }

        /// <summary>
        /// Access the response stream prior to reading the response from the stream.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public IHttpPostClient AccessResponseStream(Action<Stream> access)
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
                result = ProcessResponseStream(stream);

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
                result = ProcessResponseStream(stream);

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
                var response = this.GetResponse(state.Request, state.Callback);
                response.Close();
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
                var response = this.GetResponse(state.Request, state.Callback);
                response.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        private void ProcessRequestStream(Stream stream, string data)
        {
            this.Request.ContentLength = data.Length;

            // Allow the client to access the request stream.

            if (this._accessRequestStreamMethod != null)
            {
                this._accessRequestStreamMethod(stream);
            }

            // Write the data to the stream.

            using (var writer = new StreamWriter(stream))
            {
                writer.Write(data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private string ProcessResponseStream(Stream stream)
        {
            var result = string.Empty;

            // Allow the caller to access the stream if they've specified.

            if (this._accessResponseStreamMethod != null)
            {
                this._accessResponseStreamMethod(stream);
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
