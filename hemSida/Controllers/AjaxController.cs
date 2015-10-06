using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Data.SqlClient;
using System.Web.Security;

namespace hemSida.Controllers
{
    public class AjaxController : ControllerBass
    {
        protected override void HandleUnknownAction(string actionName)
        {
            RedirectToAction("Index", "Login").ExecuteResult(this.ControllerContext);
        }

        [HttpPost]
        public string login(string name, string pw)
        {
            long id = 0; // avnänds ej en nu
            bool loginR = false;

            using (var con = hemSida.Models.ModelBass.getCon())
            {
                loginR = UserLoginHelper.Login(con, name, pw, ref id);
            }

            if (loginR)
                FormsAuthentication.SetAuthCookie(name, false);

            var jp = new JsonResponse();

            jp.result = loginR;

            return jp.ToString();
        }

        [HttpPost]
        public string loginpage()
        {
            var jp = new JsonResponse();

            jp.result = true;
            jp.addData(this, "loginpage");

            return jp.ToString();
        }

        [HttpPost]
        public string logout()
        {
            var jp = new JsonResponse();

            if (isLogdin)
                FormsAuthentication.SignOut();
            else
                jp.ex = "not Authenticated";

            jp.result = isLogdin;

            return jp.ToString();
        }

        [HttpPost]
        public string startpage()
        {
            var jp = new JsonResponse();

            if (isLogdin)
                jp.addData(this, "startpage", new Models.StartModel());
            else
                jp.ex = "not Authenticated";

            jp.result = isLogdin;

            return jp.ToString();
        }

		[HttpPost]
        public string serchresultspage(string searchText, int searchAntal, int searchSida, bool searchPrisSort)
		{
            var jp = new JsonResponse();

            if (isLogdin)
                jp.addData(this, "SerchResultsPage", new Models.SerchResultsModel(searchText, searchAntal, searchSida, searchPrisSort));
            else
                jp.ex = "not Authenticated";

            jp.result = isLogdin;

            return jp.ToString();
		}

        [HttpPost]
        public string productpage(string name)
        {
            var jp = new JsonResponse();

            if (isLogdin)
            {
                jp.addData(this, "ProductPage", new Models.ProductModel(name));
            }
            else
                jp.ex = "not Authenticated";

            jp.result = isLogdin;

            return jp.ToString();
        }

        [HttpPost]
        public string accountcreationpage()
        {
            var jp = new JsonResponse();

            if (isLogdin)
            {
                //jp.addData(this, "accountcreationpage", new Models.AccountCreationModel());
               // jp.addData(this, "SerchResultsPage", new Models.SerchResultsModel(searchText, searchAntal, searchSida, searchPrisSort, searchNameSort));
            }
            else
                jp.ex = "not Authenticated";

            jp.result = isLogdin;

            return jp.ToString();
        }

    }
}