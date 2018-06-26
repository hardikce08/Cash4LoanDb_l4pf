﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="CashLoanShop.ViewReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .headerrow {
            font-family: Verdana;
            font-size: 23px;
        }

        #content_rbtnlstCollectionOverdue label {
            padding-left: 5px;
        }

        #content_rbtnCollectionStatus label {
            padding-left: 5px;
        }
        #content_rbtnDeptStatus label {
            padding-left: 5px;
        }

        td #contractbtn {
            display: block !important;
        }

        td a#contractbtnnew {
            display: block !important;
            font-size: 12px !important;
            padding: 3px 0px;
            color: white;
            background-color: black;
            border: 1px solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <asp:MultiView ID="mvView" runat="server" ActiveViewIndex="0">
        <asp:View ID="vopenloan" runat="server">
            <div class="page-content">

                <!-- /.page-header -->
                <div class="col-xs-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Loan Report Details</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-md-5 control-label">Report Type :</label>
                                    <div class="controls col-md-7">
                                        <asp:DropDownList ID="ddlReportType" class="form-control" runat="server">
                                            <asp:ListItem Value="0">--Select Type--</asp:ListItem>
                                            <asp:ListItem Value="Open">Loan Open</asp:ListItem>
                                            <asp:ListItem Value="Due">Loan Due</asp:ListItem>
                                            <asp:ListItem Value="Payment in Process">Payment in Process</asp:ListItem>
                                            <asp:ListItem Value="Over Due">Overdue Loan</asp:ListItem>
                                            <asp:ListItem Value="Sent for Collection">Loan Sent for Collection</asp:ListItem>
                                            <asp:ListItem Value="DEPT Management">DEPT Management</asp:ListItem>
                                            <asp:ListItem Value="Consumer Proposal">Consumer Proposal</asp:ListItem>
                                            <asp:ListItem Value="Bankrupt">Bankrupt</asp:ListItem>
                                            <asp:ListItem Value="Legal">Legal</asp:ListItem>
                                            <asp:ListItem Value="Close">Closed Loan</asp:ListItem>
                                            <asp:ListItem Value="Denied">Denied Loan</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-5 control-label">From Date :</label>
                                    <div class="controls col-md-7">
                                        <asp:TextBox ID="txtFromDate" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-5 control-label">To Date :</label>
                                    <div class="controls col-md-7">
                                        <asp:TextBox ID="txtToDate" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-5 control-label"></label>
                                    <div class="controls col-md-7">
                                        <asp:Button ID="btnSubmit" class="btn btn-primary" Text="Search" runat="server" OnClick="btnSubmit_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>




                <table style="width: 100%; display: none;" id="loanopengrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Open
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoan" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoan_PageIndexChanging" OnRowDataBound="dgvLoan_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="LoanAmountApproved" HeaderStyle-HorizontalAlign="Left" HeaderText="Loan Amount Approved" />--%>
                                    <asp:TemplateField HeaderText="Loan Amount Approved" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApproved") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalApproved" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PaymentOption" HeaderStyle-HorizontalAlign="Left" HeaderText="Payment Option" />
                                    <asp:TemplateField HeaderText="Mode of Payment" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <input type="text" class="input" style="background-color: white !important; border: 1px solid black;" id='<%# (string.Format("txtmodeofpaymentcancel_{0}", Eval("Id"))) %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" style="margin-left: 2px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"
                                                onclick="javascript:return opencancelreceipt('<%# Eval("Id") %>');" id="contractbtn">Cancel Loan</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>

                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loanoverduegrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Over Due
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanOverDue" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanOverDue_PageIndexChanging" OnRowDataBound="dgvLoanOverDue_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Loan Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>--%>
                                            <asp:Label ID="lblLateDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payments Received" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PartialPayment") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalPartialAmountReceived" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# (Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) )) - (Eval("PartialPayment")=="" ? 0:Convert.ToDecimal(Eval("PartialPayment").ToString().Replace("$",""))) %>--%>
                                            <asp:Label ID="lblTotalRemainingDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalBalanceDue" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Num Times Loan Over Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverdueCount")==null ?"0":Eval("OverdueCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan OverDue Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverDueReason") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loanclosegrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Closed
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanClose" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanClose_PageIndexChanging" OnRowDataBound="dgvLoanClose_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Amount Approved" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApproved") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalApprovedAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Loan Amount Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Closed Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "UpdatedDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No. of Loan Over Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverdueCount")==null ?"0":Eval("OverdueCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="           " HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);"
                                                onclick="javascript:return openclosereceipt('<%# Eval("Id") %>');" id="contractbtnnew">Print Receipt</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loandeniedgrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Denied
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanDenied" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanDenied_PageIndexChanging" OnRowDataBound="dgvLoanDenied_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <input type="button" value="View History" class="historybutton" id='<%# Eval("CustomerId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Amount Applied" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApplied") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>




                                    <asp:TemplateField HeaderText="Next Pay Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason To Deny Loan" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDenyReason" runat="server"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>



                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loaninprocessgrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Payment in Process
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanInProcess" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanInProcess_PageIndexChanging" OnRowDataBound="dgvLoanInProcess_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Amount Approved" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApproved") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalAmountApproved" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Option" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PaymentOption") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>



                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loansentforcollectiongrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Sent for Collection
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanSentforCollection" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanSentforCollection_PageIndexChanging" OnRowDataBound="dgvLoanSentforCollection_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="LoanAmountApproved" HeaderStyle-HorizontalAlign="Left" HeaderText="Loan Amount Approved" />--%>
                                    <asp:TemplateField HeaderText="Loan Amount Approved" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApproved") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalApproved" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Amount Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalCollectionDueAmount" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>--%>
                                            <asp:Label ID="lblCollectionDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payments Received" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PartialPayment") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalPartialAmountReceived" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# (Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) )) - (Eval("PartialPayment")=="" ? 0:Convert.ToDecimal(Eval("PartialPayment").ToString().Replace("$",""))) %>--%>
                                            <asp:Label ID="lblTotalRemainingDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalBalanceDue" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PaymentOption" HeaderStyle-HorizontalAlign="Left" HeaderText="Payment Option" />

                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>

                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loanLegalgrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan sent to Legal
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLegal" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanSentforCollection_PageIndexChanging" OnRowDataBound="dgvLoanSentforCollection_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="LoanAmountApproved" HeaderStyle-HorizontalAlign="Left" HeaderText="Loan Amount Approved" />--%>
                                    <asp:TemplateField HeaderText="Loan Amount Approved" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApproved") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalApproved" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Amount Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalCollectionDueAmount" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>--%>
                                            <asp:Label ID="lblCollectionDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payments Received" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PartialPayment") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalPartialAmountReceived" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# (Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) )) - (Eval("PartialPayment")=="" ? 0:Convert.ToDecimal(Eval("PartialPayment").ToString().Replace("$",""))) %>--%>
                                            <asp:Label ID="lblTotalRemainingDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalBalanceDue" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PaymentOption" HeaderStyle-HorizontalAlign="Left" HeaderText="Payment Option" />

                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>

                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loanduegrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Due
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanDue" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanDue_PageIndexChanging" OnRowDataBound="dgvLoanDue_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="LoanAmountApproved" HeaderStyle-HorizontalAlign="Left" HeaderText="Loan Amount Approved" />--%>
                                    <asp:TemplateField HeaderText="Loan Amount Approved" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("LoanAmountApproved") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalApproved" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$</span><asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PaymentOption" HeaderStyle-HorizontalAlign="Left" HeaderText="Payment Option" />
                                    <asp:TemplateField HeaderText="Loan Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("LoanStatus") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mode of Payment" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <input type="text" class="input" style="background-color: white !important; border: 1px solid black;" id='<%# (string.Format("txtmodeofpaymentcancel_{0}", Eval("Id"))) %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" style="margin-left: 2px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"
                                                onclick="javascript:return opencancelreceipt('<%# Eval("Id") %>');" id="contractbtn">Cancel Loan</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>

                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loanBankruptgrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Bankrupt
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvLoanBankrupt" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanOverDue_PageIndexChanging" OnRowDataBound="dgvLoanOverDue_RowDataBound" OnRowCancelingEdit="dgvLoanBankrupt_RowCancelingEdit">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Loan Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>--%>
                                            <asp:Label ID="lblLateDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payments Received" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PartialPayment") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalPartialAmountReceived" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%-- $<%# (Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) )) - (Eval("PartialPayment")=="" ? 0:Convert.ToDecimal(Eval("PartialPayment").ToString().Replace("$",""))) %>--%>
                                            <asp:Label ID="lblTotalRemainingDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalBalanceDue" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Num Times Loan Over Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverdueCount")==null ?"0":Eval("OverdueCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan OverDue Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverDueReason") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mode of Payment" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%-- <input type="text" class="input" style="background-color: white !important; border: 1px solid black;" id='<%# (string.Format("txtmodeofpaymentcancel_{0}", Eval("Id"))) %>' />--%>
                                            <asp:TextBox ID="txtModeofPayment" Style="background-color: white !important; border: 1px solid black;" CssClass="input" runat="server"></asp:TextBox>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--   <a href="javascript:void(0);" style="margin-left: 2px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"
                                            onclick="javascript:return opencancelreceipt('<%# Eval("Id") %>');" id="contractbtn">Cancel Loan</a>--%>
                                            <asp:LinkButton ID="lnkCancel" runat="server" OnClientClick="return confirm('are you sure you want to cancel this loan?');" CommandName="CancelBankrupt" CommandArgument='<%# Container.DataItemIndex + "," + Eval("Id") %>' Text="Cancel Loan" Style="margin-left: 2px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loanConsumerProposalgrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Consumer Proposal
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvConsumerProposal" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvLoanOverDue_PageIndexChanging" OnRowDataBound="dgvLoanOverDue_RowDataBound" OnRowCancelingEdit="dgvLoanBankrupt_RowCancelingEdit">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Loan Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%-- $<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>--%>
                                            <asp:Label ID="lblLateDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payments Received" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PartialPayment") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalPartialAmountReceived" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%-- $<%# (Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) )) - (Eval("PartialPayment")=="" ? 0:Convert.ToDecimal(Eval("PartialPayment").ToString().Replace("$",""))) %>--%>
                                            <asp:Label ID="lblTotalRemainingDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalBalanceDue" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Num Times Loan Over Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverdueCount")==null ?"0":Eval("OverdueCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan OverDue Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverDueReason") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mode of Payment" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%-- <input type="text" class="input" style="background-color: white !important; border: 1px solid black;" id='<%# (string.Format("txtmodeofpaymentcancel_{0}", Eval("Id"))) %>' />--%>
                                            <asp:TextBox ID="txtModeofPayment" Style="background-color: white !important; border: 1px solid black;" CssClass="input" runat="server"></asp:TextBox>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--   <a href="javascript:void(0);" style="margin-left: 2px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"
                                            onclick="javascript:return opencancelreceipt('<%# Eval("Id") %>');" id="contractbtn">Cancel Loan</a>--%>
                                            <asp:LinkButton ID="lnkCancel" runat="server" OnClientClick="return confirm('are you sure you want to cancel this loan?');" CommandName="CancelConsumer" CommandArgument='<%# Container.DataItemIndex + "," + Eval("Id") %>' Text="Cancel Loan" Style="margin-left: 2px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; display: none;" id="loandeptgrid" runat="server">
                    <tr class="headerrow">
                        <td style="text-align: center;">Loan Dept Management
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView Width="100%" ShowFooter="true" ID="dgvdeptmgmt" CssClass="EU_DataTable" AllowPaging="true" PageSize="1500" class="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" OnRowCommand="dgvLoan_RowCommand" OnPageIndexChanging="dgvdeptmgmt_PageIndexChanging" OnRowDataBound="dgvdeptmgmt_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" CommandArgument='<%# Eval("Id") %>'></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name" />--%>
                                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("CustomerName") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">Grand Total</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Loan Due Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            $<%# Eval("DueAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalDueAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Over Due Loan Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) ) %>--%>
                                            <asp:Label ID="lblDebtOverDueLateAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalOverDueLoanAmount" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payments Received" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("PartialPayment") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalPartialAmountReceived" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%--$<%# (Convert.ToDecimal(Eval("DueAmount"))  + (Eval("LateInterestCharge")==null? 0: Convert.ToDecimal( Eval("LateInterestCharge")) ) + (Eval("NSFCharge")==null? 0: Convert.ToDecimal( Eval("NSFCharge")) )) - (Eval("PartialPayment")=="" ? 0:Convert.ToDecimal(Eval("PartialPayment").ToString().Replace("$",""))) %>--%>
                                            <asp:Label ID="lblTotalRemainingDueAmount" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span class="footerrow">$<asp:Label ID="lblTotalBalanceDue" runat="server" CssClass="footerrow"></asp:Label></span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Num Times Loan Over Due" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverdueCount")==null ?"0":Eval("OverdueCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan OverDue Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Eval("OverDueReason") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="color: red; text-align: center;">No records found</div>
                                </EmptyDataTemplate>
                                <FooterStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
            </div>

        </asp:View>
        <asp:View ID="vdetails" runat="server">
            <div class="page-content">
                <div id="data" runat="server">
                    <div class="col-md-10">
                        <div class="row">
                            <asp:Repeater ID="rptCustomer" runat="server">
                                <ItemTemplate>
                                    <div class="panel panel-primary">
                                        <div class="panel-heading">
                                            <h3 class="panel-title">Customer Information</h3>
                                        </div>
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Name:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("FirstName") %> <%# Eval("LastName") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Date of Birth:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("DateofBirth") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">City:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("City") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">PostCode:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("PostCode") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Own a Home Or Rent:</label>
                                                        <label class="col-md-7">
                                                            <%#Eval("HomeType").ToString() =="Select Home Type" ? "Not Selected": Eval("HomeType") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Pay Cycle Comment:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("PhoneListedunder") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Cell Phone:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("CellPhone") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Email:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("DrivingLicenseNumber") %>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-md-5">
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Gender:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("Gender") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Address:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("Address") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Province:</label>
                                                        <label class="col-md-7">
                                                            <%# GetProvince(Convert.ToInt32(  Eval("Province")  )) %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Duration at this address:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("DurationYears") %> Years <%# Eval("DurationMonth") %> Months
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Home Phone:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("HomePhone") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Work Phone:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("WorkPhone") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Social Security Number:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("SocialSecurityNumber") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Comments:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("Comments") %>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptLoan" runat="server">
                                <ItemTemplate>
                                    <div class="panel panel-primary">
                                        <div class="panel-heading">
                                            <h3 class="panel-title">Loan Information
                            <a href="javascript:void(0);" style="margin-left: 200px; color: white; background-color: black; border: 1px solid; padding-top: 6px; padding-bottom: 6px; padding-left: 5px; padding-right: 5px;"
                                onclick="javascript:return call('<%# Eval("Id") %>');" id="contractbtn">View Contract</a></h3>
                                        </div>
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Loan Amount Applied:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("LoanAmountApplied") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Total Amount Due:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("DueAmount") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Store:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("StoreAddress").ToString().Replace(",", "<br/>").Replace("$", " , ")  %>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-md-5">
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Loan Amount Approved:</label>
                                                        <label class="col-md-7">
                                                            <%# Eval("LoanAmountApproved") %>
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-5 control-label">Due Date:</label>
                                                        <label class="col-md-7">
                                                            <%# DataBinder.Eval(Container.DataItem, "NextPayDate", "{0:MM/dd/yyyy}").Replace("-","/") %>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div id="loanopened" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-body">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:RadioButtonList ID="rbtnlstLoanType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Loan Payment In Process" Value="Payment In Process" Selected="True" style="padding: 14px;"></asp:ListItem>
                                                    <asp:ListItem Text="Loan Close" Value="Close" style="padding: 14px;"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-md-5">Mode Of Payment:</label>
                                                <div class="">
                                                    <asp:TextBox ssClass="form-control" ID="txtModeofPayment" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-md-3">Discount:</label>
                                                <div class="">
                                                    <asp:TextBox ssClass="form-control" ID="txtopenDiscount" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">

                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="btnOpenLoanSubmit" class="btn btn-primary" Text="Save" OnClientClick="javascript:return validateform();" runat="server" OnClick="btnOpenLoanSubmit_Click"></asp:Button>
                                                <asp:Button ID="btnCancel" class="btn btn-pink" Text="Cancel" runat="server" OnClick="btnCancel_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loanProcess" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-body">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:RadioButtonList ID="rbtnlstloanprocessloantype" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Loan Close" Value="Close" style="padding: 14px;"></asp:ListItem>
                                                    <asp:ListItem Text="Loan Over Due" Value="Over Due" Selected="True" style="padding: 14px;"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-md-5">Settlement:</label>
                                                <div class="">
                                                    <asp:TextBox ssClass="form-control" ID="txtProcessSettlement" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5">Reason for Over Due:</label>
                                                <div class="">
                                                    <asp:TextBox ssClass="form-control" ID="txtReasonforOverdue" TextMode="MultiLine" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-md-3">Discount:</label>
                                                <div class="">
                                                    <asp:TextBox ssClass="form-control" ID="txtProcessDiscount" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">

                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="btnLoanprocessSubmit" class="btn btn-primary" Text="Save" OnClientClick="javascript:return validateformprocess();" runat="server" OnClick="btnLoanprocessSubmit_Click"></asp:Button>
                                                <asp:Button ID="btnCancelLoanProcess" class="btn btn-pink" Text="Cancel" runat="server" OnClick="btnCancelLoanProcess_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loanoverdue" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div id="tblLoanSummary" runat="server">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">Loan Information</h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Original Due Amount:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOVerdueOriginalDueAmount" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Late Interest Charge:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverDueLAteInterestCharge" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Total Partial Amount Paid:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverDuePArtialAmountPaid" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Due Date:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOVerDueDueDate" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">No. of Times Loan Over Due:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverDueCount" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">NSF Charge:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverdueNSFCharge" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Total Overdue Amount:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblTotalOverdueAmount" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Total Remaining Amount Due:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverDueTotalAmount" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Store:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverDueStoreAddress" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Loan Over Due Reason:</label>
                                                    <label class="col-md-7">
                                                        <asp:Label ID="lblOverduereason" runat="server"></asp:Label>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tbloverdue" runat="server">
                                <div class="panel panel-primary">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Settlement:</label>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="txtOverdueSettlement" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Partial Payment Method:</label>
                                                    <label class="col-md-7">
                                                        <asp:TextBox ID="txtPartialPaymentMethod" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <label class="col-md-5 control-label">Discount:</label>
                                                    <label class="col-md-7">
                                                        <asp:TextBox ID="txtOverdueDiscount" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </label>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-md-5 ">
                                                    <asp:RadioButtonList ID="rbtnlstOverdue" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Loan Close" Value="Close" style="padding: 8px;"></asp:ListItem>
                                                        <asp:ListItem Text="Loan Over Due" Value="Over Due" style="padding: 8px;"></asp:ListItem>
                                                        <asp:ListItem Text="Partial Payment Amount" Value="Over Due" style="padding: 8px;" Selected="True"></asp:ListItem>
                                                    </asp:RadioButtonList>

                                                </div>
                                                <div class="col-md-1 ">
                                                    <asp:TextBox ID="txtPartialpayment" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">

                                                <div class="col-md-offset-3 col-md-9">
                                                    <asp:Button ID="btnOverduesubmit" class="btn btn-primary" Text="Save" OnClientClick="javascript:return validateformoverdue();" runat="server" OnClick="btnOverduesubmit_Click"></asp:Button>
                                                    <asp:Button ID="ButtonbtnCancelOversue" class="btn btn-pink" Text="Cancel" runat="server" OnClick="ButtonbtnCancelOversue_Click"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tblcollectionoverdue" runat="server">
                                <div class="panel panel-primary">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-md-12 ">
                                                    <asp:RadioButtonList ID="rbtnlstCollectionOverdue" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Sent for Collection" Value="Sent for Collection" Selected="True" style="padding: 14px;"></asp:ListItem>
                                                        <asp:ListItem Text="Consumer Proposal" Value="Consumer Proposal" style="padding: 14px;"></asp:ListItem>
                                                        <asp:ListItem Text="DEPT Management" Value="DEPT Management" style="padding: 14px;"></asp:ListItem>
                                                        <asp:ListItem Text="Bankrupt" Value="Bankrupt" style="padding: 14px;"></asp:ListItem>
                                                        <asp:ListItem Text="Legal" Value="Legal" style="padding: 14px;"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-md-offset-3 col-md-9">
                                                    <asp:Button ID="btnoverduecollectionSubmit" class="btn btn-primary" Text="Save" runat="server" OnClick="btnoverduecollectionSubmit_Click"></asp:Button>
                                                    <asp:Button ID="btnoverduecollectionCancel" class="btn btn-pink" Text="Cancel" runat="server" OnClick="btnoverduecollectionCancel_Click"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loanclosed" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Closed Date:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblClosedDate" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">NSF Charges:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblNSFCharges" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Number of Times Loan Over Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblLoanOverDueCount" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Late Interest Charges:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblLateInterestcharges" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Late Amount Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblTotalLateAmountDue" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">

                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="btnCloseBack" class="btn btn-primary" Text="Back" runat="server" OnClick="btnCloseBack_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loandenied" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Loan Information <span style="margin-left: 200px; color: white; font-weight: bold;">Loan Status: Denied</span></h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Loan Amount Applied:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblDeniedLoanAmountApplied" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Loan Deny Reason:</label>
                                                <label class="col-md-7">
                                                    <asp:TextBox ID="txtLoanDeniedreason" runat="server"></asp:TextBox>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Store:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblStoreAddress" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="Button1" class="button2" Text="Back" runat="server" OnClick="btnCloseBack_Click"></asp:Button>
                                                <asp:Button ID="btnUpdateReason" class="button3" Text="Save" runat="server" OnClick="btnUpdateReason_click"></asp:Button>
                                                <asp:Button ID="btnReOpenUser" class="button2" OnClientClick="return confirm('Are you sure you want to re open this user ?');" Text="ReOpen this User to make new loan" runat="server" OnClick="btnReOpenUser_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loansentforcollection" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Loan Information</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Original Due Amount:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionOrigionalDueAmount" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Late Interest Charge:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionInterestCharge" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Amount Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionAmountDue" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">No. of Times Loan Over Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblNoofOverDuecollection" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Store:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionStoreAddress" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">NSF Charge:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionNSFCharge" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Partial Amount Paid:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionPartialAmountPaid" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Due Date:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionDueDate" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Loan Over Due Reason:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblCollectionoverduereason" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:RadioButtonList ID="rbtnCollectionStatus" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Consumer Proposal" Value="Consumer Proposal" Selected="True" style="padding: 10px;"></asp:ListItem>
                                                <asp:ListItem Text="DEPT Management" Value="DEPT Management" style="padding: 10px;"></asp:ListItem>
                                                <asp:ListItem Text="Bankrupt" Value="Bankrupt" style="padding: 10px;"></asp:ListItem>
                                                <asp:ListItem Text="Legal" Value="Legal" style="padding: 10px;"></asp:ListItem>
                                                <asp:ListItem Text="Loan Close" Value="Close" style="padding: 10px;"></asp:ListItem>
                                                <asp:ListItem Text="Partial Payment Amount" Value="Partial Payment Amount" style="padding: 10px;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="row" style="padding-top: 10px;">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Discount:</label>
                                                <div class="controls col-md-7">
                                                    <asp:TextBox ssClass="form-control" ID="txtDiscountloancollection" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Partial Amount:</label>
                                                <div class="controls col-md-7">
                                                    <asp:TextBox ssClass="form-control" ID="txtPartialloancollection" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="padding-top: 10px;">
                                        <div class="form-group">
                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="btnCollectionSubmit" class="btn btn-primary" Text="Save" OnClientClick="javascript:return validateforsentforcollection();" runat="server" OnClick="btnCollectionSubmit_Click"></asp:Button>
                                                <asp:Button ID="btnCollectionCancel" class="btn btn-pink" Text="Cancel" runat="server" OnClick="btnCollectionCancel_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loanbankrupt" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Loan Information</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Original Due Amount:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblOriginalDueAmountBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Late Interest Charge:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblLateInterestChargeBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Amount Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblTotalDueAmountBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">No. of Times Loan Over Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblOverduecountBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Store:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblStoreAddressBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">NSF Charge:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblNSFChargeBAnkrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Partial Amount Paid:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="PartialAMountPaidBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Due Date:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblDueDateBankrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Loan Over Due Reason:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblOverDueReasonBAnkrupt" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:RadioButtonList ID="rbtnConsumerPartialpayment" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Partial Payment Amount" Value="Partial Payment Amount" style="padding: 14px;" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="row" style="padding-top: 10px;">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Partial Amount:</label>
                                                <div class="controls col-md-7">
                                                    <asp:TextBox ssClass="form-control" ID="txtConsumerPartialPayment" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="padding-top: 10px;">
                                        <div class="form-group">
                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="btnConsumerSubmit" class="btn btn-primary" Text="Save" runat="server" OnClick="btnConsumerSubmit_Click" OnClientClick="javascript:return validateforconsumer();"></asp:Button>
                                                <asp:Button ID="btnBackBankrupt" class="btn btn-pink" Text="Back" runat="server" OnClick="btnBackBankrupt_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="loandeptmanagement" runat="server">
                    <div class="row">
                        <div class="col-md-10">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Loan Information</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Original Due Amount:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblDueAmountDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Late Interest Charge:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lbllateIterestchargedept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Amount Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblTotaldueAmountDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">No. of Times Loan Over Due:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblOverduecountDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Store:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblSToreAddressDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">NSF Charge:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblNSFchargeDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Total Partial Amount Paid:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblPArtialamountDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Due Date:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblDuedateDept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Loan Over Due Reason:</label>
                                                <label class="col-md-7">
                                                    <asp:Label ID="lblOverduereasondept" runat="server"></asp:Label>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:RadioButtonList ID="rbtnDeptStatus" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Loan Close" Value="Close" style="padding: 14px;"></asp:ListItem>
                                                <asp:ListItem Text="Partial Payment Amount" Value="Partial Payment Amount" style="padding: 14px;" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Sent for Collection" Value="Sent for Collection" style="padding: 14px;"></asp:ListItem>
                                                <asp:ListItem Text="Legal" Value="Legal" style="padding: 14px;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="row" style="padding-top: 10px;">
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Discount:</label>
                                                <div class="controls col-md-7">
                                                    <asp:TextBox ssClass="form-control" ID="txtDeptDiscount" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <label class="col-md-5 control-label">Partial Amount:</label>
                                                <div class="controls col-md-7">
                                                    <asp:TextBox ssClass="form-control" ID="txtPartialAmountDept" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="padding-top: 10px;">
                                        <div class="form-group">
                                            <div class="col-md-offset-3 col-md-9">
                                                <asp:Button ID="btnDeptSubmit" class="btn btn-primary" Text="Save" OnClientClick="javascript:return validatefordept();" runat="server" OnClick="btnDeptSubmit_Click"></asp:Button>
                                                <asp:Button ID="btnDeptCancel" class="btn btn-pink" Text="Cancel" runat="server" OnClick="btnDeptCancel_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



        </asp:View>
    </asp:MultiView>
    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnselectedstatus" runat="server" Value="0" />
    <asp:HiddenField ID="hdnishidecontractbutton" runat="server" Value="0" />
    <script type="text/javascript">

        $(function () {
            $('#' + '<%= txtFromDate.ClientID %>').datetimepicker({
                format: 'L'
            });
            $('#' + '<%= txtToDate.ClientID %>').datetimepicker({
                format: 'L'
            });
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {


            if ($('#' + '<%= hdnishidecontractbutton.ClientID %>').val() == "1") {
                $("[id*=contractbtn]").hide();
            }
            else {
                $("[id*=contractbtn]").show();
            }
        });

        function call(obj) {
            window.open('/CustomerLoanContract.aspx?Id=' + obj + '', '_blank', 'width=800,height=550,location=no,left=200px');
        }
        function openclosereceipt(obj) {
            window.open('/LoanCloseReceipt.aspx?Id=' + obj + '', '_blank', 'width=800,height=550,location=no,left=200px');
        }
        function opencancelreceipt(obj) {
            //alert($('#txtmodeofpaymentcancel_' + obj).val());

            if (confirm('Are you sure you want to cancel this Loan ?')) {

                var objtextbox = $('#txtmodeofpaymentcancel_' + obj).val();
                <%--       $('#' + '<%= ddlReportType.ClientID %>').val('0');
                $('#' + '<%= txtFromDate.ClientID %>').val('');
                $('#' + '<%= txtToDate.ClientID %>').val('');
                $("#btnSubmit").click();--%>
                location.reload(true);
                window.open('/LoanCancelReceipt.aspx?Id=' + obj + '&data=' + $('#txtmodeofpaymentcancel_' + obj).val(), '_blank', 'width=800,height=550,location=no,left=200px');

            }
        }

        function validateform() {
            if ($('#' + '<%= txtModeofPayment.ClientID %>').val() == '' || $('#' + '<%= txtModeofPayment.ClientID %>').val() == undefined) {
                alert('Please enter  Mode of Payment');

                $('#' + '<%= txtModeofPayment.ClientID %>').focus();
                $('#' + '<%= txtModeofPayment.ClientID %>').attr("style", "border-color:red;");
                return false;
            }

            else {
                $('#' + '<%= txtModeofPayment.ClientID %>').removeAttr("style");
            }
            var checked_radio = $("[id*=rbtnlstLoanType] input:checked");
            var value = checked_radio.closest("td").find("label").html();
            if (value == "Loan Close") {
                if ($('#' + '<%= txtopenDiscount.ClientID %>').val() == '' || $('#' + '<%= txtopenDiscount.ClientID %>').val() == undefined) {
                    alert('Please enter  Discount Amount');

                    $('#' + '<%= txtopenDiscount.ClientID %>').focus();
                    $('#' + '<%= txtopenDiscount.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }

                else {
                    $('#' + '<%= txtopenDiscount.ClientID %>').removeAttr("style");
                }
            }
            else {
                if ($('#' + '<%= txtopenDiscount.ClientID %>').val() != '' && $('#' + '<%= txtopenDiscount.ClientID %>').val() != undefined && $('#' + '<%= txtopenDiscount.ClientID %>').val() != '0') {
                    alert('you can only give discount if you are going to close the loan');

                    $('#' + '<%= txtopenDiscount.ClientID %>').focus();
                    $('#' + '<%= txtopenDiscount.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
            }

        }
        function validateformprocess() {
            var checked_radio = $("[id*=rbtnlstloanprocessloantype] input:checked");
            var value = checked_radio.closest("td").find("label").html();
            if (value == "Loan Over Due") {
                if ($('#' + '<%= txtReasonforOverdue.ClientID %>').val() == '' || $('#' + '<%= txtReasonforOverdue.ClientID %>').val() == undefined) {
                    alert('Please enter  Reason for Overdue');

                    $('#' + '<%= txtReasonforOverdue.ClientID %>').focus();
                    $('#' + '<%= txtReasonforOverdue.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
                else {
                    $('#' + '<%= txtReasonforOverdue.ClientID %>').removeAttr("style");
                }
            }


            if (value == "Loan Close") {
                if ($('#' + '<%= txtProcessDiscount.ClientID %>').val() == '' || $('#' + '<%= txtProcessDiscount.ClientID %>').val() == undefined) {
                    alert('Please enter  Discount Amount');

                    $('#' + '<%= txtProcessDiscount.ClientID %>').focus();
                    $('#' + '<%= txtProcessDiscount.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }

                else {
                    $('#' + '<%= txtProcessDiscount.ClientID %>').removeAttr("style");
                }
            }
            else {
                if ($('#' + '<%= txtProcessDiscount.ClientID %>').val() != '' && $('#' + '<%= txtProcessDiscount.ClientID %>').val() != undefined && $('#' + '<%= txtProcessDiscount.ClientID %>').val() != '0') {
                    alert('you can only give discount if you are going to close the loan');

                    $('#' + '<%= txtProcessDiscount.ClientID %>').focus();
                    $('#' + '<%= txtProcessDiscount.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
            }
        }
        function validateformoverdue() {


            var checked_radio = $("[id*=rbtnlstOverdue] input:checked");
            var value = checked_radio.closest("td").find("label").html();

            if (value == "Partial Payment Amount") {
                if ($('#' + '<%= txtPartialpayment.ClientID %>').val() == '' || $('#' + '<%= txtPartialpayment.ClientID %>').val() == undefined) {
                    alert('Please enter  Partial Payment amount');

                    $('#' + '<%= txtPartialpayment.ClientID %>').focus();
                    $('#' + '<%= txtPartialpayment.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
                else {
                    $('#' + '<%= txtPartialpayment.ClientID %>').removeAttr("style");
                }
            }
            $('#' + '<%= hdnselectedstatus.ClientID %>').val(value);
        }

        function validateforsentforcollection() {


            var checked_radio = $("[id*=rbtnCollectionStatus] input:checked");
            var value = checked_radio.closest("td").find("label").html();

            if (value == "Partial Payment Amount") {
                if ($('#' + '<%= txtPartialloancollection.ClientID %>').val() == '' || $('#' + '<%= txtPartialloancollection.ClientID %>').val() == undefined) {
                    alert('Please enter  Partial Payment amount');

                    $('#' + '<%= txtPartialloancollection.ClientID %>').focus();
                    $('#' + '<%= txtPartialloancollection.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
                else {
                    $('#' + '<%= txtPartialloancollection.ClientID %>').removeAttr("style");
                }
            }
            if (value == "Loan Close") {
                if ($('#' + '<%= txtDiscountloancollection.ClientID %>').val() == '' || $('#' + '<%= txtDiscountloancollection.ClientID %>').val() == undefined) {
                    alert('Please enter  Discount Amount');

                    $('#' + '<%= txtDiscountloancollection.ClientID %>').focus();
                    $('#' + '<%= txtDiscountloancollection.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }

                else {
                    $('#' + '<%= txtDiscountloancollection.ClientID %>').removeAttr("style");
                }
            }
        }
        function validatefordept() {


            var checked_radio = $("[id*=rbtnDeptStatus] input:checked");
            var value = checked_radio.closest("td").find("label").html();

            if (value == "Partial Payment Amount") {
                if ($('#' + '<%= txtPartialAmountDept.ClientID %>').val() == '' || $('#' + '<%= txtPartialAmountDept.ClientID %>').val() == undefined) {
                    alert('Please enter  Partial Payment amount');

                    $('#' + '<%= txtPartialAmountDept.ClientID %>').focus();
                    $('#' + '<%= txtPartialAmountDept.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
                else {
                    $('#' + '<%= txtPartialAmountDept.ClientID %>').removeAttr("style");
                }
            }
            if (value == "Loan Close") {
                if ($('#' + '<%= txtDeptDiscount.ClientID %>').val() == '' || $('#' + '<%= txtDeptDiscount.ClientID %>').val() == undefined) {
                    alert('Please enter  Discount Amount');

                    $('#' + '<%= txtDeptDiscount.ClientID %>').focus();
                    $('#' + '<%= txtDeptDiscount.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }

                else {
                    $('#' + '<%= txtDeptDiscount.ClientID %>').removeAttr("style");
                }
            }
        }

        function validateforconsumer() {

            var checked_radio = $("[id*=rbtnConsumerPartialpayment] input:checked");
            var value = checked_radio.closest("td").find("label").html();

            if (value == "Partial Payment Amount") {
                if ($('#' + '<%= txtConsumerPartialPayment.ClientID %>').val() == '' || $('#' + '<%= txtConsumerPartialPayment.ClientID %>').val() == undefined) {
                    alert('Please enter  Partial Payment amount');

                    $('#' + '<%= txtConsumerPartialPayment.ClientID %>').focus();
                    $('#' + '<%= txtConsumerPartialPayment.ClientID %>').attr("style", "border-color:red;");
                    return false;
                }
                else {
                    $('#' + '<%= txtConsumerPartialPayment.ClientID %>').removeAttr("style");
                }
            }
        }
    </script>
    <script>
        $(document).ready(function () {
            $('.historybutton').click(function () {
                var id = $(this).attr('id');
                openhistory(id);
            });

        });
        function openhistory(obj) {
            window.open('/CustomerCashHistory.aspx?Id=' + obj + '&IsCurrencyExchange=true&IsLoan=true', '_blank', 'width=1400,height=750,location=no,left=200px');
        }
    </script>
</asp:Content>
