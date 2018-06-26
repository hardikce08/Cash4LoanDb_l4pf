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
    public partial class ChequeCashReceipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int CashChequeId = Convert.ToInt32(Request.QueryString["Id"]);
                    CashChequeService cc = new CashChequeService();
                    CashCheque objcc = cc.CashCheques.ToList().Where(p => p.Id == CashChequeId).FirstOrDefault();
                    if (objcc != null)
                    {
                        CustomerService cs = new CustomerService();
                        CustomerMaster cm = cs.CustomerMasters.ToList().Where(p => p.Id == objcc.CustomerId).FirstOrDefault();
                        lblCustomerName.Text = cm.FirstName + " " + cm.LastName;
                        lblCustomerId.Text = cm.Id.ToString();
                        lblDateTime.Text = Convert.ToDateTime(objcc.CreatedDate).ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                        lblChequeType.Text = objcc.ChequeType == "Custom" ? objcc.ChequeType + " - " + objcc.CustomPercentage.ToString()+" % " : objcc.ChequeType;
                        lblReceiptNumber.Text = objcc.Id.ToString();
                        lblChqAmount.Text = "$" + objcc.ChequeAmount.ToString();
                        lblCharges.Text = "$" + objcc.Charges.ToString();
                        lblCashout.Text = "$" + objcc.AmountIssued.ToString();
                        lblAdminFee.Text = "$" + objcc.AdminFee.ToString();
                        CompanyService cmp = new CompanyService();
                        Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == objcc.ShopStoreId).FirstOrDefault();
                        if (CompanyStores != null)
                        {
                            lblStoreInfo.Text = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.PhoneNo + "<br/>" + CompanyStores.Email;
                        }

                    }
                }
            }
        }
    }
}