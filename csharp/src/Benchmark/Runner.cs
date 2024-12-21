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

    private MyMessage _myMessageBytes;
    private MyMessage _myMessageFloats;

    private MessageParser<MyMessage> _messageParser;

    [Params(10_000, 100_000)] public int Size;


    [GlobalSetup]
    public void Setup()
    {
        floats = Enumerable.Range(0, Size).Select(i => (float) i).ToArray();
        bytes = MemoryMarshal.Cast<float, byte>(floats).ToArray();

        // floats
        _myMessageFloats = new MyMessage();
        _myMessageFloats.RepeatedFloats.AddRange(floats);
        _serialziedMyMessageFloats = _myMessageFloats.ToByteArray();


        // bytes
        _myMessageBytes = new MyMessage();
        _myMessageBytes.FloatsAsBytes = ByteString.CopyFrom(bytes.Span);
        _serialziedMyMessageBytes = _myMessageBytes.ToByteArray();

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

    [Benchmark]
    public byte[] Write_RepeatedFloats()
    {
        return _myMessageFloats.ToByteArray();
    }

    [Benchmark]
    public byte[] Write_Bytes()
    {
        return _myMessageBytes.ToByteArray();
    }
}