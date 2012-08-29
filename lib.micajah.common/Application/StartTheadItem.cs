using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Micajah.Common.Bll;

namespace Micajah.Common.Application
{
    public class StartTheadItem
    {
        public Thread Thread { get; set; }
        public IThreadStateProvider ThreadStateProvider { get; set; }
        public int Interval { get; set; }
        public DateTime TheadRunTime { get; set; }
        public int NumberOfFails { get; set; }
    }
}
