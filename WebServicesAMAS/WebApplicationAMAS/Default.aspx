﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplicationAMAS._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Безымянная страница</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Image ID="Image1" runat="server" Height="254px" />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" 
            NavigateUrl="~/WebFormAccount.aspx">Зарегистрироваться</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/WebFormLogin.aspx">Войти 
        в систему</asp:HyperLink>
    
    </div>
    </form>
</body>
</html>
