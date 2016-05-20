<%@ Page Language="C#" AutoEventWireup="true" CodeFile="scheduleTrack.aspx.cs" Inherits="ScheduleTrack" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="jquery.mobile/jquery.mobile-1.2.0.css" rel="stylesheet" />
    <script src="jquery.mobile/jquery-1.8.3.js"></script>
    <script src="jquery.mobile/jquery.mobile-1.2.0.min.js"></script>
    <title>行程回報</title>
    <!-- 增加圖示icon -->
    <link rel="apple-touch-icon" href="images/logo.png" />
    <style>
        .message {
            color: red;
            text-align: right;
            float: right;
            width: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <input type="hidden" id="hidden_scheduleTrack_account" runat="server" /><!-- 登入帳號 -->
        
        <!--行程回報-->
        <div data-role="page" id="scheduleTrackt_report">
            <div data-role="header">
                <!--<a id="btnReport_back" href="schedule.aspx?workDay=2013-02-04" data-ajax="false" data-role="button" data-icon="back" data-transition="slide">back</a>-->
                <a id="btnReport_back" data-ajax="false" data-role="button" data-icon="back" data-transition="slide">back</a>
                <h3>行程回報</h3>
            </div>
            <div data-role="content">
                <input type="hidden" id="hiddensKeyID" runat="server" />
                <!-- asp.net 傳回keyid -->
                <div data-role="fieldcontain">
                    <label for="btnReport_arriveTime">到達時間</label>
                    <h3><span id="spanReport_arriveTime"></span></h3>
                    <a data-role="button" id="btnReport_arriveTime" data-theme="c">抵達時間</a>
                    <h3>
                        <label runat="server" id="messageReport_arriveTime" for="dropReport_getQusttype" class="message">抵達時間請勿空白</label></h3>
                </div>
                <div data-role="fieldcontain">
                       <input type="hidden" id="scheduleTrackt_report_customerIndex" runat="server" />

                    <table data-role="table" border="1" id="scheduleTrackt_report_allProducts" data-mode="reflow" class="ui-responsive table-stroke">
                          <thead>
                            <tr style="border:groove">
                              <th data-priority="1">產品名稱</th>
                              <th data-priority="2">合約到期日</th>
                            </tr>
                          </thead>
                          <tbody>
                          </tbody>
                     </table>
                       <%--<ul data-role="listview" data-theme="a" id="scheduleTrackt_report_allProducts" data-filter="true" data-inset="true"></ul>--%>
                </div>
                <div data-role="fieldcontain">
                    <label for="btnReport_overTime">結束時間</label>
                    <h3><span id="spanReport_overTime"></span></h3>
                    <a data-role="button" id="btnReport_overTime" data-theme="c">結束時間</a>
                    <h3>
                        <label runat="server" id="messageReport_overTime" for="dropReport_getQusttype" class="message">結束時間請勿空白</label></h3>
                </div>
                <br />
                <hr />

                <!-- 取出問題類別資料 -->
                <div data-role="fieldcontain">
                    <label for="lbReport_clinicName">診所名稱</label>
                    <h4><span>
                        <asp:Label runat="server" ID="lbReport_clinicName"></asp:Label></span></h4>
                </div>

                <div data-role="fieldcontain">
                    <label for="lbReport_staff">服務人員</label>
                    <h4><span>
                        <asp:Label runat="server" ID="lbReport_staff"></asp:Label></span></h4>
                </div>

                <div data-role="fieldcontain">
                    <label for="lbReport_customStatus">客戶問題</label>
                    <h4><span><asp:Label runat="server" ID="lbReport_customStatus"></asp:Label></span></h4>
                </div>

                <div data-role="fieldcontain">
                    <label for="dropReport_getQusttype">問題類別</label>
                    <asp:DropDownList runat="server" ID="dropReport_getQusttype" data-theme="b"></asp:DropDownList>
                    <!-- for asp.net -->
                    <input type="hidden" runat="server" id="hiddenReport_questtype"  />
                    <h3>
                        <label runat="server" id="messgeReport_questtype" for="dropReport_getQusttype" class="message">問題類別請勿空白</label></h3>
                </div>

                <!-- 判別勿空值 -->
                <div data-role="fieldcontain">
                    <fieldset data-role="controlgroup">
                        <legend>處理狀況</legend>
                        <input type="radio" name="reportStatus" id="radioReport_complete" value="完成" />
                        <label for="radioReport_complete">完成</label>

                        <input type="radio" name="reportStatus" id="radioReport_unComplete" value="未完成" />
                        <label for="radioReport_unComplete">未完成</label>

                        <input type="radio" name="reportStatus" id="radioReport_reSchedule" value="需再排程" />
                        <label for="radioReport_reSchedule">需再排程</label>

                        <input type="radio" name="reportStatus" id="radioReport_getBackComputer" value="取回電腦檢測" />
                        <label for="radioReport_getBackComputer">取回電腦檢測</label>

                        <input type="radio" name="reportStatus" id="radioReport_attachment" value="附件" />
                        <label for="radioReport_attachment">附件</label>

                        <input type="radio" name="reportStatus" id="radioReport_other" value="其他" />
                        <label for="radioReport_other">其他</label>
                        <!-- for asp.net -->

                        <h3>
                            <label runat="server" id="messageReport_status" class="message">處理狀況勿空白</label></h3>
                        <input type="hidden" id="hiddenReport_status" runat="server" />
                    </fieldset>
                </div>

                <div data-role="fieldcontain">
                    <label for="textReport_proessResult">處理結果</label>
                    <textarea cols="40" rows="8" placeholder="請填寫,最多只輸入100中文字" maxlength="100" name="textarea" id="textReport_proessResult" runat="server"></textarea>
                </div>
                <div data-role="fieldcontain">
                    <label for="btnReport_save">存檔</label><span id="span1"></span>
                    <a href="#scheduleTract_Report_saveConfirm" data-role="button" id="btnReport_save" data-theme="c" data-transition="pop" data-rel="dialog">存檔</a>
                </div>

            </div>
            <div data-role="footer" data-position="fixed" id="footerPage_2" data-id="footerPage">
                <div data-role="navbar">
                    <ul>

                        <li><a href="#scheduleTrackt_report">行程回報</a></li>
                        <li><a href="#scheduleTract_customer">簡易客戶基本資料</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <!--行程回報的儲存確認畫面-->
        <div id="scheduleTract_Report_saveConfirm" data-role="page">
            <div data-role="header" data-backbtn="false">
                <h3>儲存視窗</h3>
            </div>
            <div data-role="content">
                <div data-role="fieldcontain">
                    <asp:LinkButton runat="server" data-role="button" Text="確認儲存" ID="confirmSave" OnClick="confirmSave_Click"></asp:LinkButton>
                    <asp:LinkButton ID="confirmCancle" runat="server" data-role="button" Text="取消" data-rel="back"></asp:LinkButton>
                </div>
            </div>
            <div data-role="footer">
                <h3></h3>
            </div>
        </div>


        <!--客戶基本資料-->
        <div data-role="page" id="scheduleTract_customer">
            <div data-role="header" data-backbtn="false">
                <a id="btnCustom_back" data-ajax="false" data-role="button" data-icon="back" data-transition="slide">back</a>
                <h3>簡易客戶基本資料</h3>
            </div>

            <div data-role="content">
                <input type="hidden" id="hiddenCustomer_json" runat="server" />
                <input type ="hidden" id="hiddenCustomServerice_json" runat="server" />

                <div data-role="collapsible-set" data-theme="a" data-content-theme="d" data-inset="false">
                <div data-role="collapsible" data-collapsed="true">
                    <h2>基本資料</h2>
                    <ul data-role="listview" data-inset="true">
                        <li data-role="list-divider">客戶基本資料</li>
                        <li>診所名稱:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_name"></asp:Label></span>
                        </li>
                        <li>客戶編號:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_index"></asp:Label></span>
                        </li>
                        <li>診所住址:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_addr"></asp:Label></span>
                        </li>
                        <li>客戶電話:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_tel"></asp:Label>
                        </span>
                        </li>
                        <li>客戶電話2:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_tel2"></asp:Label>
                        </span>
                        </li>
                        <li>行動電話:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_mobile"></asp:Label>
                        </span>
                        </li>
                        <li>負責醫師:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_doctor"></asp:Label>
                        </span>
                        </li>
                        <li>聯絡人:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_conntactStaff"></asp:Label>
                        </span>
                        </li>
                        <li>維護到期日:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_maturityDate"></asp:Label>
                        </span>
                        </li>

                        <li>ip:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_ip"></asp:Label>
                        </span>
                        </li>

                        <li>註冊密碼:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_registerPassword"></asp:Label>
                        </span>
                        </li>

                        <li>更版密碼:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_editionPassword"></asp:Label>
                        </span>
                        </li>

                        <li>簡訊:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_SMS"></asp:Label>
                        </span>
                        </li>

                        <li>巡迴:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_tour"></asp:Label>
                        </span>
                        </li>

                        <li>口檢:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_oralChecking"></asp:Label>
                        </span>
                        </li>

                        <li>電子病歷:
                        <span>
                            <asp:Label runat="server" ID="lbCustom_HMR"></asp:Label>
                        </span>
                        </li>
                        <li>服務次數:
                            <span>
                                來電次數
                                <asp:Label runat="server" ID="lbCustom_telCount"></asp:Label>
                                /
                                到府服務次數
                                <asp:Label runat="server" ID="lbCustom_serviceCount"></asp:Label>
                                <a href="#scheduleTrack_ServiceList" id="btnCustom_ServiceDetail" data-role="button" data-inline="true" data-mini="true" data-theme="c" data-transition="pop" data-rel="dialog">詳細內容</a>
                            </span>
                        </li>
                        <%--<li>合約:
                            <span>合約金額
                                <asp:Label runat="server" ID="lbCustom_contractMoney"></asp:Label>
                                /
                                合約等級
                                <asp:Label runat="server" ID="lbCustom_contractLeavel"></asp:Label>
                            </span>
                        </li>--%>
                    </ul>
                </div>

              

                <!--資料來源來自hidden_custom_js-->
                <div data-role="collapsible">
                    <h2>線上調動</h2>
                    <div id="divCustomer_transfer">                        
                        <div data-role="fieldcontain" data-theme="c">
                            <a href="#scheduleTract_allUser" data-role="button" data-rel="dialog" data-transition="pop">查詢所有人員</a>      
                        </div>
                        <div data-role="fieldcontain" data-theme="c">
                            <label for="dropCustomer_tranferEmp1">調動人員1</label>
                            <asp:DropDownList runat="server" ID="dropCustomer_tranferEmp1"></asp:DropDownList>
                        </div>
                        <div data-role="fieldcontain" data-theme="c">
                            <label for="dropCustomer_tranferEmp2">調動人員2</label>
                            <asp:DropDownList runat="server" ID="dropCustomer_tranferEmp2"></asp:DropDownList>

                        </div>
                        <div data-role="fieldcontain" data-theme="c">
                            <label for="dropCustomer_tranferEmp3">調動人員3</label>
                            <asp:DropDownList runat="server" ID="dropCustomer_tranferEmp3"></asp:DropDownList>
                        </div>
                        <div data-role="fieldcontain" data-theme="c">
                            <asp:LinkButton runat="server" data-role="button" ID="btnCustomer_transfer" Text="確定調動" OnClick="btnCustomer_transfer_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>

                 

                </div>
            </div>

            <div data-role="footer" data-id="footerPage" data-position="fixed" id="footerPage_1">
                <div data-role="navbar">
                    <ul>
                        <li><a href="#scheduleTrackt_report">行程回報</a></li>
                        <li><a href="#scheduleTract_customer">簡易客戶基本資料</a></li>
                    </ul>
                </div>
            </div>

        </div>



        <!--檢視來電畫面-->
        <div data-role="page" id="scheduleTrack_ServiceList">
            <div data-role="header">
                <h3>詳細內容</h3>
            </div>
            <div data-role="content">
                <ul id="scheduleTrack_serviceDetail_list" data-role="listview" style="white-space: normal" data-filter="true"></ul>
            </div>
            <div data-role="footer">
                <div></div>
            </div>
        </div>

        <!--檢視所有人員畫面-->
        <div id="scheduleTract_allUser" data-role="page">
            <div data-role="header" data-backbtn="false">
                <h3>所有人員行程</h3>
            </div>
            <div data-role="content">
                <asp:HiddenField runat="server" ID="hiddenAllUser_scheduleDay" />
                <ul data-role="listview" data-theme="a" id="liAllUser_schedules" data-filter="true" data-inset="true"></ul>
                <asp:LinkButton ID="btnAllUser_close" runat="server" data-role="button" Text="確定" data-rel="back"></asp:LinkButton>
            </div>
            <div data-role="footer">
                   
            </div>
        </div>        
    </form>
     <script src="jquery/date.js"></script>
    <script src="jquery/scheduleTrack_pageinit.js"></script>
    <script src="jquery/serverDetail.js"></script>
</body>
</html>
