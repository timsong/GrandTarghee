using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GrandTarghee.Framework.MVVM
{
    public class CommandOnKeyPressBehavior : Behavior<TextBox>
    {
        #region Dependency Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CommandProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty KeyProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the binding property for the command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key to fire off the event.
        /// </summary>
        public Key Key
        {
            get { return (Key)base.GetValue(KeyProperty); }
            set { base.SetValue(KeyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the binding property for the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        static CommandOnKeyPressBehavior()
        {
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandOnKeyPressBehavior), new PropertyMetadata(null));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandOnKeyPressBehavior), new PropertyMetadata(null));
            KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(CommandOnKeyPressBehavior), new PropertyMetadata(Key.Enter));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnTextBoxKeyDown;

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnTextBoxKeyDown;
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == this.Key) && (AssociatedObject.Text.Length != 0))
            {
                if (this.Command != null && this.Command.CanExecute(CommandParameter))
                {
                    var bindingExpression = AssociatedObject.GetBindingExpression(TextBox.TextProperty);

                    if (bindingExpression != null)
                    {
                        bindingExpression.UpdateSource();
                    }

                    this.Command.Execute(CommandParameter);
                }
            }
        }

        #endregion
    }
}
