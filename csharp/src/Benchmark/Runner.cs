using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Protos;

namespace Benchmark;

[MemoryDiagnoser]
[Config(typeof(BenchmarkConfig))]
public class Runner
{
    private float[] floats;
    private ReadOnlyMemory<byte> bytes;
    private byte[] _serialziedMyMessageBytes;
    private byte[] _serialziedMyMessageFloats;
    private MessageParser<MyMessage> _messageParser;

    [Params(100, 1000, 10_000, 100_000)] public int Size;


    [GlobalSetup]
    public void Setup()
    {
        floats = Enumerable.Range(0, Size).Select(i => (float) i).ToArray();
        bytes = MemoryMarshal.Cast<float, byte>(floats).ToArray();

        MyMessage floatsM = new();
        floatsM.RepeatedFloats.AddRange(floats);

        _serialziedMyMessageFloats = floatsM.ToByteArray();

        MyMessage bytesM = new();
        bytesM.FloatsAsBytes = ByteString.CopyFrom(bytes.Span);

        _serialziedMyMessageBytes = bytesM.ToByteArray();
        _messageParser = MyMessage.Parser;
    }

    [Benchmark]
    public MyMessage Parse_RepeatedFloats()
    {
        return _messageParser.ParseFrom(_serialziedMyMessageFloats);
    }

    [Benchmark]
    public MyMessage Parse_Bytes()
    {
        return _messageParser.ParseFrom(_serialziedMyMessageBytes);
    }
}