using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class SpanVsSubstringBenchmark
{
	private string _text = null!;

	[GlobalSetup]
	public void Setup()
	{
		_text = "This is a MohammadHasan's sample string for benchmarking purposes.";
	}

	[Benchmark]
	public string Substring()
	{
		return _text.Substring(10, 13); // Extracts "MohammadHasan"
	}

	[Benchmark]
	public ReadOnlySpan<char> ReadOnlySpan()
	{
		var readOnlySpan = _text.AsSpan();
		return readOnlySpan.Slice(10, 13); // Extracts "MohammadHasan"
	}



	public void DisplayBufferToConsoleBenchmark()
	{
		var readOnlySpan = _text.AsSpan();
		DisplayBufferToConsole(readOnlySpan.Slice(10, 13));
	}



	// Using Console.WriteLine within a benchmark is not recommended
	// as it may affect the benchmark results. However, for demonstration purposes:
	private void DisplayBufferToConsole(ReadOnlySpan<char> buffer)=>
		Console.WriteLine(buffer.ToString());
	

	private unsafe void SyncMethodUsingSpan(Span<char> buffer)
	{
		fixed (char* ptr = buffer)
		{
			// Simulate call to a P/Invoke method
			SomePInvokeMethod(ptr, buffer.Length);
		}
	}

	// Dummy P/Invoke method for demonstration purposes
	private unsafe void SomePInvokeMethod(char* ptr, int length)
	{
		// Simulate processing
		for (int i = 0; i < length; i++)
		{
			ptr[i] = char.ToUpper(ptr[i]);
		}
	}
}

public class Program
{
	public static void Main(string[] args)
	{
		var summary = BenchmarkRunner.Run<SpanVsSubstringBenchmark>();
	}
}