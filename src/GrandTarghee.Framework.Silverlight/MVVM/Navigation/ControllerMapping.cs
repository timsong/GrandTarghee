using System;
using System.Windows.Navigation;

namespace GrandTarghee.Framework.MVVM
{
    public class ControllerMapping
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MappedUri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Uri { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Uri MapUri(Uri uri)
        {
            var mapping = new UriMapping();
            mapping.MappedUri = new Uri(this.MappedUri, UriKind.Relative);
            mapping.Uri = new Uri(this.Uri, UriKind.Relative);

            return mapping.MapUri(uri);
        }

        #endregion
    }
}
