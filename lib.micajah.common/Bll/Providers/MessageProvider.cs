using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
using Micajah.Common.Security;
using System;
using System.ComponentModel;

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
            Guid messageId = Guid.NewGuid();
            using (MessageTableAdapter adapter = new MessageTableAdapter(OrganizationProvider.GetConnectionString(UserContext.Current.SelectedOrganizationId)))
            {
                adapter.Insert(messageId, parentMessageId, localObjectType, localObjectId, fromUserId, toUserId, subject, text, DateTime.UtcNow);
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
        public static ClientDataSet.MessageDataTable GetMessages(string localObjectType, string localObjectId)
        {
            using (MessageTableAdapter adapter = new MessageTableAdapter(OrganizationProvider.GetConnectionString(UserContext.Current.SelectedOrganizationId)))
            {
                return adapter.GetMessages(localObjectType, localObjectId);
            }
        }

        #endregion
    }
}
