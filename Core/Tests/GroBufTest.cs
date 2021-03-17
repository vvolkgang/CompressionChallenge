using Core.Data;
using GroBuf;
using GroBuf.DataMembersExtracters;
using System.Collections.Generic;

namespace Core.Tests
{
    public class GroBufTest : BaseTest
    {
        public override string TestName => "GroBuf (uncompressed)";

        public override string Filename => "test.grobuf";

        public override byte[] Execute(List<Contact> list)
        {
            var serializer = new Serializer(new PropertiesExtractor(), options: GroBufOptions.PackReferences);
            return serializer.Serialize(list);
        }
    }
}
