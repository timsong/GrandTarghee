using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace GrandTarghee.Framework.MVVM
{
    public abstract class ViewModelBase<TView> : IViewModel<TView>, IViewModel where TView : IView
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IController Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TView View { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public ViewModelBase(TView view)
        {
            this.View = view;
            this.View.ViewModel = this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IView GetView()
        {
            return this.View;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName)
        {
            this.OnNotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            this.OnNotifyPropertyChanged(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void VerifyPropertyName(string propertyName)
        {
            var type = this.GetType();

            if (type.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Propert not found.", propertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            var handler = PropertyChanged;

            if (handler != null)
            {
                DispatcherHelper.DispatchOnUIThread(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        protected virtual void OnNotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Value must be a lamda expression", "expression");
            }

            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("‘x’ should be a member expression");
            }

            OnNotifyPropertyChanged(body.Member.Name);
        }

        #endregion
    }
}
