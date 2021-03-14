using Core.Data;
using K4os.Compression.LZ4.Streams;
using MessagePack;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.IO;

namespace Core.Tests
{
    public class MsgPackLz4Test : BaseTest
    {
        public override string TestName => "MsgPack + LZ4";

        public override bool IsBaseline => false;

        public override string Filename => "task.msgpacklz4";

        public override byte[] Execute(List<Contact> list)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

            return MessagePackSerializer.Serialize(list, lz4Options);
        }
    }
}
