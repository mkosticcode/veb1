using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.BazaPodataka
{
    public class Upis
    {
        public static Object DeSerialize<T>(string json)
        {
            var jsonObject = JsonConvert.DeserializeObject<T>(json);
            return jsonObject;
        }
    }
}