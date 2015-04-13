﻿namespace EasyERP.Web.Controllers
{
    using EasyErp.Core.Infrastructure;
    using EasyERP.Web.Framework.Controllers;
    using System.Web.Mvc;
    using System.Web.Routing;

    public abstract partial class BasePublicController : BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            IController errorController = EngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }
    }
}