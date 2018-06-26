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
    public partial class CustomerCashHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    CustomerService cs = new CustomerService();
                    List<CustomerMaster> cm = cs.CustomerMasters.ToList().Where(p => p.Id == Convert.ToInt32(Request.QueryString["Id"])).ToList();
                    cm.FirstOrDefault().Province = GetProvince(Convert.ToInt32(cm.FirstOrDefault().Province));
                    rptCustomer.DataSource = cm;
                    rptCustomer.DataBind();

                    CashChequeService cc = new CashChequeService();
                    List<CashCheque> lsthistory = cc.CashChequeGriddata.ToList().Where(p => p.CustomerId == Convert.ToInt32(Request.QueryString["Id"])).ToList();
                    foreach (var CashCheque in lsthistory)
                    {
                        CompanyService cmp = new CompanyService();
                        Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == CashCheque.ShopStoreId).FirstOrDefault();
                        if (CompanyStores != null)
                        {
                            CashCheque.CompanyStoreAddres = CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.PhoneNo + "<br/>" + CompanyStores.Email;
                        }

                    }
                    rptGridData.DataSource = lsthistory;
                    rptGridData.DataBind();

                    if (Request.QueryString["IsCurrencyExchange"] != null)
                    {
                        currencydiv.Style.Add(HtmlTextWriterStyle.Display, "");
                        CurrencyExchangeService cec = new CurrencyExchangeService();
                        List<CurrencyExchange> lstcurrency = cec.CurrencyExchangesGriddata.ToList().Where(p => p.CustomerId == Convert.ToInt32(Request.QueryString["Id"])).ToList();
                        foreach (CurrencyExchange ce in lstcurrency)
                        {
                            CompanyService cmp = new CompanyService();
                            Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == ce.ShopStoreId).FirstOrDefault();
                            if (CompanyStores != null)
                            {
                                ce.StoreAddress = CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.PhoneNo + "<br/>" + CompanyStores.Email;
                            }
                        }
                        rptCurrencyExchange.DataSource = lstcurrency;
                        rptCurrencyExchange.DataBind();
                    }
                    else
                    {
                        currencydiv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }

                    if (Request.QueryString["IsLoan"] != null)
                    {
                        loandiv.Style.Add(HtmlTextWriterStyle.Display, "");
                        CustomerLoanService cls = new CustomerLoanService();
                        List<Model.CustomerLoan> lstloan = cls.CustomerLoanCashHistoryData.ToList().Where(p => p.CustomerId == Convert.ToInt32(Request.QueryString["Id"])).OrderBy(p => p.CreatedDate).ToList();
                        //if new store is view this history then Hide open loan from first store 
                        HttpCookie myCookie = Request.Cookies["UserId"];
                        if (myCookie != null)
                        {
                            // CustomerService css = new CustomerService();
                            // int? storeid = css.Users.ToList().Where(p => p.Id == Convert.ToInt32(Session["UserId"])).FirstOrDefault().StoreId;
                            HttpCookie UserStoreCookie = Request.Cookies["UserStoreId"];
                            if (Convert.ToInt32(UserStoreCookie.Value) > 1)
                            {
                                if (lstloan.Where(p => p.LoanStatus != "Denied" && p.ShopStoreId == Convert.ToInt32(UserStoreCookie.Value)).Count() == 0)
                                {
                                    //if User have opened loan from other store which is not closed yet any loan then do not show anything
                                    if (lstloan.Where(p => p.LoanStatus != "Close" && p.LoanStatus != "Denied" && p.ShopStoreId != Convert.ToInt32(UserStoreCookie.Value)).Count() > 0)
                                    {
                                        lstloan = new List<Model.CustomerLoan>();
                                        divempty.Style.Add(HtmlTextWriterStyle.Display, "");
                                    }
                                    else
                                    {
                                        divempty.Style.Add(HtmlTextWriterStyle.Display, "none");
                                    }
                                }
                                else
                                {
                                    divempty.Style.Add(HtmlTextWriterStyle.Display, "none");
                                }
                                //lstloan.RemoveAll(p => p.LoanStatus != "Close" && p.LoanStatus != "Denied" && p.ShopStoreId != Convert.ToInt32(Session["UserStoreId"]));
                            }
                            else
                            {
                                //If use logged in from old store then only show its loan only
                                //HttpCookie UserStoreCookie = Request.Cookies["UserStoreId"];
                                lstloan = lstloan.Where(p => p.ShopStoreId == Convert.ToInt32(UserStoreCookie.Value)).ToList();
                                divempty.Style.Add(HtmlTextWriterStyle.Display, "none");
                            }
                        }
                        foreach (Model.CustomerLoan cl in lstloan)
                        {
                            CustomerService ccs = new CustomerService();
                            CustomerIncome ci = ccs.CutomerIncomes.ToList().Where(p => p.CustomerId == cl.CustomerId).FirstOrDefault();
                            if (ci == null)
                            {
                                cl.LoanType = "";
                            }
                            else
                            {
                                cl.LoanType = ci.FrequencyofPay == "0" ? "" : ((LoanType)Convert.ToInt32(ci.FrequencyofPay)).ToString().Replace("_", " ");
                            }
                            List<Model.LoanPartialPayment> lstpartialpayment = cls.LoanPartialPaymentsnew.Where(p => p.LoanId == cl.Id).ToList();

                            if (lstpartialpayment.Count > 0)
                            {
                                foreach (Model.LoanPartialPayment obj in lstpartialpayment)
                                {
                                    cl.PartialPayment += "Partial Payment on " + obj.CreatedDate.ToString("MM/dd/yyyy") + ":  <b>$" + obj.PartialAmount.ToString() + (string.IsNullOrEmpty(obj.PartialPaymentMethod) ? "" : (" $$ " + obj.PartialPaymentMethod)) + "</b><br/>";
                                }

                            }


                            CompanyService cmp = new CompanyService();
                            Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == cl.ShopStoreId).FirstOrDefault();
                            if (CompanyStores != null)
                            {
                                cl.StoreAddress = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.PhoneNo + "<br/>" + CompanyStores.Email;
                                lblBusinessName.Text = CompanyStores.Businessname;
                            }
                            if (ConvertEasternTime(DateTime.Now).Date > cl.NextPayDate.Date)
                            {
                                int days = (ConvertEasternTime(DateTime.Now).Date - cl.NextPayDate.Date).Days;
                                double perdaycharge = (((double)cl.DueAmount) * 0.32) / 365;
                                double totalcharge = days * perdaycharge;
                                //lblOverDueLAteInterestCharge.Text = Convert.ToDecimal(totalcharge).ToString();
                                decimal finalval = Convert.ToDecimal(cl.DueAmount) + Convert.ToDecimal(totalcharge) + Convert.ToDecimal((cl.NSFCharge == null ? 20 : cl.NSFCharge));
                                cl.RemainingAmount = Math.Round(finalval, 2);
                            }
                            else
                            {
                                cl.RemainingAmount = cl.DueAmount;
                            }

                        }
                        rptLoan.DataSource = lstloan;
                        rptLoan.DataBind();
                    }
                    else
                    {
                        loandiv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
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
    }
}