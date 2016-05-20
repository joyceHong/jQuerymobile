<%@ Page Language="C#" AutoEventWireup="true" CodeFile="scheduleMap.aspx.cs" Inherits="scheduleMap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="content-type" charset="UTF-8" name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" /> 
    <!-- 增加圖示icon -->
    <link rel="apple-touch-icon-precomposed" href="images/logo.png" />
    <link href="jquery.mobile/jquery.mobile-1.2.0.css" rel="stylesheet" />
    <script src="jquery.mobile/jquery-1.8.3.js"></script>
    <script src="jquery.mobile/jquery.mobile-1.2.0.min.js"></script>
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=AIzaSyC3xli-jY6mslf3alE_pHoaj6yXSbzgskE" type="text/javascript"> </script>
    <script src="jquery/scheduleMap_pageinit.js"></script>
    
</head>
    <body onload="mapInitialize()">
        <form runat="server">
            <div id="divMap_showLocation">
                <input type="hidden" name="q" id="divMap_address" value="" class="address_input" size="40" runat="server" />
                <a data-role="button" onclick="window.close();" data-icon="close">關閉</a>
                <div id="map_canvas" style="width: 400px; height: 400px"></div>
                You location is: <span id="address">Unknown</span>                
            </div>
        </form>
    </body>
</html>
