using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hemSida.Controllers
{
    [AllowAnonymous]
    public class LoginController : ControllerBass
    {
        public ActionResult Index()
        {
            return BassIndex("startpage");
        }
    }
}