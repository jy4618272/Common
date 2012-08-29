using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// Represents a configuration element containing a collection of child elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    [Serializable]
    public class BaseConfigurationElementCollection<T> : ConfigurationElementCollection, ICollection<T>, IEnumerable<T>
        where T : ConfigurationElement, IConfigurationElement, new()
    {
        #region Members

        [Serializable]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            #region Members

            private BaseConfigurationElementCollection<T> m_List;
            private int m_Index;
            private T m_Current;

            #endregion

            #region Constructors

            internal Enumerator(BaseConfigurationElementCollection<T> list)
            {
                m_List = list;
                m_Index = 0;
                m_Current = default(T);
            }

            #endregion

            #region Public Properties

            public T Current
            {
                get { return m_Current; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            #endregion

            #region Private Methods

            private bool MoveNextRare()
            {
                m_Index = this.m_List.Count + 1;
                m_Current = default(T);
                return false;
            }

            #endregion

            #region Public Methods

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                BaseConfigurationElementCollection<T> list = m_List;
                if (this.m_Index < list.Count)
                {
                    m_Current = list.BaseGet(this.m_Index) as T;
                    m_Index++;
                    return true;
                }
                return this.MoveNextRare();
            }

            void IEnumerator.Reset()
            {
                m_Index = 0;
                m_Current = default(T);
            }

            #endregion
        }

        #endregion

        #region Overriden Properties

        /// <summary>
        /// Gets a value indicating whether the collection object is read only.
        /// </summary>
        public new bool IsReadOnly
        {
            get { return base.IsReadOnly(); }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates a new configuration element.
        /// </summary>
        /// <returns>A new configuration element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        /// <param name="element">The configuration element to return the key for.</param>
        /// <returns>An System.Object that acts as the key for the specified configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IConfigurationElement)element).Key;
        }

        protected override void SetReadOnly()
        {
        }

        /// <summary>
        /// Gets an enumerator which is used to iterate through the collection.
        /// </summary>
        /// <returns>An enumerator which is used to iterate through the collection.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a configuration element to the collection.
        /// </summary>
        /// <param name="item">The configuration element to add.</param>
        public virtual void Add(T item)
        {
            BaseAdd(item);
        }

        /// <summary>
        /// Adds the configuration elements to the collection.
        /// </summary>
        /// <param name="items">The array of the configuration elements to add.</param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null) return;

            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Removes all configuration element objects from the collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Determines whether the collection contains a specified configuration element.
        /// </summary>
        /// <param name="item">The configuration element to locate in the collection.</param>
        /// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return (BaseIndexOf(item) > -1);
        }

        /// <summary>
        ///  Copies the contents of the collection to an array.
        /// </summary>
        /// <param name="array">Array to which to copy the contents of the collection.</param>
        /// <param name="arrayIndex">Index location at which to begin copying.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes a configuration element from the collection.
        /// </summary>
        /// <param name="item">The configuration element to remove.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            BaseRemove(item.Key);
            return true;
        }

        #endregion
    }
}
