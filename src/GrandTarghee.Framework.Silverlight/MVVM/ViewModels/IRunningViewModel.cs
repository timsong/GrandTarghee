using System.ComponentModel;

namespace GrandTarghee.Framework.MVVM
{
    public interface IRunningViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string RunningMessage { get; set; }

        #endregion
    }
}
