# Data Serialization Compression Challenge

This project started with two questions: 

* Could 15k contacts be stored in a 1.44MB floppy disk?
* How much space can we save by improving the data serialization and adding compression?

Using JSON string as a baseline, the following format / compression combos where used:

* BSON (no compression)
* JSON + LZ4
* JSON + Gzip
* MsgPack (no compression)
* MsgPack + LZ4
* MsgPack + LZ4Array
* MsgPack + GZip

# Results

![Results](/docs/results_v2.png)


# Credits

* [MessagePack C#](https://github.com/neuecc/MessagePack-CSharp)
* [CsConsoleFormat](https://github.com/Athari/CsConsoleFormat/)
    * That sweet console grid presentation 
* [K4os.Compression.LZ4](https://github.com/MiloszKrajewski/K4os.Compression.LZ4)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* [ByteSize](https://github.com/omar/ByteSize)
    * Currently not supporting .net5, added the files to the project directly