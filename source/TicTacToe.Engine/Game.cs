using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace TicTacToe.Engine
{
	[DebuggerTypeProxy(typeof(GameDebugView))]
	public class Game
	{
		private static readonly int[] s_WinPatterns;
		//---------------------------------------------------------------------
		static Game()
		{
			const string pattern = "WWW......|...WWW...|......WWW|W..W..W..|.W..W..W.|..W..W..W|W...W...W|..W.W.W..";
			string[] parts = pattern.Split('|');
			var winPatterns = new int[parts.Length];

			for (int i = 0; i < parts.Length; ++i)
			{
				string pat = parts[i];
				int tmp = 0;

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
		public FieldState[] Fields { get; } = Enumerable.Repeat(FieldState.Empty, 9).ToArray();
		public Winner? Winner { get; internal set; }
		//---------------------------------------------------------------------
		public MoveResult MakeMove(int fieldIdx)
		{
			if (!this.IsMoveLegal(fieldIdx)) return MoveResult.IllegalMove;

			return this.PerformMove(fieldIdx, FieldState.User);
		}
		//---------------------------------------------------------------------
		public MoveResult MakeMachineMove()
		{
			int i = 0;
			for (; i < this.Fields.Length; ++i)
				if (this.Fields[i] == FieldState.Empty)
					break;

			return this.PerformMove(i, FieldState.Machine);
		}
		//---------------------------------------------------------------------
		private MoveResult PerformMove(int fieldIdx, FieldState fieldState)
		{
			this.Fields[fieldIdx] = fieldState;
			this.CheckWinner();

			return new MoveResult(true, this.IsFinal());
		}
		//---------------------------------------------------------------------
		internal bool IsMoveLegal(int index) => this.Fields[index] == FieldState.Empty;
		//---------------------------------------------------------------------
		internal unsafe void CheckWinner()
		{
			if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
			{
				fixed (int* ptr = s_WinPatterns)
				{
					Vector<int> comparand = Unsafe.ReadUnaligned<Vector<int>>(ptr);

					int machine = this.TransformGameToValue(FieldState.Machine);
					if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(machine)), comparand))
					{
						this.Winner = Engine.Winner.Machine;
						return;
					}

					int user = this.TransformGameToValue(FieldState.User);
					if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(user)), comparand))
						this.Winner = Engine.Winner.User;
				}
			}
			else
			{
				int[] patterns = s_WinPatterns;
				int machine = this.TransformGameToValue(FieldState.Machine);
				int user = this.TransformGameToValue(FieldState.User);

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
		private int TransformGameToValue(FieldState match)
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
		internal unsafe bool IsFinal()
		{
			if (this.Winner.HasValue) return true;

			// x64 -> 8
			// x86 -> 4
			if (Vector.IsHardwareAccelerated && (Vector<int>.Count == 8 || Vector<int>.Count == 4))
			{
				fixed (FieldState* tmpPtr = this.Fields)
				{
					int* ptr = (int*)tmpPtr;

					Vector<int> vec = Unsafe.ReadUnaligned<Vector<int>>(ptr + 0);
					if (Vector.EqualsAny(Vector<int>.Zero, vec))
						return false;

					if (Vector<int>.Count == 4)
					{
						vec = Unsafe.ReadUnaligned<Vector<int>>(ptr + 4);
						if (Vector.EqualsAny(Vector<int>.Zero, vec))
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
