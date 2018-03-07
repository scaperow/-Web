<%@ WebHandler Language="C#" Class="ajaxfullcalendar" %>

using System;
using System.Web;
using JZ.BLL;
using System.Collections.Generic;

public class ajaxfullcalendar : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string type = context.Request.Params["type"];


        switch (type)
        {
            case "show":
                GetList();
                break;
        }

    }

    private void GetList()
    {
      

        List<DayDetail> infos = new List<DayDetail>();

        for (int i = 0; i < 2; i++)
        {
            DayDetail day = new DayDetail();
            day.Id = i;
            day.Repeatedweeks = i;
            day.Meetingroom = "testroom" + i;
            day.Title = "title" + i;
            day.Evtstart = DateTime.Now.AddDays(i);
            day.Evtend = DateTime.Now.AddDays(i);
            day.Description = DateTime.Now.AddDays(i).ToShortDateString();

            infos.Add(day);

        }
        System.Web.Script.Serialization.JavaScriptSerializer jsS = new System.Web.Script.Serialization.JavaScriptSerializer();

        string jsonArrStr = jsS.Serialize(infos);
        HttpContext.Current.Response.Write(jsonArrStr);


    }

    
    public bool IsReusable {
        get {
            return false;
        }
    }

}