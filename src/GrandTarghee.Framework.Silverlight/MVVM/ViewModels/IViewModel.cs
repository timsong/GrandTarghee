using System.ComponentModel;

namespace GrandTarghee.Framework.MVVM
{
    /// <summary>
    /// 
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        IController Controller { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IView GetView();

        #endregion
    }
}
