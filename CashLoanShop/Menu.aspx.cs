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
    public partial class Menu : System.Web.UI.Page
    {
        public int UserId { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie myCookie = Request.Cookies["UserId"];
                if (myCookie != null && this.UserId == 0)
                {
                    this.UserId = Convert.ToInt32(myCookie.Value);
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }

                CustomerService cs = new CustomerService();
                CashLoanShop.Model.CustomMessage cm = cs.CustomMessages.ToList().Where(p => p.Id == Convert.ToInt32(1)).FirstOrDefault();
                if (cm.Message != string.Empty)
                {
                    data.Style.Add(HtmlTextWriterStyle.Display, "");
                    lblMessage.Text = cm.Message;
                }
                else
                {
                    data.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblMessage.Text = "";
                }
            }

        }
    }
}