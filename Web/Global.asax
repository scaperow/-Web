<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        // 在应用程序启动时运行的代码
        log4net.Config.XmlConfigurator.Configure();
        //
        WMReportService.Start();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  在应用程序关闭时运行的代码

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        // 在出现未处理的错误时运行的代码
        try
        {
            Exception ex = Server.GetLastError().GetBaseException();
            String error = "";
            if (ex != null)
            {
                error += ex.Message;
                error += ex.StackTrace;
                error += ex.TargetSite.Name;
                error += ex.TargetSite.DeclaringType.FullName;
            }
            // 清空最后的错误
            Server.ClearError();
            log4net.ILog logger = log4net.LogManager.GetLogger("webLogger");
            logger.Error(error);

        }
        catch { }

        //Server.Transfer("~/error.html");
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
        // 或 SQLServer，则不引发该事件。

    }
       
</script>
