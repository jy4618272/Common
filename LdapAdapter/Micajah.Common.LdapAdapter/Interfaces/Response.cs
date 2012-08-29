using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public class Response<T>
	{
        private bool showResponse = false;
        private T responseValue;
        private string responseMessage;
        private Guid loginId;
        private Guid organizationId;
        private Dictionary<Guid, GroupListItemValue> groupList;
        private bool changeGroups = false;

        public T ResponseValue
        {
            get { return responseValue; }
            set { responseValue = value; }
        }
        public string ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }
        public bool ShowResponse
        {
            get { return showResponse; }
            set { showResponse = value; }
        }
        public Dictionary<Guid, GroupListItemValue> GroupList
        {
            get { return groupList; }
            set { groupList = value; }
        }
        public Guid LoginId
        {
            get { return loginId; }
            set { loginId = value; }
        }
        public Guid OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public bool ChangeGroups
        {
            get { return changeGroups; }
            set { changeGroups = value; }
        }
        public bool HasErrors
		{
			get
			{
				return string.IsNullOrEmpty(ResponseMessage);
			}
		}
	}

	public enum AuthenticationStatus
	{
		None = 0,
		Authenticated = 1,
		NotAuthenticated = 2
	}
}
