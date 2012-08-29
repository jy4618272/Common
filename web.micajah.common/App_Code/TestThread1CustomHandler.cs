using System;
using System.Globalization;
using System.Threading;
using Micajah.Common.Bll;

public class TestThread1CustomHandler : IThreadStateProvider
{
    #region Public Properties

    public ThreadStateType ThreadState { get; set; }
    public Exception ErrorException { get; set; }

    #endregion

    #region Public Methods

    public void Start()
    {
        ThreadState = ThreadStateType.Running;
        Thread.Sleep(30000);
        ThreadState = ThreadStateType.Finished;
    }

    #endregion
}
