using System;
using System.Collections.Generic;
using System.Net;

namespace GrandTarghee.Framework.WebClient
{
    public interface IHttpClient
    {
        #region Properties

        /// <summary>
        /// The base uri of the http client.
        /// </summary>
        Uri BaseUri { get; }

        /// <summary>
        /// The query string of the http client.
        /// </summary>
        string QueryString { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the base uri of the http client.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        IHttpClient SetBaseUri(string baseUri);

        /// <summary>
        /// Sets the base uri of the http client.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        IHttpClient SetBaseUri(Uri baseUri);

        /// <summary>
        /// Adds a querystring key value pair.
        /// </summary>
        /// <param name="queryStringKey"></param>
        /// <param name="queryStringValue"></param>
        /// <returns></returns>
        IHttpClient AddQueryString(string queryStringKey, string queryStringValue);

        /// <summary>
        /// Sets the query string of the http client.
        /// format: p1=p1&p2=p2.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        IHttpClient SetQueryString(string queryString);

        /// <summary>
        /// Sets the query string of the http client by building it from the dictionary.
        /// format: key=value&...
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        IHttpClient SetQueryString(IDictionary<string, string> queryString);

        /// <summary>
        /// Sets the query string of the http client by building it with the string format.
        /// </summary>
        /// <param name="queryStringFormat"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        IHttpClient SetQueryString(string queryStringFormat, params object[] args);

        /// <summary>
        /// Sets a single header value of the http client.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IHttpClient AddHeader(string header, string value);

        /// <summary>
        /// Sets a collection of header values of the http client.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        IHttpClient AddHeaders(IDictionary<string, string> headers);

        /// <summary>
        /// Sets the time out of the http client.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IHttpClient Timeout(int timeout);

        /// <summary>
        /// Sets the content-type of the http client.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        IHttpClient ContentType(string contentType);

        /// <summary>
        /// Accesses the http web request prior to retrieving the response.
        /// </summary>
        /// <param name="modifyRequest"></param>
        /// <returns></returns>
        IHttpClient AccessHttpRequest(Action<HttpWebRequest> access);

        /// <summary>
        /// Sets the method type to a get.
        /// </summary>
        /// <returns></returns>
        IHttpGetClient Get();

        /// <summary>
        /// Sets the method type to a post.
        /// </summary>
        /// <returns></returns>
        IHttpPostClient Post();

        #endregion
    }
}
