using Core.Data;
using MessagePack;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Core.Tests
{
    public class MsgPackGzipFastTest : BaseTest
    {
        public override string TestName => "MsgPack + GZip Fast";

        public override bool IsBaseline => false;

        public override string Filename => "test.msgpackgzf";

        public override byte[] Execute(List<Contact> list)
        {
            var pack = MessagePackSerializer.Serialize(list);

            var ms = new MemoryStream();
            using (var source = new MemoryStream(pack))
            using (var compressorStream = new GZipStream(ms, CompressionLevel.Fastest, true)) //GZipStream adds CRC to ensure data correctness
            {
                source.CopyTo(compressorStream);
            }
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
