using Apex.Serialization;
using Core.Data;
using MessagePack;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Core.Tests
{
    public class ApexGzipTest : BaseTest
    {
        public override string TestName => "Apex v1.3.4 + GZip Slow";

        public override string Filename => "test.msgpackgz";

        public override byte[] Execute(List<Contact> list)
        {
            var binarySerializer = Binary.Create();

            var ms = new MemoryStream();
            using (var compressorStream = new GZipStream(ms, CompressionLevel.Optimal, true)) //GZipStream adds CRC to ensure data correctness
            {
                binarySerializer.Write(list, compressorStream);
            }
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
