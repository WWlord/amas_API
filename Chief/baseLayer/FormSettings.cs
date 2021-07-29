using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;

namespace Chief.baseLayer

{
//Application settings wrapper class

    sealed class ChefSettings : ApplicationSettingsBase
    {
        [UserScopedSettingAttribute()]
        public String FormText
        {
            get
            {
                try
                {
                    return (String)this["FormText"];
                }
                catch { return ""; }
            }
            set { this["FormText"] = value; }
        }

        [UserScopedSetting()]
        public String ServerName
        {
            get
            {
                try
                {
                    return (String)this["ServerName"];
                }
            catch {return "";}
            }
            set
            {
                try
                {
                    this["ServerName"] = value;
                }
                catch { }
            }
        }

        [UserScopedSetting()]
        public String LoginName
        {
            get
            {
                try
                {
                    return (String)this["LoginName"];
                }
                catch { return ""; }
            }

            set
            {
                try
                {
                    this["LoginName"] = value;
                }
                catch { }
            }
        }

        [UserScopedSetting()]
        public String Password
        {
            get
            {
                try
                {
                    return (String)this["Password"];
                }
                catch { return ""; }
            }

            set
            {
                try
                { this["Password"] = value; }
                catch { }
            }
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
            get
            {
                try
                {
                    return (String)this["ScanDirectory"];
                }
                catch { return ""; }
            }
            set { this["ScanDirectory"] = value; }
        }

        [UserScopedSetting()]
        public String PDFDirectory
        {
            get
            {
                try
                {
                    return (String)this["PDFDirectory"];
                }
                catch { return ""; }
            }
            set { this["PDFDirectory"] = value; }
        }

        [UserScopedSetting()]
        public String DocumentDirectory
        {
            get
            {
                try
                {
                    return (String)this["DocumentDirectory"];
                }
                catch { return ""; }
            }
            set { this["DocumentDirectory"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("5")]
        public int PDFPrintWait
        {
            get { return (int)this["PDFPrintWait"]; }
            set { this["PDFPrintWait"] = value; }
        }

    }

}
