using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;



namespace AMASControlRegisters.Design
{
    [ToolboxItemFilter("AMASControlRegisters.MarqueeBorder", ToolboxItemFilterType.Require)]
    [ToolboxItemFilter("AMASControlRegisters.MarqueeText", ToolboxItemFilterType.Require)]
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")] 

    class RegRootDesigner
    {
    }
}
