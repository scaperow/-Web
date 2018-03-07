<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AnalysisofItemTestData.aspx.cs" Inherits="BLOB_AnalysisofItemTestData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="../js/jquery-1.9.0.min.js"></script>
    <link href="../css/css.css" rel="stylesheet" />
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/initialize.css" rel="stylesheet" />
     <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/serial.js") %>"></script>
    <script src="../js/jquery.colorbox-min.js"></script>
    <style>
        .Item ,.Year{
            display:inline-block;
            border:#ccc 1px solid;
            padding:0 5px;
            color:#fff;
            background:#ccc;
        }
        .Active {
            background:#e76049;
            color:#FFF;
             border:#e76049 1px solid;
        }
        .cur {
            cursor: hand;
        }

    </style>
</head>
<body>
<div class="piece">
        <div class="piece_con">

            <h2 class="title">

                <ul class="searchbar_01 clearfix">
                     <li style="display: inline-block; padding-left: 20px; margin: 0px;" id="Item">
                     </li>

                </ul>
            </h2>
            <div class="content">

                <div id="divChart">
                    <div id="chartdiv"></div>
                    <div style="padding:5px 80px; color:#f00;">提示：  左键点击->本月 ；  右键点击->所选时间范围</div>
                </div>
                
            </div>
        </div>
    </div>
</body>
</html>
<script type="text/javascript">
   
    var Unit = {
        "引气剂": {
            "减水率": "%",
            "含气量": "%",
            "泌水率比": "%",
            "1小时含气量变化量": "%",
            "3d抗压强度比": "%",
            "7d抗压强度比": "%",
            "28d抗压强度比": "%",
            "收缩率比": "%",
            "相对耐久性指数": "%"
        },
        "混凝土外加剂": {
            "减水剂": "%",
            "7d抗压强度比": "%",
            "28d抗压强度比": "%",
            "常压泌水率": "%",
            "压力泌水率": "%",
            "含气量": "%",
            "60min坍落度变化量": "mm"
        },
        "细骨料": {
            "颗粒级配": "%",
            "含泥量": "%",
            "泥块含量": "%",
            "云母含量": "%",
            "轻物质含量": "%",
            "氯化物含量": "%",
            "吸水率": "%",
            "三氧化硫含量": "%",
            "含泥量": "%",
            "含泥量（机制砂）": "%"
        },
        "粉煤灰": {
            "细度": "%",
            "需水量比": "%",
            "烧失量": "%",
            "含水率": "%",
            "三氧化硫含量": "%",
            "游离氧化钙含量": "%",
            "氯离子含量": "%",
            "三氧化硫含量": "%",
            "碱含量": "%",
            "活性指数28d": "%",
            "氧化钙": "%",
            "安定性（mm）": "mm",
            "比表面积": "m2/kg"
        },
        "水泥": {
            "比表面积": "m2/kg",
            "密度": "g/cm3",
            "初凝时间": "min",
            "终凝时间": "min",
            "3d抗压强度": "MPa",
            "28d抗压强度": "MPa",
            "3d抗折强度": "MPa",
            "28d抗折强度": "MPa",
            "安定性": "%",
            "烧失量": "%",
            "标准稠度用水量": "%",
            "胶砂流动度": "mm",
            "三氧化硫含量": "%",
            "氯离子含量": "%",
            "碱含量": "%",
            "游离氧化钙含量": "%"
        },
        "粗骨料": {
            "颗粒筛分": "m2/kg",
            "压碎标值（碎石）": "g/cm3",
            "针片状含量": "min",
            "含泥量1": "min",
            "含泥量2": "MPa",
            "含泥量3": "MPa",
            "泥块含量1": "MPa",
            "泥块含量2": "MPa",
            "泥块含量3": "%",
            "紧密空隙率": "%",
            "压碎标值（卵石）": "%"
        },
        "钢筋原材": {
            "屈服强度1": "MPa",
            "屈服强度2": "MPa",
            "抗拉强度1": "MPa",
            "抗拉强度2": "MPa",
            "伸长率1": "%",
            "伸长率2": "%"
        }
        ,
        "矿粉": {
            "密度": "g/cm3",
            "烧失量": "MPa",
            "比表面积": "m2/kg",
            "需水量比": "MPa",
            "流动度比": "%",
            "含水率": "%",
            "三氧化硫": "%",
            "氯离子含量": "%",
            "氧化镁含量": "%",
            "碱含量": "%",
            "7d活性指数A": "%",
            "28d活性指数A": "%",
        }
    };

        $(function () {
            LoadAttr();

        });


        function LoadAttr() {
            $.post("AnalysisofItemTestData.aspx", { Act: "Attr", ItemID: '<%="ID".RequestStr()%>' }, function (d) {
                if (d == 'null') { return; }
                var json = eval(d);
                for (var i = 0; i < json.length; i++) {
                    $('#Item').append(' <a onclick="Chart(\'' + json[i].BindField + '\',this)" class="Item  ' + (i == 0 ? 'Active' : '') + '" value="' + json[i].BindField + '">' + json[i].ItemName + '</a>');

                }
                Chart(json[0].BindField, $('a.Item:first')[0]);
            });
           
        }


    var chart;
    var txtUnit = '';
    function Chart(Attr, o) {

        if (o != null) {
            $('a.Item').removeClass('Active');
            $(o).addClass('Active');
        }



        if (Attr == '')
        {
            Attr = $('a.Item.Active').attr('value');
           
        }

        if (chart != undefined && chart != null) {
            chart.clear();
        }

        $.post("AnalysisofItemTestData.aspx", { Act: 'Chart', Item: '<%="ID".RequestStr()%>', FID: '<%="FID".RequestStr()%>', AttrName: $('a.Item.Active').text(), Attr: Attr, StartDate: '<%="StartDate".RequestStr()%>', EndDate: '<%="EndDate".RequestStr()%>' }, function (d) {
         
    
 
            var data = jQuery.parseJSON(d);

          
      
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = data.D1;
            chart.categoryField = "M";
            chart.startDuration = 0.5;
            chart.balloon.color = "#000000";
            

            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.fillAlpha = 1;
            categoryAxis.fillColor = "#FAFAFA";
            categoryAxis.gridAlpha = 0;
            categoryAxis.axisAlpha = 0;
            categoryAxis.gridPosition = "start";
            categoryAxis.position = "top";

           

            try
            {
                txtUnit = $('a.Item.Active').text();
                txtUnit = "  "+Unit["<%="Name".RequestStr()%>"][txtUnit];
                txtUnit = (txtUnit == undefined ? '' : txtUnit);
                txtUnit = (txtUnit == '  undefined' ? '' : txtUnit);
            }
            catch (ex) { }
        
            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.title = "平均值[" + txtUnit+"]";
            valueAxis.unit = txtUnit;
            valueAxis.dashLength = 5;
            valueAxis.axisAlpha = 0;
            valueAxis.minimum = data.Min;
            valueAxis.maximum = data.Max;
            valueAxis.integersOnly = true;
            valueAxis.gridCount = 10;
            //valueAxis.reversed = true; // this line makes the value axis reversed
            chart.addValueAxis(valueAxis);
          

            var Colors = {};
            for (var i = 0; i < data.D2.length; i++) {
                // GRAPHS
                // Italy graph						            		
                var graph = new AmCharts.AmGraph();
                graph.title = data.D2[i];
                graph.valueField = data.D2[i];
                graph.dashLength = 3;
                graph.balloonText = "[[title]] [[category]]: 平均值[[value]] 标准差:[[" + data.D2[i] + "-FC]] 变异系数:[[" + data.D2[i] + "-BY]]";
                graph.lineAlpha = 1;
                graph.bullet = "round";
                graph.addListener("clickGraphItem", handleClick);
                graph.addListener("rightClickGraphItem", handleClickd);
                graph.showHandOnHover = true;  
                chart.addGraph(graph);
  
                Colors[data.D2[i]] = graph.lineColorR;
            }

            // 平均值划线
            for (var i = 0; i < data.D3.length; i++) {
                var guide = new AmCharts.Guide();
                guide.value = data.D3[i].SV;
                guide.lineColor = Colors[data.D3[i].tit];
                guide.position = "right";
                guide.dashLength = 1;
                guide.label = data.D3[i].tit + " 平均值：" + data.D3[i].SV + " 标准差：" + data.D3[i].FC + " 变异系数：" + data.D3[i].BY + "%" + " 标准值：" + data.D3[i].ZVal;
                guide.boldLabel = true;
                guide.lineThickness = 1;
                guide.labelRotation = 1;
                guide.inside = true;
                guide.lineAlpha = 1;

                guide.showHandOnHover = true;
           
                valueAxis.addGuide(guide);



                
            }




            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorPosition = "mouse";
            chartCursor.zoomable = false;
            chartCursor.cursorAlpha = 0;
            chart.addChartCursor(chartCursor);

            // LEGEND
            var legend = new AmCharts.AmLegend();
            legend.useGraphSettings = true;
            legend.valueText = '';
            chart.addLegend(legend);
           

            chart.write("chartdiv");
      
            $('path[stroke="#ff6600"]').on('click', function () { alert();});

        });


    }

    function handleClickd(e) {
        try {
            if (event.preventDefault) {
                event.preventDefault();
            } else {
                event.returnValue = false;
            }
        }
        catch (ex) { }
        $.colorbox({
            href: "AnalysisofItemAttrTestData.aspx?Unit=" + txtUnit + "&Name=<%="Name".RequestStr()%>&FID=<%="FID".RequestStr()%>&ID=<%="ID".RequestStr()%>&StartDate=<%="StartDate".RequestStr()%>&EndDate=<%="EndDate".RequestStr()%>&MO=" + encodeURI(e.target.valueField) + "&ATTR=" + $('a.Item.Active').attr('value') + "&ATTRNAME=" + $('a.Item.Active').text() + "&r=" + Math.random(),
           width: '100%',
           height: '100%',
           title: function () {
               return "<%="Name".RequestStr()%>[<%="StartDate".RequestStr()%>  至  <%="EndDate".RequestStr()%>" + "  " + $('a.Item.Active').text() + " 检测指标分析]";
            },
           close: '',
           iframe: true
       });
    }

    function handleClick(e) {

        $.colorbox({
            href: "AnalysisofItemAttrTestData.aspx?Unit=" + txtUnit + "&Name=<%="Name".RequestStr()%>&FID=<%="FID".RequestStr()%>&ID=<%="ID".RequestStr()%>&M=" + e.item.category + "&MO=" + encodeURI(e.target.valueField) + "&ATTR=" + $('a.Item.Active').attr('value') + "&ATTRNAME=" + $('a.Item.Active').text() + "&r=" + Math.random(),
            width: '100%',
            height: '100%',
            title: function () {
                return "<%="Name".RequestStr()%>[" + e.item.category + "  " + $('a.Item.Active').text() + " 检测指标分析]";
            },
            close: '',
            iframe: true
        });

    }
</script>