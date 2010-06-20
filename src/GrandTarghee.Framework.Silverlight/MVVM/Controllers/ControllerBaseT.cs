using System;
using System.Collections.Generic;

namespace GrandTarghee.Framework.MVVM
{
    public abstract class ControllerBase<TViewModel> : IController<TViewModel> where TViewModel : IViewModel
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> QueryStrings { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IViewModel IController.GetViewModel()
        {
            return this.ViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        public TViewModel ViewModel { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public ControllerBase(TViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        public virtual void Init(IDictionary<string, string> queryStrings)
        {
            this.QueryStrings = queryStrings;
        }

        #endregion
    }
}
