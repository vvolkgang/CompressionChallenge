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
    public class MsgPackGzipTest : BaseTest
    {
        public override string TestName => "MsgPack + GZip";

        public override bool IsBaseline => false;

        public override string Filename => "test.msgpackgz";

        public override byte[] Execute(List<Contact> list)
        {
            var pack = MessagePackSerializer.Serialize(list);

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
