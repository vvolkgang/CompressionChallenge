using Alba.CsConsoleFormat;
using ByteSizeLib;
using K4os.Compression.LZ4.Streams;
using MessagePack;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.ConsoleColor;
using static CompressionChallenge.DataSource;
using System.IO.Compression;
using System;

namespace CompressionChallenge
{
    public enum DataState
    {
        Null,
        State1,
        State2,
    }

    [MessagePackObject()]
    public record Data(
        [property: Key(0)] int ID, 
        [property: Key(1)] string Name, 
        [property: Key(2)] string Contact, 
        [property: Key(3)] DataState State);

    record TestOutput(string Method, double SizeInBytes, ByteSize Size, float GainPerc);

    class Program
    {
        static void Main(string[] args)
        {
            var dataSizeInThousands = 15;
            var randomData = true;

            var list = randomData ? GenerateRandomDataList(dataSizeInThousands) : GenerateDataList(dataSizeInThousands);

            var jsonOutput = SaveListJson(list);
            var bsonOutput = SaveListBson(list, jsonOutput.SizeInBytes);
            var jsonlz4Output = SaveListJsonLz4(list, jsonOutput.SizeInBytes);
            var jsonGzipOutput = SaveListJsonGzip(list, jsonOutput.SizeInBytes);
            var msgPackOutput = SaveListMsgPack(list, jsonOutput.SizeInBytes);
            var msgPackLZ4Output = SaveListMsgPackLZ4(list, jsonOutput.SizeInBytes);
            var msgPackLZ4ArrayOutput = SaveListMsgPackLZ4Array(list, jsonOutput.SizeInBytes);
            var msgPackLZ4ArrayGzipOutput = SaveListMsgPackLZ4ArrayGzip(list, jsonOutput.SizeInBytes);
            var msgPackGzipOutput = SaveListMsgPackGzip(list, jsonOutput.SizeInBytes);

            var outputList = new List<TestOutput>
            {
                jsonOutput,
                bsonOutput,
                jsonlz4Output,
                jsonGzipOutput,
                msgPackOutput,
                msgPackLZ4Output,
                msgPackLZ4ArrayOutput,
                msgPackLZ4ArrayGzipOutput,
                msgPackGzipOutput,
            };

            outputList.Sort((a, b) => a.GainPerc.CompareTo(b.GainPerc));
            ConsoleRenderer.RenderDocument(CreateGrid(dataSizeInThousands, randomData, outputList));
        }

        private static Document CreateGrid(int dataSizeInThousands, bool randomData, List<TestOutput> outputList)
        {
            var headerThickness = new LineThickness(LineWidth.Double, LineWidth.Single);

            ConsoleColor GetGainColor(double gainPerc) => gainPerc switch
            {
               > 0 => Green,
               0 => Gray,
               < 0 => Red
            };

            string GainToString(double gainPerc) => gainPerc switch
            {
                0 => "-",
                _ => gainPerc.ToString("0.0%")
            };

            return new Document(   
                new Span("Data Size: ") { Color = Yellow }, dataSizeInThousands * 1000, "\n",
                new Span("Random Data? ") { Color = Yellow }, randomData, "\n",
                new Grid
                {
                    Color = Gray,
                    Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto },
                    Children = {
                        new Cell("Method") { Stroke = headerThickness },
                        new Cell("Bytes") { Stroke = headerThickness },
                        new Cell("KB") { Stroke = headerThickness },
                        new Cell("Gain %") { Stroke = headerThickness },
                        outputList.Select(item => new[] {
                            new Cell(item.Method) { Color = Yellow },
                            new Cell(item.SizeInBytes),
                            new Cell(item.Size.KiloBytes.ToString("0.0")) { Align = Align.Right },
                            new Cell(GainToString(item.GainPerc)) { Color = GetGainColor(item.GainPerc) },
                        })       
                    }
                });
        }

        private static TestOutput SaveListJson(List<Data> list)
        {
            var json = JsonConvert.SerializeObject(list);
            File.WriteAllText("json.txt", json);

            float bytes = new FileInfo("json.txt").Length;

            return new TestOutput("JSON", bytes, new ByteSize(bytes), 0);
        }

        private static TestOutput SaveListBson(List<Data> list, double jsonBytes)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, list);
            }

            File.WriteAllBytes("bson", ms.ToArray());

            float bytes = new FileInfo("bson").Length;

            return new TestOutput("BSON", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListJsonLz4(List<Data> list, double jsonBytes)
        {
            var json = JsonConvert.SerializeObject(list);

            using (var source = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
            using (var target = LZ4Stream.Encode(File.Create("json.lz4")))
            {
                source.CopyTo(target);
            }

            float bytes = new FileInfo("json.lz4").Length;

            return new TestOutput("JSON + LZ4", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListJsonGzip(List<Data> list, double jsonBytes)
        {
            var json = JsonConvert.SerializeObject(list);

            using (var source = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
            using (var target = LZ4Stream.Encode(File.Create("json.gz")))
            {
                //GZipStream adds CRC to ensure data correctness
                using (var compressorStream = new GZipStream(target, CompressionLevel.Optimal, true))
                {
                    source.CopyTo(compressorStream);
                }
            }

            float bytes = new FileInfo("json.gz").Length;

            return new TestOutput("JSON + GZ", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListMsgPack(List<Data> list, double jsonBytes)
        {
            byte[] file = MessagePackSerializer.Serialize(list);
            File.WriteAllBytes("msgpack", file);
            float bytes = new FileInfo("msgpack").Length;

            return new TestOutput("MsgPack", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListMsgPackLZ4(List<Data> list, double jsonBytes)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

            byte[] file = MessagePackSerializer.Serialize(list, lz4Options);
            File.WriteAllBytes("msgpacklz4", file);
            float bytes = new FileInfo("msgpacklz4").Length;

            return new TestOutput("MsgPack LZ4", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListMsgPackLZ4Array(List<Data> list, double jsonBytes)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

            byte[] file = MessagePackSerializer.Serialize(list, lz4Options);
            File.WriteAllBytes("msgpacklz4array", file);
            float bytes = new FileInfo("msgpacklz4array").Length;

            return new TestOutput("MsgPack LZ4 Array", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListMsgPackLZ4ArrayGzip(List<Data> list, double jsonBytes)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

            byte[] file = MessagePackSerializer.Serialize(list, lz4Options);
            using (var source = new MemoryStream(file))
            using (var target = LZ4Stream.Encode(File.Create("msgpacklz4array.gz")))
            {
                //GZipStream adds CRC to ensure data correctness
                using (var compressorStream = new GZipStream(target, CompressionLevel.Optimal, true))
                {
                    source.CopyTo(compressorStream);
                }
            }
            float bytes = new FileInfo("msgpacklz4array.gz").Length;

            return new TestOutput("MsgPack LZ4 Array + Gzip", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }

        private static TestOutput SaveListMsgPackGzip(List<Data> list, double jsonBytes)
        {
            byte[] file = MessagePackSerializer.Serialize(list);
            using (var source = new MemoryStream(file))
            using (var target = LZ4Stream.Encode(File.Create("msgpack.gz")))
            {
                //GZipStream adds CRC to ensure data correctness
                using (var compressorStream = new GZipStream(target, CompressionLevel.Optimal, true))
                {
                    source.CopyTo(compressorStream);
                }
            }
            float bytes = new FileInfo("msgpack.gz").Length;

            return new TestOutput("MsgPack + Gzip", bytes, new ByteSize(bytes), CalcGain(jsonBytes, bytes));
        }



        private static float CalcGain(double jsonBytes, float otherBytes) => (float)(1 - otherBytes / jsonBytes);
    }
}
