using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;

namespace ClassDocuments.baseLayer
{
    sealed class MetadataSetting : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValueAttribute("50")]
        public int Executor
        {
            get { return (int)this["Executor"]; }
            set { this["Executor"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("50")]
        public int SendDate
        {
            get { return (int)this["SendDate"]; }
            set { this["SendDate"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("50")]
        public int ToDate
        {
            get { return (int)this["ToDate"]; }
            set { this["ToDate"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("50")]
        public int ExeDate
        {
            get { return (int)this["ExeDate"]; }
            set { this["ExeDate"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("50")]
        public int Note
        {
            get { return (int)this["Note"]; }
            set { this["Note"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValueAttribute("50")]
        public int ExecBy
        {
            get { return (int)this["ExecBy"]; }
            set { this["ExecBy"] = value; }
        }
    }
}
