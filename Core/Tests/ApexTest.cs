using Apex.Serialization;
using Core.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Tests
{
    public class ApexTest : BaseTest
    {
        public override string TestName => "Apex v1.3.4 (uncompressed)";

        public override string Filename => "test.apex";

        public override byte[] Execute(List<Contact> list)
        {
            var binarySerializer = Binary.Create();
            var ms = new MemoryStream();

            binarySerializer.Write(list, ms);
            var bytes = ms.ToArray();
            ms.Dispose();

            return bytes;
        }
    }
}
