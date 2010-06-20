namespace GrandTarghee.Framework.MVVM
{
    public static class RunningViewModelExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public static void SetRunning(this IRunningViewModel viewModel)
        {
            viewModel.IsRunning = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="runningMessage"></param>
        public static void SetRunning(this IRunningViewModel viewModel, string runningMessage)
        {
            viewModel.RunningMessage = runningMessage;
            viewModel.IsRunning = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public static void StopRunning(this IRunningViewModel viewModel)
        {
            viewModel.IsRunning = false;
        }

        #endregion
    }
}
