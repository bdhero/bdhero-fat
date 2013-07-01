using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DotNetUtils
{
    /// <see cref="http://stackoverflow.com/a/2984664/467582"/>
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to Title Case (a.k.a., Proper Case).
        /// </summary>
        public static String ToTitle(this String str)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            var titleCase = textInfo.ToTitleCase(textInfo.ToLower(str));
            return titleCase;
        }
    }

    /// <summary>
    /// Dictionary that supports keys with a list of values (one-to-many).
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, IList<TValue>>
    {
        /// <summary>
        /// Adds the specified value to the list at the specified key.
        /// If the key is not already present in the dictionary, it is added automatically.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                this[key] = new List<TValue>();
            this[key].Add(value);
        }
    }

    public static class ToMultiValueDictionaryExtension
    {
        /// <summary>
        /// Converts the Enumerable to a MultiValueDictionary using the specified key provider function.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <param name="keyProvider"></param>
        /// <returns></returns>
        public static MultiValueDictionary<TKey, TValue> ToMultiValueDictionary<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keyProvider)
        {
            var dic = new MultiValueDictionary<TKey, TValue>();
            foreach (var value in values)
            {
                dic.Add(keyProvider(value), value);
            }
            return dic;
        }
    }

    /// <see cref="http://stackoverflow.com/a/527840/467582"/>
    public static class NotificationExtensions
    {
        #region Delegates

        /// <summary>
        /// A property changed handler without the property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void PropertyChangedHandler<TSender>(TSender sender);

        #endregion

        /// <summary>
        /// Notifies listeners about a change.
        /// </summary>
        /// <param name="EventHandler">The event to raise.</param>
        /// <param name="Property">The property that changed.</param>
        public static void Notify(this PropertyChangedEventHandler EventHandler, Expression<Func<object>> Property)
        {
            // Check for null
            if (EventHandler == null)
                return;

            // Get property name
            var lambda = Property as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            ConstantExpression constantExpression;
            if (memberExpression.Expression is UnaryExpression)
            {
                var unaryExpression = memberExpression.Expression as UnaryExpression;
                constantExpression = unaryExpression.Operand as ConstantExpression;
            }
            else
            {
                constantExpression = memberExpression.Expression as ConstantExpression;
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            // Invoke event
            foreach (Delegate del in EventHandler.GetInvocationList())
            {
                del.DynamicInvoke(new[]
                    {
                        constantExpression.Value, new PropertyChangedEventArgs(propertyInfo.Name)
                    });
            }
        }


        /// <summary>
        /// Subscribe to changes in an object implementing INotifiyPropertyChanged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ObjectThatNotifies">The object you are interested in.</param>
        /// <param name="Property">The property you are interested in.</param>
        /// <param name="Handler">The delegate that will handle the event.</param>
        public static void SubscribeToChange<T>(this T ObjectThatNotifies, Expression<Func<object>> Property, PropertyChangedHandler<T> Handler) where T : INotifyPropertyChanged
        {
            // Add a new PropertyChangedEventHandler
            ObjectThatNotifies.PropertyChanged += (s, e) =>
            {
                // Get name of Property
                var lambda = Property as LambdaExpression;
                MemberExpression memberExpression;
                if (lambda.Body is UnaryExpression)
                {
                    var unaryExpression = lambda.Body as UnaryExpression;
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
                else
                {
                    memberExpression = lambda.Body as MemberExpression;
                }
                var propertyInfo = memberExpression.Member as PropertyInfo;

                // Notify handler if PropertyName is the one we were interested in
                if (e.PropertyName.Equals(propertyInfo.Name))
                {
                    Handler(ObjectThatNotifies);
                }
            };
        }
    }

    public static class TimeSpanExtensions
    {
        public static string ToStringShort(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        public static string ToStringMedium(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fff");
        }

        public static string ToStringLong(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fffffff");
        }
    }
}
