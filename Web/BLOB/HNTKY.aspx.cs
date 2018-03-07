using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using JZ.BLL;
using System.Data;
using System.Data.SqlClient;

public partial class BLOB_HNTKY : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if ("Act".IsRequest())
        {
           
            switch ("Act".RequestStr().ToUpper())
            {
                case "ATTR":
                    //       
                    ItemCollection Items = BLOB.Attr("ItemID".RequestStr());
       
                    try
                    {
                        Items.Remove("安定性");
                        Items.Remove("烧失量");
                        Items.Remove("胶砂流动度");
                        Items.Remove("三氧化硫含量");
                        Items.Remove("氯离子含量");
                        Items.Remove("碱含量");
                        Items.Remove("游离氧化钙含量");
                        Items.Add(new Item() { ItemName = "用量", BindField = "YLL", StandardBindField = "" });
                    }
                    catch { }
                    Response.Write(JsonConvert.SerializeObject(Items));
                    Response.End();
                    break;

                case "CHART":
                    Chart();
                    break;
                   
            }
        }
    }


    public void Chart()
    {

        string As = "AS".RequestStr();

        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr();

        string Json = string.Empty;


        DataSet Ds = BLOB.HNTQD("YCItem".RequestStr(), "QDDJ".RequestStr(), "Attr".RequestStr(), "YLL".RequestStr(), "YLLOffset".RequestStr(), StartDate, EndDate,SelectedTestRoomCodes);
        Json += "{";
        Json +="\"D1\":"+ Newtonsoft.Json.JsonConvert.SerializeObject(Ds.Tables[0]);
        Json += ",\"D2\":" + Newtonsoft.Json.JsonConvert.SerializeObject(Ds.Tables[1]);
        Json += "}";

        Response.Write(Json);
        Response.End();
    }
}