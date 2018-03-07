<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Ageremind.aspx.cs" Inherits="report_Ageremind" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <link href="../js/fullcalendar/fullcalendar.css" rel="stylesheet" />
    <link href="../js/fullcalendar/fullcalendar.print.css" rel="Stylesheet"   />
    <script src="../js/fullcalendar/fullcalendar.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $('#calendar').fullCalendar({
                monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
                dayNamesShort: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                header: {
                    right: 'prev,next today',
                    center: 'title'
                },
              
                
                viewDisplay: function (view) {

                    $("#calendar").fullCalendar('removeEvents');

                    $.get("<%= ResolveUrl("~/ajax/ajaxfullcalendar.ashx?sType=show") %>", { "type": "show" }, function (res) {
                        //debugger;
                        var listData = res;
                        var listJsonArr = eval("(" + listData + ")");

                        for (var i = 0; i < listJsonArr.length; i++) {
                            var obj = new Object();

                            obj.sid = 1;
                            obj.uid = 1;
                            obj.title = listJsonArr[i].Title;
                            var s = listJsonArr[i].Evtstart.substring(6, 19);
                            obj.start = $.fullCalendar.parseDate(s / 1000);
                            var e = listJsonArr[i].Evtend.substring(6, 19);
                            obj.end = $.fullCalendar.parseDate(e / 1000);                         
                            obj.confname = listJsonArr[i].Title;
                            obj.description = listJsonArr[i].Description;
                            $("#calendar").fullCalendar('renderEvent', obj, true);
                        }
                    });

                },
                eventClick: function(event) 
                {
                    alert("ok");
                },
                eventAfterRender: function (event, element, view) {

                    var confbg = '';
                    if (event.confid == 1) {
                        confbg = confbg + '<span class="fc-event-bg"></span>';
                    }



                    if (view.name == "month") {
                        var evtcontent = '<div class="fc-event-vert"><a>';
                        evtcontent = evtcontent + confbg;                       
                        evtcontent = evtcontent + '<span>qq: ' + event.confname + '</span>';
                        evtcontent = evtcontent + '<span><br/>ss: ' + event.description + '</span>';
                        evtcontent = evtcontent + '</a><div class="ui-resizable-handle ui-resizable-e"></div></div>';
                        element.html(evtcontent);
                    }
                },
                loading: function (bool) {
                    if (bool) $('#loading').show();
                    else $('#loading').hide();
                }

            });

        });

</script>

<div id='loading' style='display:none'>loading...</div>
<div id='calendar'></div>
</asp:Content>

