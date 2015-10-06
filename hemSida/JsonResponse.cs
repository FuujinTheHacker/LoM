using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace hemSida
{
    public class JsonResponse
    {
        public bool result
        {
            get
            {
                return (bool)_data["result"];
            }
            set
            {
                _data["result"] = value;
            }
        }

        public string data
        {
            get
            {
                return (string)_data["data"];
            }
            set
            {
                _data["data"] = value;
            }
        }

        public string ex
        {
            get
            {
                return (string)_data["ex"];
            }
            set
            {
                _data["ex"] = value;
            }
        }

        Dictionary<string, object> _data = new Dictionary<string, object>();

        public static string text(params object[] args)
        {
            var data = new Dictionary<string, object>();
            for (int i = 0; i < args.Length; i += 2)
                data[args[i] + ""] = args[i + 1];
            return new JavaScriptSerializer().Serialize(data);
        }

        public JsonResponse(params object[] args)
        {
            if (args.Length == 0)
                return;
            for (int i = 0; i < args.Length; i += 2)
                this[args[i] + ""] = args[i + 1];
        }

        public void addData(string data)
        {
            this["data"] = data;
        }

        public void addData(Controller con , string name , object obj = null)
        {
            this["data"] = Controllers.ControllerBass.PartialView(con, name, obj);
        }

        public Object this[string a]
        {
            get
            {
                return _data[a];
            }
            set
            {
                _data[a] = value;
            }
        }

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(_data);
        } 

    }
}