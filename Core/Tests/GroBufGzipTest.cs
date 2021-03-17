using Core.Data;
using GroBuf;
using GroBuf.DataMembersExtracters;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Core.Tests
{
    public class GroBufGzipTest : BaseTest
    {
        public override string TestName => "GroBuf + GZip";

        public override string Filename => "test.grobufgz";

        public override byte[] Execute(List<Contact> list)
        {
            var serializer = new Serializer(new PropertiesExtractor(), options: GroBufOptions.PackReferences);
            byte[] gro = serializer.Serialize(list);

            var ms = new MemoryStream();
            using (var source = new MemoryStream(gro))
            using (var compressorStream = new GZipStream(ms, CompressionLevel.Optimal, true)) //GZipStream adds CRC to ensure data correctness
            {
                source.CopyTo(compressorStream);
            }
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
