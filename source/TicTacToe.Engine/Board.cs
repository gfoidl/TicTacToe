using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace TicTacToe.Engine
{
    [DebuggerTypeProxy(typeof(BoardDebugView))]
    public struct Board
    {
        private static readonly int[] s_WinPatterns;
        //---------------------------------------------------------------------
        static Board()
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
        private int _user;
        private int _machine;
        //---------------------------------------------------------------------
        public FieldState this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (Get(_machine, index)) return FieldState.Machine;
                if (Get(_user, index))    return FieldState.User;
                return FieldState.Empty;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value == FieldState.Machine)
                {
                    Set(ref _machine, index);
                    UnSet(ref _user, index);
                }
                else if (value == FieldState.User)
                {
                    Set(ref _user, index);
                    UnSet(ref _machine, index);
                }
                else
                {
                    UnSet(ref _machine, index);
                    UnSet(ref _user, index);
                }
            }
        }
        //---------------------------------------------------------------------
        public bool IsMoveLegal(int index) => this[index] == FieldState.Empty;
        //---------------------------------------------------------------------
        public IEnumerable<int> GetEmptyFields()
        {
            int invertedSetFields = ~(_machine | _user);

            for (int i = 0; i < 9; ++i)
            {
                bool isSet = ((invertedSetFields >> i) & 1) > 0;

                if (isSet) yield return i;
            }
        }
        //---------------------------------------------------------------------
        public Winner CheckWinner()
        {
            Debug.Assert(s_WinPatterns.Length == 8);

            if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
            {
                Vector<int> comparand = Unsafe.As<int, Vector<int>>(ref s_WinPatterns[0]);

                if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(_machine)), comparand))
                    return Winner.Machine;

                if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(_user)), comparand))
                    return Winner.User;
            }
            else
            {
                int[] patterns = s_WinPatterns;
                int machine    = _machine;
                int user       = _user;

                for (int i = 0; i < patterns.Length; ++i)
                {
                    if ((patterns[i] & machine) == patterns[i])
                        return Winner.Machine;

                    if ((patterns[i] & user) == patterns[i])
                        return Winner.User;
                }
            }

            return Winner.None;
        }
        //---------------------------------------------------------------------
        public bool HasEmptyFields()
        {
            // The set fields are stored in a int, for machine and user separately.
            // So combine these int to one, to get all set fields.
            // Invert it, so that the set ones get 0, and the empty ones get 1.
            // Mask away all bits except the interesting first 9 (the game).
            // When there is any empty field, at least one 1 remains (due the inversion).

            int invertedSetFields = ~(_machine | _user);
            invertedSetFields    &= 0x1ff;       // last 9 bits set to mask. 511
            return invertedSetFields > 0;
        }
        //---------------------------------------------------------------------
        private static bool Get(int value, int index)       => ((value >> index) & 1) > 0;
        private static void Set(ref int value, int index)   => value |= 1 << index;
        private static void UnSet(ref int value, int index) => value &= ~(1 << index);
        //---------------------------------------------------------------------
        [DebuggerNonUserCode]
        public override string ToString() => BoardDebugView.GetString(this);
        //---------------------------------------------------------------------
        public int GetKey() => (_user << 16) | _machine;
    }
}
