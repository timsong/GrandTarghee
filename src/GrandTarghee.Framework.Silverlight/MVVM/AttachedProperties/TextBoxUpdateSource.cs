using System.Windows;
using System.Windows.Controls;

namespace GrandTarghee.Framework.MVVM
{
    public class Text
    {
        #region Dependency Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty UpdateSourceProperty;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        static Text()
        {
            UpdateSourceProperty = DependencyProperty.RegisterAttached("UpdateSource", typeof(bool), typeof(Text), new PropertyMetadata(false, (s, e) => OnPropertyChanged(s as TextBox, e)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetUpdateSource(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpdateSourceProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetUpdateSource(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateSourceProperty, value);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnPropertyChanged(TextBox textBox, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                textBox.TextChanged += OnTextChanged;
            }
            else
            {
                textBox.TextChanged -= OnTextChanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);

            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }

        #endregion
    }
}
