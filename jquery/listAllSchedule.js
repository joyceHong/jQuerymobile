var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
}

var showSchedule = function ($jsonData) {

    $("#listAllSchedule").empty();
    var data = "";
    var obj = ($jsonData.length > 0) ? jQuery.parseJSON($jsonData) : "";

    data += "<li data-role='list-divider'><h3>所有人的行程</h3></li>";
    for (var i = 0; i < obj.length; i++) {
        image = (obj[i].結束時間 == null) ? "help.jpg" : "success.jpg";
        staff = (obj[i].服務人員 == null) ? "無" : obj[i].服務人員;
        arriveTime = (obj[i].應到 <= "00:00") ? "" : "<font color=gree>時間(" + obj[i].應到 + ")</font>";

        data += "<li>";
        data += "<a href='#' data-ajax=false>";
        data += "<image src='images/" + image + "' width=100%>"; /*顯示是否已完成*/
        data += "<h4>" + obj[i].客戶名稱 + "/" + staff + "</h4>";
        data += "<p>" + arriveTime + " " + obj[i].客戶問題 + "</p>";
        data += "</a>";        
        data += "</li>";
    }

    $("#listAllSchedule").append(data);
    $("#listAllSchedule").listview("refresh");
}

$(document).delegate("#all_schedule_list", "pageinit", function () {
    var account = getUrlParameter('account');
    var date = getUrlParameter('date'); //passing url from index;   
    console.log(account + "   " + date);

    $.ajax({
        url: "GetScheduleData.svc/GetAllScheduleData/Account/"+account+"/WorkDay/" + date,
        type: "GET",
        dataType: "json",
        success: function (jdata) {
            console.log(jdata);
            showSchedule(jdata);
        },
        error: function () {
            (errorMsg) ? alert(xhr.responseText + "/" + status) : alert("Read GetJsonScheduleData ERROR!!!");
        }
    });

});