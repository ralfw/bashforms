using System;
using System.Diagnostics;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class Json_tests
    {
        [Test]
        public void Explore_DateTime_handling()
        {
            var json = new JavaScriptSerializer();
            var dt = new DateTime(2000, 5, 12, 9,10,11);
            Console.WriteLine(dt);
            var jsondatetime = json.Serialize(dt);
            Console.WriteLine(jsondatetime);
            var result = json.Deserialize<DateTime>(jsondatetime);
            Console.WriteLine(result.ToLocalTime());
            
            Console.WriteLine(DateTime.MaxValue);
            jsondatetime = json.Serialize(DateTime.MaxValue);
            Console.WriteLine(jsondatetime);
            result = json.Deserialize<DateTime>(jsondatetime);
            Console.WriteLine(result.ToLocalTime());
            Console.Write(result.ToLocalTime().Subtract(DateTime.MaxValue));
        }
    }
}