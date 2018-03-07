// JavaScript Document
$(function () {
    $(".topnav li").hover(
	function () { $(this).children("ul").show(); },
	function () { $(this).children("ul").hide(); }
	);

    $(".nav li").hover(
		function () {
		    $(this).children("a:first").addClass("hover");
		    $(this).children("ul").show();
		},
		function () {
		    $(this).children("a:first").removeClass("hover");
		    $(this).children("ul").stop().hide();
		}
	);

    $(".select select").change(function () {
        $(this).prev(".txt").text($(this).find("option:selected").text());
    });

    $(".ico_dow").click(function () {
        $(this).prev(".select").children("select").click().change();
    });
    $(".foot_block").css({ "width": $(".conter").width() });
    $(".leftblock").height($(window).height() - 104);
    $(".conter").height($(window).height() - 104);
    window.onresize = function () {
        $(".leftblock").height($(window).height() - 104);
        $(".conter").height($(window).height() - 104);
        $(".foot_block").css({ "width": $(".conter").width() })
    };

    $("body").css({ "min-width": $(".content").width() + 200 })
    $(".foot_block").css({ "width": $(".conter").width() })
    

    $("ul.left_menu>li>ul").hover(
	function () {
	    $(this).parents("li").children("a:first").addClass("hover");
	},
	function () {
	    $(this).parents("li").children("a:first").removeClass("hover")
	});
});

//公共调用方法
function Merger(gridName, CellName,MarkCellName) {
    //得到显示到界面的id集合

    var mya = $("#" + gridName + "").getDataIDs();
    //当前显示多少条
    var length = mya.length;
    for (var i = 0; i < length; i++) {
        //从上到下获取一条信息
        var before = $("#" + gridName + "").jqGrid('getRowData', mya[i]);

        
        //定义合并行数
        var rowSpanTaxCount = 1;
        for (j = i + 1; j <= length; j++) {
            //和上边的信息对比 如果值一样就合并行数+1 然后设置rowspan 让当前单元格隐藏
            var end = $("#" + gridName + "").jqGrid('getRowData', mya[j]);
           
            var beforename = before[CellName] + before[MarkCellName];
            var endname = end[CellName] + end[MarkCellName];
            if (beforename == endname) {
                rowSpanTaxCount++;
                $("#" + gridName + "").setCell(mya[j], CellName, '', { display: 'none' });
            } else {
                rowSpanTaxCount = 1;
                break;
            }
            $("#" + CellName + "" + mya[i] + "").attr("rowspan", rowSpanTaxCount);
        }
    }
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    }
    return null;
}

function adeptGridWidth() {
    var width = $(".piece_con")[0].clientWidth;
    if (typeof (jqGrid) != "undefined") {
        if (jqGrid != null && jqGrid != undefined) {
            jqGrid.setGridWidth(width);
            //jqGrid.setGridHeight('auto');
        }
    }
}


function maxminsize(data, type, column) {
    var len = data.length;
    var j = 0;
    if (len > 0) {
        var j = data[0][column];
        if (type == "max") {
            for (var i = 1; i < len ; i++) {
                if (data[i][column] > j) {
                    j = data[i][column];
                }
            }
        }
        else {
            for (var i = 1; i < len ; i++) {
                if (data[i][column] < j) {
                    j = data[i][column];
                }
            }
        }
    }
    return j;
}

function gridCount(max, min) {
    var i = max;
    var n = max - min;
    if (n==0) {
        i = 1;
    }
   else if (n < 10&&n>0) {
        i = max;
    }
    else {
        i = 10;
    }
    return i;
}
function labelFrequency(max, n) {
    var i = max;
    if (n==1) {
        i = 0;
    }
   else if (n <10&&n>1) {
        i = 1;
    }
    else {
        i = 2;
    }
    return i;
}