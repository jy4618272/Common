using System;

namespace Micajah.Common.Configuration
{
    public interface IConfigurationElement
    {
        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        object Key { get; }
    }
}
