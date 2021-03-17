using Core.Data;
using ProtoBuf;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Core.Tests
{
    public class ProtoBufGzipTest : BaseTest
    {
        public override string TestName => "ProtoBuf + GZip Slow";

        public override string Filename => "test.protogz";

        public override byte[] Execute(List<Contact> list)
        {
            var ms = new MemoryStream();
            using (var compressorStream = new GZipStream(ms, CompressionLevel.Optimal, true)) //GZipStream adds CRC to ensure data correctness
            {
                Serializer.Serialize(compressorStream, list);
            }
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
