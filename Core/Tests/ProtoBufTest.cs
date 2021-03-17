using Core.Data;
using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace Core.Tests
{
    public class ProtoBufTest : BaseTest
    {
        public override string TestName => "ProtoBuf (uncompressed)";

        public override string Filename => "test.proto";

        public override byte[] Execute(List<Contact> list)
        {
            var ms = new MemoryStream();

            Serializer.Serialize(ms, list);
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
