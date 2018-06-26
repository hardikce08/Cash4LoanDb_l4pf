﻿using CashLoanShop.DataAccess;
using CashLoanShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CashLoanShop
{
    public partial class ViewTransactionReport : System.Web.UI.Page
    {
        public decimal TotalApprovedAmount, TotalApprovedAmountDenied, TotalCharges, TotalChargesDenied, TotalDue, TotalDueDenied, TotalApprovedAmountClose, TotalApprovedAmountCancelled, TotalChargesClose, TotalChargesCancelled, TotalDueClose, TotalDueCancelled, TotalRemainingClose, TotalRemainingCancelled, TotalApprovedAmountPartial, TotalChargesPartial, TotalDuePartial, TotalPartialPaymentSum;
        public decimal TotalChequeAmount, TotalCashChequeCharges, TotalCashOut;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["StoreId"]) && (Request.QueryString["ReportType"] == "1" || Request.QueryString["ReportType"] == "0"))
            {
                lblDateRange.Text = Request.QueryString["FromDate"] + " to " + Request.QueryString["ToDate"];
                CustomerLoanService cs = new CustomerLoanService();
                List<Model.CustomerLoan> lstLoandata = cs.CustomerLoanTransactionReportdata(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToDateTime(Request.QueryString["FromDate"]), Convert.ToDateTime(Request.QueryString["ToDate"]));

                //List<Model.CustomerLoan> filterdopengriddata = lstLoandata.Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date && (p.LoanStatus != "Close" && p.LoanStatus != "Cancelled" && p.LoanStatus!="Denied")).ToList();
                List<Model.CustomerLoan> filterdopengriddata = lstLoandata.Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date && p.LoanStatus != "Denied" && p.LoanStatus != "Cancelled").ToList();
                List<Model.CustomerLoan> filterddeniedgriddata = lstLoandata.Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date && p.LoanStatus == "Denied").ToList();
                List<Model.CustomerLoan> filterdclosegriddata = lstLoandata.Where(p => p.UpdatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.UpdatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date && (p.LoanStatus == "Close")).ToList();
                List<Model.CustomerLoan> filterdcancelledgriddata = lstLoandata.Where(p => p.UpdatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.UpdatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date && (p.LoanStatus == "Cancelled")).ToList();

                List<LoanPartialPayment> lst = cs.LoanPartialPaymentslist().Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date).ToList();
                if (lst.Count > 0)
                {
                    //List<int> lstloanid = lst.Select(p => p.LoanId).Distinct().ToList();
                    //List<Model.CustomerLoan> filterdpartialgriddata = cs.CustomerLoanTransactionReportdata1(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToDateTime(Request.QueryString["FromDate"]), Convert.ToDateTime(Request.QueryString["ToDate"]), lstloanid);
                    List<Model.CustomerLoan> filterdpartialgriddata = cs.CustomerLoanTransactionReportdata2(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToDateTime(Request.QueryString["FromDate"]), Convert.ToDateTime(Request.QueryString["ToDate"]));
                    //foreach (Model.CustomerLoan cl in filterdpartialgriddata)
                    //{
                    //    List<LoanPartialPayment> lstpaymetlst = cs.LoanPartialPaymentslist().Where(p => p.LoanId == cl.Id).ToList();
                    //    cl.PartialAmount = lstpaymetlst.Sum(p => p.PartialAmount);
                    //}


                    rptPartialPaid.DataSource = filterdpartialgriddata;
                    rptPartialPaid.DataBind();
                }
                //lstLoandata = lstLoandata.Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date).ToList();
                rptGridData.DataSource = filterdopengriddata;
                rptGridData.DataBind();
                foreach (Model.CustomerLoan cl in filterdclosegriddata)
                {
                    List<LoanPartialPayment> lstpaymetlst = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cl.Id).ToList();
                    cl.PartialAmount = lstpaymetlst.Sum(p => p.PartialAmount);
                    cl.RemainingAmount = (cl.NSFCharge == null || cl.NSFCharge == 0) ? 0 : (cl.DueAmount + cl.NSFCharge + cl.LateInterestCharge - (cl.PartialAmount + cl.DiscountAmount));
                }
                rptClose.DataSource = filterdclosegriddata;
                rptClose.DataBind();

                foreach (Model.CustomerLoan cl in filterdcancelledgriddata)
                {
                    List<LoanPartialPayment> lstpaymetlst = cs.LoanPartialPaymentsnew.Where(p => p.LoanId == cl.Id).ToList();
                    cl.PartialAmount = lstpaymetlst.Sum(p => p.PartialAmount);
                    cl.RemainingAmount = (cl.NSFCharge == null || cl.NSFCharge == 0) ? 0 : (cl.DueAmount + cl.NSFCharge + cl.LateInterestCharge - (cl.PartialAmount + cl.DiscountAmount));
                }
                rptCancelled.DataSource = filterdcancelledgriddata;
                rptCancelled.DataBind();


                rptDenied.DataSource = filterddeniedgriddata;
                rptDenied.DataBind();


            }
            if (!string.IsNullOrEmpty(Request.QueryString["StoreId"]) && (Request.QueryString["ReportType"] == "2" || Request.QueryString["ReportType"] == "0")) //cash cheque Report
            {
                lblDateRangeCheque.Text = Request.QueryString["FromDate"] + " to " + Request.QueryString["ToDate"];
                CashChequeService cs = new CashChequeService();
                List<Model.CashCheque> lstCashchequedata = cs.CashChequeReportGriddata(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToDateTime(Request.QueryString["FromDate"]), Convert.ToDateTime(Request.QueryString["ToDate"]));
                rptCashChequeData.DataSource = lstCashchequedata.Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date).ToList();
                rptCashChequeData.DataBind();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["StoreId"]) && (Request.QueryString["ReportType"] == "3" || Request.QueryString["ReportType"] == "0")) //Currency Exchange Report
            {
                lblCurrencyDate.Text = Request.QueryString["FromDate"] + " to " + Request.QueryString["ToDate"];
                CurrencyExchangeService cs = new CurrencyExchangeService();
                List<Model.CurrencyExchange> lstCurrencyData = cs.CurrencyExchangesReportdata(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToDateTime(Request.QueryString["FromDate"]), Convert.ToDateTime(Request.QueryString["ToDate"]));
                rptCurrency.DataSource = lstCurrencyData.Where(p => p.CreatedDate.Date >= Convert.ToDateTime(Request.QueryString["FromDate"]).Date && p.CreatedDate.Date <= Convert.ToDateTime(Request.QueryString["ToDate"]).Date).ToList();
                rptCurrency.DataBind();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["StoreId"])) //Currency Exchange Report
            {
                CompanyService cmp = new CompanyService();
                Model.CompanyStore CompanyStores = cmp.CompanyStores.ToList().Where(p => p.Id == Convert.ToInt32(Request.QueryString["StoreId"])).FirstOrDefault();
                if (CompanyStores != null)
                {
                    lblStoreInfo.Text = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.Email;
                    lblCashStoreInfo.Text = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.Email;
                    lblCurrencyStoreInfo.Text = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.Email;
                    lblTransactionType.Text = CompanyStores.Businessname;
                }
            }

            if (Request.QueryString["ReportType"] == "0")
            {
                printarea.Style.Add(HtmlTextWriterStyle.Display, "");
                printareacurrency.Style.Add(HtmlTextWriterStyle.Display, "");
                printareacashcheque.Style.Add(HtmlTextWriterStyle.Display, "");
                btnmasterprintarea.Visible = true;
                btnprintarea.Visible = false;
                btnprintareacurrency.Visible = false;
                btnprintareacashcheque.Visible = false;
            }
            if (Request.QueryString["ReportType"] == "1")
            {
                printarea.Style.Add(HtmlTextWriterStyle.Display, "");
                printareacurrency.Style.Add(HtmlTextWriterStyle.Display, "none");
                printareacashcheque.Style.Add(HtmlTextWriterStyle.Display, "none");
                btnprintarea.Visible = true;
                btnmasterprintarea.Visible = false;
            }
            if (Request.QueryString["ReportType"] == "2")
            {
                printarea.Style.Add(HtmlTextWriterStyle.Display, "none");
                printareacurrency.Style.Add(HtmlTextWriterStyle.Display, "none");
                printareacashcheque.Style.Add(HtmlTextWriterStyle.Display, "");
                btnprintareacashcheque.Visible = true;
                btnmasterprintarea.Visible = false;
            }
            if (Request.QueryString["ReportType"] == "3")
            {
                printarea.Style.Add(HtmlTextWriterStyle.Display, "none");
                printareacurrency.Style.Add(HtmlTextWriterStyle.Display, "");
                printareacashcheque.Style.Add(HtmlTextWriterStyle.Display, "none");
                btnprintareacurrency.Visible = true;
                btnmasterprintarea.Visible = false;
            }
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

        protected void rptGridData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Item.DataItem;
                TotalApprovedAmount += item.LoanAmountApproved;
                TotalDue += item.DueAmount;
                TotalCharges += item.AdminFee;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
        }
        protected void rptDenied_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Item.DataItem;
                TotalApprovedAmountDenied += item.LoanAmountApproved;
                TotalDueDenied += item.DueAmount;
                TotalChargesDenied += item.AdminFee;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
        }
        protected void rptClose_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Item.DataItem;
                TotalApprovedAmountClose += item.LoanAmountApproved;
                TotalDueClose += item.DueAmount;
                TotalChargesClose += item.AdminFee;
                TotalRemainingClose += Convert.ToDecimal(item.RemainingAmount);
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
        }
        protected void rptCancelled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Item.DataItem;
                TotalApprovedAmountCancelled += item.LoanAmountApproved;
                TotalDueCancelled += item.DueAmount;
                TotalChargesCancelled += item.AdminFee;
                TotalRemainingCancelled += Convert.ToDecimal(item.RemainingAmount);
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
        }
        protected void rptPartialPaid_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Model.CustomerLoan item = (Model.CustomerLoan)e.Item.DataItem;
                TotalApprovedAmountPartial += item.LoanAmountApproved;
                TotalDuePartial += item.LoanAmountApproved + Convert.ToDecimal(item.AdminFee + (item.NSFCharge == null ? 0 : item.NSFCharge) + (item.LateInterestCharge == null ? 0 : item.LateInterestCharge));
                TotalChargesPartial += Convert.ToDecimal(item.AdminFee + (item.NSFCharge == null ? 0 : item.NSFCharge) + (item.LateInterestCharge == null ? 0 : item.LateInterestCharge));
                TotalPartialPaymentSum += item.PartialAmount;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
        }
        protected void rptCashChequeData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Model.CashCheque item = (Model.CashCheque)e.Item.DataItem;
                TotalCashChequeCharges += item.Charges + 2;
                TotalCashOut += item.AmountIssued;
                TotalChequeAmount += item.ChequeAmount;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
        }
    }
}