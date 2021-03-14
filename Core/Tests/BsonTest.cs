using Core.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.IO;

namespace Core.Tests
{
    public class BsonTest : BaseTest
    {
        public override string TestName => "BSON";

        public override bool IsBaseline => false;

        public override string Filename => "test.bson";

        public override byte[] Execute(List<Contact> list)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, list);
            }
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
