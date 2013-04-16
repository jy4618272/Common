using System.Configuration;
using System.Xml;

namespace Micajah.Common.Configuration
{
    public class TextConfigurationElement<T> : ConfigurationElement
    {
        private T m_Value;

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            m_Value = (T)reader.ReadElementContentAs(typeof(T), null);
        }

        public T Value
        {
            get { return m_Value; }
        }
    }
}
