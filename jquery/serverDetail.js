var callServiceDetail = function (data, outputList)
{
    var optAllService = "";
    var serviceTel = "";
    var serviceDispatch = "";
    var obj = (data.length > 0) ? jQuery.parseJSON(data) : "";
    $(outputList).empty(); //清空之前的紀錄

    for (var i = 0; i < obj.length; i++) {
        if (obj[i].類別 == "來電") {
            serviceTel += "<li style='white-space:normal' class='ui-li ui-li-static ui-btn-up-c'>";
            serviceTel += "<h2 class='ui-li-heading'>" + obj[i].類別 + " : " + obj[i].服務日期 + "</h2>";
            serviceTel += "<p style='white-space:normal'><font color=blue>客戶問題</font>:<b>" + obj[i].服務內容 + "</b></p>";
            serviceTel += "<p style='white-space:normal'><font color=blue>處理結果</font>:<b>" + obj[i].處理結果 + "</b></p>";
            serviceTel += "</li>";
        } else {
            serviceDispatch += "<li style='white-space:normal' class='ui-li ui-li-static ui-btn-up-c'>";
            serviceDispatch += "<h2 class='ui-li-heading'>" + obj[i].類別 + " : " + obj[i].服務日期 + "</h2>";
            serviceDispatch += "<p style='white-space:normal'><font color=blue>客戶問題</font>:<b>" + obj[i].服務內容 + "</b></p>";
            serviceDispatch += "<p style='white-space:normal'><font color=blue>處理結果</font>:<b>" + obj[i].處理結果 + "</b></p>";
            serviceDispatch += "</li>";
        }
    }
    optAllService += "<li class='ui-li ui-li-divider ui-bar-b ui-li-last'  data-role='list-divider'><h3 class='ui-li-heading'>來電</h3></li>";
    optAllService += serviceTel;//服務電話詳細內容
    optAllService += "</li>";

    optAllService += "<li class='ui-li ui-li-divider ui-bar-b ui-li-last'  data-role='list-divider'><h3 class='ui-li-heading'>派工</h3></li>";
    optAllService += serviceDispatch; //派工詳細內容
    optAllService += "</li>";
    $(outputList).append(optAllService);
}