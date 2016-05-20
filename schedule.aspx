﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="schedule.aspx.cs" Inherits="Schedule" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>    
    <meta http-equiv="content-type" charset="UTF-8"  name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <title>線上回報系統</title>
    <!-- 增加圖示icon -->
    <link rel="apple-touch-icon-precomposed" href="images/logo.png" />
    <link href="jquery.mobile/jquery.mobile-1.2.0.css" rel="stylesheet" />
    <script src="jquery.mobile/jquery-1.8.3.js"></script>
    <script src="jquery.mobile/jquery.mobile-1.2.0.min.js"></script>    
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
    <form id="Form1" runat="server">
        <!-- 顯示行程 -->
        <input type="hidden" id="hidden_schedule_account" runat="server" /><!-- asp.net和 javaScript 登入帳號 -->
        <input type="hidden" id="hidden_schedule_password" runat="server" /><!-- asp.net和 javaScript 登入帳號 -->

        <!--行程列表 -->
        <div data-role="page" id="schedule_list">
            <div data-role="header">                     
                    <h3>回報行程</h3>
                    <asp:LinkButton runat="server" data-inline="true" ID="btnScheduleList_LogOut" Text="登出" data-role="button"
                        data-icon="home" OnClick="btnScheduleList_LogOut_Click" data-mini="true"></asp:LinkButton>
                    
            </div>
            <div data-role="content">                
                <div id="schedule_list_eec"></div>
                <h3>
                    <a href="#" data-role="button" id="preDay" data-inline="true" data-mini="true">^</a>
                    <a href="#" id="today">顯示今日行程</a>
                    <a href="#" data-role="button" id="nextDay" data-inline="true" data-mini="true">v</a>
                    <a id="listAllSchedule" href="#" data-role="button" data-inline="true" data-mini="true" target="_blank">列出所有人的行程</a>              
                </h3>
                <input type="hidden" id="hidden_scheduleList_json" runat="server" /><!-- asp.net和javaScript 取回該帳號的行程-->
                <input type="date" id="scheduleDay" runat="server" /><br />
                <ul data-role="listview" data-theme="a" id="listClinic" data-filter="true"></ul>
            </div>
            <div data-role="footer" data-position="fixed" id="navorList_footer" data-id="schedule_footerPage">
                <div data-role="navbar">
                    <ul>
                        <li><a href="#schedule_list">行程列表</a></li>
                        <li><a href="#schedule_searchCustom">客戶資料</a></li>
                        <li><a href="#scheduleTrackt_newReport">新行程回報</a></li>
                    </ul>
                </div>
            </div>
        </div>     

         <!--查詢客戶 -->
        <div data-role="page" id="schedule_searchCustom">
            <div data-role="header">
                <h3>回報行程</h3>
                <a id="btnSearchCustom_back" data-ajax="false" data-role="button" data-icon="back" data-transition="slide">back</a>
            </div>
            <div data-role="content">
                <div data-role="fieldcontain">                  
                    <label for="textSearchCustom_clinicName" data-inline="true">查詢診所</label>
                    <input type="search" id="textSearchCustom_clinicName" placeholder="(查詢診所名稱)" />
                    <input type="hidden" id="hiddenSearchCustom_indexID" runat="server" data-inline="true" />
                </div>

                <ul id="ulSearchCustom_clinicListName" data-role="listview" data-inset="true">
                </ul>

                <ul data-role="listview" data-inset="true">
                    <li data-role="list-divider" >客戶基本資料</li>
                    <li>診所名稱:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_name"></asp:Label></span>
                    </li>
                    <li>
                        <label data-inline="true">診所住址:</label>
                        <span>
                            <img src="images/map.jpg" alt="查詢位置" width="34" height="25" data-inline="true">
                            <asp:Label runat="server" ID="lbSearchCustom_addr"></asp:Label>
                            <a href="javascript:void(0)" id="linkSearchCustom_addr" data-role="button" data-ajax="false" data-inline="true" data-mini="true" data-theme="c" data-transition="pop" runat="server">詳細地圖</a>
                        </span>
                    </li>
                    <li>客戶編號:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_index"></asp:Label>
                        </span>
                    </li>
                    <li>客戶電話:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_tel"></asp:Label>
                        </span>
                    </li>
                    <li>客戶電話2:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_tel2"></asp:Label>
                        </span>
                    </li>
                    <li>行動電話:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_mobile"></asp:Label>
                        </span>
                    </li>
                    <li>負責醫師:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_doctor"></asp:Label>
                        </span>
                    </li>
                    <li>聯絡人:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_conntactStaff"></asp:Label>
                        </span>
                    </li>
                    <li>維護到期日:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_maturityDate"></asp:Label>
                        </span>
                    </li>

                    <li>ip:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_ip"></asp:Label>
                        </span>
                    </li>

                    <li>註冊密碼:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_registerPassword"></asp:Label>
                        </span>
                    </li>

                    <li>更版密碼:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_editionPassword"></asp:Label>
                        </span>
                    </li>

                    <li>簡訊:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_SMS"></asp:Label>
                        </span>
                    </li>

                    <li>巡迴:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_tour"></asp:Label>
                        </span>
                    </li>

                    <li>口檢:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_oralChecking"></asp:Label>
                        </span>
                    </li>
                    <li>電子病歷:
                        <span>
                            <asp:Label runat="server" ID="lbSearchCustom_HMR"></asp:Label>
                        </span>
                    </li>
                    <li>服務次數:
                            <span>來電次數
                                <asp:Label runat="server" ID="lbSearchCustom_telCount"></asp:Label>
                                /
                                到府服務次數
                                <asp:Label runat="server" ID="lbSearchCustom_serviceCount"></asp:Label>
                                <a href="#schedule_serviceDetail" id="btnSerchCustom_ServiceDetail" data-role="button" data-inline="true" data-mini="true" data-theme="c" data-transition="pop" data-rel="dialog">詳細內容</a>
                                <a href="#" data-role="button" id="bugReport" data-mini="true" data-inline="true" data-theme="c"  >回報bug</a>
                                <a href="#" data-role="button" id="Icd10BugReport" data-mini="true" data-inline="true" data-theme="c"  >回報ICD10的Bug</a>
                            </span>
                    </li>
                   <%-- <li>合約:
                            <span>合約金額
                                <asp:Label runat="server" ID="lbSearchCustom_contractMoney"></asp:Label>
                                /
                                合約等級
                                <asp:Label runat="server" ID="lbSearchCustom_contractLeavel"></asp:Label>
                            </span>
                    </li>--%>
                </ul>
                <%--<fieldset data-role="controlgroup" data-type="horizontal" name="ICD10" id="ICD10">                  --%>
                <div id="showICD10">
                    <fieldset data-role="controlgroup">
                        <legend>ICD10顯示:</legend>
                        <input type="radio" name="ICD10" id="radioSearchCustom_no" value="0" >
                        <label for="radioSearchCustom_no">否</label>
                        <input type="radio" name="ICD10" id="radioSearchCustom_yes" value="1">
                        <label for="radioSearchCustom_yes">是</label>                   
                    </fieldset>
                     <fieldset data-role="controlgroup">
                        <legend>ICD10是否預檢完成:</legend>
                            <label>
                                <input type="checkbox" name="checkbox-0" id="checkboxSearchCustom_check">預檢完成
                            </label>
                           <%--<input type="checkbox" name="ICD10_CheckOK" id="checkboxSearchCustom_check" >--%>
                           <%--<label for="checkboxSearchCustom_check">否</label>--%>
                     </fieldset>
                 <a href="#" data-role="button" id="btnSearchCustom_SaveICD10">結果送出</a>
                </div>
                <br />
                  <fieldset data-role="controlgroup">
                      <legend>EEC預約狀況:</legend>
                      <input type="text" id="schedule_searchCustom_eecStatus" style="color:blue"  readonly/>
                  </fieldset>

<%--                <fieldset data-role="controlgroup">
                    <legend>診所設定時間:</legend>
                    <select id="schedule_searchCustom_dutyWeek">
                        <option value="1">星期一</option>
                        <option value="2">星期二</option>
                        <option value="3">星期三</option>
                        <option value="4">星期四</option>
                        <option value="5">星期五</option>
                        <option value="6">星期六</option>
                    </select>
                    <br />
                    <div class="ui-grid-b">

                        <input type="hidden" id="hiddenSearchCustom_dutyTimeJson" />
                        <div class="ui-block-a">
                            <div class="ui-bar ui-bar-b" style="height: 160px;">早上時段</div>
                        </div>

                        <div class="ui-block-b">
                            <div class="ui-bar ui-bar-c" style="height: 160px">
                                起始時間<input id="morning_start" type="number" min="-9999" max="9999" />
                                結束時間<input id="morning_end" type="number"  min="-9999" max="9999"/>
                            </div>
                        </div>
                        <div class="ui-block-a">
                            <div class="ui-bar ui-bar-b" style="height: 160px;">中午時段</div>
                        </div>
                        <div class="ui-block-b">
                            <div class="ui-bar ui-bar-c" style="height: 160px">
                                起始時間<input id="noon_start" type="number" max="9999" min="-9999" />
                                結束時間<input id="noon_end" type="number" max="9999" min="-9999" />
                            </div>
                        </div>

                        <div class="ui-block-a">
                            <div class="ui-bar ui-bar-b" style="height: 160px;">晚上時段</div>
                        </div>
                        <div class="ui-block-b">
                            <div class="ui-bar ui-bar-c" style="height: 160px">
                                起始時間<input id="night_start"type="number" max="9999"  min="-9999"/>
                                結束時間<input id="night_end" type="number"max="9999" min="-9999" />
                            </div>
                        </div>
                    </div>
                </fieldset>                         
                <a href="#" data-role="button" id="schedule_searchCustom_saveClinicDutyTime" >儲存診所時間</a>
                <table id="schedule_searchCustom_weekDutyTime" data-role="table" style="border:3px dashed; width:100%" id="table-column-toggle" data-mode="columntoggle" class="ui-responsive table-stroke">
                    <thead>
                        <tr style="background-color:silver;align-content:center">
                            <th data-priority="2">星期</th>
                            <th>早上時段</th>
                            <th data-priority="3">中午時段</th>
                            <th data-priority="1">晚上時段</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>--%>
              
            </div>
            <div data-role="footer" data-position="fixed" id="navorSearchCustom_footer" data-id="schedule_footerPage">
                <div data-role="navbar">
                    <ul>
                        <li><a href="#schedule_list">行程列表</a></li>
                        <li><a href="#schedule_searchCustom">客戶資料</a></li>
                        <li><a href="#scheduleTrackt_newReport">新行程回報</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <!--新行程回報-->
        <div data-role="page" id="scheduleTrackt_newReport">
            <div data-role="header" data-backbtn="false">
                <a id="btnNewReport_back" data-ajax="false" data-role="button" data-icon="back" data-transition="slide">back</a>
                <h3>新行程回報</h3>
            </div>
            <div data-role="content">
               <div data-role="fieldcontain">
                   <label for="textNewReport_clinicName">派工日期</label>
                   <asp:TextBox ID="dateNewReport_dispatchingDate" placeholder="範例:1040101" runat="server" MaxLength="7"  onkeyup="this.value=this.value.replace(/[^0-9]/g,'')" ></asp:TextBox>
                    <%--<input type="text" id="dateNewReport_dispatchingDate"  runat="server" maxlength="7" placeholder="範例:1040101"  />--%>
                   <h3>
                        <label runat="server" id="messageNewReport_dispatchingDate" class="message">派工日期請勿空白</label></h3>
               </div>

                <div data-role="fieldcontain">
                    <label for="textNewReport_clinicName">醫事機構診所名稱</label>
                    <input type="text" id="textNewReport_clinicName" placeholder="(可直接查詢診所名稱)" />
                    <input type="hidden" id="hiddenNewReport_customerID" runat="server" />
                    <input type="hidden" id="hiddenNewReport_indexID" runat="server" />
                    <ul id="ulNewReport_clinicListName" data-role="listview" data-inset="true">
                    </ul>
                    <h3>
                        <label runat="server" id="messageNewReport_clinicName" class="message">診所名稱請勿空白</label></h3>
                </div>

                <div data-role="fieldcontain">
                    <label for="txtNewReport_problem">診所問題</label>
                    <textarea id="txtNewReport_problem" runat="server"></textarea>
                    <h3>
                        <label runat="server" id="messageNewReport_problem" class="message">診所問題勿空白</label></h3>
                </div>

                <div data-role="fieldcontain">
                    <label for="dropNewReport_getQusttype">問題類別</label>
                    <asp:DropDownList runat="server" ID="dropNewReport_getQusttype" data-theme="b"></asp:DropDownList>
                </div>
                <div data-role="fieldcontain">
                    <fieldset data-role="controlgroup">
                        <legend>處理狀況</legend>
                        <input type="radio" name="reportNewStatus" id="radioNewReport_complete" value="完成" />
                        <label for="radioNewReport_complete">完成</label>

                        <input type="radio" name="reportNewStatus" id="radioNewReport_unComplete" value="未完成" />
                        <label for="radioNewReport_unComplete">未完成</label>

                        <input type="radio" name="reportNewStatus" id="radioNewReport_reSchedule" value="需再排程" />
                        <label for="radioNewReport_reSchedule">需再排程</label>

                        <input type="radio" name="reportNewStatus" id="radioNewReport_getBackComputer" value="取回電腦檢測" />
                        <label for="radioNewReport_getBackComputer">取回電腦檢測</label>

                        <input type="radio" name="reportNewStatus" id="radioNewReport_attachment" value="附件" />
                        <label for="radioNewReport_attachment">附件</label>

                        <input type="radio" name="reportNewStatus" id="radioNewReport_other" value="其他" />
                        <label for="radioNewReport_other">其他</label>
                        <!-- for asp.net -->
                        <input type="hidden" id="hiddenNewReport_status" runat="server" />
                    </fieldset>
                </div>

             

                <div data-role="fieldcontain">
                    <label for="textNewReport_proessResult">處理結果</label>
                    <textarea cols="40" rows="8" placeholder="請填寫,最多只輸入100中文字，英文最多輸入200字元" maxlength="100" name="textarea" id="textNewReport_proessResult" runat="server"></textarea>
                </div>

                  <div data-role="fieldcontain">
                      <label for="txtNewReport_officeOperate">是否內勤作業</label>
                      <label>
                            <input type="checkbox"  name="txtNewReport_officeOperate" id="txtNewReport_officeOperate" runat="server">是否內勤作業
                      </label>                                                                                      
                 </div>

                <div data-role="fieldcontain">
                    <label for="btnNewReport_save">存檔</label>
                    <a id="btnNewReport_confirmSave" data-role="button" data-theme="c" data-transition="pop" data-rel="dialog">存檔</a>

                </div>
            </div>
            <div data-role="footer" data-position="fixed" id="navorScheduleNewReport_footer" data-id="schedule_footerPage">
                <div data-role="navbar">
                    <ul>
                        <li><a href="#schedule_list">行程列表</a></li>
                        <li><a href="#schedule_searchCustom">客戶資料</a></li>
                        <li><a href="#scheduleTrackt_newReport">新行程回報</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <!--新行程回報的儲存確認畫面-->
        <div id="scheduleTract_newReport_saveConfirm" data-role="page">
            <div data-role="header" data-backbtn="false">
                <h3>儲存視窗</h3>
            </div>
            <div data-role="content">
                <div data-role="fieldcontain">
                    <div data-role="fieldcontain">
                        <asp:LinkButton runat="server" data-role="button" Text="確認儲存" ID="btnNewReport_save" OnClick="btnNewReport_save_Click"></asp:LinkButton>
                        <asp:LinkButton ID="btnNewReport_cancel" runat="server" data-role="button" Text="取消" data-rel="back"></asp:LinkButton>
                    </div>
                </div>
            </div>
            <div data-role="footer">
                <h3></h3>
            </div>
        </div>

        
        <div data-role="page" id="schedule_serviceDetail">
            <div data-role="header">
                <h3>詳細內容</h3>
            </div>
            <div data-role="content">
                <ul id="serviceDetail_list" data-role="listview"  style="white-space: normal" data-filter="true"></ul>
            </div>
            <div data-role="footer"><div></div></div>
        </div>
    </form>
      <script src="jquery/date.js"></script>
      <script src="jquery/jquery.base64.js"></script>
      <script src="jquery/schedule_pageinit.js"></script>
      <script src="jquery/serverDetail.js"></script>    
</body>
</html>