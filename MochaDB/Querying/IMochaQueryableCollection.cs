using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Querying {
    /// <summary>
    /// Interface for queryable collections.
    /// </summary>
    /// <typeparam name="T">Type of collection value type.</typeparam>
    public interface IMochaQueryableCollection<T> {
        #region Methods

        public IEnumerable<T> Select(Func<T,T> query);
        public IEnumerable<T> Select(Func<T,int,T> query);
        public IEnumerable<T> Where(Func<T,bool> query);
        public IEnumerable<T> Where(Func<T,int,bool> query);
        public IEnumerable<T> OrderByDescending(Func<T,T> query);
        public IEnumerable<T> OrderBy(Func<T,T> query);
        public IEnumerable<IGrouping<T,T>> GroupBy(Func<T,T> query);

        #endregion
    }
}
