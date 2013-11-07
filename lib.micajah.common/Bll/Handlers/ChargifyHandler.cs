using System;
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
                UpdateChargifyAllocations();
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

        public static int UpdateChargifyAllocations()
        {
            if (!Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.Integration.Chargify.Enabled) throw new InvalidOperationException("Chargify integration is not enabled in the application configuration file.");

            ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
            if (_chargify == null) throw new InvalidOperationException("No Chargify configuration settings found.");

            int updatedCount = 0;
            OrganizationCollection _orgs = OrganizationProvider.GetOrganizations(false, false);

            foreach (Organization _org in _orgs)
            {
                InstanceCollection _insts = InstanceProvider.GetInstances(_org.OrganizationId, false);
                foreach (Instance _inst in _insts)
                {
                    if (_inst.BillingPlan == BillingPlan.Custom) continue;
                    ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(_chargify, _org.OrganizationId, _inst.InstanceId);
                    ChargifyProvider.UpdateSubscriptionAllocations(_chargify, _custSubscr != null ? _custSubscr.SubscriptionID : 0, _inst);
                    if (_custSubscr != null) updatedCount++;
                    if (_custSubscr == null && _inst.CreditCardStatus != CreditCardStatus.NotRegistered) InstanceProvider.UpdateInstance(_inst, CreditCardStatus.NotRegistered);
                    else if (_custSubscr != null && _custSubscr.State == SubscriptionState.Expired && _inst.CreditCardStatus != CreditCardStatus.Expired) InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Expired);
                    else if (_custSubscr != null && _custSubscr.State == SubscriptionState.Active && _inst.CreditCardStatus != CreditCardStatus.Registered) InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Registered);
                    else if (_custSubscr != null && _custSubscr.State != SubscriptionState.Active && _custSubscr.State != SubscriptionState.Expired && _inst.CreditCardStatus != CreditCardStatus.Declined) InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Declined);
                }
            }

            return updatedCount;
        }

        #endregion
    }
}
