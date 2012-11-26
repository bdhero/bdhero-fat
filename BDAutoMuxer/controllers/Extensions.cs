using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDAutoMuxer.controllers
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
}
