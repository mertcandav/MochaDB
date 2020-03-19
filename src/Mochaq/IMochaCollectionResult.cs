namespace MochaDB.Mochaq {
    /// <summary>
    /// Interface for MochaDB collection results.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IMochaCollectionResult<T>:IMochaReadonlyCollection<T>, IMochaQueryableCollection<T>, IMochaResult {
    }
}
