using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Practices.ServiceLocation;

namespace GrandTarghee.Framework.MVVM
{
    public static class ApplicationExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="application"></param>
        /// <param name="initParams"></param>
        /// <returns></returns>
        public static Application Startup<TController>(this Application application, IDictionary<string, string> initParams)
        {
            // Generate the startup controller.

            IController controller = ServiceLocator.Current.GetInstance<IController>(typeof(TController).Name);
            controller.Init(initParams);

            application.RootVisual = controller.GetViewModel().GetView() as UIElement;
            return application;
        }

        #endregion
    }
}
