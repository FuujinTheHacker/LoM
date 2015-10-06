using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace hemSida.Controllers
{
    public class ContentDataController : ControllerBass
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult getProductsIMG(string name)
        {
            string file = Server.MapPath("~/") + "ContentData__\\Products\\ProductsIMG\\" + name;

            if (!goodString(name) || !System.IO.File.Exists(file))
            {
                file = Server.MapPath("~/") + "Content\\No-Access.jpg";
                return File(file, "application/force-download", "No-Access.jpg");
            }
            else
                return File(file, "application/force-download", name);
        }

        private bool goodString(string name) {
            if (!isLogdin)
                return false;

            if (name == null)
                return false;

            foreach (var item in name)
                if (!Char.IsLetterOrDigit(item))
                    return false;

            return true;
        }

        protected override void HandleUnknownAction(string actionName)
        {
            RedirectToAction("Index", "Login").ExecuteResult(this.ControllerContext);
        }
    }
}