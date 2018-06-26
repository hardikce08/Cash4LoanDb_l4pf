using CashLoanShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CashLoanShop.Admin
{
    public partial class UserManager : System.Web.UI.Page
    {
        CustomerService cs = new CustomerService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindGrid();
                BindCombo();
            }
        }
        public void BindCombo()
        {
            CompanyService cs = new CompanyService();
            List<Model.CompanyStore> lstCompanyStores = cs.CompanyStores.ToList();
            if (lstCompanyStores.Count > 0)
            {
                foreach (var item in lstCompanyStores)
                {
                    item.Name = item.Name.Replace("<br/>", " - ");
                }
            }
            ddlShopStore.DataSource = lstCompanyStores;
            ddlShopStore.DataTextField = "Name";
            ddlShopStore.DataValueField = "Id";
            //lstCompanyStores.Insert(0, new Model.CompanyStore { Id = 0, Address = "--Select--" });
            ddlShopStore.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            CashLoanShop.Model.User cm = cs.Users.ToList().Where(p => p.Id == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
            if (cm == null)
            {
                cm = new CashLoanShop.Model.User();
                cm.CreatedDate = DateTime.UtcNow;
            }
            cm.UserName = txtUserName.Text;
            cm.Password = txtPassword.Text;
            cm.UserType =Convert.ToInt32(ddlUserType.SelectedValue);
            cm.PasswordHash = MD5Hash(txtPassword.Text.ToLower());
            cm.StoreId = Convert.ToInt32(ddlShopStore.SelectedValue);
            cs.User_InsertOrUpdate(cm);
            BindGrid();
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            mvView.ActiveViewIndex = 1;
            hdnId.Value = "0";
            txtPassword.Text = "";
            txtUserName.Text = "";
            ddlUserType.SelectedValue = "1";
            ddlShopStore.SelectedIndex = 0;
        }

        private void BindGrid()
        {
            CustomerService cs = new CustomerService();
            List<CashLoanShop.Model.UserStoreDetails> lst = cs.UserStoreDetail.ToList();
         
            dgvCustomer.DataSource = lst;
            dgvCustomer.DataBind();
            mvView.ActiveViewIndex = 0;
        }
        protected void dgvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int Userid = Convert.ToInt32(e.CommandArgument);
                CashLoanShop.Model.User cm = cs.Users.ToList().Where(p => p.Id == Convert.ToInt32(Userid)).FirstOrDefault();
              
                hdnId.Value = Userid.ToString();
                mvView.ActiveViewIndex = 1;
                if (cm != null)
                {
                    txtUserName.Text = cm.UserName;
                    txtPassword.Text = cm.Password;
                    ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(cm.UserType.ToString()));
                    ddlShopStore.SelectedIndex = ddlShopStore.Items.IndexOf(ddlShopStore.Items.FindByValue(cm.StoreId.ToString()));
                }

            }
        }
        protected void dgvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvCustomer.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void dgvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvView.ActiveViewIndex = 0;
            BindGrid();
        }
    }
}