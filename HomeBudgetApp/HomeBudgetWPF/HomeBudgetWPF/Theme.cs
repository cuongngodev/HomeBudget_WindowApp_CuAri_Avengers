using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public class Theme
    {
        public enum Mode
        {
            DEFAULT = 0,
            PROTAN_DEUTERAN = 1,
            TRITAN = 2
        }

        const string LIGHT_GREEN = "#82b74b";
        const string DARK_GREEN = "#405d27";
        const string DARK_GREY = "#3e4444";
        const string GOLDEN_YELLOW = "#FFC107";
        const string LIGHT_BLUE = "#1E88E5";
        const string TURQUOISE = "#004D40";
        const string MARROON = "#D81B60";
        const string WHITE = "#fff";

        private string _elementForegroundColor;
        private string _elementBackgroundColor;
        private string _elementBorderColor;
        private string _backgroundColor;

        public Mode _currentTheme = Mode.DEFAULT;

        public Theme(int modeId = 0)
        {
            switch (modeId)
            {
                case 0:
                    _currentTheme = Mode.DEFAULT;
                    break;
                case 1:
                    _currentTheme = Mode.PROTAN_DEUTERAN;
                    break;
                case 2:
                    _currentTheme = Mode.TRITAN;
                    break;
                default:
                    _currentTheme = Mode.DEFAULT;
                    break;
            }
        }

        private void ChangeTheme()
        {
            switch (_currentTheme)
            {
                case Mode.DEFAULT:
                    _backgroundColor = DARK_GREY;
                    _elementForegroundColor = LIGHT_GREEN;
                    _elementBackgroundColor = DARK_GREEN;
                    _elementBorderColor = WHITE;
                    break;
                case Mode.PROTAN_DEUTERAN:
                    _backgroundColor = TURQUOISE;
                    _elementForegroundColor = GOLDEN_YELLOW;
                    _elementBackgroundColor = LIGHT_BLUE;
                    _elementBorderColor = WHITE;
                    break;
                case Mode.TRITAN:
                    _backgroundColor = TURQUOISE;
                    _elementForegroundColor = GOLDEN_YELLOW;
                    _elementBackgroundColor = MARROON;
                    _elementBorderColor = WHITE;
                    break;
                default:
                    _backgroundColor = DARK_GREY;
                    _elementForegroundColor = LIGHT_GREEN;
                    _elementBackgroundColor = DARK_GREEN;
                    _elementBorderColor = WHITE;
                    break;
            }
        }

        public string ElementForegroundColor
        {
            get { return _elementForegroundColor; }
        }

        public string ElementBackgroundColor
        {
            get { return _elementBackgroundColor; }
        }

        public string ElementBorderColor
        {
            get { return _elementBorderColor; }
        }

        public string BackgroundColor
        {
            get { return _backgroundColor; }
        }

        public Mode CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                _currentTheme = value;
                ChangeTheme();
            }
        }
    }
}
