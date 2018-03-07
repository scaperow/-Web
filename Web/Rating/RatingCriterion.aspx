<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RatingCriterion.aspx.cs" Inherits="Rating_RatingCriterion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>铁路工地试验室违规行为信用等级扣分标准</title>
    <link href="../Plugin/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <script src="../js/jquery-1.9.0.min.js"></script>
    <script src="../Plugin/bootstrap/js/bootstrap.js"></script>
    <style type="text/css">
        .criterion
        {
        }
            .criterion table tr td
            {
                text-align:center;
                padding:3px;
            }
             .criterion table tr th
            {
                text-align:center;
                font-weight:bold;
            }
    </style>
</head>
<body>
 <div class="container-fluid">
            
            <div class="page-header" style="margin:0px 0 10px 0; border:none;"></div>

            <form class="form-horizontal" role="form" id="ff">
            <div class="criterion">
                <div style="border-bottom:2px solid #2d5ab7; margin-bottom:5px; font-size:15px; font-weight:bold;">建设单位</div>
                <table style="width:100%;" border="1" >
                    <tr>
                        <th rowspan="2" style="text-align:center; width:45px;">序号</th>
                        <th rowspan="2" style="width:90px;">行为类别</th>
                        <th rowspan="2" style="width:200px;">违规行为描述</th>
                        <th rowspan="2" style="width:200px;">标准要求</th>
                        <th colspan="3" >扣分标准</th>
                        
                    </tr>
                    <tr>
                        <th  style="width:75px;">建设单位</th>
                        <th style="width:75px;">主管部分负责人</th>
                        <th>专职试验检测管理人员</th>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td>人员</td>
                        <td>未配备专职试验检测工作管理人员或人员资格不符合要求</td>
                        <td>按TB10424-2009要求配备专职管理人员</td>
                        <td>不得评为A级</td>
                        <td>通报批评</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td rowspan="2">制度及验收</td>
                        <td>未制定试验检测标准化管理办法（实施细则）且管理工作记录缺失</td>
                        <td>制定管理办法，晚上相关记录</td>
                        <td>-1</td>
                        <td>通报批评</td>
                        <td>-3</td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>对未安装信息系统及其他不符合标准条件的工地试验室予以验收或认可不符合条件的外委试验检测机构</td>
                        <td>建立信息系统，按计量认证及TB10442等有关要求验收或认可</td>
                        <td>-1</td>
                        <td>通报批评</td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>4</td>
                        <td rowspan="3">工作管理</td>
                        <td>未按要求定期登陆信息系统</td>
                        <td>按信息化管理要求定期登陆</td>
                        <td></td>
                        <td></td>
                        <td>-1</td>
                    </tr>
                    <tr>
                        <td>5</td>
                        <td>未按要求组织检查</td>
                        <td>按标准化管理要求定期组织检查</td>
                        <td></td>
                        <td></td>
                        <td>-1</td>
                    </tr>
                    <tr>
                        <td>6</td>
                        <td>发现不合格数据未予以督促处理</td>
                        <td>建立不合格台账并及时处理，实现闭环管理</td>
                        <td>-1</td>
                        <td>通报批评</td>
                        <td>-2</td>
                    </tr>
                </table>
                <br /><br />
                <div style="border-bottom:2px solid #2d5ab7; margin-bottom:5px; font-size:15px; font-weight:bold;">施工、监理单位</div>
                <table style="width:100%;" border="1" >
                    <tr>
                        <th rowspan="2" style="text-align:center; width:45px;">序号</th>
                        <th rowspan="2" style="width:90px;">行为类别</th>
                        <th rowspan="2" style="width:200px;">违规行为描述</th>
                        <th rowspan="2" style="width:200px;">标准要求</th>
                        <th colspan="4" >扣分标准</th>
                        
                    </tr>
                    <tr>
                        <th  style="width:55px;">试验室</th>
                        <th style="width:55px;">主任</th>
                        <th style="width:55px;">技术负责人</th>
                        <th>责任人</th>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td rowspan="7">信息管理</td>
                        <td>擅自改变信息管理系统设置，造成信息管理系统工作不正常或传输虚假数据；故意停用、损坏信息管理系统</td>
                        <td>严禁故意停用损坏信息系统和擅自修改系统</td>
                        <td>-6</td>
                        <td>-3</td>
                        <td></td>
                        <td>-6</td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>由于维护不到位，造成系统工作不正常或系统出现故障未及时联系维修处理，造成大量数据未及时（超过5天）上传的</td>
                        <td>及时进行维护系统保证工作正常</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>网络和采集系统正常，试验后24小时未上传数据，且未及时通知软件厂商处理</td>
                        <td>应及时通知厂商处理</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>4</td>
                        <td>不按规定录入信息，造成信息管理系统混乱</td>
                        <td>按规定及时录入</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td></td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>5</td>
                        <td>未配备信息化管理员</td>
                        <td>按标准化管理要求定期组织检查</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>6</td>
                        <td>未制定可操作性的信息化管理制度或基本落实相关制度</td>
                        <td>制定可操作性的信息化管理制度并执行</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>7</td>
                        <td>仪器设备不满足信息化安装条件，不按时整改</td>
                        <td>满足信息化安装条件</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>8</td>
                        <td rowspan="6">人员设备及环境</td>
                        <td>未按规定配备试验检测人员</td>
                        <td>按要求配备人员</td>
                        <td>-3</td>
                        <td>-1</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>9</td>
                        <td>未按规定合理配置主要仪器设备</td>
                        <td>按要求配备设备</td>
                        <td>-3</td>
                        <td>-1</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>10</td>
                        <td>试验检测环境达不到技术标准规定</td>
                        <td>检测环境应满足标准要求</td>
                        <td>-2</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>11</td>
                        <td>聘用纳入黑名单人员、聘用本建设项目清理出场人员和无证试验检测人员从事试验检测工作</td>
                        <td>聘用符合要求的检测人员</td>
                        <td>-3</td>
                        <td>-3</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>12</td>
                        <td>试验报告批准人不是母体机构派出的授权签字人</td>
                        <td>试验报告审批人应是母体机构排除的授权签字人</td>
                        <td>-2</td>
                        <td>-1</td>
                        <td></td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>13</td>
                        <td>工地试验室负责人未取得岗位资格证书</td>
                        <td>应取得总公司主管部门颁发的岗位资格证书</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td rowspan="9">试验检测工作</td>
                        <td>未通过验收出具试验检测报告</td>
                        <td>验收通过后开展工作</td>
                        <td>-3</td>
                        <td>-3</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>15</td>
                        <td>超出资质范围开展试验检测工作</td>
                        <td>授权范围内开展工作</td>
                        <td>-3</td>
                        <td>-3</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>16</td>
                        <td>未按相关部门报验要求进行试验检测，导致试验检测频次不足</td>
                        <td>检测频次需满足验标要求</td>
                        <td>-2</td>
                        <td>-1</td>
                        <td></td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>17</td>
                        <td>采用的标准或试验检测方法错误，造成试验结果失真</td>
                        <td>按标准、规程检测</td>
                        <td>-3</td>
                        <td></td>
                        <td>-3</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>18</td>
                        <td>未按要求进行外委试验或者试验频率不足</td>
                        <td>按要求进行试验</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>19</td>
                        <td>严重违反试验检测技术规程</td>
                        <td>按试验规程检测</td>
                        <td>-2</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>20</td>
                        <td>使用不在检定有效期内仪器设备或使用不合格仪器设备进行检测</td>
                        <td>使用在有效期内合格仪器</td>
                        <td>-1</td>
                        <td></td>
                        <td>-1</td>
                        <td>-1</td>
                    </tr>
                    <tr>
                        <td>21</td>
                        <td>能力验证结果“不满足”</td>
                        <td>能力验证结果“满足”</td>
                        <td>-1</td>
                        <td></td>
                        <td>-1</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>22</td>
                        <td>对各级管理部门提出的问题不整改闭合</td>
                        <td>各级检查问题需及时整改闭合</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td>-2</td>
                    </tr>
                    <tr>
                        <td>23</td>
                        <td>不正当行为</td>
                        <td>在试验检测工作中有徇私舞弊、吃拿卡要行为的；利用工作之便推销建筑菜窖、构配件和设备的；玩忽职守造成不良后果</td>
                        <td>工作行为应规范</td>
                        <td>-6</td>
                        <td></td>
                        <td></td>
                        <td>-12</td>
                    </tr>
                    <tr>
                        <td>24</td>
                        <td rowspan="3">试验检测报告</td>
                        <td>伪造或故意修改试验检测数据，出具虚假试验检测报告</td>
                        <td>试验数据、报告须真实</td>
                        <td>-6</td>
                        <td>-3</td>
                        <td>-3</td>
                        <td>-6</td>
                    </tr>
                    <tr>
                        <td>25</td>
                        <td>出具虚假数据、报告，蓄意隐瞒实际质量状况</td>
                        <td>试验数据、报告须真实</td>
                        <td>-12</td>
                        <td>-6</td>
                        <td></td>
                        <td>-6</td>
                    </tr>
                    <tr>
                        <td>26</td>
                        <td>越权签发、代签、漏签试验检测报告</td>
                        <td>报告批准人签发试验报告不能代签、漏签</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td>-1</td>
                    </tr>
                </table>
                <br /><br />
                <div style="border-bottom:2px solid #2d5ab7; margin-bottom:5px; font-size:15px; font-weight:bold;">监理单位</div>
                <table style="width:100%;" border="1" >
                    <tr>
                        <th rowspan="2" style="text-align:center; width:45px;">序号</th>
                        <th rowspan="2" style="width:90px;">行为类别</th>
                        <th rowspan="2" style="width:200px;">违规行为描述</th>
                        <th rowspan="2" style="width:200px;">标准要求</th>
                        <th colspan="4" >扣分标准</th>
                        
                    </tr>
                    <tr>
                        <th  style="width:55px;">试验室</th>
                        <th style="width:55px;">主任</th>
                        <th style="width:55px;">技术负责人</th>
                        <th>责任人</th>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td rowspan="3">监理实验检测管理</td>
                        <td>与施工单位选择同一外委试验检测机构</td>
                        <td>监理、施工单位不能选择同一外委试验检测机构</td>
                        <td>-1</td>
                        <td>-1</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>平行频率不够</td>
                        <td>按要求进行平行试验</td>
                        <td>-2</td>
                        <td>-2</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>利用施工单位工地试验室开展平行试验或安排施工单位完成平行试验</td>
                        <td>应独立完成平行试验</td>
                        <td>-3</td>
                        <td>-3</td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            </form>
 </div>
</body>
</html>
