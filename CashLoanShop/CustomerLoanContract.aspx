﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerLoanContract.aspx.cs" Inherits="CashLoanShop.CustomerLoanContract" %>

<!DOCTYPE html>
<link href="css/screen.css" rel="stylesheet" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
        $("#btnPrint").click(function () {
            var contents = $("#content").html();
            var frame1 = $('<iframe />');
            frame1[0].name = "frame1";
            frame1.css({ "position": "absolute", "top": "-1000000px" });
            $("body").append(frame1);
            var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
            frameDoc.document.open();
            //Create a new HTML document.
            frameDoc.document.write('<html><head>');

            //Append the external CSS file.
            frameDoc.document.write('<link href="css/screen.css" rel="stylesheet" type="text/css" />');
            frameDoc.document.write('</head><body>');
            //Append the DIV contents.
            frameDoc.document.write(contents);
            frameDoc.document.write('</body></html>');
            frameDoc.document.close();
            setTimeout(function () {
                window.frames["frame1"].focus();
                window.frames["frame1"].print();
                frame1.remove();
            }, 500);
        });

        $("#btnreturn").click(function () {
            window.location.href = "/CustomerLoanReceipt.aspx?Id=" + $('#hdnLoanId').val();
        });
    });
</script>
<div style="text-align: right; width: 80%; font-weight: bold; font-size: 15px;">


    <input type="button" id="btnPrint" value="Print" class="btn" />
    <input type="button" id="btnreturn" value="Get Receipt" class="btn" />
    <input type="hidden" id="hdnLoanId" runat="server" />
</div>
<div id="content" runat="server"></div>
