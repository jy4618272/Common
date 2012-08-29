using System;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The field of the entity.
    /// </summary>
    public class EntityFieldElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EntityFieldElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public EntityFieldElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the description of the field.
        /// </summary>
        [ConfigurationProperty("description")]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        [ConfigurationProperty("columnName")]
        public string ColumnName
        {
            get { return (string)this["columnName"]; }
            set { this["columnName"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of data stored in the field.
        /// </summary>
        [ConfigurationProperty("dataType", IsRequired = true)]
        public string DataType
        {
            get { return (string)this["dataType"]; }
            set { this["dataType"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether null values are allowed in this field.
        /// </summary>
        [ConfigurationProperty("allowDBNull")]
        public bool AllowDBNull
        {
            get { return (bool)this["allowDBNull"]; }
            set { this["allowDBNull"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the values of the field must be unique.
        /// </summary>
        [ConfigurationProperty("unique")]
        public bool Unique
        {
            get { return (bool)this["unique"]; }
            set { this["unique"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the field allows for changes.
        /// </summary>
        [ConfigurationProperty("readOnly")]
        public bool ReadOnly
        {
            get { return (bool)this["readOnly"]; }
            set { this["readOnly"] = value; }
        }

        /// <summary>
        /// Gets or sets the default value of the field.
        /// </summary>
        [ConfigurationProperty("defaultValue")]
        public string DefaultValue
        {
            get { return (string)this["defaultValue"]; }
            set { this["defaultValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the minimal value of the field.
        /// </summary>
        [ConfigurationProperty("minValue")]
        public string MinValue
        {
            get { return (string)this["minValue"]; }
            set { this["minValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximal value of the field.
        /// </summary>
        [ConfigurationProperty("maxValue")]
        public string MaxValue
        {
            get { return (string)this["maxValue"]; }
            set { this["maxValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximal length for the value of the field.
        /// </summary>
        [ConfigurationProperty("maxLength")]
        [IntegerValidator()]
        public int MaxLength
        {
            get { return (int)this["maxLength"]; }
            set { this["maxLength"] = value; }
        }

        /// <summary>
        /// Gets or sets the order number of the field.
        /// </summary>
        [ConfigurationProperty("orderNumber")]
        [IntegerValidator()]
        public int OrderNumber
        {
            get { return (int)this["orderNumber"]; }
            set { this["orderNumber"] = value; }
        }

        /// <summary>
        /// Gets or sets the number of the decimal digits for the field.
        /// </summary>
        [ConfigurationProperty("decimalDigits")]
        [IntegerValidator()]
        public int DecimalDigits
        {
            get { return (int)this["decimalDigits"]; }
            set { this["decimalDigits"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the entity that this field is related to.
        /// </summary>
        [ConfigurationProperty("entityId")]
        public Guid EntityId
        {
            get { return (Guid)this["entityId"]; }
            set { this["entityId"] = value; }
        }

        /// <summary>
        /// Gets the values of the field.
        /// </summary>
        [ConfigurationProperty("values", IsDefaultCollection = true)]
        public EntityFieldValueElementCollection Values
        {
            get { return (EntityFieldValueElementCollection)this["values"]; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Name; }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the fields of the entity.
    /// </summary>
    [ConfigurationCollection(typeof(EntityFieldElement), AddItemName = "field")]
    public class EntityFieldElementCollection : BaseConfigurationElementCollection<EntityFieldElement>
    {
    }
}
