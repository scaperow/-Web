<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master" CodeFile="SetMapLngLat.aspx.cs" Inherits="SetMapLngLat" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="http://api.amap.com/Public/css/demo.Default.css" />
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.2&key="></script>
    
    <script type="text/javascript">
        var mapObj;



        $(function () {
            mapInit();
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
        });

        //初始化地图
        function mapInit() {
            mapObj = new AMap.Map("iCenter");

            var clickEventListener = AMap.event.addListener(mapObj, 'click', function (e) {
                $('#lngX').val(e.lnglat.getLng());
                $('#latY').val(e.lnglat.getLat());
                $('#Zoom').val(mapObj.getZoom());

            });
        }

        //定位
        function rmapInit(x, y) {
            mapObj.setCenter(new AMap.LngLat(x, y));
            mapObj.setZoom(8)
        }

        function LoadItems() {
            $('#city option').remove();
            $('#city').append('<option value="">请选择</option>');
            $.post("setmaplnglat.aspx?callback=?", { ACT: "GetItems" }, function (data) {
                var obj = jQuery.parseJSON(data);

                $.each(obj.items, function (i, n) {
                    $('#city').append('<option value="' + n.id + '" lng="' + n.x + ',' + n.y + '">' + n.description + '</option>');
                });

                $('#city').find('option[lng = ","]').css({ background: "#ff6a00" });
            })
        }

        function SaveLng() {
            var x = $('#lngX').val();
            var y = $('#latY').val();
            var id = $('#city').find("option:selected").val();

            if (id == '' || x == '' || y == '') { return false; }
            $.post("setmaplnglat.aspx?callback=?&AJAX=" + Math.random(), { ACT: "SaveLng", ID: id, X: x, Y: y }, function (data) {
                if (data == 'true') {
                    LoadItems();
                    $('#lngX').val('');
                    $('#latY').val('');
                }
                else {
                    alert('保存失败');
                }
            });
        }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="iCenter"></div>
<div style="padding:10px 0px 0px 5px;font-size:12px">
实验站：
<select id="city">
	<option value="">选择</option>
	
</select> <span id="isSet" style="color:#ff6a00">未设置</span>
    <br />
<br>
  <div>X:
    <input type="text" id="lngX" name="lngX" value=""/>
    &nbsp;Y:
    <input type="text" id="latY" name="latY" value=""/>
     &nbsp; Zoom:
      <input type="text" id="Zoom" name="latY" value=""/>
      <input type="button" value="保存" onclick="SaveLng();" />
  </div>
    <div style="color:#ff6a00;">
        1：选择相应实验站 
        2：鼠标在地图相应位置点击
        3：点击保存
    </div>
</div>
</asp:Content>
