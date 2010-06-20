using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace GrandTarghee.Framework
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Methods

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
                throw new ArgumentException("Property not found.", propertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            // Verify the property changed.

            this.VerifyPropertyName(propertyName);

            // Fire off the property changed event.

            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
