using System;
using System.Windows.Input;

namespace GrandTarghee.Framework.MVVM
{
    public interface IReverseCommand : ICommand
    {
        event EventHandler<CommandEventArgs> CommandExecuted;
    }
}
