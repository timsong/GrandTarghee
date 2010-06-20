using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace GrandTarghee.Framework.MVVM
{
    public class ReverseCommand : TriggerBase<FrameworkElement>
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="ReverseCommand" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IReverseCommand Command
        {
            get { return (IReverseCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        static ReverseCommand()
        {
            CommandProperty = DependencyProperty.Register("Command", typeof(IReverseCommand), typeof(ReverseCommand), new PropertyMetadata(null, (s, e) => OnCommandChanged(s as ReverseCommand, e)));
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        private static void OnCommandChanged(ReverseCommand element, DependencyPropertyChangedEventArgs e)
        {
            var oldCommand = e.OldValue as IReverseCommand;
            var newCommand = e.NewValue as IReverseCommand;

            if (e.OldValue != null)
            {
                oldCommand.CommandExecuted -= element.OnCommandExecuted;
            }

            if (newCommand != null)
            {
                newCommand.CommandExecuted += element.OnCommandExecuted;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandExecuted(object sender, CommandEventArgs e)
        {
            // When the command is executed we execute the trigger
            // note, we pass through the command parameter.

            base.InvokeActions(e.CommandParameter);
        }

        #endregion

    }
}
