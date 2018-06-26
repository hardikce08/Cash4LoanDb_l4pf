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
    public partial class CustomerLoanContract : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int CustomerLoanId = Convert.ToInt32(Request.QueryString["Id"]);
                    hdnLoanId.Value = CustomerLoanId.ToString();
                    CustomerLoanService cc = new CustomerLoanService();
                    Model.CustomerLoan objcc = cc.CustomerLoans.Where(p => p.Id == CustomerLoanId).FirstOrDefault();
                    if (objcc != null)
                    {
                        CustomerService cs = new CustomerService();
                        CustomerMaster cm = cs.CustomerMasters.Where(p => p.Id == objcc.CustomerId).FirstOrDefault();
                        cm.ProvinceName = GetProvince(Convert.ToInt32(cm.Province));
                        int DayDiff = Convert.ToDateTime(objcc.NextPayDate).Date.Subtract(objcc.CreatedDate.Date).Days;
                        string MailTemplate = System.IO.File.ReadAllText(Server.MapPath("~/PAYDAYLOANMARTCONTRACTblank.html"));
                        CompanyService cmp = new CompanyService();
                        Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == objcc.ShopStoreId).FirstOrDefault();
                        if (CompanyStores != null)
                        {
                            MailTemplate = MailTemplate.Replace("@storeaddress", CompanyStores.Name + " " + CompanyStores.Businessname + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>Phone:" + CompanyStores.PhoneNo.Split(',')[0] + ", Fax:" + CompanyStores.PhoneNo.Split(',')[1] + "<br/> Email:" + CompanyStores.Email);
                            MailTemplate = MailTemplate.Replace("@storesubaddress", CompanyStores.Address.Split(',')[0].ToString().Replace("$", " , "));
                            MailTemplate = MailTemplate.Replace("@storename", CompanyStores.Name.Replace("<br/>", " O/A "));
                            MailTemplate = MailTemplate.Replace("@storecity", CompanyStores.City);
                        }

                        MailTemplate = MailTemplate.Replace("@customername", cm.FirstName + " "+ cm.Mi + " " + cm.LastName);
                        MailTemplate = MailTemplate.Replace("@customerid", cm.Id.ToString());
                        MailTemplate = MailTemplate.Replace("@days", DayDiff.ToString());
                        MailTemplate = MailTemplate.Replace("@borrowedamount", objcc.LoanAmountApproved.ToString());
                        MailTemplate = MailTemplate.Replace("@costofborrowing", objcc.AdminFee.ToString());
                        MailTemplate = MailTemplate.Replace("@totaldueamount", objcc.DueAmount.ToString());
                        MailTemplate = MailTemplate.Replace("@duedate", objcc.NextPayDate.ToString("dddd, MMMM d, yyyy"));
                        MailTemplate = MailTemplate.Replace("@currentdate", ConvertEasternTime(DateTime.Now).ToString("dddd, MMMM d, yyyy"));
                        MailTemplate = MailTemplate.Replace("@address", cm.Address + "<br/>" + cm.City + "," + cm.ProvinceName + "<br/>" + cm.PostCode);
                        MailTemplate = MailTemplate.Replace("@phone", cm.CellPhone);
                        MailTemplate = MailTemplate.Replace("@workphone", cm.WorkPhone);
                        if (objcc.ShopStoreId == 1)
                        {
                            MailTemplate = MailTemplate.Replace("@faxnumber", "416-326-8810");
                        }
                        else {
                            MailTemplate = MailTemplate.Replace("@faxnumber", "416-326-8665");
                        }
                        

                        CustomerBankInformation cb = cs.CustomerBankInformations.ToList().Where(p => p.CustomerId == cm.Id).FirstOrDefault();
                        if (cb != null)
                        {
                            MailTemplate = MailTemplate.Replace("@bankaccountnumber", cb.AccountNumber);
                            MailTemplate = MailTemplate.Replace("@institutionnumber", cb.InstitutionNo);
                            MailTemplate = MailTemplate.Replace("@banktansitnumber", cb.TransitNo);
                            MailTemplate = MailTemplate.Replace("@bankname", cb.BankName);
                            MailTemplate = MailTemplate.Replace("@bankaddress", cb.Address);
                            MailTemplate = MailTemplate.Replace("@bankcitystate", (cb.City == string.Empty ? "" : "<u>" + cb.City + "</u>") + " , " + (cb.Province == "Select Province" ? "" : "<u>" + cb.Province + "</u>") + " , " + (cb.PostCode == string.Empty ? "" : "<u>" + cb.PostCode + "</u>"));
                        }
                        content.InnerHtml = MailTemplate.ToString();
                    }
                }
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
    }
}