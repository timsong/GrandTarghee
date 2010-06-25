using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GrandTarghee.Framework.WebClient.DSL
{
    public class HttpClient : IHttpClient
    {
        private Dictionary<string, string> _headers;
        private Action<HttpWebRequest> _accessRequestMethod;
        private int _timeout;
        private string _contentType;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueryString { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        public HttpClient()
        {
            this._timeout = 0;
            this._headers = new Dictionary<string, string>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the base uri of the http client.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public IHttpClient SetBaseUri(string baseUri)
        {
            this.BaseUri = new Uri(baseUri);
            return this;
        }

        /// <summary>
        /// Sets the base uri of the http client.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public IHttpClient SetBaseUri(Uri baseUri)
        {
            this.BaseUri = baseUri;
            return this;
        }

        /// <summary>
        /// Adds a querystring key value pair.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHttpClient AddQueryString(string queryStringKey, string queryStringValue)
        {
            if (string.IsNullOrEmpty(this.QueryString))
            {
                if(string.IsNullOrEmpty(this.BaseUri.Query))
                {
                    this.QueryString = string.Format("?{0}={1}", queryStringKey, queryStringValue);
                }
                else
                {
                    this.QueryString = string.Format("{0}&{1}={2}", this.BaseUri.Query, queryStringKey, queryStringValue);
                }
            }
            else
            {
                this.QueryString += string.Format("&{0}={1}", queryStringKey, queryStringValue);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IHttpClient SetQueryString(string queryString)
        {
            // Remove the leading question mark or Amperstands.

            if(queryString.StartsWith("?") || queryString.StartsWith("&"))
            {
                queryString = queryString.Substring(1);
            }

            // Combine the querystring from the base url if there are any.

            if (string.IsNullOrEmpty(this.BaseUri.Query))
            {
                this.QueryString = string.Format("?{0}", queryString);
            }
            else
            {
                this.QueryString = string.Format("{0}&{1}", this.BaseUri.Query, queryString);
            }
            
            return this;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IHttpClient SetQueryString(IDictionary<string, string> queryString)
        {
            var queryStringBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(this.BaseUri.Query))
            {
                queryStringBuilder.Append("?");
            }
            else
            {
                queryStringBuilder.Append(this.BaseUri.Query);
            }

            foreach (var query in queryString)
            {
                if (queryStringBuilder.Length == 1)
                {
                    queryStringBuilder.AppendFormat("{0}={1}", query.Key, query.Value);
                }
                else
                {
                    queryStringBuilder.AppendFormat("&{0}={1}", query.Key, query.Value);
                }
            }

            this.QueryString = queryStringBuilder.ToString();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStringFormat"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IHttpClient SetQueryString(string queryStringFormat, params object[] args)
        {
            var query = string.Format(queryStringFormat, args);

            if (query.StartsWith("?") || query.StartsWith("&"))
            {
                query = query.Substring(1);
            }

            if (string.IsNullOrEmpty(this.BaseUri.Query))
            {
                this.QueryString = string.Format("?{0}", query);
            }
            else
            {
                this.QueryString = string.Format("{0}&{1}", this.BaseUri.Query, query);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHttpClient AddHeader(string header, string value)
        {
            if (this._headers.ContainsKey(header))
            {
                this._headers[header] = value;
            }
            else
            {
                this._headers.Add(header, value);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public IHttpClient AddHeaders(IDictionary<string, string> headers)
        {
            foreach (var pair in headers)
            {
                if (this._headers.ContainsKey(pair.Key))
                {
                    this._headers[pair.Key] = pair.Value;
                }
                else
                {
                    this._headers.Add(pair.Key, pair.Value);
                }
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public IHttpClient Timeout(int timeout)
        {
            this._timeout = timeout;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IHttpClient ContentType(string contentType)
        {
            this._contentType = contentType;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public IHttpClient AccessHttpRequest(Action<HttpWebRequest> access)
        {
            this._accessRequestMethod = access;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IHttpGetClient Get()
        {
            var request = BuildRequest();

            return new HttpGetClient(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IHttpPostClient Post()
        {
            var request = BuildRequest();

            return new HttpPostClient(request);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest BuildRequest()
        {
            var uri = BuildUri();
            var request = (HttpWebRequest)WebRequest.Create(uri);

            // Add the headers.

            foreach (var header in this._headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (this._timeout > 0)
            {
                request.Timeout = this._timeout;
            }

            if (!string.IsNullOrEmpty(this._contentType))
            {
                request.ContentType = this._contentType;
            }

            // Run the request access method if it exists.

            if (this._accessRequestMethod != null)
            {
                this._accessRequestMethod(request);
            }

            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Uri BuildUri()
        {
            if (this.BaseUri == null)
            {
                throw new InvalidOperationException("The base uri needs to be set.");
            }

            var builder = new UriBuilder(this.BaseUri.Scheme, this.BaseUri.Host);
            builder.Path = this.BaseUri.LocalPath;

            if (!string.IsNullOrEmpty(this.QueryString))
            {
                builder.Query = this.QueryString.Substring(1);
            }

            return builder.Uri;
        }

        #endregion
    }
}
