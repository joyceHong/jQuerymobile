var map;
var geocoder;

function mapInitialize() {
    //map = new GMap2(document.getElementById("map_canvas")); /* 建立地圖*/
    map = new GMap2($("#map_canvas").get(0));
    /*初始地圖的顯示位置和縮放大小設定 並設定縮放大小為 9 (可以從 0 到 17 設定)*/
    map.setCenter(new GLatLng(34, 0), 1);
    geocoder = new GClientGeocoder(); /*地址查詢，輸入經緯度(查出地圖位置和地址)*/
    showLocation();
    
}

function addAddressToMap(response) {
    map.clearOverlays();              /*  移除所有標示*/
    if (!response || response.Status.code != 200) {
        alert("Sorry, we were unable to geocode that address" + response.Status.code);
    } else {
        place = response.Placemark[0];
        /*
        經度 :place.Point.coordinates[0]
        緯度: place.Point.coordinates[1]
        */
        point = new GLatLng(
                           place.Point.coordinates[1], place.Point.coordinates[0]);
        marker = new GMarker(point);
        map.setCenter(point, 17);       /* 初始地圖的顯示中心*/
        map.addOverlay(marker);         /*將加疊層加入地圖中，這裡我們將新建立的 GMarker 加入地圖中。*/
        $("#address").text(place.address);
    }
}

function showLocation() {
    var address = $("#divMap_address").val();
    geocoder.getLocations(address, addAddressToMap); /*google API元件，將目前座標經緯度加mark 傳送地圖上 */
}



//function findLocation(address) {
//    document.forms[0].q.value = address;
//    showLocation();
//}


//if (navigator.geolocation) {

//    /* 取得目前的位置  */
//    navigator.geolocation.getCurrentPosition(function (position) {
//        /* latitude 緯度，longitude 經度*/
//        alert("aaa");
//        var s = position.coords.latitude + "," + position.coords.longitude;       
//        document.forms[0].q.value = $("#lbSearchCustom_addr").text();
//        showLocation();
//    });

//} else {

//    alert("I'm sorry, but geolocation services are not supported by your browser.");

//}

