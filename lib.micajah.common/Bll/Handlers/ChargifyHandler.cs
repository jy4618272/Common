using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Micajah.Common.Bll.Providers;
using ChargifyNET;
using System.Diagnostics;

namespace Micajah.Common.Bll.Handlers
{
    public class ChargifyHandler : IThreadStateProvider
    {
        #region IThreadStateProvider Members

        public ThreadStateType ThreadState { get; set; }
        public Exception ErrorException { get; set; }

        public void Start()
        {
            this.ThreadState = ThreadStateType.Running;
            try
            {
                OrganizationCollection _orgs = OrganizationProvider.GetOrganizations(false, false);
                ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
                foreach (Organization _org in _orgs)
                {
                    InstanceCollection _insts = InstanceProvider.GetInstances(_org.OrganizationId, false);
                    foreach (Instance _inst in _insts)
                    {
                        ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(_chargify, _org.OrganizationId, _inst.InstanceId);
                        if (_custSubscr == null) continue;
                        ChargifyProvider.UpdateSubscriptionAllocations(_chargify, _custSubscr.SubscriptionID, _org.OrganizationId, _inst.InstanceId);                        
                    }
                }
            }
            catch (Exception ex)
            {
                this.ThreadState = ThreadStateType.Failed;
                this.ErrorException = ex;
                EventLog.WriteEntry("Micajah.Common.Bll.Handlers.ChargifyHandler", ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
                return;
            }
            this.ThreadState = ThreadStateType.Finished;
        }

        #endregion
    }
}
