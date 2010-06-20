namespace GrandTarghee.Framework.MVVM
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public interface IViewModel<TView> : IViewModel where TView : IView
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        TView View { get; }

        #endregion
    }
}
