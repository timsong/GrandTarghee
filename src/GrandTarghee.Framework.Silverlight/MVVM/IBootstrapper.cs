using Microsoft.Practices.ServiceLocation;

namespace GrandTarghee.Framework.MVVM
{
    public interface IBootstrapper
    {
        #region Methods
        
        /// <summary>
        /// Bootstraps the silverlight application with the MVVM framework.
        /// </summary>
        /// <returns></returns>
        IServiceLocator Bootstrap();

        #endregion
    }
}
