using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

// 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IGetScheduleData"。
[ServiceContract]

public interface IGetScheduleData
{
    /// <summary>
    /// 該天的登入者行程
    /// </summary>
    /// <param name="Account">登入帳號</param>
    /// <param name="WorkDay">行程日期</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/GetJsonScheduleData/Account/{Account}/WorkDay/{WorkDay}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
    string GetJsonScheduleData( string Account, string WorkDay );


    /// <summary>
    /// 該天的所有人的行程
    /// </summary>    
    /// <param name="WorkDay">行程日期</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/GetAllScheduleData/Account/{Account}/WorkDay/{WorkDay}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
    string GetAllScheduleData(string Account, string WorkDay);

    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/GetJsonAllUserScheduleData/WorkDay/{WorkDay}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
    List<AllUserSchedule> GetJsonAllUserScheduleData(string WorkDay);

    /// <summary>
    /// 行程回報紀錄
    /// </summary>
    /// <param name="strKeyId"></param>
    /// <returns></returns>
    [OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]
    string GetReportRec(string strKeyId);

    /// <summary>
    /// 問題項目類別
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    [OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]
    Dictionary<string,string> GetQusttype(string condition);

    /// <summary>
    /// 所有員工列表
    /// </summary>
    /// <returns></returns>
    [OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]
    Dictionary<string, string> GetEmployees();

    /// <summary>
    /// 客戶基本資料
    /// </summary>
    /// <returns></returns>
    //[OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]

    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/GetCustomerData/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
    Customer GetCustomerData(string Index);

    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/CheckIn/Skey/{Skey}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
    string CheckIn(string Skey);

    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "/CheckOut/Skey/{Skey}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
    string CheckOut(string Skey);

     [OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]
     bool SaveReportSchedule(string skey, string strQusttype, string strResult, string strStatus, bool isClosed);

     [OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]
     bool SaveNewSchedule(string strCustomerID, string strIndexID, string strProblem, string strQusttype, string strResult, string strStatus, string strEmpID, string strDispatchDate, bool isOfficeOperate);

     

     [OperationContract, WebGet(RequestFormat = WebMessageFormat.Json)]
     bool SaveTransferEmployee(string skey, string strEmp1, string strEmp2, string strEmp3, string strLoginUser);

     
     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/GetCustomServiceCount/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     ServericeCount GetCustomServiceCount(string Index);


     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/ClinicList/Name/{Name}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     string ClinicList(string Name);


     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/SetICD10/UserID/{UserID}/Index/{Index}/Result/{Result}/PreviewCheck/{PreviewCheck}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     int SetICD10(string UserID, string Index, string Result,string PreviewCheck);


     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/ReadICD10/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     string ReadICD10(string Index);


     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/FindReservationICD10/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     int FindReservationICD10(string Index);

     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/GetClinicAllProductContract/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     List<Product> GetClinicAllProductContract(string Index);

     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/SaveClinicDutyTime/{Index}/{WeekDay}/{MorningStart}/{MorninigEnd}/{NoonStart}/{NoonEnd}/{NightStart}/{NightEnd}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     bool SaveClinicDutyTime(string Index, string WeekDay, string MorningStart, string MorninigEnd, string NoonStart, string NoonEnd, string NightStart, string NightEnd);

     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/ReadClinicDutyTime/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     List<ClinicDuty> ReadClinicDutyTime(string Index);

     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/ShowEecCurrentCount/UserID/{userID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     EEC ShowEecCurrentCount(string userID);

     [OperationContract]
     [WebInvoke(Method = "GET", UriTemplate = "/ShowClinicEecStatus/Index/{Index}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
     string ShowClinicEecStatus(string Index);

    
}

[DataContract]
public struct Customer
{

    [DataMember]
    /*索引編號*/
    public string index { get; set; }

    [DataMember]
    public string name { get; set; }

    [DataMember]
    public string tel { get; set; }

    [DataMember]
    public string tel2 { get; set; }

    [DataMember]
    public string mobile { get; set; }

    [DataMember]
    /*負責人*/
    public string responsibleUser { get; set; }

    [DataMember]
    /*聯繫人*/
    public string conntactStaff { get; set; }

    [DataMember]
    public string address { get; set; }

    [DataMember]
    /*到期日期*/
    public string maturityDate { get; set; }

    [DataMember]
    public string ip { get; set; }

    [DataMember]
    /*註冊密碼*/
    public string registerPassword { get; set; }

    [DataMember]
    /*更版密碼*/
    public string editionPassword { get; set; }

    [DataMember]
    /*簡訊*/
    public string sms { get; set; }

    [DataMember]
    /*巡迥*/
    public string tour { get; set; }

    [DataMember]
    public string oralChecking { get; set; }

    [DataMember]
    /*合約金額*/
    public string contractMoney { get; set; }

    [DataMember]
    /*合約等級*/
    public string contractLeavel { get; set; }

    [DataMember]
    public string EMR { get; set; }
}

[DataContract]
public struct ServericeCount
{
    [DataMember]
    /*合約金額*/
    public string telCount { get; set; }

    [DataMember]
    /*合約等級*/
    public string serviceCount { get; set; }


    [DataMember]
    public string tempTable { get; set; }
}

[DataContract]
public struct AllUserSchedule
{
    [DataMember]
    /*員工編號*/
    public string userID { get; set; }

    [DataMember]
    /*員工姓名*/
    public string userName { get; set; }

    [DataMember]
    /*所有行程*/
    public List<string> schedules { get; set; }
}

[DataContract]
public struct Product
{
    [DataMember]
    /*產品名稱*/
    public string productName
    {
        get;
        set;
    }

    [DataMember]
    /*合約到期日*/
    public string contractEndDate
    {
        get;
        set;
    }
}

[DataContract]
public struct ClinicDuty
{
     [DataMember]
    public string weekDay
    {
        get;
        set;
    }
     [DataMember]
    public string morningStart
    {
        get;
        set;
    }
     [DataMember]
    public string morningEnd
    {
        get;
        set;
    }
     [DataMember]
    public string noonStart
    {
        get;
        set;
    }
     [DataMember]
    public string noonEnd
    {
        get;
        set;
    }
     [DataMember]
    public string nightStart
    {
        get;
        set;
    }
     [DataMember]
    public string nightEnd
    {
        get;
        set;
    }
}

[DataContract]
public struct EEC
{

    [DataMember]
    public string Area
    {
        get;
        set;
    }

     [DataMember]
    public int SurplusValidCount
    {
        get;
        set;
    }

     [DataMember]
    public int SurplusWaitCount
    {
        get;
        set;
    }
}