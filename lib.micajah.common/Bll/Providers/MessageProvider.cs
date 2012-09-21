using System;
using System.ComponentModel;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with messages.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class MessageProvider
    {
        #region Public Methods

        /// <summary>
        /// Creates new message with specified details.
        /// </summary>
        /// <param name="parentMessageId">The unique identifier of the parent message.</param>
        /// <param name="localObjectType">The type of the object which the message is associated with.</param>
        /// <param name="localObjectId">The unique identifier of the object which the message is associated with.</param>
        /// <param name="fromUserId">The unique identifier of the user that the message is posted by.</param>
        /// <param name="toUserId">The unique identifier of the user that the message is posted for.</param>
        /// <param name="subject">The subject of the message.</param>
        /// <param name="text">The text of the message.</param>
        /// <returns>The unique identifier of the message.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertMessage(Guid? parentMessageId, string localObjectType, string localObjectId, Guid fromUserId, Guid? toUserId, string subject, string text)
        {
            Guid messageId = Guid.Empty;
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganization.OrganizationId);
            if (adapters != null)
            {
                messageId = Guid.NewGuid();
                adapters.MessageTableAdapter.Insert(messageId, parentMessageId, localObjectType, localObjectId, fromUserId, toUserId, subject, text, DateTime.UtcNow);
            }
            return messageId;
        }

        /// <summary>
        /// Returns the messages for the specified object.
        /// </summary>
        /// <param name="localObjectType">The type of the object which the message are associated with.</param>
        /// <param name="localObjectId">The unique identifier of the object which the message are associated with.</param>
        /// <returns>The messages for the specified object.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.MessageDataTable GetMessages(string localObjectType, string localObjectId)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganization.OrganizationId);
            OrganizationDataSet.MessageDataTable table = null;
            try
            {
                table = new OrganizationDataSet.MessageDataTable();
                if (adapters != null) adapters.MessageTableAdapter.Fill(table, 0, localObjectType, localObjectId);
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        #endregion
    }
}
