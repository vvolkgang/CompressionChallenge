using Core.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Core.Tests
{
    public class JsonGzipFast : BaseTest
    {
        public override string TestName => "JSON + GZip Fast";

        public override bool IsBaseline => false;

        public override string Filename => "test.jsongzf";

        public override byte[] Execute(List<Contact> list)
        {
            var json = JsonConvert.SerializeObject(list);

            var ms = new MemoryStream();
            using (var source = new MemoryStream(Encoding.Default.GetBytes(json)))
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
