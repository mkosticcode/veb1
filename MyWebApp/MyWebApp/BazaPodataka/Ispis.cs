using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.BazaPodataka
{
    public class Ispis
    {
        public static string Serialize(object obj)
        {
            string jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return jsonString;
        }
        public static void PrintSerialize(object obj, string location)
        {
            string jsonStrin = Ispis.Serialize(obj);
            System.IO.File.WriteAllText(location, jsonStrin);
        }
        public static void PrintSerialize(object obj, string location, string name)
        {
            Ispis.PrintSerialize(obj, location + "\\" + name + ".json");
        }
    }
}