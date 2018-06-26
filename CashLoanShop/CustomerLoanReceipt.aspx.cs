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
    public partial class CustomerLoanReceipt : System.Web.UI.Page
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
                        lblCustomerName.Text = cm.FirstName + " " + cm.LastName;
                        lblDateTime.Text = Convert.ToDateTime(objcc.CreatedDate).ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                       
                        lblReceiptNumber.Text = objcc.Id.ToString();
                        lblLoanAmount.Text = "$" + objcc.LoanAmountApproved.ToString();
                        lblCashout.Text = "$" + objcc.LoanAmountApproved.ToString();
                        lblAdminFee.Text = "$" + objcc.AdminFee.ToString();
                        lblDueAmount.Text = "$" + objcc.DueAmount.ToString();
                        lblDueDate.Text = Convert.ToDateTime(objcc.NextPayDate).ToString("MM/dd/yyyy").Replace("-", "/");
                        CompanyService cmp = new CompanyService();
                        Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == objcc.ShopStoreId).FirstOrDefault();
                        if (CompanyStores != null)
                        {
                            lblStoreInfo.Text = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.PhoneNo + "<br/>" + CompanyStores.Email;
                            lblTransactionType.Text = CompanyStores.Businessname;
                        }

                    }
                }
            }
        }
    }
}