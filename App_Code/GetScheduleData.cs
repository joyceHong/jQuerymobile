using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.ServiceModel.Activation;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Web;


[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
// 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "GetScheduleData"。
public class GetScheduleData : IGetScheduleData
{
    string httpPath = HttpContext.Current.Server.MapPath("~/");
    SqlConnection sqlCon = new SqlConnection();
    WriteEvent.WrittingEventLog writeObj = new WriteEvent.WrittingEventLog();

    string tcsConnectString = ConfigurationManager.ConnectionStrings["TCS"].ConnectionString;

    //System.Configuration.Configuration rootWebConfig =
    //         System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/CustomReportMobile");

    //System.Configuration.Configuration rootWebConfig =
    //        System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/CustomReportMobile");

    //System.Configuration.Configuration rootWebConfig = System.Configuration.ConfigurationManager.ConnectionStrings["TCS"].CurrentConfiguration;

    //System.Configuration.ConnectionStringSettings connString;

    
    /// <summary>
    /// 列出行程清單
    /// </summary>
    /// <param name="Account">登入帳號</param>
    /// <param name="WorkDay">選擇日期</param>
    /// <returns></returns>
    public string GetJsonScheduleData(string Account, string WorkDay)
    {
        try
        {
            //connString = new ConnectionStringSettings();
            //connString.ConnectionString = "Server=61.219.132.198;Database=TCS;User ID=asuser;Password=asusernet";

            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString =
            //        rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "ShowQuestions7";
            SqlParameter spAccount = new SqlParameter("@Account", Account);
            DateTime dtWork = Convert.ToDateTime(WorkDay);

            SqlParameter spWorkDay = new SqlParameter("@qDate", dtWork);
            sqlCmd.Parameters.Add(spAccount);
            sqlCmd.Parameters.Add(spWorkDay);
            DataTable dt = new DataTable();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(sqlReader);
            string strJsonResult = "";
            if (dt.Rows.Count > 0)
            {
                /* 使用外掛元件將dt轉成json格式 */
                strJsonResult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            else
            {
                strJsonResult = "";
            }
            sqlReader.Close();
            sqlCmd.Dispose();
            return strJsonResult;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError"+DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }      
    }


    /// <summary>
    /// 顯示所有的人的列表
    /// </summary>
    /// <param name="WorkDay"></param>
    /// <returns></returns>
    public string GetAllScheduleData(string Account, string WorkDay)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString =
            //        rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ReSchedule1";            
            DateTime dtWork = Convert.ToDateTime(WorkDay);
            SqlParameter spWorkDay = new SqlParameter("@qDate", dtWork);            
            sqlCmd.Parameters.Add(spWorkDay);

            SqlParameter spUser = new SqlParameter("@uid", Account);
            sqlCmd.Parameters.Add(spUser);
            DataTable dt = new DataTable();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(sqlReader);
            string strJsonResult = "";
            if (dt.Rows.Count > 0)
            {
                /* 使用外掛元件將dt轉成json格式 */
                strJsonResult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            else
            {
                strJsonResult = "";
            }
            sqlReader.Close();
            sqlCmd.Dispose();
            return strJsonResult;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message); 
        }
    }

   /// <summary>
   /// 取得客戶基本資料和此筆處理流程的紀錄
   /// </summary>
   /// <param name="strKeyId"></param>
   /// <returns></returns>
    public string GetReportRec(string strKeyId)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();           

            string strSql = "SELECT b.索引編號, a.客戶問題, a.處理結果,a.案件狀態," +
                          "b.問題類別,a.備註,  CONVERT(varchar(16),  b.打卡時間, 20) as '打卡時間'," +
                          "CONVERT(varchar(16),  b.結束時間,20) as '結束時間',b.已回寫," +
                          "b.員工代號,b.員工代號2, b.員工代號3,c.客戶名稱, c.客戶地址," +
                          "u1.員工姓名 員工姓名,u2.員工姓名 員工姓名2, u3.員工姓名 員工姓名3 " +
                          "FROM m_detail a WITH(NOLOCK) " +
                          "LEFT JOIN m_resv b WITH (NOLOCK) on a.M_resv_Skey=b.skey " +
                          "LEFT JOIN asuser c WITH (NOLOCK) ON c.索引編號=b.索引編號 " +
                          "LEFT JOIN z_emp u1 WITH (NOLOCK) ON u1.員工代號=b.員工代號 " +
	                      "LEFT JOIN z_emp u2 WITH (NOLOCK) ON u2.員工代號=b.員工代號2 " +
	                      "LEFT JOIN z_emp u3 WITH (NOLOCK) ON u3.員工代號=b.員工代號3 " +
                          "WHERE a.M_resv_Skey=@SKEYID";

            SqlCommand sqlCmd = new SqlCommand(strSql, sqlCon);
            SqlParameter spSkeyId = new SqlParameter("@SKEYID", strKeyId);
            sqlCmd.Parameters.Add(spSkeyId);
            DataTable dt = new DataTable();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(sqlReader);
            string strJsonResult = "";
            if (dt.Rows.Count > 0)
            {
                strJsonResult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            else
            {
                strJsonResult = "";
            }
            sqlReader.Close();
            sqlCmd.Dispose();
            return strJsonResult;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
      
    }

   /// <summary>
   /// 問題分類的項目
   /// </summary>
   /// <param name="strCondition">條件過濾</param>
   /// <returns>json格式的datatable</returns>
    public Dictionary<string,string> GetQusttype(string strCondition)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "GetQusttype";
            SqlParameter spCondition = new SqlParameter("@Where", strCondition);
            sqlCmd.Parameters.Add(spCondition);
            
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            Dictionary<string, string> dicQuestypes = new Dictionary<string, string>();
            if (sqlReader.HasRows)
            {
                while (sqlReader.Read()) {
                    dicQuestypes[sqlReader["問題代號"].ToString()] = sqlReader["代號類別"].ToString();
                }
            }

            
            sqlReader.Close();
            sqlCmd.Dispose();
            return dicQuestypes;
            
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
    }

    /// <summary>
    /// 打卡時間
    /// </summary>
    /// <param name="Skey">客戶問題流水編號</param>
    /// <returns></returns>
    public string CheckIn(string Skey)
    {
        /* db只能測自已local端 */
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "MCheckIn";
            SqlParameter spSkey = new SqlParameter("@Skey", Skey);            
            sqlCmd.Parameters.Add(spSkey);
            SqlParameter spTime = new SqlParameter("@Time", SqlDbType.DateTime);
            spTime.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spTime);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return spTime.Value.ToString();
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }   
    }

   /// <summary>
   /// 結束時間
   /// </summary>
    /// <param name="SkeyID">客戶問題流水編號</param>
   /// <returns></returns>
    public string CheckOut(string SkeyID)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "MCheckOut";
            SqlParameter spSkey = new SqlParameter("@Skey", SkeyID);
            sqlCmd.Parameters.Add(spSkey);
            SqlParameter spTime = new SqlParameter("@Time", SqlDbType.DateTime);
            spTime.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spTime);

            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return spTime.Value.ToString();
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
    }

    public bool SaveReportSchedule(string skey, string strQusttype, string strResult, string strStatus, bool isClosed)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SaveReport";

            int intSkey = 0;
            int.TryParse(skey, out intSkey);

            SqlParameter spSkey = new SqlParameter("@skey", intSkey);
            sqlCmd.Parameters.Add(spSkey);

            SqlParameter spQuesttype = new SqlParameter("@Qusttype", strQusttype);
            sqlCmd.Parameters.Add(spQuesttype);

            SqlParameter spResult = new SqlParameter("@Result", strResult);
            sqlCmd.Parameters.Add(spResult);

            SqlParameter spStatus = new SqlParameter("@Status", strStatus);
            sqlCmd.Parameters.Add(spStatus);

            SqlParameter spIsClose = new SqlParameter("@IsClose", isClosed);
            sqlCmd.Parameters.Add(spIsClose);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return true;
        }
        catch (SqlException ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw;
        }
    }     

   /// <summary>
    /// 查詢客戶的的醫事機構代號
   /// </summary>
   /// <param name="strName">診所名稱</param>
   /// <returns></returns>
    public string ClinicList(string Name)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            /* 註 : 客戶編號 是產品編號(分網路版/單機版)  ; 索引編號:客戶唯一碼 */
            //string strSQL = "SELECT 醫事代號, 客戶名稱, 索引編號,客戶編號,客戶地址,負責醫師 " +
            //                "FROM asuser where 客戶名稱 like '%" + Name + "%' " +
            //                "AND 醫事代號 <> ''"; // 如使用@變數，似乎有異常現象

            string strSQL = "SELECT 醫事代號, 客戶名稱, 索引編號,客戶編號,客戶地址,負責醫師 " +
                          "FROM asuser where 客戶名稱 like '%" + Name + "%' or 負責醫師 like '%" +Name +"%' " +
                          "or 體系名稱 LIKE '%"+Name+"%' OR 客戶電話 LIKE '%"+Name+"%' " +
                          "AND 醫事代號 <> ''"; // 如使用@變數，似乎有異常現象

            SqlCommand sqlCmd = new SqlCommand(strSQL, sqlCon);
            
            DataTable dt = new DataTable();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(sqlReader);

            string strJsonResult = "";
            if (dt.Rows.Count > 0)
            {
                /* 使用外掛元件將dt轉成json格式 */
                strJsonResult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            else
            {
                strJsonResult = "";
            }

            sqlReader.Close();
            sqlCmd.Dispose();
            return strJsonResult;
        }
        catch (Exception ex) {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
    }

    /// <summary>
    /// 儲存新行程回報
    /// </summary>
    /// <param name="strCustomerID">客戶產品編號</param>
    /// <param name="strIndexID">客戶流水號</param>
    /// <param name="strProblem">問題反應</param>
    /// <param name="strQusttype">問題類別</param>
    /// <param name="strResult">處理結果</param>
    /// <param name="strStatus">問題狀態</param>
    /// <param name="isOfficialOperate">是否內勤作業</param>
    /// <returns></returns>
    public bool SaveNewSchedule(string strCustomerID, string strIndexID, string strProblem, string strQusttype,
        string strResult, string strStatus, string strEmpID, string strDispatchDate, bool isOfficeOperate)
    {
        try 
        {
            var test = strDispatchDate;

            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "Insert_M_resv5";

            SqlParameter spCustomerID = new SqlParameter("@CustomerID", strCustomerID);
            sqlCmd.Parameters.Add(spCustomerID);
            SqlParameter spIndexID = new SqlParameter("@IndexID", strIndexID);
            sqlCmd.Parameters.Add(spIndexID);
            SqlParameter spProblem = new SqlParameter("@Problem", strProblem);
            sqlCmd.Parameters.Add(spProblem);
            SqlParameter spQusttype = new SqlParameter("@Qusttype", strQusttype);
            sqlCmd.Parameters.Add(spQusttype);
            SqlParameter spResult = new SqlParameter("@Result", strResult);
            sqlCmd.Parameters.Add(spResult);
            SqlParameter spStatus = new SqlParameter("@Status", strStatus);
            sqlCmd.Parameters.Add(spStatus);

            SqlParameter spInsertor = new SqlParameter("@Insertor", strEmpID); //登入人員
            sqlCmd.Parameters.Add(spInsertor);

            SqlParameter spOfficeOperate = new SqlParameter("@OfficeOperations", isOfficeOperate); //是否內勤作業
            sqlCmd.Parameters.Add(spOfficeOperate);

            //西元年轉民國年
            //strDispatchDate = strDispatchDate.Replace("-","");
            //int intDispatchDate =0;
            //int.TryParse(strDispatchDate, out intDispatchDate);
            //string strChiDispatchingDate = (intDispatchDate-19110000).ToString().PadLeft(7,'0');

            SqlParameter spDispatchingDate = new SqlParameter("@DispatchingDate", strDispatchDate);
            sqlCmd.Parameters.Add(spDispatchingDate);


            /*暫用不到的的預儲函式的參數--暫以空白儲存*/
            SqlParameter spSoftTeaching = new SqlParameter("@SoftTeaching", "");
            sqlCmd.Parameters.Add(spSoftTeaching);
            SqlParameter spNote = new SqlParameter("@Note", "");
            sqlCmd.Parameters.Add(spNote);


            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return true;
        }
        catch(Exception ex) 
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
        
    }

    /// <summary>
    /// 儲存調動人員
    /// </summary>
    /// <param name="strKey">客戶問題流水編號</param>
    /// <param name="strEmp1">調動員工1</param>
    /// <param name="strEmp2">調動員工2</param>
    /// <param name="strEmp3">調動員工3</param>
    /// <returns></returns>
    public bool SaveTransferEmployee(string strKey, string strEmp1, string strEmp2, string strEmp3, string strLoginUser)
    {
        try {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            //sqlCon.ConnectionString = connString.ToString();
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_Save_New_Eng1";

            SqlParameter spSkey = new SqlParameter("@Skey", strKey);
            sqlCmd.Parameters.Add(spSkey);
            SqlParameter spEmp1 = new SqlParameter("@Eng1", strEmp1);
            sqlCmd.Parameters.Add(spEmp1);
            SqlParameter spEmp2 = new SqlParameter("@Eng2", strEmp2);
            sqlCmd.Parameters.Add(spEmp2);
            SqlParameter spEmp3 = new SqlParameter("@Eng3", strEmp3);
            sqlCmd.Parameters.Add(spEmp3);

            SqlParameter spLoginUser = new SqlParameter("@LoginUser", strLoginUser);
            sqlCmd.Parameters.Add(spLoginUser);
           
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return true;
        }
        catch (Exception ex) {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
        
    }

    /// <summary>
    /// 所有人員清單
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetEmployees()
    {
        try { 
            
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_Get_Emp";

            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            Dictionary<string, string> emps = new Dictionary<string, string>();
            if (sqlReader.HasRows)
            {
                while (sqlReader.Read())
                {
                    emps[sqlReader["員工代號"].ToString().Trim()] = sqlReader["員工姓名"].ToString();
                }
            }
            sqlCmd.Dispose();
            sqlCon.Close();
            return emps;
        }
        catch (Exception ex) {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
        
    }

    /// <summary>
    /// 客戶基本資料
    /// </summary>
    /// <param name="strIndex">客戶索引編號</param>
    /// <returns></returns>
    public Customer GetCustomerData(string Index)
    {
        
       

        Customer customerObj = new Customer();
        try {

            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;            
            sqlCmd.CommandText = "GetOneCustomer3";
            SqlParameter spIndex = new SqlParameter("@IndexID2", Index);
            sqlCmd.Parameters.Add(spIndex);
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (sqlReader.HasRows) {

                while (sqlReader.Read()) {

                    customerObj.index = sqlReader["編號"].ToString();
                    customerObj.name = sqlReader["客戶名稱"].ToString();
                    customerObj.tel = sqlReader["客戶電話"].ToString();
                    customerObj.tel2 = sqlReader["客戶電話2"].ToString();
                    customerObj.mobile = sqlReader["行動電話"].ToString();
                    customerObj.responsibleUser = sqlReader["負責醫師"].ToString();
                    customerObj.conntactStaff = sqlReader["聯絡人"].ToString();
                    customerObj.address = sqlReader["客戶地址"].ToString();
                    customerObj.maturityDate = sqlReader["維護到期日"].ToString();
                    customerObj.ip = sqlReader["ip"].ToString();
                    customerObj.registerPassword = sqlReader["註冊密碼"].ToString();
                    customerObj.editionPassword = sqlReader["更版密碼"].ToString();
                    customerObj.sms = sqlReader["簡訊"].ToString();
                    customerObj.tour = sqlReader["巡迴"].ToString();
                    customerObj.oralChecking = sqlReader["口檢"].ToString();
                    customerObj.EMR = sqlReader["EMR"].ToString(); // 尚未完成
                    //customerObj.contractMoney = sqlReader["合約金額"].ToString();
                    //customerObj.contractLeavel = sqlReader["合約等級"].ToString();
                }
            }
            
            sqlCmd.Dispose();
            sqlCon.Close();
            return customerObj;
        }
        catch (Exception ex) {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }        
    }

    /// <summary>
    /// 取出服務次數/來電次數
    /// </summary>
    /// <param name="strIndexID">客戶索引編號</param>
    /// <returns>陣列:來電次數/派工次數</returns>
    public ServericeCount GetCustomServiceCount(string strIndexID)
    {
        ServericeCount serviceCountObj = new ServericeCount();

        try 
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ServiceRecord";
            SqlParameter spIndex = new SqlParameter("@IndexID", strIndexID);
            sqlCmd.Parameters.Add(spIndex);

            SqlParameter spTelCount = new SqlParameter("@TelCount", SqlDbType.Int);
            spTelCount.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spTelCount);

            SqlParameter spServiceCount = new SqlParameter("@ServiceCount", SqlDbType.Int);
            spServiceCount.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spServiceCount);
            //sqlCmd.ExecuteNonQuery();
            
            /*取出所有服務列表*/
            DataTable dt = new DataTable();
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

            dt.Load(sqlReader);
            string strJsonResult = "";
            if (dt.Rows.Count > 0)
            {
                /* 使用外掛元件將dt轉成json格式 */             

                strJsonResult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            else
            {
                strJsonResult = "";
            }
            sqlReader.Close();
            sqlCmd.Dispose();
            sqlCon.Close();

            serviceCountObj.telCount = spTelCount.Value.ToString();
            serviceCountObj.serviceCount = spServiceCount.Value.ToString();
            serviceCountObj.tempTable = strJsonResult;
            return serviceCountObj;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
    }

    /// <summary>
    /// 查詢所有人員的行程列表
    /// </summary>
    /// <param name="WorkDay">工作天</param>
    /// <returns>物件陣列</returns>
    public List<AllUserSchedule> GetJsonAllUserScheduleData(string WorkDay)
    {
        List<AllUserSchedule> alluser_schedules = new List<AllUserSchedule>(); //儲存所有人員的行程表 物件集合
        Dictionary<string, List<string>> dicAllUserSchedule = new Dictionary<string, List<string>>(); //將所有人員行程依員工編號分類

        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            string strSql = "SELECT  c.客戶名稱, c.客戶地址,d.員工代號, d.使用者名稱 服務人員 "+
		                    "from m_resv a with (nolock)" +
			                "left join m_detail b with (nolock) on b.M_resv_Skey = a.skey " +
			                "left join asuser c with (nolock) on a.索引編號=c.索引編號 "+
			                "left join z_user d with (nolock) on (upper(d.員工代號)=upper(a.員工代號) or upper(d.員工代號)=upper(a.員工代號2) or upper(d.員工代號)=upper(a.員工代號3)) " +
		                    "where a.日期=dbo.ToTaiwanDate(@qDate) and 內勤作業=0 " +
		                    "order by d.使用者名稱, 應到";

            SqlCommand sqlCmd = new SqlCommand(strSql, sqlCon);
            SqlParameter spSkeyId = new SqlParameter("@qDate", WorkDay);
            sqlCmd.Parameters.Add(spSkeyId);
            
            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            
            if (sqlReader.HasRows)
            {
                while (sqlReader.Read())
                {

                    if (dicAllUserSchedule.ContainsKey(sqlReader["員工代號"].ToString().Trim()) == false)
                    {
                        dicAllUserSchedule[sqlReader["員工代號"].ToString().Trim()] = new List<string>();
                        dicAllUserSchedule[sqlReader["員工代號"].ToString().Trim()].Add(sqlReader["服務人員"].ToString().Trim());
                    }
                    dicAllUserSchedule[sqlReader["員工代號"].ToString().Trim()].Add(sqlReader["客戶名稱"].ToString().Trim() + " / " + sqlReader["客戶地址"].ToString().Trim());
                }
            }

            AllUserSchedule user_schedule = new AllUserSchedule(); //轉換成物件變數            
            foreach (KeyValuePair<string, List<string>> kvp in dicAllUserSchedule)
            {
                user_schedule.userID = kvp.Key;
                user_schedule.userName = kvp.Value[0];
                kvp.Value.RemoveAt(0); //因為陣列0 暫儲存員工姓名，需去掉
                user_schedule.schedules =kvp.Value;
                alluser_schedules.Add(user_schedule);
            }
            sqlReader.Close();
            sqlCmd.Dispose();
            return alluser_schedules;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
    }

    /// <summary>
    /// 取得客戶的所有產品的合約到期日
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public List<Product> GetClinicAllProductContract(string Index)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            string strSQL = @" select  p.產品代碼,p.產品名稱,c.合約迄日 from product  p 
                            left join (
                            select 
	                            b.產品代碼,
	                            max(b.合約迄日) as 合約迄日
                            from asuser A LEFT JOIN contract B 
                            on (a.索引編號=b.索引編號) where a.索引編號=@index group by b.產品代碼 ) as c 
                            on(p.產品代碼=c.產品代碼) where  p.產品代碼 
                                in ('001','006','019','020') ";

            SqlCommand sqlCmd = new SqlCommand(strSQL, sqlCon);
            SqlParameter spIndex = new SqlParameter("@index", Index);
            sqlCmd.Parameters.Add(spIndex);

            SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Product> liClinicProducts = new List<Product>();

            if (sqlReader.HasRows)
            {
                while (sqlReader.Read())
                {
                    liClinicProducts.Add(new Product()
                    {
                        productName = sqlReader["產品名稱"].ToString().Trim(),
                        contractEndDate = sqlReader["合約迄日"].ToString().Trim(),
                    });
                }
            }
            return liClinicProducts;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


    public bool SaveClinicDutyTime(string Index,string WeekDay ,string MorningStart, string MorninigEnd, string NoonStart, string NoonEnd, string NightStart, string NightEnd)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SaveClinicDutyTime";

            SqlParameter spIndex = new SqlParameter("@Index", Index);
            SqlParameter spWeekDay = new SqlParameter("@WeekDay", WeekDay);

            SqlParameter spMorningStart = new SqlParameter("@MorningStartTime", (MorningStart == "null") ? "  " : MorningStart);
            SqlParameter spMorningEnd = new SqlParameter("@MorningEndTime", (MorninigEnd == "null") ? "" : MorninigEnd);

            SqlParameter spNoonStart = new SqlParameter("@NoonStartTime", (NoonStart == "null") ? "" : NoonStart);
            SqlParameter spNoonEnd = new SqlParameter("@NoonEndTime", (NoonEnd =="null") ? "" : NoonEnd );

            SqlParameter spNightStart = new SqlParameter("@NightStartTime", (NightStart == "null") ? "" : NightStart);
            SqlParameter spNightEnd = new SqlParameter("@NightEndTime", (NightEnd == "null") ? "" : NightEnd);

            sqlCmd.Parameters.Add(spIndex);
            sqlCmd.Parameters.Add(spWeekDay);
            sqlCmd.Parameters.Add(spMorningStart);
            sqlCmd.Parameters.Add(spMorningEnd);
            sqlCmd.Parameters.Add(spNoonStart);
            sqlCmd.Parameters.Add(spNoonEnd);
            sqlCmd.Parameters.Add(spNightStart);
            sqlCmd.Parameters.Add(spNightEnd);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public List<ClinicDuty> ReadClinicDutyTime(string Index)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}

            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            string strSQL = @"select weekday,morningStart,morningEnd,noonStart,noonEnd,nightStart,nightEnd
                                from  (
                                select '1'  as weekDay ,t111 as morningStart,t112 as morningEnd,t121 as noonStart,t122 as noonEnd,t131 as nightStart,t132 as nightEnd from asschedule where 索引編號=@customerIndex
                                UNION
                                  select '2',t211,t212,t221,t222,t231,t232 from asschedule where 索引編號=@customerIndex
                                UNION
                                  select '3',t311,t312,t321,t322,t331,t332 from asschedule where 索引編號=@customerIndex
                                UNION
                                  select '4',t411,t412,t421,t422,t431,t432 from asschedule where 索引編號=@customerIndex
                                UNION
                                  select '5',t511,t512,t521,t522,t531,t532 from asschedule where 索引編號=@customerIndex
                                UNION
                                  select '6',t611,t612,t621,t622,t631,t632 from asschedule where 索引編號=@customerIndex) as a order by a.weekDay";

            SqlCommand sqlCmd = new SqlCommand(strSQL, sqlCon);
            SqlParameter spIndex = new SqlParameter("@customerIndex", Index);
            sqlCmd.Parameters.Add(spIndex);
            
             SqlDataReader sqlReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
             List<ClinicDuty> clinicDutyObjs = new List<ClinicDuty>();

             if (sqlReader.HasRows)
             {
                 while (sqlReader.Read())
                 {
                     clinicDutyObjs.Add(new ClinicDuty()
                     {
                          weekDay = sqlReader["weekday"].ToString(),
                          morningStart = sqlReader["morningStart"].ToString(),
                          morningEnd = sqlReader["morningEnd"].ToString(),
                          noonStart = sqlReader["noonStart"].ToString(),
                          noonEnd = sqlReader["noonEnd"].ToString(),
                          nightStart = sqlReader["nightStart"].ToString(),
                          nightEnd = sqlReader["nightEnd"].ToString()
                     });
                 }
             }
            return clinicDutyObjs;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public int SetICD10(string UserID,string IndexID, string Result, string PreviewCheck)
    {
        try
        {
            PreviewCheck = (PreviewCheck == "null") ? "" : "1"; //空白:未預檢成功  1:預檢成功
            //取得IP
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "Out_SetAreaICD10_2"; //加入預儲程序

            //IP
            SqlParameter spIP = new SqlParameter("@tc20IP", ip);
            sqlCmd.Parameters.Add(spIP);

            //客戶編號
            SqlParameter spIndex = new SqlParameter("@tc5id", IndexID);
            sqlCmd.Parameters.Add(spIndex);

            //預檢上傳
            SqlParameter spPreviewOK = new SqlParameter("@previewCheck", PreviewCheck);
            sqlCmd.Parameters.Add(spPreviewOK);

            //登入者
            SqlParameter spUser = new SqlParameter("@tc5login", UserID);
            sqlCmd.Parameters.Add(spUser);
            
            //回傳值
            SqlParameter spResult = new SqlParameter("@iret", Result);
            sqlCmd.Parameters.Add(spResult);

            SqlParameter spErrCode = new SqlParameter("@iErrCode",SqlDbType.SmallInt);
            spErrCode.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spErrCode);


            sqlCmd.ExecuteNonQuery();

            int intRetval = 0;
            int.TryParse(spErrCode.SqlValue.ToString(), out intRetval);
            sqlCmd.Dispose();
            sqlCon.Close();
            return intRetval;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
    }

    public string ReadICD10(string Index)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();


            SqlCommand sqlCmd = new SqlCommand("select count(*) from ApplyCard WHERE 索引編號=@tc5id AND  申請項目 = 'ICD10預檢安裝'", sqlCon);

            //客戶編號
            SqlParameter spIndex = new SqlParameter("@tc5id", Index);
            sqlCmd.Parameters.Add(spIndex);

            int haveRegisteredICD10 = (int)sqlCmd.ExecuteScalar();
            if (haveRegisteredICD10 == 0)
            {
                return "無線上預約";
            }


            //檢查是否已經有ICD10 開啟的資料
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "Out_GetUpdArea2"; //加入預儲程序

            ////客戶編號
            //SqlParameter spIndex = new SqlParameter("@tc5id", Index);
            //sqlCmd.Parameters.Add(spIndex);

            SqlParameter spRetval = new SqlParameter("@retval",SqlDbType.VarChar,30);
            spRetval.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spRetval);

            SqlParameter spRetva2 = new SqlParameter("@retval2", SqlDbType.VarChar, 30);
            spRetva2.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(spRetva2);


            //@retval2
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();
            return spRetval.SqlValue.ToString()+","+spRetva2.SqlValue.ToString();
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw new NotImplementedException(ex.Message);
        }
        
    }

    public int FindReservationICD10(string Index)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            SqlCommand sqlCmd = new SqlCommand("select count(*) from ApplyCard WHERE 索引編號=@tc5id AND  申請項目 = 'ICD10預檢安裝'", sqlCon);

            //客戶編號
            SqlParameter spIndex = new SqlParameter("@tc5id", Index);
            sqlCmd.Parameters.Add(spIndex);
            int haveRegisteredICD10 = (int)sqlCmd.ExecuteScalar();
            sqlCmd.Dispose();
            sqlCon.Close();
            return haveRegisteredICD10;
        }
        catch (Exception ex)
        {
            writeObj.writeToFile("CustomerReportScheduleError" + DateTime.Today.ToString("yyyyMMdd"), httpPath, ex.Message);
            throw;
        }
    }

    public EEC ShowEecCurrentCount(string userID)
    {
        try
        {
            //if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();
            
            SqlCommand sqlCmd = new SqlCommand("select sarea from alluser where uid=@paramUserID", sqlCon);
            SqlParameter spUserID = new SqlParameter("@paramUserID", userID);
            sqlCmd.Parameters.Add(spUserID);
            string userArea = (string)sqlCmd.ExecuteScalar();
            int intTotalCount = 0;
            int intTotalWait = 0;

            switch  (userArea){
                case "1":
                    sqlCmd.CommandText = " select 北區正取 as totalValidCount from EECAreaCount";
                    int.TryParse( sqlCmd.ExecuteScalar().ToString(), out intTotalCount); 

                    sqlCmd.CommandText = " select 北區備取 as totalValidCount from EECAreaCount";
                    int.TryParse(sqlCmd.ExecuteScalar().ToString(), out intTotalWait); 
                    
                    break;
                case "2":
                    sqlCmd.CommandText = " select 中區正取 as totalValidCount from EECAreaCount";
                    int.TryParse( sqlCmd.ExecuteScalar().ToString(), out intTotalCount);

                    sqlCmd.CommandText = " select 中區備取 as totalValidCount from EECAreaCount";
                    int.TryParse(sqlCmd.ExecuteScalar().ToString(), out intTotalWait); 

                    break;
                case "3":
                    sqlCmd.CommandText = " select 南區正取 as totalValidCount from EECAreaCount";
                    int.TryParse( sqlCmd.ExecuteScalar().ToString(), out intTotalCount);

                    sqlCmd.CommandText = " select 南區備取 as totalValidCount from EECAreaCount";
                    int.TryParse(sqlCmd.ExecuteScalar().ToString(), out intTotalWait); 
                    break;
            }

            //正取的已申請的名額
            sqlCmd.CommandText = "select count(*) from ApplyCard where 申請項目='EEC'  AND rtrim(收件1)='' and 寄回='" + userArea + "'";
            int intCurrentCount = (int) sqlCmd.ExecuteScalar();

            sqlCmd.CommandText = "select count(*) from ApplyCard where 申請項目='EEC'  AND rtrim(收件1)='後補' and 寄回='" + userArea + "'";
            int intCurrentWaitCount = (int)sqlCmd.ExecuteScalar();

            EEC eecTotalApplied = new EEC();

            string strArea="";
            if (userArea == "1")
                strArea = "北區";
            else if (userArea == "2")
                strArea = "中區";
            else if (userArea == "3")
                strArea = "南區";

            eecTotalApplied.Area = strArea;
            eecTotalApplied.SurplusValidCount = intTotalCount-intCurrentCount;
            eecTotalApplied.SurplusWaitCount = intTotalWait-intCurrentWaitCount;
            return eecTotalApplied;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string ShowClinicEecStatus(string Index)
    {
        try
        {
            // if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            //{
            //    connString = rootWebConfig.ConnectionStrings.ConnectionStrings["TCS"];
            //}
            sqlCon.ConnectionString = tcsConnectString;
            sqlCon.Open();

            SqlCommand sqlCmd = new SqlCommand("select 收件1 from ApplyCard a left join asuser b on (a.醫事代號=b.醫事代號) where a.申請項目='EEC' and b.索引編號=@paramIndex", sqlCon);

            //客戶編號
            SqlParameter spIndex = new SqlParameter("@paramIndex", Index);
            sqlCmd.Parameters.Add(spIndex);
            string strEecStatus = "尚未預約";
            var objEecStatus = sqlCmd.ExecuteScalar();
            if (objEecStatus != null)
            {
                if (objEecStatus.ToString().Trim() == "")
                {
                    strEecStatus = "已預約";
                }
                else if (objEecStatus.ToString().Trim() == "後補")
                {
                    strEecStatus = "後補";
                }
                else if (objEecStatus.ToString().Trim() == "取消")
                {
                    strEecStatus = "取消";
                }
            }
            sqlCmd.Dispose();
            sqlCon.Close();
            return strEecStatus;
        }
        catch (Exception ex)
        {   
            throw new Exception(ex.Message);
        }
    }
}
