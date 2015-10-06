using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hemSida.Controllers
{
    [AllowAnonymous]
    public class AccountCreationController : ControllerBass
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public  ActionResult Index(string anActivationKey)
        {
            return View(new Models.AccountCreationModel(anActivationKey));
        }

        [HttpPost]
        public ActionResult Index(string anActivationKey, string aName, string aPassword, string aPassword2)
        {
           return (View(new Models.AccountCreationModel(anActivationKey, aName, aPassword, aPassword2)));
        }
    }
}