# Data Serialization Compression Challenge

This project started with two questions: 

* Could 15k contacts be stored in a 1.44MB floppy disk?
* How much space can we save by improving the data serialization and adding compression?

# The Data

Each contact has:

* ID: int
* Name: String with 5 to 24 random chars
* PhoneNumber: String with a fixed country code and 9 random digits
* DataState: Generic enum with 3 states defined

The JSON serialization of one contact looks like this:

`{"ID":0,"Name":"rQGPnBpGJT","PhoneNumber":"+351445593358","State":2}`

# The Contenders

Using JSON string as a baseline, the following format / compression combos where used:

* BSON (uncompressed)
* JSON + LZ4
* JSON + Gzip Fast and Slow
* MsgPack (uncompressed)
* System.Text.Json (uncompressed)
* MsgPack + LZ4
* MsgPack + LZ4Array
* MsgPack + GZip Fast and Slow
* MsgPack + Brotli Fast and Slow (Brotli slow is _*slow*_, like 1.5s slow in my machine, disabled it by default)
* Apex.Serialization, uncompressed + GZip
* GroBuf, uncompressed + GZip
* ProtoBuf, uncompressed + GZip

# Results

![Results](/docs/results_v4.png)

Xamarin Android:
![Results Android](/docs/android_v1.png)

# How to add a new test

Tests (located in: `Core/Tests/`) are loaded in runtime using reflection. Anything inherithing `BaseTest` will automatically be executed. 

Just copy `JsonTest.cs` and add your own implementation to it.

# Credits

* [MessagePack C#](https://github.com/neuecc/MessagePack-CSharp)
* [CsConsoleFormat](https://github.com/Athari/CsConsoleFormat/)
    * That sweet console grid presentation 
* [K4os.Compression.LZ4](https://github.com/MiloszKrajewski/K4os.Compression.LZ4)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* [ByteSize](https://github.com/omar/ByteSize)
    * Currently not supporting .net5, added the files to the project directly
* [Apex.Serialization](https://github.com/dbolin/Apex.Serialization/)
* [GroBuf](https://github.com/skbkontur/GroBuf/)
* [ProtoBuf](https://github.com/protobuf-net/protobuf-net)


