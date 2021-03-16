using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Core.Data;

namespace Core.Tests
{
    public class SystemTextJsonTest : BaseTest
    {
        public override string TestName => "System.Text.Json (uncompressed)";

        public override string Filename => "test.json";
        
        public override byte[] Execute(List<Contact> list)
        {
            var json = JsonSerializer.Serialize(list);
            return Encoding.Default.GetBytes(json);
        }
    }
}