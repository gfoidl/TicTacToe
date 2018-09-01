using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace TicTacToe.Engine.Benchmarks.GameBenchmarks
{
    [ClrJob, CoreJob]
    public class IsFinal
    {
        public FieldState[] Fields { get; } = Enumerable.Repeat(FieldState.User, 9).ToArray();
        //---------------------------------------------------------------------
        [Benchmark(Baseline = true)]
        public bool Sequential()
        {
            FieldState[] fields = this.Fields;

            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i] == FieldState.Empty)
                    return false;
            }

            return true;
        }
        //---------------------------------------------------------------------
        [Benchmark]
        public unsafe bool Vectorized()
        {
            // x64 -> 8
            // x86 -> 4
            if (Vector.IsHardwareAccelerated && (Vector<int>.Count == 8 || Vector<int>.Count == 4))
            {
                fixed (FieldState* tmpPtr = this.Fields)
                {
                    int* ptr = (int*)tmpPtr;

                    Vector<int> emptyFields = Vector<int>.Zero;

                    Vector<int> vec = Unsafe.ReadUnaligned<Vector<int>>(ptr + 0);
                    if (Vector.EqualsAny(emptyFields, vec))
                        return false;

                    if (Vector<int>.Count == 4)
                    {
                        vec = Unsafe.ReadUnaligned<Vector<int>>(ptr + 4);
                        if (Vector.EqualsAny(emptyFields, vec))
                            return false;
                    }

                    if (ptr[8] == (int)FieldState.Empty)
                        return false;
                }
            }
            else
            {
                for (int i = 0; i < 9; ++i)
                {
                    if (this.Fields[i] == FieldState.Empty)
                        return false;
                }
            }

            return true;
        }
    }
}
