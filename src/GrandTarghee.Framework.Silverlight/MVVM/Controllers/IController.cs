using System.Collections.Generic;

namespace GrandTarghee.Framework.MVVM
{
    /// <summary>
    /// 
    /// </summary>
    public interface IController
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IViewModel GetViewModel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStrings"></param>
        void Init(IDictionary<string, string> queryStrings);

        #endregion
    }
}
