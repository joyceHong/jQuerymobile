﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using ServiceReference1;
using System.Data;
using System.Web.Configuration;
using System.Collections.Specialized;
public partial class ScheduleTrack : System.Web.UI.Page
{
    string strConn = ConfigurationManager.ConnectionStrings["TCS"].ConnectionString;
    SqlConnection sqlCon = new SqlConnection();
    GetScheduleData server1 = new GetScheduleData();
    string strRecordID;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /*如果session 過期自動登出*/
            if (Session["Account"] ==null ||
               Session["Password"] ==null)
            {
                Response.Redirect("~/Login.aspx",false);
            }

            if(!IsPostBack)
            {
                hiddensKeyID.Value = ""; /*先清空*/
                if (Request.QueryString["id"] != "")
                {
                    /*登入人員資料*/
                    hidden_scheduleTrack_account.Value = Session["Account"].ToString();

                    /*客戶問題記錄編號*/
                    strRecordID = Request.QueryString["id"];
                    hiddensKeyID.Value = strRecordID;
                    hiddenAllUser_scheduleDay.Value = Request.QueryString["workday"]; /*檢視所有人某天的行程*/
                    
                    /* 回報行程資料 */
                    string returnCustomerValue = server1.GetReportRec(strRecordID);
                    hiddenCustomer_json.Value = returnCustomerValue; /*經由<input 元件> 達到和javaScript 溝通*/
                  
                    
                    /*客戶基本資料*/
                    DataTable dtReport = JsonConvert.DeserializeObject<DataTable>(returnCustomerValue); //反解json
                    Customer customerObj = server1.GetCustomerData(dtReport.Rows[0]["索引編號"].ToString() );
                    lbCustom_name.Text = customerObj.name;
                    lbCustom_addr.Text = customerObj.address;
                    lbCustom_index.Text = customerObj.index;
                    lbCustom_tel.Text = customerObj.tel;
                    lbCustom_tel2.Text = customerObj.tel2;
                    lbCustom_mobile.Text = customerObj.mobile;
                    lbCustom_doctor.Text = customerObj.responsibleUser;
                    lbCustom_conntactStaff.Text = customerObj.conntactStaff;
                    lbCustom_maturityDate.Text = customerObj.maturityDate;
                    lbCustom_ip.Text = customerObj.ip;
                    lbCustom_registerPassword.Text = customerObj.registerPassword;
                    lbCustom_editionPassword.Text = customerObj.editionPassword;
                    lbCustom_SMS.Text = customerObj.sms;
                    lbCustom_tour.Text = customerObj.tour;
                    lbCustom_HMR.Text = customerObj.EMR; //電子病歷
                    lbCustom_oralChecking.Text = customerObj.oralChecking;

                    scheduleTrackt_report_customerIndex.Value = dtReport.Rows[0]["索引編號"].ToString();
                    /*暫時保留欄位~~ 因為曾經有人需要顯示合約等級和金額*/
                    //lbCustom_contractMoney.Text = customerObj.contractMoney;
                    //lbCustom_contractLeavel.Text = customerObj.contractLeavel;
                    

                    /*服務次數*/
                    ServericeCount serviceCountObj = server1.GetCustomServiceCount(dtReport.Rows[0]["索引編號"].ToString());
                    lbCustom_telCount.Text = serviceCountObj.telCount;
                    lbCustom_serviceCount.Text = serviceCountObj.serviceCount;
                    hiddenCustomServerice_json.Value = serviceCountObj.tempTable;

                    // serviceCountObj.tempTable;

                    /*線上調動-人員清單*/
                    Dictionary<string, string> emps = server1.GetEmployees();
                    dropCustomer_tranferEmp1.DataSource = emps;
                    dropCustomer_tranferEmp1.DataValueField = "key";
                    dropCustomer_tranferEmp1.DataTextField = "value";
                    dropCustomer_tranferEmp1.DataBind();

                    dropCustomer_tranferEmp2.DataSource = emps;
                    dropCustomer_tranferEmp2.DataValueField = "key";
                    dropCustomer_tranferEmp2.DataTextField = "value";
                    dropCustomer_tranferEmp2.DataBind();

                    dropCustomer_tranferEmp3.DataSource = emps;
                    dropCustomer_tranferEmp3.DataValueField = "key";
                    dropCustomer_tranferEmp3.DataTextField = "value";
                    dropCustomer_tranferEmp3.DataBind();


                    
                    Dictionary<string,string> dicQuestypeValue = server1.GetQusttype("");                   

                    /* 行程回報-問題類別*/
                    dropReport_getQusttype.DataSource = dicQuestypeValue;
                    dropReport_getQusttype.DataValueField = "key";
                    dropReport_getQusttype.DataTextField = "value";
                    dropReport_getQusttype.DataBind();

                    ///* 新行程回報-問題類別*/
                    //dropNewReport_getQusttype.DataSource = dicQuestypeValue;
                    //dropNewReport_getQusttype.DataValueField = "key";
                    //dropNewReport_getQusttype.DataTextField = "value";
                    //dropNewReport_getQusttype.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Session["errorMsg"] = "讀取資料失敗:" + ex.Message;
            Response.Redirect("~/Login.aspx", false);
        }
    }




    protected void confirmSave_Click(object sender, EventArgs e)
    {
        bool isSaveOK = false;
        bool isClose = false;
        try
        {
            string strOrginQuestype = hiddenReport_questtype.Value;
            
            //無法將狀態為C168改為其他類別
            //string connectionInfo = ConfigurationSettings.AppSettings["disabledQuestionType"];

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string disabledQuestionType = appSettings["disabledQuestionType"];

            if (!string.IsNullOrEmpty(disabledQuestionType))
            {

                if (strOrginQuestype.Trim() == disabledQuestionType.Trim() && dropReport_getQusttype.SelectedValue != disabledQuestionType.Trim())
                {
                    Response.Write("<script>alert('ICD10無法修改其他類別')</script>");
                    return;
                }
                else if (strOrginQuestype.Trim() != disabledQuestionType.Trim() && dropReport_getQusttype.SelectedValue == disabledQuestionType.Trim())
                {
                    //如非C168，也無法把狀態改為C168
                    Response.Write("<script>alert('ICD10只能從線上預約，無法在回報網新增')</script>");
                    return;
                }
            }

            //不允許在沒有線上預約的情況下，使用C167
            int icd10Reservation = server1.FindReservationICD10(lbCustom_index.Text.Trim());
            if (icd10Reservation == 0 && dropReport_getQusttype.SelectedValue == "C167")
            {
                Response.Write("<script>alert('不允許在沒有ICD10線上預約的情況下，使用ICD10 臨時排程')</script>");
                return;
            }

            isClose = (hiddenReport_status.Value == "完成") ? true : false;
            isSaveOK = server1.SaveReportSchedule(hiddensKeyID.Value, dropReport_getQusttype.SelectedValue, textReport_proessResult.Value, hiddenReport_status.Value, isClose);


            if(isSaveOK) 
            {
                Response.Redirect("schedule.aspx", false);
            }
            else 
            {
                throw new Exception("執行SaveReportSchedule錯誤");
            }
        }
        catch (Exception ex) {
            Session["errorMsg"] = "行程回報儲存失敗:" + ex.Message;
            Response.Redirect("errorPage.aspx", false);
        }
    }

    //protected void btnNewReport_save_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        server1.SaveNewSchedule(hiddenNewReport_customerID.Value, hiddenNewReport_indexID.Value,
    //             txtNewReport_problem.Value, dropNewReport_getQusttype.SelectedValue,
    //             textNewReport_proessResult.Value, hiddenNewReport_status.Value, hidden_scheduleTrack_account.Value);
    //        Response.Redirect("schedule.aspx", false);    
    //    }
    //    catch (Exception ex) {
    //        Session["errorMsg"] = "新行程回報儲存失敗:" + ex.Message;
    //        Response.Redirect("errorPage.aspx", false);
    //    }
    //}


    protected void btnCustomer_transfer_Click(object sender, EventArgs e)
    {
        try 
        {

            if (server1.SaveTransferEmployee(hiddensKeyID.Value, dropCustomer_tranferEmp1.SelectedValue,
                 dropCustomer_tranferEmp2.SelectedValue, dropCustomer_tranferEmp3.SelectedValue, hidden_scheduleTrack_account.Value))
            {
                Response.Redirect("schedule.aspx", false);
            }
        }
        catch (Exception ex) 
        {
            Session["errorMsg"] = "調動失敗:" + ex.Message;
            Response.Redirect("errorPage.aspx", false);
        }
    }
    
}