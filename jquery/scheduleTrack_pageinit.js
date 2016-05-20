// define function name 
/* 取得下拉式問題類別(來源資料json格式的id， 目的端的id) */
var errorMsg = false;


//// 如果有檢核成功 即可前往下一頁 ，否則停留在該頁
var showMessage = function ($messageID, $checkOk) {
    if ($checkOk) {
        $($messageID).hide();
        return 0;
    } else {
        $($messageID).show();
        return 1; /* 錯誤碼  可累加*/
    }
}

/*設定back指定網址*/
var backHomeUrl = function ($backID)
{
    $($backID).attr("href", "schedule.aspx?workDay=" + $("#hiddenAllUser_scheduleDay").val());
}





/*客戶基本資料*/
$(document).delegate("#scheduleTract_customer", "pageinit", function () {
    var dtOneSchedule = $("#hiddenCustomer_json").val();
    var dtAllCustomServices = $("#hiddenCustomServerice_json").val();

    var oneRec = (dtOneSchedule.length > 0) ? jQuery.parseJSON(dtOneSchedule) : "";
    
    backHomeUrl("#btnCustom_back"); /*設定反回的列表的網址*/

    /*預設的派工員工*/
    if (oneRec.length > 0) {
        $("#dropCustomer_tranferEmp1").val(oneRec[0].員工代號.replace(/^\s*|\s*$/g, "")).selectmenu("refresh");
        $("#dropCustomer_tranferEmp2").val(oneRec[0].員工代號2.replace(/^\s*|\s*$/g, "")).selectmenu("refresh");
        $("#dropCustomer_tranferEmp3").val(oneRec[0].員工代號3.replace(/^\s*|\s*$/g, "")).selectmenu("refresh");
    }

    /*來電或派工的列表*/
    callServiceDetail(dtAllCustomServices, "#scheduleTrack_serviceDetail_list");
});

/*回報行程*/
$(document).delegate("#scheduleTrackt_report", "pageinit", function () {
    
    /*清空hidden 資料*/
    $("#hiddenReport_status").val(""); 

   
    /*回到原列表，日期是被選擇;而非預設今天*/
    // $("#btnReport_back").attr("href", "schedule.aspx?workDay="+ $("#hiddenAllUser_scheduleDay").val());

    backHomeUrl("#btnReport_back");

    /* 來自客戶資料_page*/
    var dtOneSchedule = $("#hiddenCustomer_json").val();
    var oneRec = (dtOneSchedule.length >0) ? jQuery.parseJSON(dtOneSchedule) : "";

    /* 先將警告訊息hide */
    $("#messgeReport_questtype").hide();
    $("#messageReport_status").hide();
    $("#messageReport_arriveTime").hide();
    $("#messageReport_overTime").hide();


    /* 讀取所有的產品列表 */
    $.ajax({
        url: 'GetScheduleData.svc/GetClinicAllProductContract/Index/' + $("#scheduleTrackt_report_customerIndex").val(),
        dataType: "json",
        success: function (data) {
            var optAllSchedule = "";
            if (!!(data) == true) {
                for (var i = 0; i < data.length; i++) {
                    optAllSchedule += "<tr>";
                    optAllSchedule += "<td><b class='ui-table-cell-label'>" + data[i].productName + "</b></td>";
         
                    var stringShowDangerDate = "";

                    /* 用來警告即將到期 或 已經過期的 產品呈現 紅色字樣 */
                    if (data[i].contractEndDate.trim() != "") {
                        var now = new Date();
                        var dateDateContratEnd = convertAdDate(data[i].contractEndDate.trim());  /* return datetime*/                      
                        var diffDays = (dateDateContratEnd - now) / 1000 / 60 / 60 / 24; /*傳回天數 */
                        
                        if (diffDays <= 30) {
                            stringShowDangerDate = "<font color=red>" + data[i].contractEndDate + "</font>"                            
                        } else {
                            stringShowDangerDate = data[i].contractEndDate;
                        }
                    }

                    optAllSchedule += "<td><b class='ui-table-cell-label'>" + stringShowDangerDate + "</b></td>";
                    optAllSchedule += "</tr>";                   
                }                
                $("#scheduleTrackt_report_allProducts > tbody").append(optAllSchedule);
            }            
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("連線失敗");
        }
    });

    /*初始化… 先讀取資料*/
    if (oneRec.length > 0) {
        if (!!(oneRec[0].打卡時間) == true) {
            $("#spanReport_arriveTime").text(oneRec[0].打卡時間);
            $("#btnReport_arriveTime").hide();
        } else {
            $("#btnReport_arriveTime").show();
        }

        if (!!(oneRec[0].結束時間) == true) {
            $("#spanReport_overTime").text(oneRec[0].結束時間);
            $("#btnReport_overTime").hide();
        } else {
            $("#btnReport_overTime").show();
        }

        /*------------開始讀取資料---------------------*/

        if (!!(oneRec[0].客戶名稱) == true) {
            $("#lbReport_clinicName").text(oneRec[0].客戶名稱 + "  /  " + oneRec[0].客戶地址);
        } else {
            alert("no Data");
        }

        //服務人員
        var strEmps = "";
        strEmps += (!!oneRec[0].員工姓名 == true) ? oneRec[0].員工姓名+"," : "";
        strEmps += (!!oneRec[0].員工姓名2 == true) ? oneRec[0].員工姓名2 + "," : "";
        strEmps += (!!oneRec[0].員工姓名3 == true) ? oneRec[0].員工姓名3 + "," : "";
        strEmps = strEmps.substr(0, strEmps.length - 1);
        $("#lbReport_staff").text(strEmps);
        

        if (!!(oneRec[0].客戶問題) == true) {
            $("#lbReport_customStatus").text(oneRec[0].客戶問題);
        } 

        if (!!(oneRec[0].問題類別) == true) {
            $("#dropReport_getQusttype").val(oneRec[0].問題類別).selectmenu("refresh");
            $("#hiddenReport_questtype").val(oneRec[0].問題類別);
        } else {
            $("#dropReport_getQusttype").eq(0).attr("selected", "selected").selectmenu("refresh");
        }

        if (!!(oneRec[0].案件狀態) == true) {
            $("#hiddenReport_status").val(oneRec[0].案件狀態);
            switch (oneRec[0].案件狀態) {
                case "完成":
                    $("input[name=reportStatus]").eq(0).attr("checked", "checked").checkboxradio("refresh");
                    break;
                case "未完成":
                    $("input[name=reportStatus]").eq(1).attr("checked", "checked").checkboxradio("refresh");
                    break;
                case "需再排程":
                    $("input[name=reportStatus]").eq(2).attr("checked", "checked").checkboxradio("refresh");
                    break;
                case "取回電腦檢測":
                    $("input[name=reportStatus]").eq(3).attr("checked", "checked").checkboxradio("refresh");
                    break
                case "附件":
                    $("input[name=reportStatus]").eq(4).attr("checked", "checked").checkboxradio("refresh");
                    break;
                default:
                    $("input[name=reportStatus]").eq(5).attr("checked", "checked").checkboxradio("refresh");
                    break;
            }
        }

        if (!!(oneRec[0].處理結果) == true) {
            $("#textReport_proessResult").val(oneRec[0].處理結果);
        }

        if (!!(oneRec[0].已回寫) == true) {
            $("#btnReport_save").addClass('ui-disabled');
        }
        /*------------結束讀取資料---------------------*/
    }

    $("#btnReport_arriveTime").click(function () {
        $.ajax({
            url: 'GetScheduleData.svc/Checkin/Skey/' + $("#hiddensKeyID").val(),
            dataType: "json",
            success: function (data) {
                $('#btnReport_arriveTime').fadeOut('slow', function () {
                    $('#btnReport_arriveTime').hide();
                    $("#messageReport_arriveTime").hide();
                });
                $("#spanReport_arriveTime").html(data);
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("連線失敗");
            }
        });
    });

    $("#btnReport_overTime").click(function () {
        $.ajax({
            url: 'GetScheduleData.svc/CheckOut/Skey/' + $("#hiddensKeyID").val(),
            dataType: "json",
            success: function (data) {
                $('#btnReport_overTime').fadeOut('slow', function () {
                    $('#btnReport_overTime').hide();
                    $("#messageReport_overTime").hide();
                });
                $("#spanReport_overTime").html(data);
            },
            error: function (xhr, status) {
                (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("連線失敗");
            }
        });
    });

   

    // 取得radio status的值
    $("input[name='reportStatus']").click(function () {
        $("#hiddenReport_status").val($("input[name='reportStatus']:checked").val());
    });
   
    // 做最後的檢查
    $("#btnReport_save").click(function () {

        var errorCode=0;

        errorCode += showMessage("#messgeReport_questtype", !!($("#dropReport_getQusttype").val()));
        
        errorCode += showMessage("#messageReport_arriveTime", !!($("#spanReport_arriveTime").text()));
        
        errorCode += showMessage("#messageReport_overTime", !!($("#spanReport_overTime").text()));
        
        errorCode += showMessage("#messageReport_status", !!($("#hiddenReport_status").val()));
        
        if (errorCode > 0) {
            //停留此頁
            alert("欄位勿空白");
            $("#btnReport_save").attr("href", "#scheduleTrackt_report");
        } else {
            $("#btnReport_save").attr("href", "#scheduleTract_Report_saveConfirm");
        }
      
    });
});


/*檢視所有人行程 測試中*/
$(document).delegate("#scheduleTract_allUser", "pageinit", function () {
    $.ajax({
        url: "GetScheduleData.svc/GetJsonAllUserScheduleData/WorkDay/" + $("#hiddenAllUser_scheduleDay").val(),
        dataType: "json",
        success: function (data) {
            var optAllSchedule = "";
            if (!!(data) == true) {
                for (var i = 0; i < data.length; i++) {
                    optAllSchedule += "<li class='ui-li ui-li-divider ui-bar-b ui-li-last'  data-role='list-divider'><h3 class='ui-li-heading'>" + data[i].userName + "</h3></li>"; //列出人員姓名
                    for (var j = 0; j < data[i].schedules.length; j++) {
                        optAllSchedule += "<li class='ui-li ui-li-static ui-btn-up-c ui-corner-top ui-corner-bottom ui-li-last'>" + data[i].schedules[j] + "</li>";
                    }
                }
                $("#liAllUser_schedules").append(optAllSchedule);
            }
        },
        error: function (xhr, status) {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : "";
        }
    });
});


