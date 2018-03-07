<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="UserManage.aspx.cs" Inherits="sys_UserManage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #testroomList ul li
        {
            line-height: 20px;
            height: 20px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            getUserManageList();
        });
        var getUserManageList = function () {
            var data = getSearchCondition('UserManageList');
            $("#UserManageList").GridUnload();
            $("#UserManageList").jqGrid({
                url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["用户名", "密码", "真实姓名", "是否活动", "编辑", "id", "segment"],
                colModel: [
                    { name: "UserName", index: "UserName", align: "center", sortable: false },
                    { name: "Password", index: "Password", align: "center", sortable: false },
                    { name: "TrueName", index: "TrueName", align: "center", sortable: false },
                    { name: "Active", index: "Active", align: "center", sortable: false },
                    { name: "act", index: "act", width: 80, align: "center", sortable: false },
                    { name: "id", index: "id", hidden: true },
                    { name: "segment", index: "segment", hidden: true }
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
                    var ids = jQuery("#UserManageList").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var be = "<span  style=\"color:#1c7c9c;cursor:pointer;\" onclick=\"openColorBox('" + cl + "')\">编辑 </span>";
                        be += "<span  style=\"color:#1c7c9c;cursor:pointer;\" onclick=\"DeleteData('" + cl + "')\"> 删除</span>";
                        jQuery("#UserManageList").jqGrid("setRowData", cl, { act: be });
                    }
                }
            }).navGrid("#pageList", { edit: false, add: false, del: false, search: false, pdf: true });
        }
        var openColorBox = function (data) {
            if (data != "") {
                var row = $("#UserManageList").getRowData(data);
                $("#hid_RID").val(row["id"]);
                $("#txt_uname").val(row["UserName"]);
                $("#txt_uname").attr("readOnly", true);
                $("#txt_upwd").val(row["Password"]);
                $("#txt_truename").val(row["TrueName"])

                document.getElementById("chk_active").checked = false;
                if (row["Active"] == "是") {
                    document.getElementById("chk_active").checked = true;
                }
                $("input:radio[name='group']").each(function () {
                    if (this.value == row["segment"]) { this.checked = true; } else { this.checked = false; }
                });

            }
            else {
                $("#hid_RID,#txt_uname,#txt_upwd,#txt_truename").val("");
                $("#txt_uname").removeAttr("readOnly");
                document.getElementById("chk_active").checked = true;
            }
            getTestRoomList();
            $.colorbox({ href: "#colorBox", width: 1300, height: 500, title: function () { return "用户管理"; }, close: "", inline: true, scrolling: false });
        }

        var DeleteData = function (data) {
            if (confirm("您确定删除该用户吗?")) {
                $.ajax({
                    url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx?num=") %>' + Math.random(),
                    type: 'post', datatype: 'text',
                    data: { sType: "DeleteUserManageList", UserName: $("#UserManageList").getRowData(data)["UserName"] },
                    success: function (msg) {
                        if (msg == "1") { getUserManageList(); alert("删除成功!"); }
                        else { alert("删除失败!"); }
                    }
                });
        }
        }
    var getTestRoomList = function () {
        $.ajax({ url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx?num=") %>' + Math.random(), type: 'post', datatype: 'html', data: { sType: "getGCList", uname: $("#txt_uname").val() }, success: function (msg) { $("#testroomList").html(msg); } });
    }
    var save = function () {
        var RID = $("#hid_RID").val();
        var uname = $("#txt_uname").val();
        var upwd = $("#txt_upwd").val();
        var truename = $("#txt_truename").val();
        var active = $("#chk_active:checked").length;
        var group = $("input:radio[name='group']:checked").val();
        var temp = "";
        if ($.trim(uname).length == 0) { alert("请输入用户名!"); return false; }
        if ($.trim(upwd).length == 0) { alert("请输入密码!"); return false; }
        if ($.trim(truename).length == 0) { alert("请输入真实姓名!"); return false; }
        $("#testroomList input:checkbox:checked").each(function () { temp += this.value + ","; });
        if (temp.length == 0) { alert("请选择工程或试验室!"); return false; }
        $.ajax({
            url: '<%= ResolveUrl("~/ajax/AjaxJson.aspx?num=") %>' + Math.random(),
            type: 'post',
            datatype: 'text',
            data: { sType: "SaveSysBsUsers", RID: RID, uname: uname, upwd: upwd, truename: truename, active: active, group: group, temp: temp },
            success: function (msg) {
                if (msg == "1") {
                    getUserManageList();
                    $.colorbox.close();
                }
                else if (msg == "2") {
                    alert("用户名已经存在!");
                    $("#txt_uname").val("");
                    $("#txt_uname").focus();
                }
                else {
                    alert("操作失败!");
                }
            }
        });
    }

        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.SelectName = $('#txt_username').val();
            data.sType = stype;
            return data;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>添加用户</span>

                <ul class="searchbar_01 clearfix">
                      <li><span class="text">用户名</span><input class="searchadd" type="text" id="txt_username" /></li>
                    <li class="right"><input name="" type="button" class="list_btn" title="列表"  onclick="getUserManageList();"  /></li>
                </ul>
            </h2>
            <div class="content">
             

                <a  onclick="openColorBox('')"   >新增</a>
                <table id="UserManageList">
                    </table>
                    <div id="pageList">
                    </div>
                
            </div>
        </div>
    </div>


    <div style="display: none;">
        <div id="colorBox" style="height: 83%; width: 100%; text-align: center;">
            <table width="100%">
                <tr>
                    <td style="padding-bottom: 10px;">用户:&nbsp;&nbsp;<input type="text" id="txt_uname" />&nbsp;&nbsp;&nbsp;&nbsp; 密码:&nbsp;&nbsp;<input type="text" id="txt_upwd" />&nbsp;&nbsp;&nbsp;&nbsp; 真实姓名: &nbsp;&nbsp;<input type="text"
                            id="txt_truename" />&nbsp;&nbsp;&nbsp;&nbsp; IsActive:&nbsp;&nbsp;<input type="checkbox"
                                id="chk_active" checked="checked" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px;">
                        <input type="radio" id="radioSS" value="SS" name="group"  />高级管理员&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" id="radioS" value="S" name="group"  />管理员&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" id="radioX" value="X" name="group" />信息中心账号&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" id="radioA" value="A" name="group" checked="checked" />业主&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" id="radioB" value="B" name="group" />施工中心试验室主任&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" id="radioC" value="C" name="group" />施工工区主任&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" id="radioD" value="D" name="group"  />监理帐号&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="testroomList" style="height: 280px; overflow-y: scroll; overflow-x: scroll; border: 1px solid #d3d3d3; text-align: left;">
                        </div>
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


