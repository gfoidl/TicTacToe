using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using TicTacToe.Engine.Engines;

namespace TicTacToe.Engine
{
	[DebuggerTypeProxy(typeof(GameDebugView))]
	public class Game
	{
		private static readonly int[] s_WinPatterns;
		private Board 			 _board = new Board(init: true);
		private readonly IEngine _engine;
		//---------------------------------------------------------------------
		static Game()
		{
			const string pattern = "WWW......|...WWW...|......WWW|W..W..W..|.W..W..W.|..W..W..W|W...W...W|..W.W.W..";
			string[] parts 		 = pattern.Split('|');
			var winPatterns 	 = new int[parts.Length];

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
		public Game() : this(new AlphaBetaPruningEngine()) { }
		public Game(IEngine engine) => _engine = engine ?? throw new ArgumentNullException(nameof(engine));
		//---------------------------------------------------------------------
		public Board Board => _board;
		public Winner Winner { get; internal set; } = Winner.None;
		//---------------------------------------------------------------------
		public MoveResult MakeMove(int fieldIdx)
		{
			if (!this.IsMoveLegal(fieldIdx)) return MoveResult.IllegalMove;

			return this.PerformMove(fieldIdx, FieldState.User);
		}
		//---------------------------------------------------------------------
		public MoveResult MakeMachineMove()
		{
			int bestMove = _engine.FindBestMove(_board);

			if (bestMove > -1)
				return this.PerformMove(bestMove, FieldState.Machine);
			else
				return new MoveResult(false, true);
		}
		//---------------------------------------------------------------------
		private MoveResult PerformMove(int fieldIdx, FieldState fieldState)
		{
			_board[fieldIdx] = fieldState;
			this.CheckWinner();

			return new MoveResult(true, this.IsFinal());
		}
		//---------------------------------------------------------------------
		internal bool IsMoveLegal(int index) => _board.IsMoveLegal(index);
		//---------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void CheckWinner() => this.Winner = CheckWinner(ref _board.Fields);
		//---------------------------------------------------------------------
		internal static Winner CheckWinner(ref FieldState fields)
		{
			if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
			{
				Vector<int> comparand = Unsafe.As<int, Vector<int>>(ref s_WinPatterns[0]);

				int machine = TransformGameToValue(ref fields, FieldState.Machine);
				if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(machine)), comparand))
					return Winner.Machine;

				int user = TransformGameToValue(ref fields, FieldState.User);
				if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(user)), comparand))
					return Winner.User;
			}
			else
			{
				int[] patterns = s_WinPatterns;
				int machine    = TransformGameToValue(ref fields, FieldState.Machine);
				int user 	   = TransformGameToValue(ref fields, FieldState.User);

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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int TransformGameToValue(ref FieldState fields, FieldState match)
		{
			int tmp = 0;

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
		internal bool IsFinal()
		{
			Winner winner = this.Winner;
			bool result   = IsFinal(ref winner, ref _board.Fields);
			this.Winner   = winner;

			return result;
		}
		//---------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsFinal(ref Winner winner, ref FieldState fields)
		{
			// x64 -> 8
			// x86 -> 4
			if (Vector.IsHardwareAccelerated && (Vector<int>.Count == 8 || Vector<int>.Count == 4))
			{
				bool isFinal = true;

				if (winner != Winner.None) goto Exit;

				Vector<int> vec = Unsafe.As<FieldState, Vector<int>>(ref fields);

				if (Vector.EqualsAny(Vector<int>.Zero, vec))
				{
					isFinal = false;
					goto Exit;
				}

				if (Vector<int>.Count == 4)
				{
					vec = Unsafe.As<FieldState, Vector<int>>(ref Unsafe.Add(ref fields, 4));
					if (Vector.EqualsAny(Vector<int>.Zero, vec))
					{
						isFinal = false;
						goto Exit;
					}
				}

				if (Unsafe.Add(ref fields, 8) == FieldState.Empty)
				{
					isFinal = false;
					goto Exit;
				}

			Exit:
				if (isFinal && winner == Winner.None)
					winner = Winner.Draw;

				return isFinal;
			}
			else
			{
				if (winner != Winner.None) return true;

				for (int i = 0; i < 9; ++i)
				{
					if (Unsafe.Add(ref fields, i) == FieldState.Empty)
						return false;
				}

				winner = Winner.Draw;
				return true;
			}
		}
	}
}
