using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
    public class GroupListItemValue
    {
        public string Name { get; set; }
        public bool IsDirect { get; set; }
        public GroupListItemValue(string name, bool isDirect) { this.Name = name; this.IsDirect = isDirect; }
    }
}
