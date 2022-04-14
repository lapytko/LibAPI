using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibAPI.Extensions
{
    public static class JsonExtension
    {
        public static bool JsonIsObject(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            json = json.Trim();
            if (!json.StartsWith("{") || !json.EndsWith("}")) return false;
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}