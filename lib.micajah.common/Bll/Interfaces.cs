namespace Micajah.Common.Bll
{
    /// <summary>
    /// Provides a mechanism to get an thread state.
    /// </summary>
    public interface IThreadStateProvider
    {
        /// <summary>
        /// Class store current thread state.
        /// </summary>
        ThreadStateType ThreadState { get; set; }
        System.Exception ErrorException { get; set; }
        void Start();
    }
}
