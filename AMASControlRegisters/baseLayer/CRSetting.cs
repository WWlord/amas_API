using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;

namespace AMASControlRegisters.baseLayer
{
    sealed class DocViewSettings : ApplicationSettingsBase
    {
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
}

