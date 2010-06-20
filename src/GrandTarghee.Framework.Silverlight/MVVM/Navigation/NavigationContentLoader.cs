using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Windows.Navigation;

using Microsoft.Practices.ServiceLocation;

namespace GrandTarghee.Framework.MVVM
{
    [ContentProperty("ControllerMappings")]
    public class NavigationContentLoader : INavigationContentLoader
    {
        private Collection<ControllerMapping> _controllerMappings;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Collection<ControllerMapping> ControllerMappings
        {
            get { return this._controllerMappings; }
            private set { this._controllerMappings = value; }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        public NavigationContentLoader()
        {
            this.ControllerMappings = new Collection<ControllerMapping>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUri"></param>
        /// <param name="currentUri"></param>
        /// <param name="userCallback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public IAsyncResult BeginLoad(Uri targetUri, Uri currentUri, AsyncCallback userCallback, object asyncState)
        {
            Uri destinationUri = null;

            // Parse the controller name to get its type.

            var controllerName = ParseControllerName(targetUri, out destinationUri);

            // Build an instance of the controller.

            IController controller = ServiceLocator.Current.GetInstance<IController>(controllerName);
            controller.Init(ParseQueryString(destinationUri));

            // Return the view as the async result.

            var result = new NavigationContentLoaderAsyncResult(asyncState);
            result.Result = controller.GetViewModel().GetView();
            userCallback(result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUri"></param>
        /// <param name="currentUri"></param>
        /// <returns></returns>
        public bool CanLoad(Uri targetUri, Uri currentUri)
        {
            Uri destinationUri = null;

            // Parse the controller name to get its type.

            var controllerName = ParseControllerName(targetUri, out destinationUri);

            if (string.IsNullOrEmpty(controllerName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        public void CancelLoad(IAsyncResult asyncResult)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public LoadResult EndLoad(IAsyncResult asyncResult)
        {
            return new LoadResult(((NavigationContentLoaderAsyncResult)asyncResult).Result);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUri"></param>
        /// <param name="destinationUri"></param>
        /// <returns></returns>
        private string ParseControllerName(Uri targetUri, out Uri destinationUri)
        {
            // Remove trailing slashes from the target uri.

            if (targetUri.OriginalString.EndsWith("/"))
            {
                targetUri = new Uri(targetUri.OriginalString.Substring(0, targetUri.OriginalString.Length - 1));
            }

            // Find a matching uri.

            foreach (var item in this.ControllerMappings)
            {
                if ((destinationUri = item.MapUri(targetUri)) != null)
                {
                    return item.Controller;
                }
            }

            destinationUri = null;
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private IDictionary<string, string> ParseQueryString(Uri uri)
        {
            var dictionary = new Dictionary<string, string>();

            // Grab the query string off of the uri

            var parts = uri.OriginalString.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1)
            {
                var query = parts[1];

                // Parse each query string.

                var queryStrings = query.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var queryString in queryStrings)
                {
                    // Parse each identifier.

                    var identifier = queryString.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                    if (identifier.Length > 1)
                    {
                        if (!dictionary.ContainsKey(identifier[0]))
                        {
                            dictionary.Add(identifier[0], identifier[1]);
                        }
                    }
                }
            }

            return dictionary;
        }

        #endregion
    }
}
