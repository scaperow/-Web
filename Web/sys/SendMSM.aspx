<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SendMSM.aspx.cs" Inherits="sys_SendMSM" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #testroomList ul li
        {
            line-height: 30px;
           
        }
    </style>
    <script type="text/javascript">
        $(function () {
            getSendMSMList();
        });
        var getSendMSMList = function () {
            var data = getSearchCondition('SendMSMList');
            $("#SendMSMList").GridUnload();
            $("#SendMSMList").jqGrid({
                url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["试验室编码", "试验室名称", "用户", "电话", "是否活动", "编辑", "ID"],
                colModel: [
                    { name: "TestRoomCode", index: "TestRoomCode", align: "center", sortable: false },
                    { name: "TestRoom", index: "TestRoom", align: "center", sortable: false },
                    { name: "PersonName", index: "PersonName", align: "center", sortable: false },
                    { name: "CellPhone", index: "CellPhone", align: "center", sortable: false },
                    { name: "Active", index: "Active", align: "center", sortable: false },
                    { name: "act", index: "act", width: 80, align: "center", sortable: false },
                    { name: "ID", index: "ID", hidden: true }
                ],
                autowidth: true,
                shrinkToFit: true,
                jsonReader: { page: "page", total: "total", repeatitems: false },
                pager: jQuery("#pageList"),
                rowNum: 15,
                rowList: [15, 30, 50, 100],
                viewrecords: true,
                height: 420,
                postData: data,
                loadui: "disable",
                gridComplete: function () {
                    var ids = jQuery("#SendMSMList").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var  be = "<span  style=\"color:#1c7c9c;cursor:pointer;\" onclick=\"DeleteData('" + cl + "')\"> 删除</span>";
                        jQuery("#SendMSMList").jqGrid("setRowData", cl, { act: be });
                    }
                }
            }).navGrid("#pageList", { edit: false, add: false, del: false, search: false, pdf: true });
        }

        
        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.SelectName = $('#txt_username').val();
            data.Tel = $('#txt_tel').val();
            data.sType = stype;
            return data;
        }


        var openColorBox = function (data) {
            if (data != "") {
                var row = $("#SendMSMList").getRowData(data);
                $("#hid_RID").val(row["ID"]);
                $("#txt_uname").val(row["PersonName"]);
                $("#txt_uname").attr("readOnly", true);
                $("#txt_utel").val(row["CellPhone"]);
                document.getElementById("chk_active").checked = false;
                if (row["Active"] == "是") {
                    document.getElementById("chk_active").checked = true;
                }
                $("#testroomList").html("<div style=\"padding:20px;\">修改只能修改手机和IsActive!</div>");
            }
            else {
                getTestRoomList();
                $("#hid_RID,#txt_uname,#txt_utel").val();
                $("#txt_uname").removeAttr("readOnly");
                document.getElementById("chk_active").checked = true;
            }
            $.colorbox({ href: "#colorBox", width: 960, height: 600, title: function () { return "短信配置"; }, close: "", inline: true, scrolling: false });
        }

        var DeleteData = function (data) {
            if (confirm("您确定删除该用户吗?")) {
                $.ajax({
                    url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx?num=") %>' + Math.random(),
                    type: 'post', datatype: 'text',
                    data: { sType: "DeleteSendMSMList", SID: $("#SendMSMList").getRowData(data)["ID"] },
                    success: function (msg) {
                        if (msg == "1") { getSendMSMList(); alert("删除成功!"); }
                        else { alert("操作失败!"); }
                    }
                });
        }
        }
    var getTestRoomList = function () {
        $.ajax({ url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx?num=") %>' + Math.random(), type: 'post', datatype: 'html', data: { sType: "getTestRoomList" }, success: function (msg) { $("#testroomList").html(msg); } });
    }
    var jl_ChkChange = function (obj) {
        var checked = obj.checked;
        $(obj).parent().find("input:checkbox").each(function () { this.checked = checked; });
    }
    var selectAll = function (checked) {
        $("#testroomList input:checkbox").each(function () { this.checked = checked; });
    }
    var save = function () {
        var RID = $("#hid_RID").val();
        var uname = $("#txt_uname").val();
        var utel = $("#txt_utel").val();
        var active = $("#chk_active:checked").length;
        var temp = "", temp1 = "";
        if ($.trim(uname).length == 0) { alert("请输入用户名!"); return false; }
        if ($.trim(utel).length == 0) { alert("请输入手机号码!"); return false; }
        if (utel.length != 11 || isNaN(utel)) { alert("请输入正确的手机号码!"); return false; }
        if (RID == "") {
            $("#testroomList input:checkbox:checked[name='group']").each(function () {
                temp += this.value + ",";
                temp1 += $(this).next().text() + ","
            });
            if (temp.length == 0) { alert("请选择试验室!"); return false; }
        }
        $.ajax({
            url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx?num=") %>' + Math.random(),
                type: 'post',
                datatype: 'text',
                data: { sType: "SaveSmsReceiver", RID: RID, uname: uname, utel: utel, active: active, temp: temp, temp1: temp1 },
                success: function (msg) {
                    if (msg == "1") {
                        getSendMSMList();
                        $.colorbox.close();
                        alert("操作成功!");
                    }
                    else {
                        alert("操作失败!");
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

       <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>短信配置</span>

                <ul class="searchbar_01 clearfix">

                     <li><span class="text">用户</span><input class="searchadd" type="text" id="txt_username" /></li>
                    <li><span class="text">电话</span><input class="searchadd" type="text" id="txt_tel" /></li>

                    <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="getSendMSMList();"  /></li>
                </ul>
            </h2>
            <div class="content">

 

                <a  onclick="openColorBox('')" >新增</a>

                <table id="SendMSMList">
                    </table>
                    <div id="pageList">
                    </div>
                
            </div>
        </div>
    </div>


    <div style="display: none;">
        <div id="colorBox" style="height: 100%; width: 100%; text-align: center;">
            <table width="100%">
                <tr>
                    <td style="text-align: right; height: 20px;">
                        <a onclick="selectAll(true)" style="cursor: pointer;">全选</a><a style="margin-left: 10px; cursor: pointer;"
                            onclick="selectAll(false)">取消</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="testroomList" style="height: 390px; overflow-y: scroll; overflow-x: scroll; border: 1px solid #d3d3d3; text-align: left;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">用户: &nbsp;&nbsp;<input type="text" id="txt_uname" />&nbsp;&nbsp;&nbsp;&nbsp; 手机:&nbsp;&nbsp;<input
                        type="text" id="txt_utel" />&nbsp;&nbsp;&nbsp;&nbsp; IsActive:&nbsp;&nbsp;<input
                            type="checkbox" id="chk_active" checked="checked" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 60px; vertical-align: bottom; text-align: right;">
                        <input type="hidden" id="hid_RID" />
                        <input type="button" id="btn_save" value="提交" onclick="save()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

