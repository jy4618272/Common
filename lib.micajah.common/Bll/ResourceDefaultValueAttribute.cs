using System;
using System.ComponentModel;
using Micajah.Common.Properties;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// Provides the functionality to get the default value for a property from resources.
    /// </summary>
    internal sealed class ResourceDefaultValueAttribute : DefaultValueAttribute
    {
        #region Members

        private bool m_Localized;
        private Type m_Type;

        #endregion

        #region Constructors

        internal ResourceDefaultValueAttribute(string value) : base(value) { }

        internal ResourceDefaultValueAttribute(Type type, string value)
            : base(value)
        {
            m_Type = type;
        }

        #endregion

        #region Overriden Properties

        public override object TypeId
        {
            get { return typeof(DefaultValueAttribute); }
        }

        public override object Value
        {
            get
            {
                if (!m_Localized)
                {
                    m_Localized = true;
                    string str = (string)base.Value;
                    if (!string.IsNullOrEmpty(str))
                    {
                        object obj2 = Resources.ResourceManager.GetString(str);
                        if (m_Type != null)
                        {
                            try
                            {
                                obj2 = TypeDescriptor.GetConverter(m_Type).ConvertFromInvariantString((string)obj2);
                            }
                            catch (NotSupportedException)
                            {
                                obj2 = null;
                            }
                        }
                        base.SetValue(obj2);
                    }
                }
                return base.Value;
            }
        }

        #endregion
    }
}
