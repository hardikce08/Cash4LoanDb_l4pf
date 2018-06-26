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
    public partial class LoanCloseReceipt : System.Web.UI.Page
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
                        //LoanPartialPayment objpp = cc.LoanPartialPaymentsnew.Where(p => p.LoanId == objcc.Id).ToList().OrderByDescending(t => t.Id).ToList().FirstOrDefault();
                        CustomerService cs = new CustomerService();
                        CustomerMaster cm = cs.CustomerMasters.ToList().Where(p => p.Id == objcc.CustomerId).FirstOrDefault();

                        CompanyService cmp = new CompanyService();
                        Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p => p.Id == objcc.ShopStoreId).FirstOrDefault();
                        if (CompanyStores != null)
                        {
                            lblStoreInfo.Text = CompanyStores.Name + "<br/>" + CompanyStores.Address.Replace(",", "<br/>").Replace("$", " , ") + "<br/>" + CompanyStores.Email;
                            lblTransactionType.Text = CompanyStores.Businessname;
                        }
                        //decimal Partialamountpaid = objpp.PartialAmount;
                        lblCustomerName.Text = cm.FirstName + " " + cm.LastName;
                        lblDateTime.Text = Convert.ToDateTime(objcc.UpdatedDate).ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                      
                        lblReceiptNumber.Text = objcc.Id.ToString();
                        lblLoanAmount.Text = "$" + objcc.LoanAmountApproved.ToString();
                        lblLastDueAmount.Text = "$0.00";
                        lblAdminFee.Text = "$" + objcc.AdminFee.ToString();
                        lblDueAmount.Text = "$" + objcc.DueAmount.ToString();
                        lblDueDate.Text = Convert.ToDateTime(objcc.NextPayDate).ToString("MM/dd/yyyy").Replace("-", "/");
                        lblLateInterestCharges.Text = "$" + (objcc.LateInterestCharge == null ? "0.00" : objcc.LateInterestCharge.ToString());
                        lblNSFCharges.Text = "$" + (objcc.NSFCharge == null ? "0.00" : objcc.NSFCharge.ToString());
                        lblTotalDueAmount.Text = "$" + (objcc.DueAmount + (objcc.LateInterestCharge == null ? 0 : objcc.LateInterestCharge) + (objcc.NSFCharge == null ? 0 : objcc.NSFCharge)).ToString();
                        lblDiscount.Text = "$" + objcc.DiscountAmount.ToString();
                        lblCashpaid.Text = "$" + ((objcc.DueAmount + (objcc.LateInterestCharge == null ? 0 : objcc.LateInterestCharge) + (objcc.NSFCharge == null ? 0 : objcc.NSFCharge)) - objcc.DiscountAmount).ToString();

                        objcc.RemainingAmount = 0;

                        cc.CustomerLoan_InsertOrUpdate(objcc);
                        //CompanyService cmp = new CompanyService();
                        //Model.CompanyStore CompanyStores = cmp.CompanyStores.Where(p=>p.Id==objcc.ShopStoreId).FirstOrDefault();
                        //if (CompanyStores != null)
                        //{
                        //    lblAddress.Text = CompanyStores.Address;
                        //}

                    }
                }
            }
        }
    }
}