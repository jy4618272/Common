using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Micajah.Common.Configuration;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class RulesEngine
    {
        #region Members

        private Guid m_Id;
        private string m_Name;
        private string m_DisplayName;
        private Dictionary<string, Guid> m_InputParameters;
        private EntityCollection m_InputEntities;
        private string m_OutputEditPage;
        private string m_OutputClass;
        private RuleCollection m_Rules;

        #endregion

        #region Properties

        public Guid Id
        {
            get { return m_Id; }
            private set { m_Id = value; }
        }

        public string Name
        {
            get { return m_Name; }
            private set { m_Name = value; }
        }

        public string DisplayName
        {
            get { return m_DisplayName; }
            private set { m_DisplayName = value; }
        }

        public Dictionary<string, Guid> InputParameters
        {
            get { return m_InputParameters; }
        }

        public string OutputEditPage
        {
            get { return m_OutputEditPage; }
            private set { m_OutputEditPage = value; }
        }

        public string OutputClass
        {
            get { return m_OutputClass; }
            private set { m_OutputClass = value; }
        }

        public RuleCollection Rules
        {
            get
            {
                if (m_Rules == null) m_Rules = RuleCollection.GetRules(m_Id);
                return m_Rules;
            }
        }

        public EntityCollection InputEntities
        {
            get { return m_InputEntities; }
        }

        #endregion

        #region Constructors

        private RulesEngine()
        {
            m_InputParameters = new Dictionary<string, Guid>();
            m_InputEntities = new EntityCollection();
        }

        #endregion

        #region Internal Methods

        internal static RulesEngine Create(RulesEngineElement value)
        {
            RulesEngine rulesEngine = new RulesEngine();
            rulesEngine.Id = value.Id;
            rulesEngine.Name = value.Name;
            rulesEngine.DisplayName = value.DisplayName;
            rulesEngine.OutputEditPage = value.Output.EditPageUrl;
            rulesEngine.OutputClass = value.Output.ClassFullName;

            foreach (RulesEngineInputElement input in value.Inputs)
            {
                rulesEngine.InputParameters.Add(input.Name, input.EntityId);
            }

            return rulesEngine;
        }

        #endregion

        #region Public Methods

        public void AddInputEntity(Entity entity)
        {
            m_InputEntities.Add(entity);
        }

        public void AddInputEntity(EntityCollection entities)
        {
            if (entities != null)
            {
                foreach (Entity entity in entities)
                {
                    m_InputEntities.Add(entity);
                }
            }
        }

        public void ClearEntities()
        {
            m_InputEntities.Clear();
        }

        /// <summary>
        /// Execute the Rule Engine
        /// </summary>
        /// <returns>Returns the RuleId where Rule Engine has been resolved. 
        /// Otherwise returns Guid.Empty</returns>
        public Guid Execute()
        {
            Guid result = Guid.Empty;
            foreach (Rule rule in this.Rules)
            {
                rule.ClearEntities();
                rule.AddInputEntity(m_InputEntities);
                if (rule.Resolve())
                {
                    return rule.RuleId;
                }
            }
            return result;
        }

        #endregion
    }

    [Serializable]
    public class RulesEngineCollection : Collection<RulesEngine>
    {
        #region Private Properties

        private List<RulesEngine> ItemList
        {
            get { return base.Items as List<RulesEngine>; }
        }

        #endregion

        #region Public Properties

        public RulesEngine this[string name]
        {
            get
            {
                int index = this.FindIndexByIdOrName(name);
                return (((index < 0) || (index >= this.Count)) ? null : base[index]);
            }
            set
            {
                int index = this.FindIndexByIdOrName(name);
                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }

        #endregion

        #region Private Methods

        private int FindIndexByIdOrName(string value)
        {
            int index = this.ItemList.FindIndex(
                delegate(RulesEngine rulesEngine)
                {
                    return (string.Compare(rulesEngine.Name, value, StringComparison.Ordinal) == 0);
                });

            if (index == -1)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    Guid id = (Guid)obj;

                    index = this.ItemList.FindIndex(
                        delegate(RulesEngine rulesEngine)
                        {
                            return (rulesEngine.Id == id);
                        });
                }
            }

            return index;
        }

        #endregion

        #region Internal Methods

        internal static RulesEngineCollection Load()
        {
            RulesEngineCollection rulesEngines = new RulesEngineCollection();

            foreach (RulesEngineElement element in FrameworkConfiguration.Current.RulesEngines)
            {
                RulesEngine ruleEngine = RulesEngine.Create(element);
                if (ruleEngine != null) rulesEngines.Add(ruleEngine);
            }

            return rulesEngines;
        }

        #endregion
    }
}
