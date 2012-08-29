using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Micajah.Common.LdapAdapter;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class LdapProcess
    {
        #region Properties

        public string Message { get; set; }
        public string MessageDeactivatedLogins { get; set; }
        public string MessageActivatedLogins { get; set; }
        public string MessageCreatedLogins { get; set; }
        public string MessageError { get; set; }
        public string ProcessId { get; set; }
        public ThreadStateType ThreadStateType { get; set; }
        public DataView Data { get; set; }
        public DataView DataDeactivatedLogins { get; set; }
        public DataView DataActivatedLogins { get; set; }
        public DataView DataCreatedLogins { get; set; }
        public List<LdapProcessLog> Logs { get; set; }

        #endregion
    }
}
