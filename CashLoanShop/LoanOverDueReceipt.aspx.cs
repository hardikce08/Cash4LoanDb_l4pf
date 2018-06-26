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
    public partial class LoanOverDueReceipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int CustomerLoanId = Convert.ToInt32(Request.QueryString["Id"]);
                    CustomerLoanService cc = new CustomerLoanService();
                    Model.CustomerLoan objcc = cc.CustomerLoans.ToList().Where(p => p.Id == CustomerLoanId).FirstOrDefault();
                    if (objcc != null)
                    {
                        CustomerService cs = new CustomerService();
                        CustomerMaster cm = cs.CustomerMasters.ToList().Where(p => p.Id == objcc.CustomerId).FirstOrDefault();


                        int DayDiff = Convert.ToDateTime(objcc.NextPayDate).Subtract(objcc.CreatedDate).Days;
                        string MailTemplate = System.IO.File.ReadAllText(Server.MapPath("~/OverDueReceipt.html"));
                        CompanyService cmp = new CompanyService();
                        Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == objcc.ShopStoreId).FirstOrDefault();
                        if (CompanyStores != null)
                        {
                            MailTemplate = MailTemplate.Replace("@storeaddress", CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.Email);
                        }
                        List<Model.LoanPartialPayment> lstpartialpayment = cc.LoanPartialPaymentsnew.Where(p => p.LoanId == objcc.Id).ToList();
                        if (lstpartialpayment.Count > 0)
                        {
                            MailTemplate = MailTemplate.Replace("@PartialAmount", "$" + lstpartialpayment.Sum(p => p.PartialAmount).ToString());
                        }
                        else
                        {
                            MailTemplate = MailTemplate.Replace("@PartialAmount", "$0.00");
                        }
                        MailTemplate = MailTemplate.Replace("@customername", cm.FirstName + " " + cm.Mi + " " + cm.LastName);
                        MailTemplate = MailTemplate.Replace("@currentdate", objcc.UpdatedDate.ToString("dddd, MMMM d, yyyy"));
                        MailTemplate = MailTemplate.Replace("@address", cm.Address + "<br/>" + cm.City + "," + cm.PostCode);
                        MailTemplate = MailTemplate.Replace("@loanamount", "$" + objcc.DueAmount.ToString());
                        MailTemplate = MailTemplate.Replace("@lateinterestcharge", "$" + (objcc.LateInterestCharge == null ? 0 : objcc.LateInterestCharge).ToString());
                        MailTemplate = MailTemplate.Replace("@NSFCharge", "$" + (objcc.NSFCharge == null ? 0 : objcc.NSFCharge).ToString());
                        MailTemplate = MailTemplate.Replace("@totaldueamount", ((objcc.DueAmount + (objcc.NSFCharge == null ? 0 : objcc.NSFCharge) + (objcc.LateInterestCharge == null ? 0 : objcc.LateInterestCharge)) - objcc.DiscountAmount - (lstpartialpayment.Count > 0 ? lstpartialpayment.Sum(p => p.PartialAmount) : 0)).ToString());
                        MailTemplate = MailTemplate.Replace("@DiscAmount", "$" + objcc.DiscountAmount.ToString());


                        content.InnerHtml = MailTemplate.ToString();
                    }
                }
            }
        }
    }
}