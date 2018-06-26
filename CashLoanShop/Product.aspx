<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="CashLoanShop.Product" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Made My Money</title>
    <style>
        .center {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 80%;
        }
    </style>
</head>
<body style="background-color: #252525;">
    <form id="form1" runat="server">
        <div>
            <table class="center">
                <tr>
                    <td>
                        <div style="float: right; width: 100%; text-align: right;">
                            <table style="width: 90%;">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="imgRequestCall" runat="server" ImageUrl="Request%20a%20Call.png" Height="65" Width="200" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:ImageButton ID="imgStores" runat="server" ImageUrl="Nearest%20Store.png" Height="65" Width="200" />
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="Working.jpg" class="center" />
                    </td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
