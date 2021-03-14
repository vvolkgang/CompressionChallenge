using Core.Data;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Core.Tests
{
    public class MsgPackLz4GzipTest : BaseTest
    {
        public override bool IsEnabled => false;

        public override string TestName => "MsgPack + LZ4 + GZip";

        public override bool IsBaseline => false;

        public override string Filename => "task.msgpacklz4gz";

        public override byte[] Execute(List<Contact> list)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

            var pack = MessagePackSerializer.Serialize(list, lz4Options);

            var ms = new MemoryStream();
            using (var source = new MemoryStream(pack))
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
