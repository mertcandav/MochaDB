using System.Collections.Generic;

namespace MochaDB.Streams {
    /// <summary>
    /// Interface for MochaDB writers.
    /// </summary>
    /// <typeparam name="T">Value type of writer.</typeparam>
    public interface IMochaWriter<T> {
        #region Methods

        void Write(IMochaCollection<T> destination);
        void Write(IMochaCollection<T> destination,int start);
        void Write(IMochaCollection<T> destination,int start,int count);
        void Read(IEnumerable<T> source);
        void Read(IEnumerable<T> source,int start);
        void Read(IEnumerable<T> source,int start,int count);
        T[] ToArray();

        #endregion

        #region Properties

        int Length { get; }

        #endregion
    }
}
