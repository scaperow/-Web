using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using JZ.BLL;

public partial class ajaxTree : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String result = "";
        String sType = Request.Params["type"];
        if (!String.IsNullOrEmpty(sType))
        {
            switch (sType)
            {
                case "init":
                    result = GetTreeJson();
                    break;
                case "checked":
                    TreeNodeClicked();
                    break;
                case "select":
                    SetSelect();
                    break;
                default:
                    break;
            }
        }
        Response.Write(result);
    }

    public String GetTreeJson()
    {
        String json = "";
        if (Session["leftTree"] == null || Session["leftTree"].ToString() == "" || Session["leftTree"].ToString() == "[]")
        {
            #region 
            string Sql;
            ResultInfo RI = System.Web.HttpContext.Current.Session["UserInfo"] as ResultInfo;
            if (RI.RoleName == "YZ" || RI.RoleName == "ADMIN" || RI.RoleName == "GGZX")
            {
                 Sql = @" SELECT NodeCode ,
                                  DESCRIPTION ,
                                  DepType ,
                                  OrderID FROM dbo.Sys_Tree where len(NodeCode)>4 ORDER BY NodeCode ,OrderID ";
            }
            else
            {

                Sql = @" SELECT TestRoomCode FROM dbo.sys_user_testroom WHERE UserID ='" + RI.ID + "' ";

                DataTable Dt = new JZ.BLL.BOBase().GetDataTable(Sql);

                if (Dt != null)
                {
                    List<String> unions = new List<string>();
                    String[] aa;
                    
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        string t = Dr["TestRoomCode"].ToString();
                        if (t.Length == 16)
                        {
                            unions.Add("  SELECT NodeCode ,[DESCRIPTION] ,DepType ,OrderID FROM dbo.Sys_Tree  WHERE (NodeCode = '#'  OR NodeCode LIKE '" + t.Substring(0, 8) + "%') AND ((LEN(NodeCode) = 8 OR LEN(NodeCode) = 12 )  OR NodeCode = '" + t + "')");
                        }
                        else if (t.Length < 16)
                        {
                            unions.Add("  SELECT NodeCode ,[DESCRIPTION] ,DepType ,OrderID FROM dbo.Sys_Tree  WHERE (NodeCode = '#'  OR NodeCode LIKE '" + t + "%') AND ((LEN(NodeCode) = 8 OR LEN(NodeCode) = 12 )  OR LEN(NodeCode) = 16)");
                        }
                       
                    }
                    if (unions.Count > 0)
                    {
                        Sql = "SELECT distinct t.NodeCode,t.[DESCRIPTION],t.DepType ,t.OrderID FROM (" + String.Join(" UNION ", unions.ToArray()) + ") t ORDER BY t.OrderID";
                    }
                }

            }
            DataTable Dt2 = new JZ.BLL.BOBase().GetDataTable(Sql);
            List<TreeItem> nodes = new List<TreeItem>();
            String segName = "";
            TreeItem item = null;
            foreach (DataRow row in Dt2.Rows)
            {
                int length = row["NodeCode"].ToString().Length;
                switch (length)
                {
                    case 8:

                        segName = row["Description"].ToString();

                        break;
                    case 12:
                        item = new TreeItem();
                        item.children = new List<TreeItem>();
                        item.text = segName + " " + row["Description"].ToString();
                        item.code = row["NodeCode"].ToString();
                        if (row["DepType"].ToString() == "@unit_监理单位")
                        {
                            item.isJL = 1;
                        }
                        else
                        {
                            item.isJL = 0;
                        }

                        nodes.Add(item);

                        break;
                    case 16:
                        TreeItem subItem = new TreeItem();
                        String testRoom = row["NodeCode"].ToString();
                        if (testRoom.EndsWith("0001"))
                        {
                            subItem.isCenter = 1;
                        }
                        else
                        {
                            subItem.isCenter = 0;
                        }
                        subItem.isJL = item.isJL;
                        subItem.text = row["Description"].ToString();
                        subItem.code = testRoom;
                        subItem.css = "";
                        subItem.Checked = true;
                        item.children.Add(subItem);

                        break;
                }
            }

            json =  JsonConvert.SerializeObject(nodes);
            #endregion
            ////////////////////////////////////////////////////////////
            

            Session["leftTree"] = json;

            #region

            List<TreeItem> list = JsonConvert.DeserializeObject<List<TreeItem>>(json);
            if (list != null)
            {
                List<String> selectedTestRoomList = new List<String>();
                foreach (var item1 in list)
                {


                    if (item1.children != null && item1.children.Count > 0)
                    {
                        foreach (var testroom in item1.children)
                        {
                            if (testroom.Checked)
                            {
                                selectedTestRoomList.Add("'" + testroom.code + "'");
                            }
                        }
                    }
                }
                SelectedTestRoomCodes = String.Join(",", selectedTestRoomList.ToArray());
            }

            #endregion
        }
        else
        {
            json = Session["leftTree"].ToString();
        }
        return json;
    }

    public void TreeNodeClicked()
    {
        String json = Request.Params["nodes"];
        Session["leftTree"] = json;
        List<TreeItem> list = JsonConvert.DeserializeObject<List<TreeItem>>(json);
        if (list != null)
        {
            List<String> selectedTestRoomList = new List<String>();
            foreach (var item in list)
            {


                if (item.children != null && item.children.Count > 0)
                {
                    foreach (var testroom in item.children)
                    {
                        if (testroom.Checked)
                        {
                            selectedTestRoomList.Add("'" + testroom.code + "'");
                        }
                    }
                }
            }
            SelectedTestRoomCodes = String.Join(",", selectedTestRoomList.ToArray());
        }
    }

    public void SetSelect()
    {
        String value = Request.Params["value"];
        String testRoom = Request.Params["testRoom"];
        Session["Companyfilter"] = value;
        Session["Testroomfilter"] = testRoom;
    }
}

