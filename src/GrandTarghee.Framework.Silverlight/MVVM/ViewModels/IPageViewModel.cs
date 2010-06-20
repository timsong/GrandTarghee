using System.ComponentModel;

namespace GrandTarghee.Framework.MVVM
{
    public interface IPageViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        string PageTitle { get; set; }

        #endregion
    }
}
