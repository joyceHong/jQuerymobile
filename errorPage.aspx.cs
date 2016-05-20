using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["errorMsg"] != null)
        {
            errorPage_message.Text = Session["errorMsg"].ToString();
        }
    }
}