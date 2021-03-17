using ByteSizeLib;
using MessagePack;
using ProtoBuf;

namespace Core.Data
{
    public enum DataState
    {
        Null,
        State1,
        State2,
    }

    [ProtoContract]
    [MessagePackObject()]
    public record Contact(
        [property: Key(0), ProtoMember(1)] int ID,
        [property: Key(1), ProtoMember(2)] string Name,
        [property: Key(2), ProtoMember(3)] string PhoneNumber,
        [property: Key(3), ProtoMember(4)] DataState State);
}
