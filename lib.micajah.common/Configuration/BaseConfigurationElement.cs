using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using Micajah.Common.Bll;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// Represents a configuration element within a configuration file.
    /// </summary>
    public abstract class BaseConfigurationElement : ConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected BaseConfigurationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        protected BaseConfigurationElement(IXPathNavigable node)
            : base()
        {
            Initialize(node as XmlNode, this);
        }

        #endregion

        #region Private Methods

        private static string GetPropertyName(PropertyInfo p)
        {
            string pname = null;

            object[] cpas = p.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), false);
            if (cpas.Length > 0)
            {
                ConfigurationPropertyAttribute cpa = cpas[0] as ConfigurationPropertyAttribute;
                if (cpa != null)
                    pname = cpa.Name;
            }

            return pname;
        }

        private static object GetSingleValue(string pname, PropertyInfo p, XmlNode node)
        {
            object value = null;
            XmlAttribute attr = node.Attributes[pname];
            if (attr != null)
            {
                TypeConverter converter = null;
                object[] tcas = p.GetCustomAttributes(typeof(TypeConverterAttribute), false);
                if (tcas.Length > 0)
                {
                    TypeConverterAttribute tca = tcas[0] as TypeConverterAttribute;
                    if (tca != null)
                        converter = Activator.CreateInstance(Type.GetType(tca.ConverterTypeName)) as TypeConverter;
                }
                value = Support.ConvertStringToType(attr.Value, p.PropertyType, converter);
            }
            return value;
        }

        private static object GetCollection(string pname, PropertyInfo p, XmlNode node)
        {
            object value = null;
            XmlNode collNode = node.SelectSingleNode(pname);
            if (collNode != null)
            {
                object[] ccas = p.PropertyType.GetCustomAttributes(typeof(ConfigurationCollectionAttribute), false);
                if (ccas.Length > 0)
                {
                    ConfigurationCollectionAttribute cca = ccas[0] as ConfigurationCollectionAttribute;
                    if (cca != null)
                    {
                        object coll = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName);
                        if (coll != null)
                        {
                            foreach (XmlNode childNode in collNode.SelectNodes(cca.AddItemName))
                            {
                                object collItem = p.PropertyType.Assembly.CreateInstance(cca.ItemType.FullName, false, BindingFlags.Public | BindingFlags.Instance, null
                                    , new object[] { childNode }, null, null);
                                if (collItem != null)
                                    p.PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, coll, new object[] { collItem }, CultureInfo.CurrentCulture);
                            }
                            value = coll;
                        }
                    }
                }
            }
            return value;
        }

        private static void SetObjectPropertyValue(object obj, Type objType, PropertyInfo p, string pname, object value)
        {
            if (value != null)
            {
                if (p.CanWrite)
                    p.SetValue(obj, value, null);
                else
                {
                    foreach (PropertyInfo p1 in objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance))
                    {
                        if (string.Compare(p1.Name, "Item", StringComparison.Ordinal) == 0)
                        {
                            ParameterInfo[] paramInfos = p1.GetIndexParameters();
                            if (paramInfos.Length > 0)
                            {
                                if (paramInfos[0].ParameterType == typeof(string))
                                {
                                    p1.SetValue(obj, value, new object[] { pname });
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        internal static void Initialize(XmlNode node, object obj)
        {
            if ((node != null) && (obj != null))
            {
                Type objType = obj.GetType();
                foreach (PropertyInfo p in objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    bool processed = false;
                    object value = null;

                    string pname = GetPropertyName(p);
                    Type propBaseType = p.PropertyType.BaseType;

                    if (propBaseType != null)
                    {
                        if (propBaseType == typeof(BaseConfigurationElement))
                        {
                            processed = true;
                            value = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName, false, BindingFlags.Public | BindingFlags.Instance, null
                                , new object[] { node.SelectSingleNode(pname) }, null, null);
                        }
                        else if (propBaseType.BaseType != null)
                        {
                            if ((propBaseType.BaseType == typeof(ConfigurationElementCollection)) && propBaseType.IsGenericType)
                            {
                                processed = true;
                                value = GetCollection(pname, p, node);
                            }
                        }
                    }

                    if (!processed)
                        value = GetSingleValue(pname, p, node);

                    SetObjectPropertyValue(obj, objType, p, pname, value);
                }
            }
        }

        #endregion
    }
}
