<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="IndexMap.aspx.cs" Inherits="IndexMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <link rel="stylesheet" type="text/css" href="css/map.css" />
    <script language="javascript" src="http://webapi.amap.com/maps?v=1.2&key=fdd8f1726f1974ccc889ed6ab4f82a49"></script>
    <script type="text/javascript" src="js/ajax_loader1.js"></script>
    <script type="text/javascript" src="js/ajaxLoaderBasePage1.js"></script>

    <script language="javascript">
        var Lines = <%=Lines%>;
        var LinesTemp,dataType,polyline,Marker1,Marker2;
        var mapObj,lineIDTemp,titleTemp;
        var markers = [];
        var step = 0;
        var tp = 1;
        var state = true,state1 = true;
        //初始化地图对象，加载地图
        function mapInit() {
            mapObj = new AMap.Map("iCenter", {
                center: new AMap.LngLat(<%=Site.MapCenterX%>, <%=Site.MapCenterY%>),  
                level: <%=Site.ZoomLevel%>
            });
            mapObj.plugin(["AMap.ToolBar"], function () {
                toolBar = new AMap.ToolBar(); //设置地位标记为自定义标记  
                mapObj.addControl(toolBar);
                AMap.event.addListener(toolBar, 'location', function callback(e) {
                    locationInfo = e.lnglat;
                });
            });
            //添加地图类型切换插件  
            mapObj.plugin(["AMap.MapType"],function(){  
                //地图类型切换  
                type= new AMap.MapType({defaultType:0});//初始状态使用2D地图  
                mapObj.addControl(type);  
            });
            addOverlays(Lines);
            
        }

        $(function () {
            
            $("#leftblock").hide();
            $("#leftMenu").hide();
            $(".jq_content").removeClass();
            $(".leftblock").removeClass();
            $(".conter").removeClass();
            $(".conters").removeClass();
            $(".left_menu").removeClass();
            $(".foot_block").hide();
            $("body").css("background-color","#fff");
            mapInit();
            $("#iResult").hide();
            $(".navbar").hide();
            
            LoadItems();
            $('#city').change(function () {
                var v = $(this).find("option:selected").attr('lng');
                if (v == '') { return false; }
                var lng = v.split(',');
                if (lng[0] != "") {
                    rmapInit(lng[0], lng[1]);
                    $('#isSet').each(function () {
                        var myvalue = '已设置';
                        $(this).css({ color: "#000000" });
                        $(this).html(myvalue);
                    });
                } else {
                    $('#isSet').each(function () {
                        var myvalue = '未设置';
                        $(this).css({ color: "#ff6a00" });
                        $(this).html(myvalue);
                    });
                }

            });

            var clickEventListener = AMap.event.addListener(mapObj, 'click', function (e) {
                $('#lngX').val(e.lnglat.getLng());
                $('#latY').val(e.lnglat.getLat());
            });
        });
        
        //定位
        function rmapInit(x, y) {
            mapObj.setCenter(new AMap.LngLat(x, y));
            mapObj.setZoom(8)
        }        function LoadItems() {
            $('#city option').remove();
            $('#city').append('<option value="">请选择</option>');
            $.post("IndexMap.aspx?callback=?", { ACT: "GetItems" }, function (data) {
                var obj = jQuery.parseJSON(data);

                $.each(obj.items, function (i, n) {
                    $('#city').append('<option value="' + n.id + '" lng="' + n.x + ',' + n.y + '">' + n.description + '</option>');
                });

                $('#city').find('option[lng = ","]').css({ background: "#ff6a00" });
            })
        }                function InitLines(LinesStr)
        {
            for (var i = 0; i < LinesStr.Lines.length; i++)
            {
                var lineArr = new Array();
      
                for (var ii = 0; ii < LinesStr.Lines[i].Sites.length; ii++)
                {
                    lineArr.push(new AMap.LngLat(LinesStr.Lines[i].Sites[ii].X, LinesStr.Lines[i].Sites[ii].Y));
                }
          
                addLine(lineArr);
                
            }
        }
        
        var SiteLine;
        function SetLine(extData) {
            $('#hd_leftTreeRefresh').val('1');
            var _Data = $.parseJSON(extData);
            flashLine();
            if (_Data.LineName != '') {
                $.post("ForIndexMapRight.aspx?callback=?&AJAX="+Math.random(), { ACT: "Rooms", LID: _Data.LineID, LName: _Data.LineName}, function (data) {
                    $('#date').empty();
                    $('#RightData').empty();
                  
                    $("#<%=Master.FindControl("Label1").ClientID %>").text(_Data.LineName);
                    $('#date').html( '<strong>'+_Data.LineName+'</strong>&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" name="date" checked value="1" />本月 <input type="radio" name="date" value="2" />上月 <input type="radio" name="date" value="3" />本年');
                    $(':radio[name="date"]').on('click',function(){GetData(_Data.LineID, _Data.LineName,_Data.X,_Data.Y);});

                    $('#RightData').append('<div id="Accor"  style="width:100%; margin:0px;">'+data+'</div>')
              
                    $('#Accor').accordion({animate:true,width:$('#RightData').width() ,onSelect:function(tit,index){


                        var p = $('#Accor').accordion('getSelected');
                     
                       
                        $(p).empty().html('正在加载...');
                        var id=$(p).attr('id') ;
                        var dt=$(p).attr('DepType') ;
                        var idate = $(':radio[name="date"]:checked').val();
                        $.post("ForIndexMapRight.aspx?callback=?&AJAX="+Math.random(), { ACT: "Room",LID: _Data.LineID, LName:tit, RoomID: id,RoomType:dt,SearchType:idate}, function (html) {

                           
                            $(p).empty().html(html);

                        });
                        if(id!='ALL'){
                            $.post("ForIndexMapRight.aspx?callback=?&AJAX="+Math.random(), { ACT: "RoomInfo",LID: _Data.LineID, LName:tit, RoomID: id}, function (html) {

                                openInfo(tit,$(p).attr('x'),$(p).attr('y'),html);
                                $('#iResultRight').scrollTop(index*27.5);
                            });
                        }

                    }});
                });
                $.post("IndexMap.aspx?<%="c".IsRequest()?"c=0&":""%>callback=?&AJAX="+Math.random(), { ACT: "SetLine", Name: _Data.LineName,Type:""}, function (data) {
                    LinesTemp = data;
                    SiteLine = eval('(' + LinesTemp + ');');
                    InitLine(SiteLine);
                    
                    addOverlaysSite(SiteLine);
                    GetLineData(_Data.LineID,_Data.LineName);
                    $('#hd_leftTreeRefresh').val('0');
                });
            }
        }
        //获取当前线路的Json串（用于调试）
        function LinesJson(LinesStr)
        {
            var infoLine = [];
            var _Data = $.parseJSON(extData);
            infoLine.push("{Line:[");
            for (var i = 0; i < LinesStr.Sites.length; i++)
            {
                infoLine.push('"X":"'+LinesStr.Sites[i].X+'",');
                infoLine.push('"Y":"'+LinesStr.Sites[i].Y+'"');
                infoLine.push(i < LinesStr.Sites.length - 1 ? "," : "");
            }
            infoLine.push("]}");

        }

        //查询统计数据
        function GetData(lineID,lName,x,y)
        {
            $.post("ForIndexMapRight.aspx?<%="c".IsRequest()?"c=0&":""%>callback=?&AJAX="+Math.random(), { ACT: "Rooms", LID: lineID, LName:lName}, function (data) {
              
                $('#RightData').empty();

                $('#RightData').append('<div id="Accor"  style="width:100%; margin:0px;">'+data+'</div>')
                $('#Accor').accordion({animate:false,width:$('#RightData').width() ,onSelect:function(tit,index){
                    var p = $('#Accor').accordion('getSelected');
                    $(p).empty().html('正在加载...');
                    var id=$(p).attr('id') ;
                    var dt=$(p).attr('DepType') ;
                    var idate = $(':radio[name="date"]:checked').val();
             
                    $.post("ForIndexMapRight.aspx?<%="c".IsRequest()?"c=0&":""%>callback=?&AJAX="+Math.random(), { ACT: "Room",LID: lineID, LName:tit, RoomID: id,RoomType:dt,SearchType:idate}, function (html) {

                        $(p).empty().html(html);

                        });
                    if(id!='ALL'){
                        $.post("ForIndexMapRight.aspx?callback=?&AJAX="+Math.random(), { ACT: "RoomInfo",LID: lineID, LName:tit, RoomID: id}, function (html) {

                            openInfo(tit,$(p).attr('x'),$(p).attr('y'),html);
                            $('#iResultRight').scrollTop(index*27.5);
                        });
                    }    
                    }});
            });
        }
        //添加线路及节点
        function InitLine(LinesStr)
        {
            var lineArr = new Array();
            var tempLine = "";
            
            for (var ii = 0; ii < LinesStr.LineCoordinate.length; ii++) {
                lineArr.push(new AMap.LngLat(LinesStr.LineCoordinate[ii].X, LinesStr.LineCoordinate[ii].Y));
            }

            for (var i = 0; i < LinesStr.Sites.length; i++)
            {
                if (LinesStr.Sites[i].IsSG != '0') {
                    tempLine +="{\"X\":\""+LinesStr.Sites[i].X+"\",\"Y\":\""+LinesStr.Sites[i].Y+"\"},";
                }
                
                var Marker = "Marker"+ i;
                Marker = new AMap.Marker({map: mapObj,icon: 'images/map/' + (i+1) + '.png',
                    position: new AMap.LngLat(LinesStr.Sites[i].X,LinesStr.Sites[i].Y),
                    offset: new AMap.Pixel(-12, -36),
                    clickable: true,
                    extData: '{"LineID":"' + LinesStr.Sites[i].LineID + '","X":"' + LinesStr.Sites[i].X + '","Y":"' + LinesStr.Sites[i].Y + '","LineName":"' + LinesStr.Sites[i].LineName + '","TestCode":"'+LinesStr.Sites[i].TestCode+'"}'
                });
                Marker.setTitle(LinesStr.Sites[i].SYSName);
                AMap.event.addListener(Marker, "click", function callback(e) {
                    //openSiteInfo(this.getExtData());
                    //alert(this.getExtData());
                 
                    var _TData = jQuery.parseJSON(this.getExtData());
                    $.post("ForIndexMapRight.aspx?callback=?&AJAX="+Math.random(), { ACT: "RoomInfo",LID: _TData.LineID, LName:_TData.LineName, RoomID: _TData.TestCode}, function (html) {
                      
                        openInfo(_TData.LineName,_TData.X,_TData.Y,html);
                          
                        });
                   
                });
                markers.push(Marker);
            }
            //alert(tempLine);
            addLine(lineArr);
        }

        //添加线路
        function addLine(lineArr) {            
            polyline = new AMap.Polyline({
                path: lineArr, //设置线覆盖物路径  
                strokeColor: "#3366FF", //线颜色  
                strokeOpacity: 1, //线透明度   
                strokeWeight: 5, //线宽  
                strokeStyle: "dashed", //线样式  
                strokeDasharray: [10, 5] //补充线样式   
            });
            polyline.setMap(mapObj);
        }
        //添加所有线路节点
        function addOverlays(lineData) {
            for (var i = 0; i < lineData.Lines.length; i++) {
                //自定义点标记内容   
                var markerContent = document.createElement("div");
                markerContent.className = "markerContentStyle";

                //点标记中的图标
                var markerImg = document.createElement("img");
                markerImg.className = "markerlnglat";
                markerImg.src = "images/map/0.png";
                markerContent.appendChild(markerImg);

                //点标记中的文本
                var markerSpan = document.createElement("span");
                markerSpan.innerHTML = lineData.Lines[i].LineName;
                markerContent.appendChild(markerSpan);
                Marker2 = new AMap.Marker({ 
                    map: mapObj, 
                    //icon: "images/map/"+Lines.Lines[i].TitleImg+"Tag.png", 
                    position: new AMap.LngLat(lineData.Lines[i].TagX, lineData.Lines[i].TagY),
                    offset: new AMap.Pixel(-10, -36), clickable: true ,//(-25, -59)
                    title: lineData.Lines[i].LineName,
                    content: markerContent,
                    extData: '{"LineName":"' + lineData.Lines[i].LineName + '","LineID":"' + lineData.Lines[i].LineID + '","X":"' + lineData.Lines[i].X + '","Y":"' + lineData.Lines[i].Y + '","ZoomLevel":"' + lineData.Lines[i].ZoomLevel + '"}'
                });
                AMap.event.addListener(Marker2, "click", function callback(e) {
                    
                    //this.setMap(null)
                    flashLine();
                    GoToLine(this.getExtData());
                    SetLine(this.getExtData())
                    LoadItems();//加载试验站
                    UpdateMarker(this.getExtData());
                });
            }
        }
        
        //更新隐藏站点图标
        function UpdateMarker(extData){
            var _Data = $.parseJSON(extData);
            var a=$("[title='"+_Data.LineName+"']").find("img");
            var b=$("[title='"+_Data.LineName+"']");
            if ( state ) {
                a.attr("src", "images/map/0.png");
                b.removeClass();
                b.addClass("markerContentStyle");
            } else {
                a.attr("src", "images/map/04.png");
                b.addClass("markerContentStyleNew");
            }

        }

        //添加点标记覆盖物，点的位置在地图上分布  
        function addOverlaysSite(SiteLine) {
            if (state1) {
                InitLine(SiteLine);
            }else {
                linkForImg('1');
                polyline.setMap(null);
            }
            state1 =!state1;        

        }
        //移除线路子节点
        function linkForImg(stat){
            var divICenter=document.getElementById("iCenter");
            var images=document.getElementsByTagName("img");
            var imgLen=images.length;
            for(var i=0;i<imgLen;i++){
                if (stat == '1') {
                    var s=images[i].src;
                    var imgName = s.substring(s.lastIndexOf("/")+1);
                    if (parseInt(imgName)>0 && parseInt(imgName) < 100 ) {
                        images[i].style.display="none";
                    }
                    
                }else {
                    images[i].style.display="block";
                }
            }
        }



        //在指定位置打开信息窗体  
        function openInfo(title, x, y, html) {
        

            inforWindow = new AMap.InfoWindow({
                content: html,  //使用默认信息窗体框样式，显示信息内容  
                offset: new AMap.Pixel(0, -28),size:new AMap.Size(500, 0)
            });
            inforWindow.open(mapObj, new AMap.LngLat(x, y));
        }
        
        //转到所选线路
        function GoToLine(extData)
        {  
            var _Data = $.parseJSON(extData);
        
            mapObj.setZoomAndCenter(_Data.ZoomLevel,new AMap.LngLat(_Data.X,_Data.Y));
            UpDataUserLine(_Data.LineID);
        
            lineIDTemp = _Data.LineID;
            titleTemp = _Data.LineName;
            ShrinkDiv(_Data.X,_Data.Y,_Data.ZoomLevel);
        }
        //获取线路统计数据
        function GetLineData(lineID,tit)
        {
            var leftMidInfo = '<span class="menuultop" onclick="openInfo(\'SG-1-中铁大桥局-北岸中心试验室\',112.400866,30.041532,95,0,16,0,38,0,15,0,26,0,5,0,0,0,0,37,60,57,\'MengXiHuaZhong\')">SG-1-中铁大桥局-北岸中心试验室</span>';
            //$('#iResultRight').html(leftMidInfo);
        }

        //更新用户所在线路
        function UpDataUserLine(lineID)
        {
            $.post("IndexMap.aspx?AJAX="+Math.random(),{ACT:"UPDATEUSERLINE",LineID:lineID},function(d){});
        }

        //线路闪烁效果
        function startFlashLine() {
            step++
            //if (step==3) {step=1}
            if (step == 1) { polyline.hide() }
            if (step == 2) { polyline.show() }
            if (step == 3) { polyline.hide() }
            if (step == 4) { polyline.show() }
            if (step == 5) { polyline.hide() }
            if (step == 6) { polyline.show() }
            setTimeout("startFlashLine()", 380);
            clearTimeout(step);
        }
        function flashLine() {
            step = 0;
        }

        //显示右边数据区
        function  ShrinkDiv(x,y,level) {
            var divCenter = $("#iCenter"),
                curCenterWidth = divCenter.width(),
                autoCenterWidth = divCenter.css('width', '66%').width(),
                autoMaxCenterWidth = divCenter.css('width', '100%').width();
            var divResult = $("#iResult"),
                curResultWidth = divResult.width(),
                autoResultWidth = divResult.css('width', autoMaxCenterWidth*0.325).width();
            
            if ( state ) {
                $( "#iCenter" ).animate({
                    width: autoCenterWidth
                }, 1000 );
                $( "#iResult" ).animate({
                    width: autoResultWidth
                }, 1500 );
                //mapObj.setZoomAndCenter(parseInt(level-1),new AMap.LngLat(x,y));
                mapObj.setZoomAndCenter(parseInt(level),new AMap.LngLat(x,y));
                $("#iResult").show();
            } else {
                $( "#iCenter" ).animate({
                    width: autoMaxCenterWidth
                }, 1000 );
                $( "#iResult" ).animate({
                    width: 10
                }, 1500 );
                //mapObj.setFitView();
                mapObj.setZoomAndCenter(5,new AMap.LngLat(104.765625, 35.603719));
                $("#iResult").hide();
            }
            state = !state;

            $('#RightData').css({
                width:autoResultWidth-20
            });

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        $("#<%=Master.FindControl("Label1").ClientID %>").parent("a").removeAttr('onclick');
        $("#<%=Master.FindControl("Label1").ClientID %>").parent("a").html($("#<%=Master.FindControl("Label1").ClientID %>"))

    </script>
    <style>

        .Info TD {
            border: solid 1px #a0c6e5;
            padding:3px;
         
           
        }

    </style>
    <link href="Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script src="Plugin/EasyUI/jquery.easyui.min.js"></script>
    <div class="blockcon" style="margin-bottom:10px;width:100%">
            <div style="margin-top:10px;">
                <div id="iCenter"></div>
                <div id="iResult"><div id="iResultRight" style="">
                   <div id="date"></div>
                    
                    <!-- 右边数据统计 -->
                    <div id="RightData"  style="width:100%; margin:0px;">
          
                    </div>


                    <!-- 右边数据统计 -->


                                  </div>


                


                    <div class="clear"></div>
                
            </div>
        </div>
        
                <input type="hidden" id="hd_leftTreeRefresh" value="0" />
        </div>
</asp:Content>

