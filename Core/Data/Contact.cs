using ByteSizeLib;
using MessagePack;

namespace Core.Data
{
    public enum DataState
    {
        Null,
        State1,
        State2,
    }

    [MessagePackObject()]
    public record Contact(
        [property: Key(0)] int ID,
        [property: Key(1)] string Name,
        [property: Key(2)] string PhoneNumber,
        [property: Key(3)] DataState State);
}
