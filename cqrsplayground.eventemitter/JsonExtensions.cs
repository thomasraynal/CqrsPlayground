using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public static class JsonExtensions
    {
        public static Type GetEvenType(this String jsonObject)
        {
            var obj = JObject.Parse(jsonObject);
            var type = (string)obj["Type"];
            return Type.GetType(type);
        }
    }
}
