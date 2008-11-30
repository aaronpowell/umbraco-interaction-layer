<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodeGen.ascx.cs" Inherits="Umbraco.InteractionLayer.Web.dashboard.CodeGen" %>
<h3>
    Umbraco Interaction Layer Generator</h3>
<p>
    This tool allows you to easily generate an O/R for the Umbraco DocTypes. Below is
    a list of the available DocTypes within this CMS instance.
</p>
<p>
    Select the code file type you wish to generate:
    <asp:DropDownList ID="languageType" runat="server">
        <asp:ListItem Text="C#" />
        <asp:ListItem Value="VB" />
    </asp:DropDownList>
</p>
<p>
    Namespace for the generated code:
    <asp:TextBox ID="generateNamespace" runat="server" />
    <asp:RequiredFieldValidator ID="generateNamespace_Required" runat="server" Text="Namespace is required" SetFocusOnError="true" ControlToValidate="generateNamespace" ValidationGroup="ns" />
</p>
<asp:Repeater ID="rptDocTypes" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <asp:CheckBox ID="docType" runat="server" Text='<%# Eval("Text") %>' Checked="true" />
            <asp:HiddenField ID="docTypeId" runat="server" Value='<%# Eval("Id") %>' />
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
<asp:Button ID="generate" runat="server" OnClick="GenerateClasses" Text="Generate!" CausesValidation="true" ValidationGroup="ns" />
