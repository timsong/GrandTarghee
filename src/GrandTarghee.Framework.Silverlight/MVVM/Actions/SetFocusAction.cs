using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GrandTarghee.Framework.MVVM
{
    public class SetFocusAction : TriggerAction<Control>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.Focus();
            }
        }

        #endregion
    }
}
