<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="CashLoanShop.Menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div>
        <table id="data" runat="server">
            <tr>
                <td style="font-family:Arial;font-weight:bold;color:black;">
                    Important Message
                </td>
            </tr>
             <tr>
                <td style="font-family:Arial;font-weight:bold;color:black;">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
