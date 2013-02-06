﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Micajah.Common.Bll.Providers;
using ChargifyNET;
using System.Diagnostics;
using System.Globalization;

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
                ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
                if (_chargify == null) throw new Exception("No Chargify configuration settings found.");

                int updatedCount = 0;
                OrganizationCollection _orgs = OrganizationProvider.GetOrganizations(false, false);

                foreach (Organization _org in _orgs)
                {
                    InstanceCollection _insts = InstanceProvider.GetInstances(_org.OrganizationId, false);
                    foreach (Instance _inst in _insts)
                    {
                        ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(_chargify, _org.OrganizationId, _inst.InstanceId);
                        ChargifyProvider.UpdateSubscriptionAllocations(_chargify, _custSubscr != null ? _custSubscr.SubscriptionID : 0, _org.OrganizationId, _inst.InstanceId);
                        if (_custSubscr!=null) updatedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ThreadState = ThreadStateType.Failed;
                this.ErrorException = ex;
                try
                {
                    if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.ChargifyHandler"))
                        EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.ChargifyHandler", "Application");
                    EventLog.WriteEntry("Micajah.Common.Bll.Handlers.ChargifyHandler", ex.ToString(), EventLogEntryType.Error);
                }
                catch
                {
                    throw new Exception(ex.ToString());
                }
            }
            this.ThreadState = ThreadStateType.Finished;
        }

        #endregion
    }
}
