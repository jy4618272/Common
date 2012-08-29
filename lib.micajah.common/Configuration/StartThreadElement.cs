using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// Represents the definition for the thread that is started when the web application starts.
    /// </summary>
    public class StartThreadElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StartThreadElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public StartThreadElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the full name of the custom class.
        /// </summary>
        [ConfigurationProperty("classFullName")]
        public string ClassFullName
        {
            get { return (string)this["classFullName"]; }
            set { this["classFullName"] = value; }
        }

        /// <summary>
        /// Gets or sets the start time of sheduling.
        /// </summary>
        [ConfigurationProperty("startTime")]
        public string StartTime
        {
            get { return (string)this["startTime"]; }
            set { this["startTime"] = value; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.ClassFullName; }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the start threads.
    /// </summary>
    [ConfigurationCollection(typeof(StartThreadElement), AddItemName = "startThread")]
    public class StartThreadElementCollection : BaseConfigurationElementCollection<StartThreadElement>
    {
    }
}
