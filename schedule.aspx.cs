using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using ServiceReference1;
using System.Collections.Specialized;
public partial class Schedule : System.Web.UI.Page
{

    
    string strConn = ConfigurationManager.ConnectionStrings["TCS"].ConnectionString;
    SqlConnection sqlCon = new SqlConnection();
    string strScheduleJson = ""; /* 取出schedule行程清單 */    

    GetScheduleData server1 = new GetScheduleData();

    protected void Page_Load(object sender, EventArgs e)
    {

        var httpPath = HttpContext.Current.Server.MapPath("~/");

        try
        {
           
           
            /*如果session 過期自動登出*/
            if (Session["Account"] == null ||
               Session["Password"] ==null)
            {
                Response.Redirect("~/Login.aspx",false);
            }


            if (!IsPostBack)
            {
                if (Request.QueryString["workDay"] == null)
                {
                    scheduleDay.Value = DateTime.Today.ToString("yyyy-MM-dd"); //預設
                }
                else
                {
                    scheduleDay.Value = Request.QueryString["workDay"];
                }

                

                strScheduleJson = server1.GetJsonScheduleData(Session["Account"].ToString(), scheduleDay.Value);
                hidden_scheduleList_json.Value = strScheduleJson; /* 取出的資料也放在hidden中供javascript取用*/
                hidden_schedule_account.Value = Session["Account"].ToString(); /*帳號放在hidden 供javaScript 取用*/
                hidden_schedule_password.Value = Session["Password"].ToString();//密碼
                hiddenSearchCustom_indexID.Value = Request.QueryString["indexID"]; /*取得客戶ID自動帶入*/
                
              

                ///* 新行程回報-問題類別*/
                Dictionary<string,string> dicQuestypeValue = server1.GetQusttype("");

                dropNewReport_getQusttype.DataSource = dicQuestypeValue;
                dropNewReport_getQusttype.DataValueField = "key";
                dropNewReport_getQusttype.DataTextField = "value";
                dropNewReport_getQusttype.DataBind();

            }
        }
        catch (Exception ex)
        {
            Session["Account"] = null;
            Session["Password"] = null;
            hidden_schedule_account.Value = null;
            Session["errorMsg"] = ex.Message;
            Response.Redirect("~/Login.aspx", false);
        }
    }


    protected void btnScheduleList_LogOut_Click(object sender, EventArgs e)
    {
        Session["Account"] = null;
        Session["Password"] = null;
        hidden_schedule_account.Value = null;
        Response.Redirect("~/Login.aspx",false);
    }


    protected void btnNewReport_save_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string disabledQuestionType = appSettings["disabledQuestionType"];
            if (!string.IsNullOrEmpty(disabledQuestionType))
            {
                if (dropNewReport_getQusttype.SelectedValue == disabledQuestionType.Trim())
                {
                    Response.Write("<script>alert('無法新增ICD10的行程')</script>");
                    return;
                }
            }

            //不允許C172的類別新增
            if (dropNewReport_getQusttype.SelectedValue == "C172" || dropNewReport_getQusttype.SelectedValue == "C173")
            {
                Response.Write("<script>alert('無法新增EEC的行程')</script>");
                return;
            }

            //不允許在沒有線上預約的情況下，使用C167
            int icd10Reservation = server1.FindReservationICD10(hiddenNewReport_indexID.Value);
            if (icd10Reservation == 0 && dropNewReport_getQusttype.SelectedValue == "C167")
            {
                Response.Write("<script>alert('不允許在沒有ICD10線上預約的情況下，使用ICD10 臨時排程')</script>");
                return;
            }

            bool officeOperate = txtNewReport_officeOperate.Checked;

            server1.SaveNewSchedule(hiddenNewReport_customerID.Value, hiddenNewReport_indexID.Value,
                 txtNewReport_problem.Value, dropNewReport_getQusttype.SelectedValue,
                 textNewReport_proessResult.Value, hiddenNewReport_status.Value, hidden_schedule_account.Value, dateNewReport_dispatchingDate.Text, officeOperate);
            Response.Redirect("schedule.aspx", false);
        }
        catch (Exception ex)
        {
            Session["errorMsg"] = "新行程回報儲存失敗:" + ex.Message;
            Response.Redirect("errorPage.aspx", false);
        }
    }


 protected void messageBox(string message)
    {
    
        Response.Write("<script type='text/javascript'>alert('"+message+"')</script>");
    }
}