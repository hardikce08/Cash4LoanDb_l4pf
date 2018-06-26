using CashLoanShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CashLoanShop.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            CustomerService cs = new CustomerService();
            //CashLoanShop.Model.User u = cs.Users.ToList().Where(p => p.UserName.ToLower() == txtUserName.Text.ToLower() && p.Password.ToLower() == txtPassword.Text.ToLower()).FirstOrDefault();
            if(txtUserName.Text=="admin" && txtPassword.Text=="admin")
            {
                Session["UserId"] = "1";
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "run", "alert('Invalid username or password');", true);
            }
            Response.Redirect("~/Admin/UserManager.aspx");
        }
    }
}