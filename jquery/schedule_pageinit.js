/// <reference path="jquery.base64.min.js" />
/// <reference path="base64.js" />
$(document).ready(function () {
   
    //$.getScript("jquery.base64.min.js");
    var now = new Date();
    //$("#scheduleDay").val(now.toJSON().substring(0, 10));
    if ($.browser.msie == true) {
        // IE
        switch ($.browser.version) {
            case 5:
                // 靠么,還有人用.
                break;
            case 6:
                // 萬惡的IE6
                break;
            case 7:
                // 小進步的IE7
                break;
            case 8:
                // XP到頂就它了
                break;
            case 9:
                // 歐歐歐,有進步
              
                break;
            default:
                break;
        }
    }
    if ($.browser.mozilla == true) {
        // Firefox Mozilla
       
    }
    if ($.browser.opera == true) {
        // Opera
    }
    if ($.browser.safari == true) {
        // Apple Safari
    }
    if ($.browser.webkit == true) {
        // Webkit系列: Chrome、lunascape      
    }
})


var errorMsg = true;


// 如果有檢核成功 即可前往下一頁 ，否則停留在該頁
var showMessage = function ($messageID, $checkOk) {
    if ($checkOk) {
        $($messageID).hide();
        return 0;
    } else {
        $($messageID).show();
        return 1; /* 錯誤碼  可累加*/
    }
}

/* 將json資料放入<li></li>節點中 */
var showSchedule = function ($jsonData) {
    $("#listClinic").empty();

    var data = "";
    var obj = ($jsonData.length >0) ? jQuery.parseJSON($jsonData) : "";

    data += "<li data-role='list-divider'><h3>我的行程</h3></li>";
    for (var i = 0; i < obj.length; i++) {
        image = (obj[i].結束時間 == null) ? "help.jpg" : "success.jpg";
        staff = (obj[i].服務人員 == null) ? "無" : obj[i].服務人員;
        arriveTime = (obj[i].應到 <= "00:00") ? "" : "<font color=gree>時間(" + obj[i].應到 + ")</font>";
        

        data += "<li>";
        data += "<a href=scheduleTrack.aspx?id=" + obj[i].skey + "&workday=" + $("#scheduleDay").val() + " data-ajax=false>";        
        data += "<image src='images/" + image + "' width=100%>"; /*顯示是否已完成*/
        data += "<h4>" + obj[i].客戶名稱 + "/" + staff + "</h4>";
        data += "<p>" + arriveTime + " "+ obj[i].客戶問題 + "</p>";
        data += "</a>";
        data += "<a href='schedule.aspx?indexID=" + obj[i].索引編號 + "&workday=" + $("#scheduleDay").val() + "#schedule_searchCustom' data-ajax=false>客戶資料</a>";
        data += "</li>";
    }

    $("#listClinic").append(data);
    $("#listClinic").listview("refresh");
}

//客戶的醫事機構代號 
function setClinicID($IndexID, $clinicName) {
    if ($clinicName.length > 0) {
        $("#textSearchCustom_clinicName").val($clinicName);
        $("#ulSearchCustom_clinicListName").empty(); /*清空列表內容*/
        $("#hiddenSearchCustom_indexID").val($IndexID);       
        callCustomData();   /*客戶基本資料*/
        callServiceCount(); /*服務電話*/
        showICD10(); /*取得是否要icd10*/
        showEEC(); /*顯示EEC預約狀況*/
        //showClinicDutyTime(); /*顯示診所時段*/
    } else {
        $("#textSearchCustom_clinicName").attr("placeHolder", "查無客戶編號 請重新選擇");
        $("#ulSearchCustom_clinicListName").empty(); /*清空列表內容*/
        $("#hiddenSearchCustom_indexID").val("");
    }
}

//顯示客戶的基本資訊
function callCustomData() {
    $.ajax({
        url: "GetScheduleData.svc/GetCustomerData/Index/" + $("#hiddenSearchCustom_indexID").val(),
        dataType: "json",
        success: function (data) {
            if (!!(data) == true) {
                $("#lbSearchCustom_index").text(data.index);
                $("#lbSearchCustom_name").text(data.name);
                $("#lbSearchCustom_addr").text(data.address);
                $("#lbSearchCustom_tel").text(data.tel);
                $("#lbSearchCustom_tel2").text(data.tel2);
                $("#lbSearchCustom_mobile").text(data.mobile);
                $("#lbSearchCustom_doctor").text(data.responsibleUser);
                $("#lbSearchCustom_conntactStaff").text(data.conntactStaff);
                $("#lbSearchCustom_maturityDate").text(data.maturityDate);
                $("#lbSearchCustom_ip").text(data.ip);
                $("#lbSearchCustom_registerPassword").text(data.registerPassword);
                $("#lbSearchCustom_editionPassword").text(data.editionPassword);
                $("#lbSearchCustom_HMR").text(data.EMR);
                $("#lbSearchCustom_SMS").text(data.sms);
                $("#lbSearchCustom_tour").text(data.tour);
                $("#lbSearchCustom_oralChecking").text(data.oralChecking);
                //$("#lbSearchCustom_contractMoney").text(data.contractMoney);
                //$("#lbSearchCustom_contractLeavel").text(data.contractLeavel);

                /*傳送地址到googleMap*/
                var url = "scheduleMap.aspx?addr=" + $("#lbSearchCustom_addr").text();              
                //$("#linkSearchCustom_addr").attr("href", url);
                $("#linkSearchCustom_addr").attr("onclick", "window.open('" + url + "', '', '')");
                var ur2 = "../JQueryMobile/geolocation.aspx?userID=" + encodeURIComponent($("#hidden_schedule_account").val()) + "&address=" + encodeURI($("#lbSearchCustom_addr").text() + "&range=300"  );
                $("#linkSearchCustom_sales").attr("onclick", "window.open('" + ur2 + "', '', '')");
            }
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
}

function callServiceCount()
{
    $.ajax({
        url: "GetScheduleData.svc/GetCustomServiceCount/Index/" + $("#hiddenSearchCustom_indexID").val(),
        dataType: "json",
        success: function (data) {
            if (!!(data) == true) {
                $("#serviceDetail_list").empty(); //清空之前的資料
                $("#lbSearchCustom_telCount").text(data.telCount);
                $("#lbSearchCustom_serviceCount").text(data.serviceCount);          
                callServiceDetail(data.tempTable, "#serviceDetail_list"); //將服務細項列表顯示
                
            }
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
}

//顯示客戶是否為ICD10
function showICD10() {
    $.ajax({
        url: "GetScheduleData.svc/ReadICD10/Index/" + $("#hiddenSearchCustom_indexID").val(),
        dataType: "json",
        success: function (data) {
            var dataArray = data.split(',');
            if (dataArray[0] == "無線上預約") {
                $("#showICD10").hide();                
            } else {
                if (dataArray[0] == "ICD10")
                {
                    $('#radioSearchCustom_no').attr('checked', false).checkboxradio("refresh");
                    $('#radioSearchCustom_yes').attr('checked', true).checkboxradio("refresh");
                } else {
                    $('#radioSearchCustom_yes').attr('checked', false).checkboxradio("refresh");
                    $('#radioSearchCustom_no').attr('checked', true).checkboxradio("refresh");
                    //如果預設為 『否』 尚未安裝ICD10成功，所以無法勾選檢核成功
                    $('#checkboxSearchCustom_check').attr('disabled', "disabled").checkboxradio("refresh");
                }

                if (dataArray[1] == "PREVIEWOK") {
                    $('#checkboxSearchCustom_check').attr('checked',true).checkboxradio("refresh");
                } else {
                    $('#checkboxSearchCustom_check').attr('checked', false).checkboxradio("refresh");                    
                }
                $("#showICD10").show();
            }
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
}

//顯示診所eec的預約狀況
function showEEC() {
    $.ajax({
        url: "GetScheduleData.svc/ShowClinicEecStatus/Index/" + $("#hiddenSearchCustom_indexID").val(),
        dataType: "json",
        success: function (data) {
            console.log(data);
            console.log(hiddenSearchCustom_indexID);
            $("#schedule_searchCustom_eecStatus").val(data)
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
}

/*顯示診所時間*/
function showClinicDutyTime() {

    $("#schedule_searchCustom_weekDutyTime > tbody").empty();

    $.ajax({
        url: "GetScheduleData.svc/ReadClinicDutyTime/Index/" + $("#hiddenSearchCustom_indexID").val(),
        dataType: "json",
        success: function (data) {
            $("#hiddenSearchCustom_dutyTimeJson").val(JSON.stringify(data));

            $("#schedule_searchCustom_dutyWeek").trigger('change');

            $.each(data, function (index, value) {                
                $("#schedule_searchCustom_weekDutyTime > tbody").append("<tr><td align='center'>" + value.weekDay + "</td><td align='center'>" + value.morningStart + "~" + value.morningEnd + "</td><td align='center'>" + value.noonStart + "~" + value.noonEnd + "</td><td align='center'>" + value.nightStart + "~" + value.nightEnd + "</td></tr>");
            });
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
}

//顯示eec目前已成功預約家數 和 後補家數
function showEecCurrentCount() {
    $.ajax({
        url: "getScheduleData.svc/ShowEecCurrentCount/UserID/" + $("#hidden_schedule_account").val(),
        dataType: "json",
        success: function (data) {
            $("#schedule_list_eec").html("<h3><font color=liteSeaGreen>EEC " + data["Area"]+ "預約</font><br>已剩餘預約家數:&nbsp;&nbsp;<font color=blue>" + data["SurplusValidCount"] + "</font>  <br> 後補剩餘家數:&nbsp;&nbsp;<font color=blue>" + data["SurplusWaitCount"] + "</font></h3>")
            console.log(data);
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
}

//新行程回報查詢客戶列表
function serarchClinicID($cliniID, $clinicName, $customerID, $IndexID) {
    if ($clinicName.length > 0) {
        $("#textNewReport_clinicName").val($clinicName);
        $("#ulNewReport_clinicListName").empty(); /*清空列表內容*/
        $("#hiddenNewReport_customerID").val($customerID);
        $("#hiddenNewReport_indexID").val($IndexID);
    } else {
        $("#textNewReport_clinicName").attr("placeHolder", "查無醫事機構代號 請重新選擇");
        $("#ulNewReport_clinicListName").empty(); /*清空列表內容*/
        $("#hiddenNewReport_customerID").val("");
        $("#hiddenNewReport_indexID").val("");
    }
}

/*設定back指定網址*/
function backHomeUrl($backID) {
    $($backID).attr("href", "schedule.aspx?workDay=" + $("#scheduleDay").val());
}

/*下拉式選單自動完成*/

$(document).delegate("#schedule_list", "pageinit", function () {
    var account = $("#hidden_schedule_account").val();
    var now = new Date();
    var preDay; //往前遞減
    var nextDay; //往後遞增
    var cDay = 0;
    var selectedWorkDay;
    selectedWorkDay = change_date(now.toJSON().substr(0, 10), 0, 1);  

    if ($("#scheduleDay").val() == null)
    {
        $("#scheduleDay").val(selectedWorkDay);
    } 

    //遞減今天日期
    $("#preDay").click(function () { 
        preDay = change_date( $("#scheduleDay").val() , 0, 0); /*如設為0 反而為減1*/
        $("#scheduleDay").val(preDay);
        $.ajax({
            url: "GetScheduleData.svc/GetJsonScheduleData/Account/" + account + "/WorkDay/" + preDay,
            type: "GET",
            dataType: "json",
            success: function (Jdata) {
                showSchedule(Jdata);
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
            }
        });
    })

    //預設今天
    $("#today").click(function () {
        var today = now.toJSON().substring(0, 10);
        $("#scheduleDay").val(today);
        $.ajax({
            url: "GetScheduleData.svc/GetJsonScheduleData/Account/" + account + "/WorkDay/" + today,
            type: "GET",
            dataType: "json",
            success: function (Jdata) {
                showSchedule(Jdata);
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
            }
        });
    });

    //遞增今天日期
    $("#nextDay").click(function () {
        nextDay = change_date($("#scheduleDay").val(), 0, 2); /* 加2 = 設為今天的下一天*/
        console.log(nextDay);
        $("#scheduleDay").val(nextDay);
        $.ajax({
            url: "GetScheduleData.svc/GetJsonScheduleData/Account/" + account + "/WorkDay/" + nextDay,
            type: "GET",
            dataType: "json",
            success: function (Jdata) {
                showSchedule(Jdata);
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
            }
        });
    });

    $("#scheduleDay").change(function () {
        var selectedDay = $("#scheduleDay").val();
      
        $.ajax({
            url: "GetScheduleData.svc/GetJsonScheduleData/Account/" + account + "/WorkDay/" + selectedDay,
            type: "GET",
            dataType: "json",
            success: function (Jdata) {
                showSchedule(Jdata);
            },
            error: function () {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
            }
        });
    });

    $("#listAllSchedule").click(function () {
        var selectedDay = $("#scheduleDay").val();
        window.open("listAllSchedule.html?account="+account+"&date=" + selectedDay );
    });

    //逐行列出某天的所有行程
    var dtSchedule = $("#hidden_scheduleList_json").val();
    showSchedule(dtSchedule);

    //已預約成功的家數
    showEecCurrentCount();
});

/*回報新行程*/
$(document).delegate("#scheduleTrackt_newReport", "pageinit", function () {

    /*返回列表的網址*/
    backHomeUrl("#btnNewReport_back");

    /*清空hidden 資料*/
    $("#hiddenNewReport_status").val("");

    /* 先將警告訊息hide */
    $("#messageNewReport_clinicName").hide();
    $("#messageNewReport_problem").hide();
    $("#messageNewReport_dispatchingDate").hide();
    /*來源資料來自『行程回報』的page*/
    $("#dropNewReport_getQusttype").eq(0).attr("selected", "selected").selectmenu("refresh");

    // 取得radio status的值
    $("input[name='reportNewStatus']").click(function () {
        $("#hiddenNewReport_status").val($("input[name='reportNewStatus']:checked").val());
    });

    /* 如同發生change 文字*/
    $("#textNewReport_clinicName").on("input", function (e) {        

        if ($("#textNewReport_clinicName").val().length > 0) {

            /* 先清空所有值, 以免有不正確資料*/
            $("#ulNewReport_clinicListName").empty();
            $("#hiddenNewReport_customerID").val("");
            $("#hiddenNewReport_indexID").val("");
            var optClinic = "";
            var objData=null;

            $.ajax({
                url: "GetScheduleData.svc/ClinicList/Name/" + encodeURI($("#textNewReport_clinicName").val()),
                dataType: "json",
                success: function (data) {
                    objData = jQuery.parseJSON(data);
                    if (!!(objData) == true) {
                        optClinic = "";
                        $("#ulNewReport_clinicListName").empty();
                        for (var i = 0; i < objData.length; i++) {
                            optClinic += "<a href='#'  onclick=serarchClinicID('" + objData[i].醫事代號.replace(/\s/g, "") + "','" + objData[i].客戶名稱.replace(/\s/g, "") + "','" + objData[i].客戶編號.replace(/\s/g, "") + "','" + objData[i].索引編號 + "')>";
                            optClinic += "<li class='ui-li ui-li-static ui-btn-up-c ui-corner-top ui-corner-bottom ui-li-last'>" + objData[i].客戶名稱 + "   /  " + objData[i].客戶地址 + "</li>";
                            optClinic += "</a>";
                        }
                        $("#ulNewReport_clinicListName").append(optClinic);
                    }
                },
                error: function (xhr, status) {
                    (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
                }
            });
        } else {
            $("#ulNewReport_clinicListName").empty(); /* 先清空所有值*/
        }
    });

    /*儲存*/
    $("#btnNewReport_confirmSave").click(function () {
        var errorCode = 0;
        errorCode += showMessage("#messageNewReport_clinicName", !!($("#textNewReport_clinicName").val()));
        errorCode += showMessage("#messageNewReport_clinicName", !!($("#hiddenNewReport_customerID").val()));
        errorCode += showMessage("#messageNewReport_clinicName", !!($("#hiddenNewReport_indexID").val()));
        errorCode += showMessage("#messageNewReport_problem", !!($("#txtNewReport_problem").val()));
        errorCode += showMessage("#messageNewReport_dispatchingDate", !!($("#dateNewReport_dispatchingDate").val()));

        if (errorCode > 0) {
            alert("欄位勿空白");
            //停留此頁
            $("#btnNewReport_confirmSave").attr("href", "#scheduleTrackt_newReport");
        } else {
            $("#btnNewReport_confirmSave").attr("href", "#scheduleTract_newReport_saveConfirm");
        }
    });
});

/*客戶查詢頁面*/
$(document).delegate("#schedule_searchCustom", "pageinit", function () {

    /* 如同發生change 文字*/
    $("#textSearchCustom_clinicName").on("input", function (e) {
        if ($("#textSearchCustom_clinicName").val().length > 0) {

            /* 先清空所有值, 以免有不正確資料*/
            $("#ulSearchCustom_clinicListName").empty();
            $("#hiddenSearchCustom_indexID").val("");
            var optClinic = "";
            var objData = null;
            $.ajax({
                url: "GetScheduleData.svc/ClinicList/Name/" + encodeURI($("#textSearchCustom_clinicName").val()),
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                success: function (data) {
                    optClinic = "";
                    objData = jQuery.parseJSON(data);
                    if (!!(objData) == true) {
                        $("#ulSearchCustom_clinicListName").empty();
                        for (var i = 0; i < objData.length; i++) {
                            optClinic += "<a href='#' onclick=setClinicID('" + objData[i].索引編號 + "','" + objData[i].客戶名稱.replace(/\s/g, "") + "')>";
                            optClinic += "<li class='ui-li ui-li-static ui-btn-up-c ui-corner-top ui-corner-bottom ui-li-last'>" + objData[i].客戶名稱 + " ／ " + objData[i].客戶地址 + "</li>";
                            optClinic += "</a>";
                        }
                        $("#ulSearchCustom_clinicListName").append(optClinic);
                    }
                },
                error: function (xhr, status) {
                    (errorMsg) ?  alert(xhr.responseText + "/" + status) : "";
                }
            });
        } else {
            $("#ulSearchCustom_clinicListName").empty(); /* 先清空所有值*/
        }

    })

    /* 如有預設值帶入，先讀取該值*/
    if ($("#hiddenSearchCustom_indexID").val().length > 0) {
        callCustomData(); /*讀取客戶資料*/
        callServiceCount(); /*讀取客戶服務詳細資料*/
        showICD10(); /*讀取客戶是否已設定icd10 */
        showEEC(); /*讀取客戶是否已預約EEC */
        //showClinicDutyTime(); /*重新讀取客戶的診所時間*/
    }
   
    //回報bug
    $("#bugReport").on("click", function (e) {
        var account = $("#hidden_schedule_account").val();
        var password = $("#hidden_schedule_password").val();
        var index = $("#hiddenSearchCustom_indexID").val();        
        var url = "http://mail.drhome.com.tw/report/home/Add?userName=" + account + "&password=" + $.base64.encode(password) + "&clinicIndex=" + index;
        //var url = "http://192.168.1.63/fileManagement/Home/Add?userName=" + account + "&password=" + $.base64.encode(password) + "&clinicIndex=" + index;
        window.open(url, "_blank");
    });

    $("#Icd10BugReport").on("click", function (e) {
        var account = $("#hidden_schedule_account").val();
        var password = $("#hidden_schedule_password").val();
        var index = $("#hiddenSearchCustom_indexID").val();
        var url = "http://mail.drhome.com.tw/report/home/Add?userName=" + account + "&password=" + $.base64.encode(password) + "&clinicIndex=" + index + "&fromICD10=1";
        //var url = "http://192.168.1.63/fileManagement/Home/Add?userName=" + account + "&password=" + $.base64.encode(password) + "&clinicIndex=" + index;
        window.open(url, "_blank");
    });
    

    /*ICD10儲存*/
    $("#btnSearchCustom_SaveICD10").click(function () {
        var userID = $("#hidden_schedule_account").val();// 登入者UID/Account
        var index = $("#hiddenSearchCustom_indexID").val(); //回傳客戶索編        
        var result = $('input[name=ICD10]:checked').val() //回傳ICD10的結果
        var previewCheck = ($("input[type=checkbox]:checked").val() == undefined) ? null : $("input[type=checkbox]:checked").val();
        
        $.ajax({
            url: "GetScheduleData.svc/SetICD10/UserID/" + userID + "/Index/" + index + "/Result/" + result + "/PreviewCheck/" + previewCheck,
            type: "GET",
            dataType: "json",
            success: function (Jdata) {
                switch (Jdata) {
                    case -1: alert("該名客戶不存在");
                    case -2: alert("使用者不存在");
                    case -3: alert("該客戶已經設置過ICD10");
                    case  0: alert("儲存成功");
                }                
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
            }
        });
    });

    //儲存診所時間
    $("#schedule_searchCustom_saveClinicDutyTime").click(function () {
        
        var index = $("#hiddenSearchCustom_indexID").val();
        var weekDay = $("#schedule_searchCustom_dutyWeek").val();
        var morningStart = ($("#morning_start").val().trim() == "") ? "null" : $("#morning_start").val();
        var morningEnd = ($("#morning_end").val().trim() == "") ? "null" : $("#morning_end").val();
        var noonStart = ($("#noon_start").val().trim() == "") ? "null" : $("#noon_start").val();
        var noonEnd = ($("#noon_end").val().trim() == "") ? "null" : $("#noon_end").val();
        var nightStart = ($("#night_start").val().trim() == "") ? "null" : $("#night_start").val();
        var nightEnd = ($("#night_end").val().trim() == "") ? "null": $("#night_end").val();
       

        //儲存診所時間
        $.ajax({
            url: "GetScheduleData.svc/SaveClinicDutyTime/" + index + "/" + weekDay + "/" + morningStart + "/" + morningEnd + "/" + noonStart + "/" + noonEnd + "/" + nightStart + "/" + nightEnd,
            type: "GET",
            dataType: "json",
            success: function () {
                alert("儲存完畢");
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
            }
        });

        //showClinicDutyTime();
    })

    $("#schedule_searchCustom_dutyWeek").change(function () {
        var jsonData = $("#hiddenSearchCustom_dutyTimeJson").val();
        var obj = (jsonData.length > 0) ? jQuery.parseJSON(jsonData) : "";
        var intWeekDay = parseInt($("#schedule_searchCustom_dutyWeek").val());
        var jMorningStart = "";
        var jMorningEnd  = "";
        var jNoonStart ="";
        var jNoonEnd = "";
        var jNightStart ="";
        var jNightEnd = "";

        if (obj.length > 0) {
            jMorningStart = obj[intWeekDay - 1].morningStart;
            jMorningEnd = obj[intWeekDay - 1].morningEnd;
            jNoonStart = obj[intWeekDay - 1].noonStart;
            jNoonEnd = obj[intWeekDay - 1].noonEnd;
            jNightStart = obj[intWeekDay - 1].nightStart;
            jNightEnd = obj[intWeekDay - 1].nightEnd;
        }
        $("#morning_start").val(jMorningStart);
        $("#morning_end").val(jMorningEnd);
        $("#noon_start").val(jNoonStart);
        $("#noon_end").val(jNoonEnd);
        $("#night_start").val(jNightStart);
        $("#night_end").val(jNightEnd);       
    })

    backHomeUrl("#btnSearchCustom_back"); //返回上一頁
  
    $("#radioSearchCustom_no").click(function () {
        $('#checkboxSearchCustom_check').attr('checked', false).checkboxradio("refresh");
        $('#checkboxSearchCustom_check').attr('disabled', "disabled").checkboxradio("refresh");
    })

    $("#radioSearchCustom_yes").click(function () {        
        $('#checkboxSearchCustom_check').checkboxradio('enable');
    })
});





