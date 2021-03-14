using Core.Data;
using K4os.Compression.LZ4.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.IO;

namespace Core.Tests
{
    public class JsonLz4Test : BaseTest
    {
        public override string TestName => "JSON + LZ4";

        public override bool IsBaseline => false;

        public override string Filename => "test.jsonlz4";

        public override byte[] Execute(List<Contact> list)
        {
            var json = JsonConvert.SerializeObject(list);

            var ms = new MemoryStream();
            using (var source = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
            using (var target = LZ4Stream.Encode(ms))
            {
                source.CopyTo(target);
            }

            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
