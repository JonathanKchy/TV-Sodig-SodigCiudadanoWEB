using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Util
{
    public class SecurityFilter : ActionFilterAttribute, IActionFilter
    {
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			try
			{
				base.OnActionExecuting(filterContext);				
				string identificacion = (string)HttpContext.Current.Session["Identificacion"];

				if (string.IsNullOrEmpty(identificacion))
                {
					filterContext.HttpContext.Response.Redirect("~/Auth/login");
				}
					
			}
			catch (Exception)
			{
				filterContext.Result = new RedirectResult("/");
			}
		}
	}
}