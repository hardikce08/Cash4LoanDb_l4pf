using CashLoanShop.DataAccess;
using CashLoanShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CashLoanShop
{
    public partial class ViewReport : System.Web.UI.Page
    {
        CustomerLoanService cs = new CustomerLoanService();
        public int UserId { get; set; }
        public decimal FooterSumApproved, TotalDueAmount, TotalOverDueAmount, TotalPartialAmount, TotalOriginalDueAMount, totalBalanceDueAMount;
        protected void Page_Load(object sender, EventArgs e)
        {
            GetUserId();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindLoanEntryData();
        }
        #region Grid events
        protected void dgvLoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoan.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }

        protected void dgvLoan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                FooterSumApproved += item.LoanAmountApproved;
                TotalDueAmount += item.DueAmount;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotalApproved");
                lbl.Text = FooterSumApproved.ToString();
                Label lblTotalDue = (Label)e.Row.FindControl("lblTotalDueAmount");
                lblTotalDue.Text = TotalDueAmount.ToString();
            }
        }
        protected void dgvLoanSentforCollection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoan.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }

        protected void dgvLoanSentforCollection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                FooterSumApproved += item.LoanAmountApproved;
                TotalOriginalDueAMount += item.DueAmount;
                TotalDueAmount += item.DueAmount;
                TotalPartialAmount += string.IsNullOrEmpty(item.PartialPayment) ? 0 : Convert.ToDecimal(item.PartialPayment.Replace("$", ""));
                TotalOverDueAmount += Convert.ToDecimal(item.DueAmount + (item.NSFCharge == null ? 0 : item.NSFCharge) + (item.LateInterestCharge == null ? 0 : item.LateInterestCharge));
                Label lblCollectionDueAmount = (Label)e.Row.FindControl("lblCollectionDueAmount");
                lblCollectionDueAmount.Text = "$" + item.DueAmount.ToString();
                Label lblTotalBalanceDue = (Label)e.Row.FindControl("lblTotalRemainingDueAmount");
                lblTotalBalanceDue.Text = "$" + item.DueAmount.ToString();
                if (ConvertEasternTime(DateTime.Now).Date > item.NextPayDate.Date)
                {
                    int days = (ConvertEasternTime(DateTime.Now).Date - item.NextPayDate.Date).Days;
                    double perdaycharge = (((double)item.DueAmount) * 0.32) / 365;
                    double totalcharge = days * perdaycharge;
                    //lblOverDueLAteInterestCharge.Text = Convert.ToDecimal(totalcharge).ToString();
                    decimal finalval = Convert.ToDecimal(item.DueAmount) + Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge));
                    lblCollectionDueAmount.Text = "$" + Math.Round(finalval, 2).ToString();
                    TotalDueAmount += Math.Round(Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge)), 2);

                    decimal partialamount = string.IsNullOrEmpty(item.PartialPayment) ? 0 : Convert.ToDecimal(item.PartialPayment.Replace("$", ""));
                    lblTotalBalanceDue.Text = "$" + Math.Round(finalval - partialamount, 2).ToString();
                    totalBalanceDueAMount += Math.Round(Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge)), 2) - partialamount;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalApproved = (Label)e.Row.FindControl("lblTotalApproved");
                lblTotalApproved.Text = FooterSumApproved.ToString();


                Label lblTotalOverDueLoanAmount = (Label)e.Row.FindControl("lblTotalOverDueLoanAmount");
                lblTotalOverDueLoanAmount.Text = Math.Round(TotalDueAmount, 2).ToString();
                Label lblTotalPartialAmountReceived = (Label)e.Row.FindControl("lblTotalPartialAmountReceived");
                lblTotalPartialAmountReceived.Text = TotalPartialAmount.ToString();
                Label lblTotalBalanceDue = (Label)e.Row.FindControl("lblTotalBalanceDue");
                lblTotalBalanceDue.Text = (Math.Round(TotalDueAmount, 2) - TotalPartialAmount).ToString();

                Label lblTotalCollectionDueAmount = (Label)e.Row.FindControl("lblTotalCollectionDueAmount");
                lblTotalCollectionDueAmount.Text = Math.Round(TotalOriginalDueAMount, 2).ToString();
            }
        }

        public string GetProvince(int Id)
        {
            switch (Id)
            {
                case 1:
                    return "Alberta";

                case 2:
                    return "British Columbia";
                case 3:
                    return "Manitoba";
                case 4:
                    return "New Brunswick";
                case 5:
                    return "Newfoundland";
                case 6:
                    return "Nova Scotia";
                case 7:
                    return "Northwest Territories";
                case 8:
                    return "Ontario";
                case 9:
                    return "Prince Edward Island";
                case 10:
                    return "Quebec";
                case 11:
                    return "Saskatchewan";
                case 12:
                    return "Yukon";
                case 13:
                    return "Other";
                default:
                    return "Not Selected";

            }
        }
        protected void dgvLoan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int LoanId = Convert.ToInt32(e.CommandArgument);

                CustomerLoanService cc = new CustomerLoanService();
                List<Model.CustomerLoan> objcc = cc.CustomerLoans.Where(p => p.Id == LoanId).ToList();
                Model.CustomerLoan objtloan = objcc.FirstOrDefault();
                hdnCustomerId.Value = objtloan.CustomerId.ToString();
                CustomerService cs = new CustomerService();
                List<CustomerMaster> cm = cs.CustomerMasters.Where(p => p.Id == objtloan.CustomerId).ToList();
                cm.FirstOrDefault().ProvinceName = GetProvince(Convert.ToInt32(cm.FirstOrDefault().Province));

                mvView.ActiveViewIndex = 1;
                rptCustomer.DataSource = cm;
                rptCustomer.DataBind();
                hdnId.Value = e.CommandArgument.ToString();

                CompanyService cms = new CompanyService();
                Model.CompanyStore CompanyStores = cms.CompanyStores.Where(p => p.Id == objtloan.ShopStoreId).FirstOrDefault();
                objtloan.StoreAddress = CompanyStores.Address;
                rptLoan.DataSource = objcc;
                rptLoan.DataBind();
                txtPartialpayment.Text = "";
                txtDiscountloancollection.Text = "";
                txtPartialloancollection.Text = "";
                txtDeptDiscount.Text = "";
                txtPartialAmountDept.Text = "";
                if (objcc.FirstOrDefault().LoanStatus == "Open")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    hdnishidecontractbutton.Value = "0";
                    txtModeofPayment.Text = "";
                    rptLoan.Visible = true;
                }
                if (objcc.FirstOrDefault().LoanStatus == "Payment In Process")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    hdnishidecontractbutton.Value = "1";
                    rptLoan.Visible = true;
                    txtProcessSettlement.Text = objcc.FirstOrDefault().Settlement;
                    txtReasonforOverdue.Text = "";
                }
                if (objcc.FirstOrDefault().LoanStatus == "Over Due")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblOverduereason.Text = objcc.FirstOrDefault().OverDueReason;
                    lblOverDueCount.Text = objcc.FirstOrDefault().OverdueCount.ToString();
                    rbtnlstOverdue.SelectedIndex = 2;
                    hdnishidecontractbutton.Value = "1";
                    rptLoan.Visible = false;
                    Model.CustomerLoan objloan = objcc.FirstOrDefault();
                    txtOverdueSettlement.Text = "";
                    txtOverdueSettlement.Text = objcc.FirstOrDefault().Settlement;
                    lblOverdueNSFCharge.Text = "$20.00";
                    txtPartialPaymentMethod.Text = "";
                    txtOverdueDiscount.Text = "";
                    lblOverDueLAteInterestCharge.Text = "$" + (objloan.LateInterestCharge == null ? 0 : objloan.LateInterestCharge).ToString();
                    if (ConvertEasternTime(DateTime.Now).Date > objloan.NextPayDate.Date)
                    {
                        int days = (ConvertEasternTime(DateTime.Now).Date - objloan.NextPayDate.Date).Days;
                        double perdaycharge = (((double)objloan.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        lblOverDueLAteInterestCharge.Text = Math.Round(Convert.ToDecimal(totalcharge), 2).ToString();
                    }
                    lblOverDuePArtialAmountPaid.Text = "$0.00";
                    List<Model.LoanPartialPayment> lstpartialpayment = cc.LoanPartialPaymentsnew.Where(p => p.LoanId == objloan.Id).ToList();

                    if (lstpartialpayment.Count > 0)
                    {
                        lblOverDuePArtialAmountPaid.Text = "";
                        foreach (Model.LoanPartialPayment obj in lstpartialpayment)
                        {
                            lblOverDuePArtialAmountPaid.Text += "Partial Payment on " + obj.CreatedDate.ToString("MM/dd/yyyy") + ":  <b>$" + obj.PartialAmount.ToString() + "</b><br/>";
                        }
                        //lblOverDuePArtialAmountPaid.Text = "$" + lstpartialpayment.Sum(p => p.PartialAmount).ToString();

                    }
                    lblOVerdueOriginalDueAmount.Text = "$" + objloan.DueAmount.ToString();
                    lblOverDueTotalAmount.Text = "$" + Math.Round(((objloan.DueAmount + Convert.ToDecimal(lblOverdueNSFCharge.Text.Replace("$", "")) + Convert.ToDecimal(lblOverDueLAteInterestCharge.Text.Replace("$", ""))) - lstpartialpayment.Sum(p => p.PartialAmount)), 2).ToString();
                    lblTotalOverdueAmount.Text = "$" + Math.Round((objloan.DueAmount + Convert.ToDecimal(lblOverdueNSFCharge.Text.Replace("$", "")) + Convert.ToDecimal(lblOverDueLAteInterestCharge.Text.Replace("$", ""))), 2).ToString();
                    lblOVerDueDueDate.Text = objloan.NextPayDate.ToString("MM/dd/yyyy");
                    CompanyService cmp = new CompanyService();
                    Model.CompanyStore store = cmp.CompanyStores.Where(p => p.Id == objloan.ShopStoreId).FirstOrDefault();
                    if (store != null)
                    {
                        lblOverDueStoreAddress.Text = store.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + store.PhoneNo + "<br/>" + store.Email;
                    }
                    if (objloan.OverdueCount >= 3)
                    {
                        tbloverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                        tblcollectionoverdue.Style.Add(HtmlTextWriterStyle.Display, "");
                    }
                    else
                    {
                        tbloverdue.Style.Add(HtmlTextWriterStyle.Display, "");
                        tblcollectionoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
                if (objcc.FirstOrDefault().LoanStatus == "Close")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblNSFCharges.Text = "$" + (objcc.FirstOrDefault().NSFCharge == null ? "0.00" : objcc.FirstOrDefault().NSFCharge.ToString());
                    lblTotalLateAmountDue.Text = "$" + (objcc.FirstOrDefault().LateInterestCharge == null ? "0.00" : objcc.FirstOrDefault().LateInterestCharge.ToString()); ;
                    lblLoanOverDueCount.Text = objcc.FirstOrDefault().OverdueCount.ToString();
                    //if (ConvertEasternTime(DateTime.Now).Date > objcc.FirstOrDefault().NextPayDate.Date)
                    //{
                    //    int days = (ConvertEasternTime(DateTime.Now).Date - objcc.FirstOrDefault().NextPayDate.Date).Days;
                    //    double perdaycharge = (((double)objcc.FirstOrDefault().DueAmount) * 0.32) / 365;
                    //    double totalcharge = days * perdaycharge;
                    //    lblLateInterestcharges.Text = Convert.ToDecimal(totalcharge).ToString();
                    //}
                    lblLateInterestcharges.Text = "$ " + objcc.FirstOrDefault().LateInterestCharge.ToString();
                    lblClosedDate.Text = objcc.FirstOrDefault().UpdatedDate.ToString("MM/dd/yyyy").Replace("-", "/");
                    hdnishidecontractbutton.Value = "1";

                    rptLoan.Visible = true;
                }
                if (objcc.FirstOrDefault().LoanStatus == "Denied")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    rptLoan.Visible = false;
                    lblDeniedLoanAmountApplied.Text = objcc.FirstOrDefault().LoanAmountApplied.ToString();
                    if (CompanyStores != null)
                    {
                        lblStoreAddress.Text = CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>";
                    }
                    long idd = objcc.FirstOrDefault().Id;
                    LoanDenyReason objdn = cc.LoanDenyReasons.Where(p => p.LoanId == idd).FirstOrDefault();
                    txtLoanDeniedreason.Text = objdn == null ? "" : objdn.DenyReason;
                    hdnishidecontractbutton.Value = "1";
                    //
                    CustomerLoanService css = new CustomerLoanService();
                    CustomerLoanException cl = css.CustomerLoanExceptions.ToList().Where(p => p.CustomerId == objcc.FirstOrDefault().CustomerId && p.LoanId == objcc.FirstOrDefault().Id).FirstOrDefault();
                    if (cl == null)
                    {
                        btnReOpenUser.Visible = true;
                    }
                    else
                    {
                        btnReOpenUser.Visible = false;
                    }
                }

                if (objcc.FirstOrDefault().LoanStatus == "Sent for Collection" || objcc.FirstOrDefault().LoanStatus == "Legal")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblCollectionoverduereason.Text = objcc.FirstOrDefault().OverDueReason;
                    lblNoofOverDuecollection.Text = objcc.FirstOrDefault().OverdueCount.ToString();
                    hdnishidecontractbutton.Value = "1";
                    rptLoan.Visible = false;
                    if (objcc.FirstOrDefault().LoanStatus == "Sent for Collection")
                    {
                        rbtnCollectionStatus.Items[3].Text = "Legal";
                        rbtnCollectionStatus.Items[3].Value = "Legal";
                    }
                    if (objcc.FirstOrDefault().LoanStatus == "Legal")
                    {
                        rbtnCollectionStatus.Items[3].Text = "Sent for Collection";
                        rbtnCollectionStatus.Items[3].Value = "Sent for Collection";
                    }
                    Model.CustomerLoan objloan = objtloan;
                    lblCollectionNSFCharge.Text = "$20.00";
                    lblCollectionInterestCharge.Text = "$" + (objloan.LateInterestCharge == null ? 0 : Math.Round((decimal)objloan.LateInterestCharge,2)).ToString();
                    if (ConvertEasternTime(DateTime.Now).Date > objloan.NextPayDate.Date)
                    {
                        int days = (ConvertEasternTime(DateTime.Now).Date - objloan.NextPayDate.Date).Days;
                        double perdaycharge = (((double)objloan.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        lblCollectionInterestCharge.Text = Math.Round(Convert.ToDecimal(totalcharge),2).ToString();
                    }
                    lblCollectionPartialAmountPaid.Text = "$0.00";
                    List<Model.LoanPartialPayment> lstpartialpayment = cc.LoanPartialPaymentsnew.Where(p => p.LoanId == objloan.Id).ToList();

                    if (lstpartialpayment.Count > 0)
                    {
                        lblCollectionPartialAmountPaid.Text = "";
                        foreach (Model.LoanPartialPayment obj in lstpartialpayment)
                        {
                            lblCollectionPartialAmountPaid.Text += "Partial Payment on " + obj.CreatedDate.ToString("MM/dd/yyyy") + ":  <b>$" + obj.PartialAmount.ToString() + "</b><br/>";
                        }
                        //lblOverDuePArtialAmountPaid.Text = "$" + lstpartialpayment.Sum(p => p.PartialAmount).ToString();

                    }
                    lblCollectionOrigionalDueAmount.Text = "$" + objloan.DueAmount.ToString();
                    lblCollectionAmountDue.Text = "$" +Math.Round ( ((objloan.DueAmount + Convert.ToDecimal(lblCollectionNSFCharge.Text.Replace("$", "")) + Convert.ToDecimal(lblCollectionInterestCharge.Text.Replace("$", ""))) - lstpartialpayment.Sum(p => p.PartialAmount)),2).ToString();
                    lblCollectionDueDate.Text = objloan.NextPayDate.ToString("MM/dd/yyyy");
                    CompanyService cmp = new CompanyService();
                    Model.CompanyStore store = cmp.CompanyStores.Where(p => p.Id == objloan.ShopStoreId).FirstOrDefault();
                    if (store != null)
                    {
                        lblCollectionStoreAddress.Text = store.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + store.PhoneNo + "<br/>" + store.Email;
                    }
                }
                if (objtloan.LoanStatus == "Bankrupt" || objtloan.LoanStatus == "Consumer Proposal")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "");
                    lblOverDueReasonBAnkrupt.Text = objtloan.OverDueReason;
                    lblOverduecountBankrupt.Text = objtloan.OverdueCount.ToString();
                    hdnishidecontractbutton.Value = "1";
                    rptLoan.Visible = false;
                    Model.CustomerLoan objloan = objtloan;

                    lblNSFChargeBAnkrupt.Text = "$20.00";
                    lblLateInterestChargeBankrupt.Text = "$" + (objloan.LateInterestCharge == null ? 0 : Math.Round((decimal)objloan.LateInterestCharge, 2)).ToString();
                    if (ConvertEasternTime(DateTime.Now).Date > objloan.NextPayDate.Date)
                    {
                        int days = (ConvertEasternTime(DateTime.Now).Date - objloan.NextPayDate.Date).Days;
                        double perdaycharge = (((double)objloan.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        lblLateInterestChargeBankrupt.Text = Math.Round(Convert.ToDecimal(totalcharge), 2).ToString();
                    }
                    PartialAMountPaidBankrupt.Text = "$0.00";
                    List<Model.LoanPartialPayment> lstpartialpayment = cc.LoanPartialPaymentsnew.Where(p => p.LoanId == objloan.Id).ToList();

                    if (lstpartialpayment.Count > 0)
                    {
                        PartialAMountPaidBankrupt.Text = "";
                        foreach (Model.LoanPartialPayment obj in lstpartialpayment)
                        {
                            PartialAMountPaidBankrupt.Text += "Partial Payment on " + obj.CreatedDate.ToString("MM/dd/yyyy") + ":  <b>$" + obj.PartialAmount.ToString() + "</b><br/>";
                        }
                        //PartialAMountPaidBankrupt.Text = "$" + lstpartialpayment.Sum(p => p.PartialAmount).ToString();

                    }
                    lblOriginalDueAmountBankrupt.Text = "$" + objloan.DueAmount.ToString();
                    lblTotalDueAmountBankrupt.Text = "$" + Math.Round(((objloan.DueAmount + Convert.ToDecimal(lblNSFChargeBAnkrupt.Text.Replace("$", "")) + Convert.ToDecimal(lblLateInterestChargeBankrupt.Text.Replace("$", ""))) - lstpartialpayment.Sum(p => p.PartialAmount)), 2).ToString();
                    lblDueDateBankrupt.Text = objloan.NextPayDate.ToString("MM/dd/yyyy");
                    CompanyService cmp = new CompanyService();
                    Model.CompanyStore store = cmp.CompanyStores.Where(p => p.Id == objloan.ShopStoreId).FirstOrDefault();
                    if (store != null)
                    {
                        lblStoreAddressBankrupt.Text = store.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + store.PhoneNo + "<br/>" + store.Email;
                    }
                    if (objtloan.LoanStatus == "Consumer Proposal")
                    {
                        txtConsumerPartialPayment.Text = "";
                    }
                }
                if (objtloan.LoanStatus == "DEPT Management")
                {
                    loanopened.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanProcess.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverdue.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosed.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandenied.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollection.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptmanagement.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanbankrupt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblOverduereasondept.Text = objtloan.OverDueReason;
                    lblOverduecountDept.Text = objtloan.OverdueCount.ToString();
                    hdnishidecontractbutton.Value = "1";
                    rptLoan.Visible = false;
                    Model.CustomerLoan objloan = objtloan;

                    lblNSFchargeDept.Text = "$20.00";
                    lbllateIterestchargedept.Text = "$" + (objloan.LateInterestCharge == null ? 0 : Math.Round((decimal) objloan.LateInterestCharge,2)).ToString();
                    if (ConvertEasternTime(DateTime.Now).Date > objloan.NextPayDate.Date)
                    {
                        int days = (ConvertEasternTime(DateTime.Now).Date - objloan.NextPayDate.Date).Days;
                        double perdaycharge = (((double)objloan.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        lbllateIterestchargedept.Text = Math.Round(Convert.ToDecimal(totalcharge),2).ToString();
                    }
                    lblPArtialamountDept.Text = "$0.00";
                    List<Model.LoanPartialPayment> lstpartialpayment = cc.LoanPartialPaymentsnew.Where(p => p.LoanId == objloan.Id).ToList();

                    if (lstpartialpayment.Count > 0)
                    {
                        lblPArtialamountDept.Text = "";
                        foreach (Model.LoanPartialPayment obj in lstpartialpayment)
                        {
                            lblPArtialamountDept.Text += "Partial Payment on " + obj.CreatedDate.ToString("MM/dd/yyyy") + ":  <b>$" + obj.PartialAmount.ToString() + "</b><br/>";
                        }
                        //lblPArtialamountDept.Text = "$" + lstpartialpayment.Sum(p => p.PartialAmount).ToString();

                    }
                    lblDueAmountDept.Text = "$" + objloan.DueAmount.ToString();
                    lblTotaldueAmountDept.Text = "$" + Math.Round(((objloan.DueAmount + Convert.ToDecimal(lblNSFchargeDept.Text.Replace("$", "")) + Convert.ToDecimal(lbllateIterestchargedept.Text.Replace("$", ""))) - lstpartialpayment.Sum(p => p.PartialAmount)),2).ToString();
                    lblDuedateDept.Text = objloan.NextPayDate.ToString("MM/dd/yyyy");
                    CompanyService cmp = new CompanyService();
                    Model.CompanyStore store = cmp.CompanyStores.Where(p => p.Id == objloan.ShopStoreId).FirstOrDefault();
                    if (store != null)
                    {
                        lblSToreAddressDept.Text = store.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + store.PhoneNo + "<br/>" + store.Email;
                    }
                }
            }
            else if (e.CommandName == "CancelConsumer")
            {
                string[] arg = e.CommandArgument.ToString().Split(',');
                int LoanId = Convert.ToInt32(arg[1]);
                int Index = Convert.ToInt32(arg[0]);
                string val = ((TextBox)dgvConsumerProposal.Rows[Index].FindControl("txtModeofPayment")).Text.ToString();
                CustomerLoanService cc = new CustomerLoanService();
                List<Model.CustomerLoan> objcc = cc.CustomerLoans.Where(p => p.Id == LoanId).ToList();
                if (objcc.FirstOrDefault() != null)
                {
                    Model.CustomerLoan ln = objcc.FirstOrDefault();
                    ln.LoanStatus = "Cancelled";
                    ln.ModeofPayment = val;
                    ln.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cc.CustomerLoan_InsertOrUpdate(ln);
                }
                ResetForm();
            }
            else if (e.CommandName == "CancelBankrupt")
            {
                string[] arg = e.CommandArgument.ToString().Split(',');
                int LoanId = Convert.ToInt32(arg[1]);
                int Index = Convert.ToInt32(arg[0]);
                string val = ((TextBox)dgvLoanBankrupt.Rows[Index].FindControl("txtModeofPayment")).Text.ToString();
                CustomerLoanService cc = new CustomerLoanService();
                List<Model.CustomerLoan> objcc = cc.CustomerLoans.Where(p => p.Id == LoanId).ToList();
                if (objcc.FirstOrDefault() != null)
                {
                    Model.CustomerLoan ln = objcc.FirstOrDefault();
                    ln.LoanStatus = "Cancelled";
                    ln.ModeofPayment = val;
                    ln.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cc.CustomerLoan_InsertOrUpdate(ln);
                }
                ResetForm();
            }
        }

        protected void dgvLoanOverDue_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoanOverDue.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }
        protected void dgvLoanOverDue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                //FooterSumApproved += item.LoanAmountApproved;
                TotalOriginalDueAMount += item.DueAmount;
                totalBalanceDueAMount += item.DueAmount;

                TotalDueAmount += item.DueAmount;
                TotalPartialAmount += string.IsNullOrEmpty(item.PartialPayment) ? 0 : Convert.ToDecimal(item.PartialPayment.Replace("$", ""));
                TotalOverDueAmount += Convert.ToDecimal(item.DueAmount + (item.NSFCharge == null ? 0 : item.NSFCharge) + (item.LateInterestCharge == null ? 0 : item.LateInterestCharge));
                Label lbl = (Label)e.Row.FindControl("lblLateDueAmount");
                lbl.Text = "$" + item.DueAmount.ToString();
                Label lblTotalBalanceDue = (Label)e.Row.FindControl("lblTotalRemainingDueAmount");
                lblTotalBalanceDue.Text = "$" + item.DueAmount.ToString();
                if (ConvertEasternTime(DateTime.Now).Date > item.NextPayDate.Date)
                {
                    int days = (ConvertEasternTime(DateTime.Now).Date - item.NextPayDate.Date).Days;
                    double perdaycharge = (((double)item.DueAmount) * 0.32) / 365;
                    double totalcharge = days * perdaycharge;
                    //lblOverDueLAteInterestCharge.Text = Convert.ToDecimal(totalcharge).ToString();
                    decimal finalval = Convert.ToDecimal(item.DueAmount) + Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge));
                    lbl.Text = "$" + Math.Round(finalval, 2).ToString();
                    TotalDueAmount += Math.Round(Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge)), 2);

                    decimal partialamount = string.IsNullOrEmpty(item.PartialPayment) ? 0 : Convert.ToDecimal(item.PartialPayment.Replace("$", ""));
                    lblTotalBalanceDue.Text = "$" + Math.Round(finalval - partialamount, 2).ToString();
                    totalBalanceDueAMount += Math.Round(Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge)), 2) - partialamount;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotalDueAmount");
                lbl.Text = Math.Round(TotalOriginalDueAMount, 2).ToString();
                Label lblTotalOverDueLoanAmount = (Label)e.Row.FindControl("lblTotalOverDueLoanAmount");
                lblTotalOverDueLoanAmount.Text = Math.Round(TotalDueAmount, 2).ToString();
                Label lblTotalPartialAmountReceived = (Label)e.Row.FindControl("lblTotalPartialAmountReceived");
                lblTotalPartialAmountReceived.Text = TotalPartialAmount.ToString();
                Label lblTotalBalanceDue = (Label)e.Row.FindControl("lblTotalBalanceDue");
                lblTotalBalanceDue.Text = (Math.Round(TotalDueAmount, 2) - TotalPartialAmount).ToString();
            }
        }

        protected void dgvdeptmgmt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                //FooterSumApproved += item.LoanAmountApproved;
                TotalOriginalDueAMount += item.DueAmount;
                totalBalanceDueAMount += item.DueAmount;

                TotalDueAmount += item.DueAmount;
                TotalPartialAmount += string.IsNullOrEmpty(item.PartialPayment) ? 0 : Convert.ToDecimal(item.PartialPayment.Replace("$", ""));
                TotalOverDueAmount += Convert.ToDecimal(item.DueAmount + (item.NSFCharge == null ? 0 : item.NSFCharge) + (item.LateInterestCharge == null ? 0 : item.LateInterestCharge));
                Label lbl = (Label)e.Row.FindControl("lblDebtOverDueLateAmount");
                lbl.Text = "$" + item.DueAmount.ToString();
                Label lblTotalBalanceDue = (Label)e.Row.FindControl("lblTotalRemainingDueAmount");
                lblTotalBalanceDue.Text = "$" + item.DueAmount.ToString();
                if (ConvertEasternTime(DateTime.Now).Date > item.NextPayDate.Date)
                {
                    int days = (ConvertEasternTime(DateTime.Now).Date - item.NextPayDate.Date).Days;
                    double perdaycharge = (((double)item.DueAmount) * 0.32) / 365;
                    double totalcharge = days * perdaycharge;
                    //lblOverDueLAteInterestCharge.Text = Convert.ToDecimal(totalcharge).ToString();
                    decimal finalval = Convert.ToDecimal(item.DueAmount) + Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge));
                    lbl.Text = "$" + Math.Round(finalval, 2).ToString();
                    TotalDueAmount += Math.Round(Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge)), 2);

                    decimal partialamount = string.IsNullOrEmpty(item.PartialPayment) ? 0 : Convert.ToDecimal(item.PartialPayment.Replace("$", ""));
                    lblTotalBalanceDue.Text = "$" + Math.Round(finalval - partialamount, 2).ToString();
                    totalBalanceDueAMount += Math.Round(Convert.ToDecimal(totalcharge) + Convert.ToDecimal((item.NSFCharge == null ? 0 : item.NSFCharge)), 2) - partialamount;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotalDueAmount");
                lbl.Text = Math.Round(TotalOriginalDueAMount, 2).ToString();
                Label lblTotalOverDueLoanAmount = (Label)e.Row.FindControl("lblTotalOverDueLoanAmount");
                lblTotalOverDueLoanAmount.Text = Math.Round(TotalDueAmount, 2).ToString();
                Label lblTotalPartialAmountReceived = (Label)e.Row.FindControl("lblTotalPartialAmountReceived");
                lblTotalPartialAmountReceived.Text = TotalPartialAmount.ToString();
                Label lblTotalBalanceDue = (Label)e.Row.FindControl("lblTotalBalanceDue");
                lblTotalBalanceDue.Text = (Math.Round(TotalDueAmount, 2) - TotalPartialAmount).ToString();
            }
        }

        protected void dgvdeptmgmt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvdeptmgmt.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }

        protected void dgvLoanClose_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoanClose.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }
        protected void dgvLoanClose_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                //FooterSumApproved += item.LoanAmountApproved;
                TotalDueAmount += item.DueAmount;
                FooterSumApproved += item.LoanAmountApproved;
                TotalOverDueAmount += Convert.ToDecimal(item.DueAmount + (item.NSFCharge == null ? 0 : item.NSFCharge) + (item.LateInterestCharge == null ? 0 : item.LateInterestCharge));

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotalDueAmount");
                lbl.Text = TotalDueAmount.ToString();
                Label lblTotalOverDueLoanAmount = (Label)e.Row.FindControl("lblTotalOverDueLoanAmount");
                lblTotalOverDueLoanAmount.Text = TotalOverDueAmount.ToString();
                Label lblTotalApprovedAmount = (Label)e.Row.FindControl("lblTotalApprovedAmount");
                lblTotalApprovedAmount.Text = FooterSumApproved.ToString();

            }
        }
        protected void dgvLoanDue_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoanDue.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }
        protected void dgvLoanDue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                FooterSumApproved += item.LoanAmountApproved;
                TotalDueAmount += item.DueAmount;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotalApproved");
                lbl.Text = FooterSumApproved.ToString();
                Label lblTotalDue = (Label)e.Row.FindControl("lblTotalDueAmount");
                lblTotalDue.Text = TotalDueAmount.ToString();
            }
        }
        protected void dgvLoanDenied_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoanDenied.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }
        protected void dgvLoanDenied_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDenyReason = (Label)e.Row.FindControl("lblDenyReason");
                Model.CustomerLoan obj = (Model.CustomerLoan)e.Row.DataItem;
                LoanDenyReason objdn = cs.LoanDenyReasons.Where(p => p.LoanId == obj.Id).FirstOrDefault();
                lblDenyReason.Text = objdn == null ? "" : objdn.DenyReason;
            }
        }
        protected void dgvLoanInProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvLoanInProcess.PageIndex = e.NewPageIndex;
            BindLoanEntryData();
        }
        protected void dgvLoanInProcess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Row.DataItem;
                FooterSumApproved += item.LoanAmountApproved;
                TotalDueAmount += item.DueAmount;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotalDueAmount");
                lbl.Text = TotalDueAmount.ToString();
                Label lblTotalAmountApproved = (Label)e.Row.FindControl("lblTotalAmountApproved");
                lblTotalAmountApproved.Text = FooterSumApproved.ToString();

            }
        }
        #endregion
        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            mvView.ActiveViewIndex = 0;
        }

        public void BindLoanEntryData()
        {
            GetUserId();
            CustomerLoanService cs = new CustomerLoanService();
            HttpCookie UserStoreCookie = Request.Cookies["UserStoreId"];
            int shopstoreid = Convert.ToInt32(UserStoreCookie.Value);
            List<Model.CustomerLoan> lst = cs.CustomerReportGridData.Where(p => p.ShopStoreId == shopstoreid).ToList();

            if (ddlReportType.SelectedValue.ToString() != "0")
            {
                string status = ddlReportType.SelectedValue.ToString().ToLower();
                if (ddlReportType.SelectedValue != "Due")
                    lst = lst.Where(p => p.LoanStatus.ToLower() == status).ToList();
                if (ddlReportType.SelectedValue.ToString() == "Denied")
                {
                    lst = lst.GroupBy(x => x.CustomerId).Select(g => g.Last()).ToList();
                }
            }
            if (txtFromDate.Text != string.Empty)
            {
                if (ddlReportType.SelectedValue.ToString() == "Open" || ddlReportType.SelectedValue.ToString() == "Denied")
                {
                    lst = lst.Where(p => Convert.ToDateTime(p.CreatedDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.CreatedDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date).ToList();
                }
                else if (ddlReportType.SelectedValue.ToString() == "Due")
                {
                    //lst = lst.Where(p => Convert.ToDateTime(p.NextPayDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.NextPayDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date && p.LoanStatus != "Close" && p.LoanStatus != "Cancelled").ToList();
                    lst = lst.Where(p => Convert.ToDateTime(p.NextPayDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.NextPayDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date && p.LoanStatus == "Open").ToList();
                }
                else if (ddlReportType.SelectedValue.ToString() == "Close")
                {
                    lst = lst.Where(p => Convert.ToDateTime(p.UpdatedDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.UpdatedDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date && p.LoanStatus == "Close").ToList();
                }
                else if (ddlReportType.SelectedValue.ToString() == "Cancelled")
                {
                    lst = lst.Where(p => Convert.ToDateTime(p.UpdatedDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.UpdatedDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date && p.LoanStatus == "Cancelled").ToList();
                }
                else if (ddlReportType.SelectedValue.ToString() == "Legal")
                {
                    lst = lst.Where(p => Convert.ToDateTime(p.CreatedDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.CreatedDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date && p.LoanStatus == "Legal").ToList();
                }
                else
                {
                    lst = lst.Where(p => Convert.ToDateTime(p.CreatedDate).Date >= Convert.ToDateTime(txtFromDate.Text.ToLower()).Date && Convert.ToDateTime(p.CreatedDate).Date <= Convert.ToDateTime(txtToDate.Text.ToLower()).Date && p.LoanStatus != "Close" && p.LoanStatus != "Cancelled").ToList();
                }
            }
            foreach (Model.CustomerLoan cl in lst)
            {
                List<Model.LoanPartialPayment> lstpartialpayment = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cl.Id).ToList();

                if (lstpartialpayment.Count > 0)
                {
                    cl.PartialPayment = "$" + lstpartialpayment.Sum(p => p.PartialAmount).ToString();
                }
            }

            if (ddlReportType.SelectedValue.ToString() != "0")
            {
                if (ddlReportType.SelectedValue == "Open")
                {
                    dgvLoan.DataSource = lst;
                    dgvLoan.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Denied")
                {
                    dgvLoanDenied.DataSource = lst;
                    dgvLoanDenied.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Close")
                {
                    dgvLoanClose.DataSource = lst;
                    dgvLoanClose.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");

                }
                if (ddlReportType.SelectedValue == "Over Due")
                {
                    dgvLoanOverDue.DataSource = lst;
                    dgvLoanOverDue.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Payment in Process")
                {
                    dgvLoanInProcess.DataSource = lst;
                    dgvLoanInProcess.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Sent for Collection")
                {
                    dgvLoanSentforCollection.DataSource = lst;
                    dgvLoanSentforCollection.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Due")
                {
                    dgvLoanDue.DataSource = lst.Where(p => p.LoanStatus == "Open").ToList();
                    dgvLoanDue.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Consumer Proposal")
                {
                    dgvConsumerProposal.DataSource = lst;
                    dgvConsumerProposal.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Bankrupt")
                {
                    dgvLoanBankrupt.DataSource = lst;
                    dgvLoanBankrupt.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "DEPT Management")
                {
                    dgvdeptmgmt.DataSource = lst;
                    dgvdeptmgmt.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlReportType.SelectedValue == "Legal")
                {
                    dgvLegal.DataSource = lst;
                    dgvLegal.DataBind();
                    loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                    loanLegalgrid.Style.Add(HtmlTextWriterStyle.Display, "");
                }

            }
            else
            {
                loanopengrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loanoverduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loanclosegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loandeniedgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loaninprocessgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loansentforcollectiongrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loanduegrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loanConsumerProposalgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loanBankruptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");
                loandeptgrid.Style.Add(HtmlTextWriterStyle.Display, "none");

            }
            //mvView.ActiveViewIndex = 2;
        }

        private void ClearTextBoxes(ControlCollection cc)
        {
            foreach (Control ctrl in cc)
            {
                TextBox tb = ctrl as TextBox;
                if (tb != null)
                    tb.Text = "";
                DropDownList tb1 = ctrl as DropDownList;
                if (tb1 != null)
                    tb1.SelectedIndex = 0;
                else
                    ClearTextBoxes(ctrl.Controls);
            }
        }

        protected void btnOpenLoanSubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                cm.LoanStatus = rbtnlstLoanType.SelectedValue.ToString();
                cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                cm.ModeofPayment = txtModeofPayment.Text;
                cm.DiscountAmount = string.IsNullOrEmpty(txtopenDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtopenDiscount.Text);
                cm.RemainingAmount = cm.DueAmount - (string.IsNullOrEmpty(txtopenDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtopenDiscount.Text));
                cs.CustomerLoan_InsertOrUpdate(cm);
                ResetForm();
                if (cm.LoanStatus == "Close")
                {
                    string Url = "/LoanCloseReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprintclosefromopen", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        protected void btnBackBankrupt_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        public DateTime ConvertEasternTime(DateTime date)
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            //Set the time zone information to US Mountain Standard Time 
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //Get date and time in US Mountain Standard Time 
            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            //Print out the date and time
            //Console.WriteLine(dateTime.ToString("yyyy-MM-dd HH-mm-ss"));
            return dateTime;
        }
        protected void btnLoanprocessSubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {

                //open over due notice recipt form if over due is selected
                if (rbtnlstloanprocessloantype.SelectedValue == "Over Due")
                {
                    cm.LoanStatus = rbtnlstloanprocessloantype.SelectedValue.ToString();
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cm.OverDueReason = txtReasonforOverdue.Text;
                    //cm.ModeofPayment = txtReasonforOverdue.Text;
                    cm.OverdueCount = (cm.OverdueCount == null ? 0 : cm.OverdueCount) + 1;
                    cm.Settlement = txtProcessSettlement.Text;
                    cm.NSFCharge = 20;
                    cm.LateInterestCharge = 0;
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    //if (cm.OverdueCount >= 3)
                    //{
                    //    cm.LoanStatus = "Sent for Collection";
                    //}
                    cs.CustomerLoan_InsertOrUpdate(cm);

                    string Url = "/LoanOverDueReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprint1", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else
                {
                    cm.LoanStatus = rbtnlstloanprocessloantype.SelectedValue.ToString();
                    cm.LastStatus = "Payment in Process";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cm.DiscountAmount = string.IsNullOrEmpty(txtProcessDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtProcessDiscount.Text);
                    //cm.ModeofPayment = txtReasonforOverdue.Text;
                    cm.Settlement = txtProcessSettlement.Text;
                    cm.RemainingAmount = cm.DueAmount - cm.DiscountAmount;
                    cs.CustomerLoan_InsertOrUpdate(cm);

                    string Url = "/LoanCloseReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprintclose", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                ResetForm();
            }
        }
        public void ResetForm()
        {
            mvView.ActiveViewIndex = 0;
            ddlReportType.SelectedIndex = 0;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            btnSubmit_Click(null, null);
            //Response.Redirect("~/ViewReport.aspx");
        }
        protected void btnCancelLoanProcess_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void btnOverduesubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                GetUserId();

                //open over due notice recipt form if over due is selected
                if (hdnselectedstatus.Value == "Partial Payment Amount")
                {
                    GetUserId();
                    //cm.RemainingAmount = cm.RemainingAmount == 0 ? (cm.LoanAmountApproved - pp.PartialAmount) : (cm.RemainingAmount - pp.PartialAmount);
                    cm.LoanStatus = "Over Due";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cm.Settlement = txtOverdueSettlement.Text;
                    cm.NSFCharge = 20;
                    cm.DiscountAmount = string.IsNullOrEmpty(txtOverdueDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtOverdueDiscount.Text);
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);

                    LoanPartialPayment pp = new LoanPartialPayment();
                    pp.Createdby = this.UserId;
                    pp.CreatedDate = ConvertEasternTime(DateTime.Now);
                    pp.PartialAmount = Convert.ToDecimal(txtPartialpayment.Text);
                    pp.LoanId = Convert.ToInt32(hdnId.Value);
                    pp.IntrestCharge = cm.LateInterestCharge == null ? 0 : cm.LateInterestCharge;
                    pp.DueAmount = cm.DueAmount + pp.IntrestCharge + cm.NSFCharge;
                    pp.PartialPaymentMethod = txtPartialPaymentMethod.Text;
                    List<Model.LoanPartialPayment> lstpartialpayment = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cm.Id).ToList();
                    if (lstpartialpayment.Count > 0)
                    {
                        pp.DueAmount = pp.DueAmount - lstpartialpayment.Sum(p => p.PartialAmount);
                    }
                    cs.LoanPartialPayment_InsertOrUpdate(pp);

                    string Url = "/LoanPartialPaidReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprint12", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                //open over due notice recipt form if over due is selected
                else if (hdnselectedstatus.Value == "Loan Over Due")
                {
                    cm.LoanStatus = "Over Due";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cm.OverdueCount = (cm.OverdueCount == null ? 0 : cm.OverdueCount) + 1;
                    cm.Settlement = txtOverdueSettlement.Text;
                    cm.DiscountAmount = string.IsNullOrEmpty(txtOverdueDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtOverdueDiscount.Text);
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        cm.NSFCharge = 20;
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    //if (cm.OverdueCount >= 3)
                    //{
                    //    cm.LoanStatus = "Sent for Collection";
                    //}
                    cs.CustomerLoan_InsertOrUpdate(cm);

                    string Url = "/LoanOverDueReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprint1", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else
                {
                    cm.LoanStatus = "Close";
                    cm.LastStatus = "Over Due";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cm.Settlement = txtOverdueSettlement.Text;
                    cm.DiscountAmount = string.IsNullOrEmpty(txtOverdueDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtOverdueDiscount.Text);
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        cm.NSFCharge = 20;
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);
                    string Url = "/LoanCloseReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprintclose", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                ResetForm();
            }
        }

        protected void ButtonbtnCancelOversue_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void btnCloseBack_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        protected void btnUpdateReason_click(object sender, EventArgs e)
        {
            CashLoanShop.Model.LoanDenyReason obj = cs.LoanDenyReasons.ToList().Where(p => p.LoanId == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (obj == null) obj = new LoanDenyReason();
            if (obj != null)
            {
                obj.LoanId = Convert.ToInt32(hdnId.Value);
                obj.DenyReason = txtLoanDeniedreason.Text;
                cs.CustomerLoanReason_InsertOrUpdate(obj);
            }
            ResetForm();
        }

        protected void btnCollectionSubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                string Status = rbtnCollectionStatus.SelectedValue.ToString();
                if (Status == "Partial Payment Amount")
                {
                    GetUserId();
                    //cm.RemainingAmount = cm.RemainingAmount == 0 ? (cm.LoanAmountApproved - pp.PartialAmount) : (cm.RemainingAmount - pp.PartialAmount);
                    if (cm.LoanStatus != "Legal")
                    {
                        cm.LoanStatus = "Sent for Collection";
                    }
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    //cm.Settlement = txtOverdueSettlement.Text;
                    cm.NSFCharge = 20;
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);
                    LoanPartialPayment pp = new LoanPartialPayment();
                    pp.Createdby = this.UserId;
                    pp.CreatedDate = ConvertEasternTime(DateTime.Now);
                    pp.PartialAmount = Convert.ToDecimal(txtPartialloancollection.Text);
                    pp.LoanId = Convert.ToInt32(hdnId.Value);
                    pp.IntrestCharge = cm.LateInterestCharge == null ? 0 : cm.LateInterestCharge;
                    pp.DueAmount = cm.DueAmount + pp.IntrestCharge + cm.NSFCharge;
                    List<Model.LoanPartialPayment> lstpartialpayment = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cm.Id).ToList();
                    if (lstpartialpayment.Count > 0)
                    {
                        pp.DueAmount = pp.DueAmount - lstpartialpayment.Sum(p => p.PartialAmount);
                    }
                    cs.LoanPartialPayment_InsertOrUpdate(pp);

                    string Url = "/LoanPartialPaidReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprint12", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else if (Status == "Close")
                {
                    cm.LoanStatus = "Close";
                    if (cm.LoanStatus != "Legal")
                    {
                        cm.LastStatus = "Sent for Collection";
                    }
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    //cm.Settlement = txtOverdueSettlement.Text;
                    cm.DiscountAmount = string.IsNullOrEmpty(txtDiscountloancollection.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtDiscountloancollection.Text);
                    cm.NSFCharge = 20;
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);
                    string Url = "/LoanCloseReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprintclose", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else
                {
                    cm.LoanStatus = rbtnCollectionStatus.SelectedValue.ToString();
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    cs.CustomerLoan_InsertOrUpdate(cm);
                }

            }
            ResetForm();
        }

        protected void btnCollectionCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        protected void btnDeptCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        protected void btnDeptSubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                string Status = rbtnDeptStatus.SelectedValue.ToString();
                if (Status == "Partial Payment Amount")
                {
                    GetUserId();
                    //cm.RemainingAmount = cm.RemainingAmount == 0 ? (cm.LoanAmountApproved - pp.PartialAmount) : (cm.RemainingAmount - pp.PartialAmount);
                    cm.LoanStatus = "DEPT Management";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    //cm.Settlement = txtOverdueSettlement.Text;
                    cm.NSFCharge = 20;
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);
                    LoanPartialPayment pp = new LoanPartialPayment();
                    pp.Createdby = this.UserId;
                    pp.CreatedDate = ConvertEasternTime(DateTime.Now);
                    pp.PartialAmount = Convert.ToDecimal(txtPartialAmountDept.Text);
                    pp.LoanId = Convert.ToInt32(hdnId.Value);
                    pp.IntrestCharge = cm.LateInterestCharge == null ? 0 : cm.LateInterestCharge;
                    pp.DueAmount = cm.DueAmount + pp.IntrestCharge + cm.NSFCharge;
                    List<Model.LoanPartialPayment> lstpartialpayment = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cm.Id).ToList();
                    if (lstpartialpayment.Count > 0)
                    {
                        pp.DueAmount = pp.DueAmount - lstpartialpayment.Sum(p => p.PartialAmount);
                    }
                    cs.LoanPartialPayment_InsertOrUpdate(pp);

                    string Url = "/LoanPartialPaidReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprint12as", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else if (Status == "Close")
                {
                    cm.LoanStatus = "Close";
                    cm.LastStatus = "DEPT Management";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    //cm.Settlement = txtOverdueSettlement.Text;
                    cm.DiscountAmount = string.IsNullOrEmpty(txtDeptDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtDeptDiscount.Text);
                    cm.NSFCharge = 20;
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);
                    string Url = "/LoanCloseReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprintclose", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else
                {
                    cm.LoanStatus = rbtnDeptStatus.SelectedValue.ToString();
                    cm.LastStatus = "DEPT Management";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    //cm.Settlement = txtOverdueSettlement.Text;
                    cm.DiscountAmount = string.IsNullOrEmpty(txtDeptDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtDeptDiscount.Text);
                    cm.NSFCharge = 20;
                    cs.CustomerLoan_InsertOrUpdate(cm);
                }


            }
            ResetForm();
        }

        private void GetUserId()
        {
            HttpCookie myCookie = Request.Cookies["UserId"];
            if (myCookie == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                this.UserId = Convert.ToInt32(myCookie.Value);
            }
        }
        protected void btnConsumerSubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                string Status = rbtnConsumerPartialpayment.SelectedValue.ToString();
                if (Status == "Partial Payment Amount")
                {
                    GetUserId();
                    //cm.RemainingAmount = cm.RemainingAmount == 0 ? (cm.LoanAmountApproved - pp.PartialAmount) : (cm.RemainingAmount - pp.PartialAmount);
                    cm.LoanStatus = "Consumer Proposal";
                    cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    //cm.Settlement = txtOverdueSettlement.Text;
                    cm.NSFCharge = 20;
                    if (cm.UpdatedDate.Date > cm.NextPayDate.Date)
                    {
                        int days = (cm.UpdatedDate.Date - cm.NextPayDate.Date).Days;
                        double perdaycharge = (((double)cm.DueAmount) * 0.32) / 365;
                        double totalcharge = days * perdaycharge;
                        cm.LateInterestCharge = Convert.ToDecimal(totalcharge);
                    }
                    cs.CustomerLoan_InsertOrUpdate(cm);
                    LoanPartialPayment pp = new LoanPartialPayment();
                    pp.Createdby = this.UserId;
                    pp.CreatedDate = ConvertEasternTime(DateTime.Now);
                    pp.PartialAmount = Convert.ToDecimal(txtConsumerPartialPayment.Text);
                    pp.LoanId = Convert.ToInt32(hdnId.Value);
                    pp.IntrestCharge = cm.LateInterestCharge == null ? 0 : cm.LateInterestCharge;
                    pp.DueAmount = cm.DueAmount + pp.IntrestCharge + cm.NSFCharge;
                    List<Model.LoanPartialPayment> lstpartialpayment = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cm.Id).ToList();
                    if (lstpartialpayment.Count > 0)
                    {
                        pp.DueAmount = pp.DueAmount - lstpartialpayment.Sum(p => p.PartialAmount);
                    }
                    cs.LoanPartialPayment_InsertOrUpdate(pp);

                    string Url = "/LoanPartialPaidReceipt.aspx?Id=" + cm.Id.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "runprint12asas", " window.open('" + Url + "', '_blank', 'width=800,height=550,location=no,left=200px');", true);
                }
                else
                {
                    //cm.LoanStatus = rbtnDeptStatus.SelectedValue.ToString();
                    //cm.LastStatus = "DEPT Management";
                    //cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                    ////cm.Settlement = txtOverdueSettlement.Text;
                    //cm.DiscountAmount = string.IsNullOrEmpty(txtDeptDiscount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtDeptDiscount.Text);
                    //cm.NSFCharge = 45;
                    //cs.CustomerLoan_InsertOrUpdate(cm);
                }


            }
            ResetForm();
        }
        protected void btnoverduecollectionSubmit_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                cm.LoanStatus = rbtnlstCollectionOverdue.SelectedValue.ToString();
                cm.UpdatedDate = ConvertEasternTime(DateTime.Now);
                cs.CustomerLoan_InsertOrUpdate(cm);
            }
            ResetForm();
        }

        protected void btnoverduecollectionCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void dgvLoanBankrupt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void btnReOpenUser_Click(object sender, EventArgs e)
        {
            CashLoanShop.Model.CustomerLoan cm = cs.CustomerLoans.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm != null)
            {
                CustomerLoanService css = new CustomerLoanService();
                CustomerLoanException cl = css.CustomerLoanExceptions.Where(p => p.CustomerId == cm.CustomerId && p.LoanId == cm.Id).FirstOrDefault();
                if (cl == null)
                {
                    cl = new CustomerLoanException();
                    cl.CustomerId = cm.CustomerId;
                    cl.LoanId = Convert.ToInt32(cm.Id);
                    css.CustomerLoanException_InsertOrUpdate(cl);
                }
            }
            ResetForm();
        }

    }
}