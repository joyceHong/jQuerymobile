using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
//System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/CustomReportMobile");
//System.Configuration.Configuration strConn = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/CustomReportMobile");
//System.Configuration.ConnectionStringSettings connString;

    string strAccount;
    string strPassword;

    string strConn = ConfigurationManager.ConnectionStrings["TCS"].ConnectionString;
    SqlConnection sqlCon = new SqlConnection();
    int intProdID = 0; /* 確認有權限*/

    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {
            /* 登入失敗時錯訊息顯示*/
            if (Session["errorMsg"] != null)
            {
                lbLogin_msg.Text = Session["errorMsg"].ToString();
            }

            Session["LoginID"] = "";
            Session["Password"] = "";

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        
        Session["Account"] = account.Value.ToUpper(); /*轉換大寫*/
        Session["Password"] = password.Value;

        strAccount = account.Value.ToUpper();
        strPassword = password.Value;
        sqlCon.ConnectionString = strConn;


        try
        {
            sqlCon.Open();



            SqlCommand sqlCmd = new SqlCommand("m_resv", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "IdentifyLogin3"; /* 確認身份別 */
            /* 帳號 */
            SqlParameter spAccount = new SqlParameter("@Account", strAccount);
            /* 密碼 */
            SqlParameter spPassword = new SqlParameter("@Password", strPassword);
            /* Client ip */
            SqlParameter spClientIp = new SqlParameter("@lIp", GetClientIP());

            sqlCmd.Parameters.Add(spAccount);
            sqlCmd.Parameters.Add(spPassword);
            sqlCmd.Parameters.Add(spClientIp);
            intProdID = Convert.ToInt32(sqlCmd.ExecuteScalar());
            sqlCmd.Dispose();
            sqlCon.Close();


            if (intProdID <= 0)
            {
                Session["Account"] = "";
                Session["Password"] = "";
                Response.Write("<script type='text/javascript'>alert('帳號或密碼錯誤!')</script>");
                //Response.Write("<script type='text/javascript'>alert('帳號或密碼錯誤!');history.go(-1)</script>");

            }
            else
            {

               // 前往下一個行程畫面
                Session["errorMsg"] = ""; //因為登入成功 所以清除登入失敗的訊息

                //Response.Write("<script>window.location.href='schedule.aspx';window.onbeforeunload=null;window.open('','self');window.close()</script>");
                Response.Write("<script>window.location.href='schedule.aspx'</script>");

            }
        }
        catch(Exception ex)
        {

            Session["errorMsg"] = "登入失敗:" + ex.Message;
            Response.Redirect("login.aspx",false);

        }

    }

    protected string GetClientIP()
    {
        string strIPAddr = "";
        if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
        {
            strIPAddr = Request.ServerVariables["REMOTE_ADDR"].ToString();

        }
        else if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf("unknown") > 0)
        {
            strIPAddr = Request.ServerVariables["REMOTE_ADDR"];
        }
        else if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf(",") > 0)
        {
            strIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Substring(0, Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().IndexOf(",") - 1);
        }
        else if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf(";") > 0)
        {
            strIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Substring(0, Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().IndexOf(";") - 1);
        }
        else
        {
            strIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        }
        return strIPAddr.Trim();
    }

 protected void messageBox(string message)
    {
    
        Response.Write("<script type='text/javascript'>alert('"+message+"')</script>");
    }

}