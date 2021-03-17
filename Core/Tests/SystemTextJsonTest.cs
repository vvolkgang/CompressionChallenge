using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Core.Data;

namespace Core.Tests
{
    public class SystemTextJsonTest : BaseTest
    {
        public override string TestName => "System.Text.Json (uncompressed)";

        public override string Filename => "test.msjson";
        
        public override byte[] Execute(List<Contact> list)
        {
            // By default this serializer encodes the country code + with it's unicode textual equivalent
            // making the output file slightly larger than Newtonsoft.JSON
            // Dirty hack so both output the same result, don't use this in production code
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };

            var json = JsonSerializer.Serialize(list, options);
            return Encoding.Default.GetBytes(json);
        }
    }
}