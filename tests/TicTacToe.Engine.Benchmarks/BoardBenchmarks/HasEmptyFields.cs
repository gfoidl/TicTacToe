using System;
using BenchmarkDotNet.Attributes;

#if NETCOREAPP2_1
using System.Runtime.Intrinsics.X86;
#endif

namespace TicTacToe.Engine.Benchmarks.BoardBenchmarks
{
#if NETCOREAPP2_1
	public class HasEmptyFields
	{
		static HasEmptyFields()
		{
			if (!Popcnt.IsSupported)
				throw new NotSupportedException($"{nameof(Popcnt)} is not supported");
		}
		//---------------------------------------------------------------------
		private uint _machine = 0b_0_0110_1000;
		private uint _user 	  = 0b_0_1001_0010;
		//---------------------------------------------------------------------
		[Benchmark(Baseline = true)]
		public bool BitOperation()
		{
			uint invertedSetFields = ~(_machine | _user);
			invertedSetFields     &= 0x1ff;       // last 9 bits set to mask. 511
			return invertedSetFields > 0;
		}
		//---------------------------------------------------------------------
		[Benchmark]
		public bool Intrinsics()
		{
			uint invertedSetFields = ~(_machine | _user);
			int popCount 		   = Popcnt.PopCount(invertedSetFields);
			return popCount > (32 - 9);
		}
	}
#endif
}
