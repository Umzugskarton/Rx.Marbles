using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Extension
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Creates an observable sequence that emits the new value of a specific property
        /// whenever that property changes on an INotifyPropertyChanged object.
        /// </summary>
        /// <typeparam name="TSource">The type of the object implementing INotifyPropertyChanged.</typeparam>
        /// <typeparam name="TProperty">The type of the property being observed.</typeparam>
        /// <param name="source">The object to observe.</param>
        /// <param name="propertyExpression">An expression representing the property to observe (e.g., x => x.MyProperty).</param>
        /// <returns>An observable sequence of the property's new values.</returns>
        public static IObservable<TProperty?> ObserveProperty<TSource, TProperty>(
            this TSource source,
            Expression<Func<TSource, TProperty>> propertyExpression)
            where TSource : INotifyPropertyChanged
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

            // Get the PropertyInfo from the expression
            if (propertyExpression.Body is not MemberExpression memberExpression)
            {
                throw new ArgumentException("Expression must be a property access.", nameof(propertyExpression));
            }

            if (memberExpression.Member is not PropertyInfo propertyInfo)
            {
                throw new ArgumentException("Expression must be a property access.", nameof(propertyExpression));
            }

            string propertyName = propertyInfo.Name;

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => source.PropertyChanged += handler,
                handler => source.PropertyChanged -= handler)
                .Where(e => e.EventArgs.PropertyName == propertyName)
                .Select(_ => (TProperty?)propertyInfo.GetValue(source)); // Get the actual property value
        }

    }
}
