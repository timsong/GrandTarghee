using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace GrandTarghee.Framework
{
    public static class NotifyPropertyChangedExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="expression"></param>
        public static void Notify(this PropertyChangedEventHandler handler, object sender, Expression<Func<object>> expression)
        {
            if (handler != null)
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

                var propertyName = body.Member.Name;
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="expression"></param>
        /// <param name="handler"></param>
        public static void SubscribeToNotification<T>(this T sender, Expression<Func<object>> expression, PropertyChangedEventHandler handler) where T : INotifyPropertyChanged
        {
            sender.PropertyChanged += (s, e) =>
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

                var propertyName = body.Member.Name;

                if (e.PropertyName.Equals(propertyName))
                {
                    handler(s, e);
                }
            };
        }

        #endregion
    }
}
