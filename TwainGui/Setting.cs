
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Configuration;
    using System.Drawing;

    namespace TwainGui.baseLayer
    {
        //Application settings wrapper class

        sealed class TwainGuiSettings : ApplicationSettingsBase
        {
            [UserScopedSettingAttribute()]
            public String FormText
            {
                get { return (String)this["FormText"]; }
                set { this["FormText"] = value; }
            }

            [UserScopedSetting()]
            public String ServerName
            {
                get { return (String)this["ServerName"]; }
                set { this["ServerName"] = value; }
            }

            [UserScopedSetting()]
            public String LoginName
            {
                get { return (String)this["LoginName"]; }
                set { this["LoginName"] = value; }
            }

            [UserScopedSetting()]
            public String Password
            {
                get { return (String)this["Password"]; }
                set { this["Password"] = value; }
            }


            [UserScopedSetting()]
            [DefaultSettingValueAttribute("0, 0")]
            public Point FormLocation
            {
                get { return (Point)(this["FormLocation"]); }
                set { this["FormLocation"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValueAttribute("800, 600")]
            public Size FormSize
            {
                get { return (Size)this["FormSize"]; }
                set { this["FormSize"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValueAttribute("LightGray")]
            public Color FormBackColor
            {
                get { return (Color)this["FormBackColor"]; }
                set { this["FormBackColor"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValueAttribute("300")]
            public int Splitter1
            {
                get { return (int)this["Splitter1"]; }
                set { this["Splitter1"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValueAttribute("300")]
            public int Splitter2
            {
                get { return (int)this["Splitter2"]; }
                set { this["Splitter2"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValueAttribute("300")]
            public int Splitter3
            {
                get { return (int)this["Splitter3"]; }
                set { this["Splitter3"] = value; }
            }
        }

        sealed class RegisrationSettings : ApplicationSettingsBase
        {
            [UserScopedSetting()]
            public String ScanDirectory
            {
                get { return (String)this["ScanDirectory"]; }
                set { this["ScanDirectory"] = value; }
            }

            [UserScopedSetting()]
            public String PDFDirectory
            {
                get { return (String)this["PDFDirectory"]; }
                set { this["PDFDirectory"] = value; }
            }

            [UserScopedSetting()]
            public String DocumentDirectory
            {
                get { return (String)this["DocumentDirectory"]; }
                set { this["DocumentDirectory"] = value; }
            }

        }

    }

