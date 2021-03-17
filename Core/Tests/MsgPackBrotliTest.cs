using Core.Data;
using MessagePack;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Core.Tests
{
    public class MsgPackBrotliTest : BaseTest
    {
        public override bool IsEnabled => true;

        public override string TestName => "MsgPack + Brotli Slow";

        public override bool IsBaseline => false;

        public override string Filename => "test.msgpackbr";

        public override byte[] Execute(List<Contact> list)
        {
            var pack = MessagePackSerializer.Serialize(list);

            var ms = new MemoryStream();
            using (var source = new MemoryStream(pack))
            using (var compressorStream = new BrotliStream(ms, CompressionLevel.Optimal)) 
            {
                source.CopyTo(compressorStream);
            }
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
