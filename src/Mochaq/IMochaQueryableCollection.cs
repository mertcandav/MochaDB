using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MochaDB.Mochaq {
    /// <summary>
    /// Interface for queryable collections.
    /// </summary>
    /// <typeparam name="T">Type of collection value type.</typeparam>
    public interface IMochaQueryableCollection<T>:IEnumerable<T>, IEnumerable {
        #region Methods

        IEnumerable<T> Select(Func<T,T> query);
        IEnumerable<T> Select(Func<T,int,T> query);
        IEnumerable<T> Where(Func<T,bool> query);
        IEnumerable<T> Where(Func<T,int,bool> query);
        IEnumerable<T> OrderByDescending(Func<T,T> query);
        IEnumerable<T> OrderBy(Func<T,T> query);
        IEnumerable<IGrouping<T,T>> GroupBy(Func<T,T> query);

        #endregion
    }
}
