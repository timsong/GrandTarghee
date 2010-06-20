using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GrandTarghee.Framework.MVVM
{
    public class EventCommand : TriggerAction<FrameworkElement>
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty;

        /// <summary>
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty;

        /// <summary>
        /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        static EventCommand()
        {
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventCommand), new PropertyMetadata(null, (s, e) => OnCommandChanged(s as EventCommand, e)));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventCommand), new PropertyMetadata(null, (s, e) => OnCommandPropertyChanged(s as EventCommand, e)));
            MustToggleIsEnabledProperty = DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(EventCommand), new PropertyMetadata(false, (s, e) => OnMustToggleIsEnabledPropertyChanged(s as EventCommand, e)));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the EventArgs passed to the event handler will be forwarded to the ICommand's Execute method
        /// when the event is fired (if the bound ICommand accepts an argument of type EventArgs).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }

        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty. Because of limitations
        /// of Silverlight, you can only set databindings on this property. If you
        /// wish to use hard coded values, use the <see cref="CommandParameterValue" />
        /// property.
        /// </summary>
        public object CommandParameter
        {
            get { return base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. If this property
        /// is false, the element will not be disabled when the command's
        /// CanExecute method changes.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get { return (bool)this.GetValue(MustToggleIsEnabledProperty); }
            set { this.SetValue(MustToggleIsEnabledProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Provides a simple way to invoke this trigger programatically without any EventArgs.
        /// </summary>
        public void Invoke()
        {
            Invoke(null);
        }

        /// <summary>
        /// Executes the trigger.
        /// </summary>
        /// <param name="parameter">The EventArgs of the fired event.</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedElementIsDisabled())
            {
                return;
            }

            var command = this.GetCommand();
            var commandParameter = this.CommandParameter;

            if (commandParameter == null && this.PassEventArgsToCommand)
            {
                commandParameter = parameter;
            }

            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        /// <summary>
        /// Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Control GetAssociatedObject()
        {
            return base.AssociatedObject as Control;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ICommand GetCommand()
        {
            return this.Command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        private static void OnCommandChanged(EventCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            var oldCommand = e.OldValue as ICommand;
            var newCommand = e.NewValue as ICommand;

            if (e.OldValue != null)
            {
                oldCommand.CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += element.OnCommandCanExecuteChanged;
            }

            element.EnableDisableElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        private static void OnCommandPropertyChanged(EventCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            if (element.AssociatedObject == null)
            {
                return;
            }

            element.EnableDisableElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        private static void OnMustToggleIsEnabledPropertyChanged(EventCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            if (element.AssociatedObject == null)
            {
                return;
            }

            element.EnableDisableElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool AssociatedElementIsDisabled()
        {
            var element = GetAssociatedObject();

            return element != null && !element.IsEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableDisableElement()
        {
            var element = GetAssociatedObject();

            if (element == null)
            {
                return;
            }

            var command = this.GetCommand();

            if (this.MustToggleIsEnabled && command != null)
            {
                element.IsEnabled = command.CanExecute(this.CommandParameter);
            }
        }

        #endregion
    }
}
