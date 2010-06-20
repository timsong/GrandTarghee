using GrandTarghee.Framework.MVVM;

namespace GrandTarghee.Framework.MVVM
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public interface IController<TViewModel> : IController where TViewModel : IViewModel
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        TViewModel ViewModel { get; }

        #endregion
    }
}
