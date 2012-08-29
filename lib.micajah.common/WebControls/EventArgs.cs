using System;
using System.Collections.Generic;
using System.Text;
using Micajah.Common.WebControls;

namespace Micajah.Common.WebControls
{
    internal class PostRenderEventArgs : EventArgs
    {
        #region Members

        private string m_Content;

        #endregion

        #region Public Properties

        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        #endregion

        #region Constructors

        public PostRenderEventArgs(string content)
        {
            m_Content = content;
        }

        #endregion
    }

    public class MagicFormActionEventArgs : EventArgs
    {
        #region Members

        private CommandActions m_Action;

        #endregion

        #region Public Properties

        public MagicFormActionEventArgs(CommandActions action)
        {
            m_Action = action;
        }

        #endregion

        #region Constructors

        public CommandActions Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        #endregion
    }

    public class CommonGridViewActionEventArgs : EventArgs
    {
        #region Members

        private CommandActions m_Action;
        private int m_RowIndex;

        #endregion

        #region Public Methods

        public CommandActions Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        public int RowIndex
        {
            get { return m_RowIndex; }
            set { m_RowIndex = value; }
        }

        #endregion

        #region Constructors

        public CommonGridViewActionEventArgs(CommandActions action, int rowIndex)
        {
            m_Action = action;
            m_RowIndex = rowIndex;
        }

        #endregion

    }
}
