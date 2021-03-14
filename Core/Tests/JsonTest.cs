using Core.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Tests
{
    public class JsonTest : BaseTest
    {
        public override string TestName => "JSON";

        public override bool IsBaseline => true;

        public override string Filename => "task.json";

        public override byte[] Execute(List<Contact> list)
        {
            var json = JsonConvert.SerializeObject(list);
            return Encoding.Default.GetBytes(json);
        }
    }
}
