using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Sitecore.PasswordRecovery.Feature.Security.Pipeline
{
    public class CustomRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("RecoverPassword", "sitecore/api/security/recoverpassword/{userName}/{token}", new { controller = "Security", action = "RecoverPassword" });
        }
    }
}