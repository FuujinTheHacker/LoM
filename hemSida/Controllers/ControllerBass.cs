using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace hemSida.Controllers
{
    public abstract class ControllerBass : Controller
    {
        protected bool isLogdin { get { return (User != null && User.Identity.IsAuthenticated); } }

        protected ActionResult BassIndex(string target)
        
        {
            if (!isLogdin)
                ViewBag.initTarget = "loginpage";
            else
                ViewBag.initTarget = target;

            return View("Index");
        }

        //this codewontwork;

        protected override void HandleUnknownAction(string actionName)
        {
            RedirectToAction("Index").ExecuteResult(this.ControllerContext);
        }

        // http://stackoverflow.com/questions/18667447/return-partial-view-and-json-from-asp-net-mvc-action
        new protected string PartialView(string viewName, object model)
        {
            return ControllerBass.PartialView(this, viewName, model);
        }

        public static string PartialView(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.ToString();
            }
        }

    }
}