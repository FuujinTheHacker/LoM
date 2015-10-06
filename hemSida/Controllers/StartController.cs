using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hemSida.Controllers
{
    public class StartController : ControllerBass
    {
        public ActionResult Index()
        {
            return BassIndex("startpage");
        }
    }
}