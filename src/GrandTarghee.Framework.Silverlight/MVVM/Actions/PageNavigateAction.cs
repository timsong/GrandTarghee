using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GrandTarghee.Framework.MVVM
{
    public class PageNavigateAction : TargetedTriggerAction<Page>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (this.Target != null)
            {
                this.Target.NavigationService.Navigate(new Uri(parameter as string, UriKind.Relative));
            }
        }

        #endregion
    }
}
