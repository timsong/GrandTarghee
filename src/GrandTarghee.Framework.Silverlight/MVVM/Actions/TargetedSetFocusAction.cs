using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GrandTarghee.Framework.MVVM
{
    [TypeConstraintAttribute(typeof(DependencyObject))]
    public class TargetedSetFocusAction : TargetedTriggerAction<Control>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (Target != null)
            {
                Target.Focus();
            }
        }

        #endregion
    }
}
