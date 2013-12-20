﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
    [Serializable]
    public class LdapProcessLog
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
