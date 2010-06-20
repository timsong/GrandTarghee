using System;
using System.Windows;
using System.Windows.Data;

namespace OTI.Framework.Silverlight.MVVM.Components
{
    public class DataContextProxy : FrameworkElement
    {
        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register("Context", typeof(Object), typeof(DataContextProxy), null);

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public object Context
        {
            get { return (object)base.GetValue(ContextProperty); }
            set { base.SetValue(ContextProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BindingPropertyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BindingMode BindingMode { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        public DataContextProxy()
        {
            this.Loaded += (s, e) =>
            {
                var binding = new Binding();

                if (!string.IsNullOrEmpty(this.BindingPropertyName))
                {
                    binding.Path = new PropertyPath(this.BindingPropertyName);
                }

                binding.Source = this.DataContext;
                binding.Mode = this.BindingMode;

                this.SetBinding(DataContextProxy.ContextProperty, binding);
            };
        }

        #endregion
    }
}
