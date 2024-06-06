using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VXI_GAMS_US.HELPER
{
    /// <summary>
    /// Json Helper
    /// </summary>
    /// <typeparam name="T">Return Class Type Of The JSON String</typeparam>
    public class JsonHelper<T> where T : class
    {
        /// <summary>
        /// Deserialize a Json string to the specified class
        /// </summary>
        /// <param name="stringData">The data to be converted to a class</param>
        /// <returns></returns>
        public static T Deserialize(string stringData)
        {
            if (string.IsNullOrEmpty(stringData))
            {
                return null;
            }

            stringData = Regex.Unescape(stringData);
            stringData = stringData.TrimStart('"');
            stringData = stringData.TrimEnd('"');
            T test;
            if (stringData == "\"Source sequence doesn't contain any elements.\"" || stringData.Equals("{}"))
            {
                return null;
            }

            stringData = stringData.StartsWith("[") && stringData.EndsWith("]") ? $"{{\"Table\":{stringData}}}" : stringData;
            try
            {
                test = JsonConvert.DeserializeObject<T>(stringData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                stringData = $"{{\"Table\":[{stringData}]}}";
                test = JsonConvert.DeserializeObject<T>(stringData);
            }
            return test;
        }
    }
}
