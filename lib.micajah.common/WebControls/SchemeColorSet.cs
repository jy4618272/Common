using System.Drawing;

namespace Micajah.Common.WebControls
{
    public class SchemeColorSet
    {
        #region Members

        private string m_dark;
        private string m_light;
        private string m_highlight;
        private ColorScheme m_scheme;

        #endregion

        #region Constructors

        public SchemeColorSet() : this(ColorScheme.White) { }

        public SchemeColorSet(ColorScheme colorScheme)
        {
            this.Scheme = colorScheme;
        }

        #endregion

        #region Public Properties

        public string DarkHtml
        {
            get { return this.m_dark; }
        }

        public Color Dark
        {
            get { return ColorTranslator.FromHtml(this.m_dark); }
        }

        public string LightHtml
        {
            get { return this.m_light; }
        }

        public Color Light
        {
            get { return ColorTranslator.FromHtml(this.m_light); }
        }

        public string HighlightHtml
        {
            get { return m_highlight; }
        }

        public Color Highlight
        {
            get { return ColorTranslator.FromHtml(m_highlight); }
        }

        public ColorScheme Scheme
        {
            get { return this.m_scheme; }
            set
            {
                m_scheme = value;
                switch (m_scheme)
                {
                    case ColorScheme.Blue:
                        m_dark = "#66669A";
                        m_light = "#F1F0FF";
                        m_highlight = "#CCCCFF";
                        break;
                    case ColorScheme.Brown:
                        m_dark = "#9E755F";
                        m_light = "#FAFACC";
                        m_highlight = "#F8E5A8";
                        break;
                    case ColorScheme.TanGray:
                        m_dark = "#666666";
                        m_light = "#FAFACC";
                        m_highlight = "#F8E5A8";
                        break;
                    case ColorScheme.Gray:
                        m_dark = "#666666";
                        m_light = "#F0F0F0";
                        m_highlight = "#B2B4BF";
                        break;
                    case ColorScheme.Green:
                        m_dark = "#3F8640";
                        m_light = "#E4F7E4";
                        m_highlight = "#B5E9B5";
                        break;
                    case ColorScheme.Red:
                        m_dark = "#DF572D";
                        m_light = "#FFF0E1";
                        m_highlight = "#FDDAB7";
                        break;
                    case ColorScheme.Silver:
                        m_dark = "#5A8282";
                        m_light = "#F0F0F0";
                        m_highlight = "#B2B4BF";
                        break;
                    case ColorScheme.White:
                        m_dark = "#333333";
                        m_light = "#FFFFFF";
                        m_highlight = "#F0F0F0";
                        break;
                    case ColorScheme.Yellow:
                        m_dark = "#9E755F";
                        m_light = "#FFF7BE";
                        m_highlight = "#FBEE89";
                        break;
                }
            }
        }

        #endregion
    }
}
