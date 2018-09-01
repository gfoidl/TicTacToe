using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace TicTacToe.Engine.Benchmarks.GameBenchmarks
{
    [ClrJob, CoreJob]
    public class CheckWinner
    {
        [Params(true, false)]
        public bool UserWins { get; set; }
        //---------------------------------------------------------------------
        private static readonly int[] s_WinPatterns;
        //---------------------------------------------------------------------
        public FieldState[] Fields { get; } = Enumerable.Repeat(FieldState.Empty, 9).ToArray();
        public Winner? Winner { get; internal set; }
        //---------------------------------------------------------------------
        static CheckWinner()
        {
            const string pattern = "WWW......|...WWW...|......WWW|W..W..W..|.W..W..W.|..W..W..W|W...W...W|..W.W.W..";
            string[] parts       = pattern.Split('|');
            var winPatterns      = new int[parts.Length];

            for (int i = 0; i < parts.Length; ++i)
            {
                string pat = parts[i];
                int tmp    = 0;

                for (int j = 0; j < 9; ++j)
                {
                    if (pat[j] == 'W')
                        tmp |= 1 << j;
                }

                winPatterns[i] = tmp;
            }

            s_WinPatterns = winPatterns;
        }
        //---------------------------------------------------------------------
        [GlobalSetup]
        public void GlobalSetup()
        {
            if (this.UserWins)
            {
                this.Fields[0] = FieldState.User;
                this.Fields[4] = FieldState.User;
                this.Fields[8] = FieldState.User;

                this.Fields[1] = FieldState.Machine;
                this.Fields[3] = FieldState.Machine;
            }
            else
            {
                this.Fields[0] = FieldState.Machine;
                this.Fields[4] = FieldState.Machine;
                this.Fields[8] = FieldState.Machine;

                this.Fields[1] = FieldState.User;
                this.Fields[3] = FieldState.User;
            }
        }
        //---------------------------------------------------------------------
        [Benchmark(Baseline = true)]
        public void Array_sequential()
        {
            int[] patterns = s_WinPatterns;
            int machine    = TransformToValue(FieldState.Machine);
            int user       = TransformToValue(FieldState.User);

            for (int i = 0; i < patterns.Length; ++i)
            {
                if ((patterns[i] & machine) == patterns[i])
                {
                    this.Winner = Engine.Winner.Machine;
                    return;
                }

                if ((patterns[i] & user) == patterns[i])
                {
                    this.Winner = Engine.Winner.User;
                    return;
                }
            }
            //-----------------------------------------------------------------
            int TransformToValue(FieldState match)
            {
                int tmp = 0;
                ref FieldState fields = ref this.Fields[0];

                // Loop unrolled
                if (Unsafe.Add(ref fields, 0) == match) tmp |= 1 << 0;
                if (Unsafe.Add(ref fields, 1) == match) tmp |= 1 << 1;
                if (Unsafe.Add(ref fields, 2) == match) tmp |= 1 << 2;
                if (Unsafe.Add(ref fields, 3) == match) tmp |= 1 << 3;
                if (Unsafe.Add(ref fields, 4) == match) tmp |= 1 << 4;
                if (Unsafe.Add(ref fields, 5) == match) tmp |= 1 << 5;
                if (Unsafe.Add(ref fields, 6) == match) tmp |= 1 << 6;
                if (Unsafe.Add(ref fields, 7) == match) tmp |= 1 << 7;
                if (Unsafe.Add(ref fields, 8) == match) tmp |= 1 << 8;

                return tmp;
            }
        }
        //---------------------------------------------------------------------
        [Benchmark]
        public unsafe void Array_vectorized()
        {
            if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
            {
                fixed (int* ptr = s_WinPatterns)
                {
                    Vector<int> comparand = Unsafe.ReadUnaligned<Vector<int>>(ptr);

                    int machine = TransformToValue(FieldState.Machine);
                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(machine)), comparand))
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    int user = TransformToValue(FieldState.User);
                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(user)), comparand))
                        this.Winner = Engine.Winner.User;
                }
            }
            else
            {
                int[] patterns = s_WinPatterns;
                int machine    = TransformToValue(FieldState.Machine);
                int user       = TransformToValue(FieldState.User);

                for (int i = 0; i < patterns.Length; ++i)
                {
                    if ((patterns[i] & machine) == patterns[i])
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    if ((patterns[i] & user) == patterns[i])
                    {
                        this.Winner = Engine.Winner.User;
                        return;
                    }
                }
            }
            //-----------------------------------------------------------------
            int TransformToValue(FieldState match)
            {
                int tmp = 0;
                ref FieldState fields = ref this.Fields[0];

                // Loop unrolled
                if (Unsafe.Add(ref fields, 0) == match) tmp |= 1 << 0;
                if (Unsafe.Add(ref fields, 1) == match) tmp |= 1 << 1;
                if (Unsafe.Add(ref fields, 2) == match) tmp |= 1 << 2;
                if (Unsafe.Add(ref fields, 3) == match) tmp |= 1 << 3;
                if (Unsafe.Add(ref fields, 4) == match) tmp |= 1 << 4;
                if (Unsafe.Add(ref fields, 5) == match) tmp |= 1 << 5;
                if (Unsafe.Add(ref fields, 6) == match) tmp |= 1 << 6;
                if (Unsafe.Add(ref fields, 7) == match) tmp |= 1 << 7;
                if (Unsafe.Add(ref fields, 8) == match) tmp |= 1 << 8;

                return tmp;
            }
        }
        //---------------------------------------------------------------------
        [Benchmark]
        public unsafe void Array_vectorized_transform_inlined()
        {
            if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
            {
                fixed (int* ptr = s_WinPatterns)
                {
                    Vector<int> comparand = Unsafe.ReadUnaligned<Vector<int>>(ptr);

                    int machine = this.TransformToValue(FieldState.Machine);
                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(machine)), comparand))
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    int user = this.TransformToValue(FieldState.User);
                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(user)), comparand))
                        this.Winner = Engine.Winner.User;
                }
            }
            else
            {
                int[] patterns = s_WinPatterns;
                int machine    = this.TransformToValue(FieldState.Machine);
                int user       = this.TransformToValue(FieldState.User);

                for (int i = 0; i < patterns.Length; ++i)
                {
                    if ((patterns[i] & machine) == patterns[i])
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    if ((patterns[i] & user) == patterns[i])
                    {
                        this.Winner = Engine.Winner.User;
                        return;
                    }
                }
            }
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int TransformToValue(FieldState match)
        {
            int tmp = 0;
            ref FieldState fields = ref this.Fields[0];

            // Loop unrolled
            if (Unsafe.Add(ref fields, 0) == match) tmp |= 1 << 0;
            if (Unsafe.Add(ref fields, 1) == match) tmp |= 1 << 1;
            if (Unsafe.Add(ref fields, 2) == match) tmp |= 1 << 2;
            if (Unsafe.Add(ref fields, 3) == match) tmp |= 1 << 3;
            if (Unsafe.Add(ref fields, 4) == match) tmp |= 1 << 4;
            if (Unsafe.Add(ref fields, 5) == match) tmp |= 1 << 5;
            if (Unsafe.Add(ref fields, 6) == match) tmp |= 1 << 6;
            if (Unsafe.Add(ref fields, 7) == match) tmp |= 1 << 7;
            if (Unsafe.Add(ref fields, 8) == match) tmp |= 1 << 8;

            return tmp;
        }
        //---------------------------------------------------------------------
        [Benchmark]
        public unsafe void Array_vectorized_different_locals()
        {
            if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
            {
                fixed (int* ptr = s_WinPatterns)
                {
                    int machine           = TransformToValue(FieldState.Machine);
                    int user              = TransformToValue(FieldState.User);
                    Vector<int> comparand = Unsafe.ReadUnaligned<Vector<int>>(ptr);

                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(machine)), comparand))
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(user)), comparand))
                        this.Winner = Engine.Winner.User;
                }
            }
            else
            {
                int[] patterns = s_WinPatterns;
                int machine    = TransformToValue(FieldState.Machine);
                int user       = TransformToValue(FieldState.User);

                for (int i = 0; i < patterns.Length; ++i)
                {
                    if ((patterns[i] & machine) == patterns[i])
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    if ((patterns[i] & user) == patterns[i])
                    {
                        this.Winner = Engine.Winner.User;
                        return;
                    }
                }
            }
            //-----------------------------------------------------------------
            int TransformToValue(FieldState match)
            {
                int tmp = 0;
                ref FieldState fields = ref this.Fields[0];

                // Loop unrolled
                if (Unsafe.Add(ref fields, 0) == match) tmp |= 1 << 0;
                if (Unsafe.Add(ref fields, 1) == match) tmp |= 1 << 1;
                if (Unsafe.Add(ref fields, 2) == match) tmp |= 1 << 2;
                if (Unsafe.Add(ref fields, 3) == match) tmp |= 1 << 3;
                if (Unsafe.Add(ref fields, 4) == match) tmp |= 1 << 4;
                if (Unsafe.Add(ref fields, 5) == match) tmp |= 1 << 5;
                if (Unsafe.Add(ref fields, 6) == match) tmp |= 1 << 6;
                if (Unsafe.Add(ref fields, 7) == match) tmp |= 1 << 7;
                if (Unsafe.Add(ref fields, 8) == match) tmp |= 1 << 8;

                return tmp;
            }
        }
        //---------------------------------------------------------------------
        [Benchmark]
        public unsafe void Array_vectorized_values_combined()
        {
            if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
            {
                fixed (int* ptr = s_WinPatterns)
                {
                    Vector<int> comparand = Unsafe.ReadUnaligned<Vector<int>>(ptr);
                    TransformToValue(out int machine, out int user);

                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(machine)), comparand))
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(user)), comparand))
                        this.Winner = Engine.Winner.User;
                }
            }
            else
            {
                int[] patterns = s_WinPatterns;
                TransformToValue(out int machine, out int user);

                for (int i = 0; i < patterns.Length; ++i)
                {
                    if ((patterns[i] & machine) == patterns[i])
                    {
                        this.Winner = Engine.Winner.Machine;
                        return;
                    }

                    if ((patterns[i] & user) == patterns[i])
                    {
                        this.Winner = Engine.Winner.User;
                        return;
                    }
                }
            }
            //-----------------------------------------------------------------
            void TransformToValue(out int machine, out int user)
            {
                int m = 0;
                int u = 0;
                ref FieldState fields = ref this.Fields[0];

                // Loop unrolled
                FieldState tmp = Unsafe.Add(ref fields, 0);
                if (tmp == FieldState.Machine) m |= 1 << 0;
                if (tmp == FieldState.User)    u |= 1 << 0;

                tmp = Unsafe.Add(ref fields, 1);
                if (tmp == FieldState.Machine) m |= 1 << 1;
                if (tmp == FieldState.User)    u |= 1 << 1;

                tmp = Unsafe.Add(ref fields, 2);
                if (tmp == FieldState.Machine) m |= 1 << 2;
                if (tmp == FieldState.User)    u |= 1 << 2;

                tmp = Unsafe.Add(ref fields, 3);
                if (tmp == FieldState.Machine) m |= 1 << 3;
                if (tmp == FieldState.User)    u |= 1 << 3;

                tmp = Unsafe.Add(ref fields, 4);
                if (tmp == FieldState.Machine) m |= 1 << 4;
                if (tmp == FieldState.User)    u |= 1 << 4;

                tmp = Unsafe.Add(ref fields, 5);
                if (tmp == FieldState.Machine) m |= 1 << 5;
                if (tmp == FieldState.User)    u |= 1 << 5;

                tmp = Unsafe.Add(ref fields, 6);
                if (tmp == FieldState.Machine) m |= 1 << 6;
                if (tmp == FieldState.User)    u |= 1 << 6;

                tmp = Unsafe.Add(ref fields, 7);
                if (tmp == FieldState.Machine) m |= 1 << 7;
                if (tmp == FieldState.User)    u |= 1 << 7;

                tmp = Unsafe.Add(ref fields, 8);
                if (tmp == FieldState.Machine) m |= 1 << 8;
                if (tmp == FieldState.User)    u |= 1 << 8;

                machine = m;
                user    = u;
            }
        }
    }
}
