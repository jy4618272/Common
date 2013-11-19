using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Micajah.Common.Configuration;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class StartThread
    {
        #region Members

        private string m_ClassName;
        private string m_StartTime;        

        #endregion

        #region Properties

        public string ClassName
        {
            get { return m_ClassName; }
            private set { m_ClassName = value; }
        }

        public string StartTime
        {
            get { return m_StartTime; }
            private set { m_StartTime = value; }
        }

        #endregion

        #region Internal Methods

        internal static StartThread Create(StartThreadElement value)
        {
            StartThread startThread = new StartThread();
            startThread.ClassName = value.ClassFullName;
            startThread.StartTime = value.StartTime;

            return startThread;
        }

        #endregion

        #region Public Methods

        public IThreadStateProvider GetStartThreadClassInstance()
        {
            return (IThreadStateProvider)Support.CreateInstance(ClassName);
        }

        #endregion
    }

    [Serializable]
    public class StartThreadCollection : Collection<StartThread>
    {
        #region Private Properties

        private List<StartThread> ItemList
        {
            get { return base.Items as List<StartThread>; }
        }

        #endregion

        #region Public Properties

        public StartThread this[string className]
        {
            get
            {
                int index = this.FindIndexByClassName(className);
                return (((index < 0) || (index >= this.Count)) ? null : base[index]);
            }
            set
            {
                int index = this.FindIndexByClassName(className);
                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }

        #endregion

        #region Private Methods

        private int FindIndexByClassName(string value)
        {
            return this.ItemList.FindIndex(
                delegate(StartThread startThread)
                {
                    return (string.Compare(startThread.ClassName, value, StringComparison.Ordinal) == 0);
                });
        }

        #endregion

        #region Internal Methods

        internal static StartThreadCollection Load()
        {
            StartThreadCollection startThreads = new StartThreadCollection();

            if (FrameworkConfiguration.Current.StartThreads == null) return startThreads;

            foreach (StartThreadElement element in FrameworkConfiguration.Current.StartThreads)
            {
                StartThread startThread = StartThread.Create(element);
                if (startThread != null) startThreads.Add(startThread);
            }

            return startThreads;
        }

        #endregion
    }

    public class StartThreadTimer : System.Timers.Timer
    {
        public StartThread StartThread { get; set; }

        public StartThreadTimer()
            : base()
        {
        }
    }
}
