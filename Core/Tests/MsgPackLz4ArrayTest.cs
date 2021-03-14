using Core.Data;
using K4os.Compression.LZ4.Streams;
using MessagePack;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.IO;

namespace Core.Tests
{
    public class MsgPackLz4ArrayTest : BaseTest
    {
        public override string TestName => "MsgPack + LZ4 Array";

        public override bool IsBaseline => false;

        public override string Filename => "test.msgpacklz4array";

        public override byte[] Execute(List<Contact> list)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
            return MessagePackSerializer.Serialize(list, lz4Options);
        }
    }
}
