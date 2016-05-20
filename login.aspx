<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Login" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="jquery.mobile/jquery.mobile-1.2.0.css" rel="stylesheet" />
    <script src="jquery.mobile/jquery-1.8.3.js"></script>
    <script src="jquery.mobile/jquery.mobile-1.2.0.min.js"></script>  
    <title>線上回報系統</title>
    <!-- 增加圖示icon -->
    <link rel="apple-touch-icon" href="images/logo.png" />
</head>

<body>
    <!-- 登入畫面 -->
    <form runat="server">
        <div id="login" data-role="page">
            <div data-role="header">
                <h1>回報行程</h1>
            </div>
            <div data-role="content">
                <div data-role="fieldcontain" data-theme="c">
                    <label for="account" >帳號</label>
                    <input type="text" placeholder="請輸入帳號" id="account" runat="server" />
                </div>
                <div data-role="fieldcontain" data-theme="c">
                    <label for="password" >密碼</label>
                    <input type="password" placeholder="請輸入密碼" id="password" runat="server" />
                </div>
                <div data-role="fieldcontain" data-theme="c">
                    <label for="btnSubmit"></label>
                    <!-- 轉址位址ShowSchedule  -->
                    <asp:LinkButton runat="server" ID="btnSubmit"  data-ajax="false" data-role="button"   OnClick="btnSubmit_Click"   >確認送出</asp:LinkButton> 
                    <asp:Label runat="server" ID="lbLogin_msg"></asp:Label>
                </div>
            </div>
            <div data-role="footer" data-position="fixed">
                <h3>corpy right by 2013-01-29 北昕資訊</h3>
            </div>
        </div>
    </form>
</body>
</html>
