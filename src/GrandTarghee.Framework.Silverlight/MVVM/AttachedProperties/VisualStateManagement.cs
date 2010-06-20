using System.Windows;
using System.Windows.Controls;

namespace GrandTarghee.Framework.MVVM
{
    public class VisualStateManagement
    {
        #region Dependency Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty VisualStateProperty;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        static VisualStateManagement()
        {
            VisualStateProperty = DependencyProperty.RegisterAttached("VisualState", typeof(string), typeof(VisualStateManagement), new PropertyMetadata(string.Empty, (s, e) => OnPropertyChanged(s as Control, e)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetVisualState(DependencyObject obj)
        {
            return (string)obj.GetValue(VisualStateProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetVisualState(DependencyObject obj, string value)
        {
            obj.SetValue(VisualStateProperty, value);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="e"></param>
        private static void OnPropertyChanged(Control control, DependencyPropertyChangedEventArgs e)
        {
            if (control == null || e.NewValue == null)
            {
                return;
            }

            VisualStateManager.GoToState(control, e.NewValue.ToString(), true);
        }

        #endregion
    }
}
