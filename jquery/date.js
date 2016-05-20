function parseISO8601(dateStringInRange) {
    var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
        date = new Date(NaN), month,
        parts = isoExp.exec(dateStringInRange);
    if (parts) {
        month = +parts[2];
        date.setFullYear(parts[1], month - 1, parts[3]);
        if (month != date.getMonth() + 1) {
            date.setTime(NaN);
        }
    }
    return date;
};
var change_date = function (dates, months, days) {
    var datestr = parseISO8601(dates);
    // 参数表示在当前日期下要增加的天数  
    var now = new Date(datestr);
    if (months != 0) {
        now.setMonth(now.getMonth() + months);
    }
    now.setDate((now.getDate() - 1) + 1 * days);
    var year = now.getFullYear();
    if (months != 0) {
        var month = now.getMonth() + 1;
    } else {
        var month = now.getMonth() + 1;
    }
    var day = now.getDate();
    if (month < 10) {
        month = '0' + month;
    }
    if (day < 10) {
        day = '0' + day;
    }
    return year + '-' + month + '-' + day;
};

/* adDate 20150101 */
var convertChiDate = function (adDate) {
    var sDate = new Date(Date.parse(adDate, "yyyy-MM-dd"));    
    var chiYear = sDate.getFullYear() - 1911;
    var yyy = chiYear.toString();
    var mm = (sDate.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = sDate.getDate().toString();
    return yyy + (mm[1] ? mm : "0" + mm[0]) + (dd[1] ? dd : "0" + dd[0]); // padding
};

/* return 1040101 */
var convertAdDate = function (chiDate) {
    var intAdYMD = parseInt(chiDate) + 19110000;
    var adDate = intAdYMD.toString();
    var adYear = adDate.substring(0, 4);
    var adMonth = adDate.substring(4, 6);
    var adDay = adDate.substring(6, 8);

    var tempDate = adYear + "/" + adMonth + "/" + adDay;    
    var sDate = new Date(Date.parse(tempDate, "yyyy/MM/dd"));
    return sDate;
}
