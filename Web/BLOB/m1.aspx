<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="m1.aspx.cs" Inherits="Rating_m1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
     <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>试验检测数据分析</span>
                <ul style="display: inline-block; padding-left: 20px; margin: 0px;">
                    原材料:
                    <select id="Item">
                    </select>
                    检测指标:
                    <select id="Attr">
                    </select>
                    生产厂家   :
                    <select id="Factory">
                    </select>
                    型号:
                    <select id="Model">
                    </select>
                   
                    <input type="button" value="查询" onclick="Search();" />
                </ul>
                <ul class="searchbar_01 clearfix">
                    <li class="left" id="liZVal"></li>
                    <li class="right">
                        <input name="" type="button" class="list_btn" title="列表" onclick="createGrid()" /></li>
                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1)" /></li>


                </ul>
            </h2>
            <div class="content">

                <div id="divChart">
                    <div id="chartdiv"></div>
                </div>
                <div id="divList">
                    <!-- 列表 -->
                    <table id="List" iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
                        width="100%" height="500px;" data-options='' url=""
                        striped="true" singleselect="true" pagination="true" fit="true"
                        sortname="" sortorder="">

                        <thead>
                            <tr>

                                <th field="description" width="80" align="center">试验室
                                </th>
                                <th field="BGBH" width="50" align="center">报告编号
                                </th>
                                <th field="YC" width="30" align="center">原材料
                                </th>
                                <th field="FactoryName" width="100" align="center">生产厂家
                                </th>
                                <th field="XH" width="30" align="center">型号
                                </th>
                                <th field="ZVal" width="30" align="center">标准值
                                </th>
                                <th field="Val" width="30" align="center">检测
                                </th>
                                <th field="BVal" width="30" align="center">标准差
                                </th>
                                <th field="BGRQ" width="40" align="center" formatter="FBGRQ">检测日期
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>


                    </table>

                    <!-- 列表 -->
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function FBGRQ(value, row, index) {
            return value.replace('T', ' ');
        }

        var chartData = [
            {
                "DescriptionDate": "2014-07-13",
                "DoubleNumber": 17.3

            },
        {
            "DescriptionDate": "2014-07-13",
            "DoubleNumber": 27.3
        }];

        $(function () {
            LoadItem();
            $("#divChart,#divList").hide();

            $('#Item').change(function () {
                LoadFactory($('#Item').find("option:selected").val());
                LoadAttr($('#Item').find("option:selected").val());
                LoadModel($('#Item').find("option:selected").val());
              
            });
            createChart(1);
        });

        function Search() {
            if ($('#Item').find("option:selected").val() == '' || $('#Attr').find("option:selected").val() == '') {
                alert("请选择 原材 检测指标");
                return;
            }

            createChart(1);
        }

        function LoadItem() {
            $('#Item').empty();
            $.post("m1.aspx", { Act: "Item" }, function (d) {
                if (d == 'null') { return; }
                $('#Item').append(' <option value="">请选择</option>');
                var json = eval(d);
                for (var i = 0; i < json.length; i++) {
                    $('#Item').append(' <option value="' + json[i].ItemID + '">' + json[i].ItemName + '</option>');
                }
            });
        }

       

        function LoadFactory(id) {

            $('#Factory').empty(); if (id == 'null') { return; }
            $.post("m1.aspx", { Act: "Factory", ItemID: id }, function (d) {
                $('#Factory').append(' <option value="">全部</option>');
                if (d == 'null') { return; }
                var json = eval(d);
                for (var i = 0; i < json.length; i++) {
                    $('#Factory').append(' <option value="' + json[i].FactoryID + '">' + json[i].FactoryName + '</option>');
                }
            });
        }

        function LoadAttr(id) {

            $('#Attr').empty(); if (id == 'null') { return; }
            $.post("m1.aspx", { Act: "Attr", ItemID: id }, function (d) {
                if (d == 'null') { return; }
                var json = eval(d);
                for (var i = 0; i < json.length; i++) {
                    $('#Attr').append(' <option value="' + json[i].BindField + '">' + json[i].ItemName + '</option>');
                }
            });
        }

        function LoadModel(id) {

            $('#Model').empty(); if (id == 'null') { return; }
            $.post("m1.aspx", { Act: "Model", ItemID: id }, function (d) {
                $('#Model').append(' <option value="">全部</option>');
                if (d == 'null') { return; }
                var json = eval(d);
                for (var i = 0; i < json.length; i++) {
                    $('#Model').append(' <option value="' + json[i].XH + '">' + json[i].XH + '</option>');
                }
            });
        }


        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");

            $("#divChart,#bysys,#byper").hide();
            $("#divList").show();
            $('#List').datagrid({
                url: 'm1.aspx',
                queryParams: {
                    Act: 'List',
                    Item: $('#Item').find("option:selected").val(),
                    Factory: $('#Factory').find("option:selected").val(),
                    Attr: $('#Attr').find("option:selected").val(),
                    AttrName: $('#Attr').find("option:selected").text(),
                    Model: $('#Model').find("option:selected").val(),
                    StartDate: $('#txt_startDate').val(),
                    EndDate: $('#txt_endDate').val()
                }
            });


        }
   
        var tongChart;
        function createChart(n) {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();

            if (tongChart != undefined && tongChart != null) {
                tongChart.clear();
            }

            $.post('m1.aspx', {
                Act: 'Chart',
                Item: $('#Item').find("option:selected").val(),
                Factory: $('#Factory').find("option:selected").val(),
                Attr: $('#Attr').find("option:selected").val(),
                AttrName: $('#Attr').find("option:selected").text(),
                Model: $('#Model').find("option:selected").val(),
                StartDate: $('#txt_startDate').val(),
                EndDate: $('#txt_endDate').val()
            }, function (d) {

                chartData = eval(d);
                if (chartData.length == 0) {
                    return;
                }

                tongChart = new AmCharts.AmSerialChart();
                tongChart.dataProvider = chartData;
                tongChart.categoryField = "BGRQ";
                tongChart.pathToImages = "../images/";
                var categoryAxis = tongChart.categoryAxis;
                categoryAxis.parseDates = true;
                categoryAxis.minPeriod = "DD";
                categoryAxis.axisColor = "#DADADA";
                categoryAxis.title = "日期";
                categoryAxis.labelRotation = 45;
                categoryAxis.dateFormats = [{
                    period: 'DD',
                    format: 'M月D日'
                }, {
                    period: 'WW',
                    format: 'M月D日'
                }, {
                    period: 'MM',
                    format: 'M月'
                }, {
                    period: 'YYYY',
                    format: 'YYYY年'
                }];

                var valueAxis = new AmCharts.ValueAxis();
                valueAxis.axisColor = "#438EB9";
                valueAxis.dashLength = 1;
                valueAxis.title = "检测值";

               

                $('#liZVal').html('');
                if ($('#Model').find("option:selected").val() != '' && chartData[0].ZVal != undefined) {

                    $('#liZVal').html('<font color="green">标准值: ('+chartData[0].ZVal+')</font>');

                    var reg = RegExp("合格\\S*", "gmi");

                    if (reg.test(chartData[0].ZVal)) {
                        return;
                    }
                    reg = RegExp("^[\\d]+～[\\d]+$", "gmi");
                    var reg1 = RegExp("^[-|+]?\\d+\\.?\\d*～[-|+]?\\d+\\.?\\d*$", "gmi");

                   
                    var guide = new AmCharts.Guide();

                    if (reg.test(chartData[0].ZVal)) {

                        var arr = chartData[0].ZVal.split('～');

                        guide.value = parseInt(arr[0]);
                        guide.lineColor = "#179430";
                        guide.position = "right";
                        guide.dashLength = 0;
                        guide.label = "标准值(" + chartData[0].ZVal + ")";
                        guide.lineThickness = 1;
                        guide.toValue = parseInt(arr[1]);
                        guide.fillColor = '#179430';
                        guide.fillAlpha = 0.2;
                        guide.labelRotation = 1;
                        guide.inside = true;
                        guide.lineAlpha = 1;
                       
                        valueAxis.minimum = parseInt(arr[0]) - 1;
                        valueAxis.maximum = parseInt(arr[1]) + 1;

                    }
                    else if (reg1.test(chartData[0].ZVal)) {

                        var arr = chartData[0].ZVal.split('～');

                       
                        var guide = new AmCharts.Guide();
                        guide.value = Number(arr[0]);
                        guide.lineColor = "#179430";
                        guide.position = "right";
                        guide.dashLength = 0;
                        guide.label = "标准值(" + chartData[0].ZVal + ")";
                        guide.lineThickness = 1;
                        guide.toValue = Number(arr[1]);
                        guide.fillColor = '#179430';
                        guide.fillAlpha = 0.2;
                        guide.labelRotation = 1;
                        guide.inside = true;
                        guide.lineAlpha = 1;
                        valueAxis.minimum = Number(arr[0]) - 1;
                        valueAxis.maximum = Number(arr[1]) + 1;
                    }
                    else {

                        var ZVal = chartData[0].ZVal.replace(/\(\S*\)/g, '');
                        ZVal = chartData[0].ZVal.replace(/^[\d]+～[\d]+$/, '');
                        ZVal = chartData[0].ZVal.replace(/^≥+|≤+|>+|<+/g, '');

                        var max = parseInt(maxsize(chartData, chartData[0].Val, "Val")) + 1;
                       
                        max = max >= parseInt(ZVal) ? max : parseInt(ZVal);
                       
                        max = Math.round(max);

                        var min = minsize(chartData, chartData[0].Val, "Val") - 1; 
                        min = min >= parseInt(ZVal) ? parseInt(ZVal)-1 : min;
                  
                        guide.value = ZVal;
                        guide.lineColor = "#179430";
                        guide.position = "right";
                        guide.dashLength = 0;
                        guide.label = "标准值(" + chartData[0].ZVal + ")";
                        guide.lineThickness = 1;
                        guide.toValue = max;
                        guide.fillColor = '#179430';
                        guide.fillAlpha = 0.2;
                        guide.labelRotation = 1;
                        guide.inside = true;
                        guide.lineAlpha = 1;
                     
                        valueAxis.minimum = min;
                        valueAxis.maximum = max;
                    }
                    valueAxis.addGuide(guide);
                }
                else {
                    valueAxis.minimum = parseInt(minsize(chartData, 0, "Val")) - 1;
                    valueAxis.maximum = parseInt(maxsize(chartData, 100, "Val")) + 1;

                }
               

                //valueAxis.precision = 0;//数值后的小数位数
                valueAxis.minMaxMultiplier = 1;//上下刻度的冗余
                valueAxis.autoGridCount = false;//自动生成刻度线
                valueAxis.integersOnly = true;
                valueAxis.gridCount = parseInt(valueAxis.maximum - valueAxis.minimum) + 2;//纵坐标显示几个刻度线
                valueAxis.labelFrequency = 5;//每个几个刻度显示一个label

                tongChart.addValueAxis(valueAxis);


                var graph = new AmCharts.AmGraph();
                graph.type = "smoothedLine";
                //graph.title = "组值";
                graph.bullet = "round";
                graph.bulletSize = 6;
                graph.bulletColor = "#438EB9";
                graph.balloonText = "试验室:[[description]] 报告编号:[[BGBH]] 日期:[[BGRQ]] 检测值:[[Val]]";
                graph.valueField = "Val";
                graph.lineThickness = 0;
                graph.lineColor = "#438EB9";
                tongChart.addGraph(graph);

                var chartScrollbar = new AmCharts.ChartScrollbar();//鼠标拖动时间
                tongChart.addChartScrollbar(chartScrollbar);

                // WRITE
                tongChart.write("chartdiv");

            });
        }

        function minsize(data, m, column) {
            var len = data.length;
            var j = 0;
            if (len > 0) {
                var j = data[0][column];
                for (var i = 1; i < len ; i++) {
                    if (data[i][column] < j) {
                        j = data[i][column];
                    }
                }
            }
            if (m < j) {
                j = m;
            }
            return j;
        }

        function maxsize(data, m, column) {
            var len = data.length;
            var j = 0;
            if (len > 0) {
                var j = data[0][column];
                for (var i = 1; i < len ; i++) {
                    if (data[i][column] > j) {
                        j = data[i][column];
                    }
                }
            }
            if (m > j) {
                j = m;
            }
            return j;
        }


    </script>
</asp:Content>

