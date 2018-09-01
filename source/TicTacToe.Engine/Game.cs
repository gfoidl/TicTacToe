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
		private Board 			 _board = new Board();
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
		internal void CheckWinner() => this.Winner = CheckWinner(_board);
		//---------------------------------------------------------------------
		internal static Winner CheckWinner(Board board)
		{
			Debug.Assert(s_WinPatterns.Length == 8);

			if (Vector.IsHardwareAccelerated && Vector<int>.Count == 8)
			{
				Vector<int> comparand = Unsafe.As<int, Vector<int>>(ref s_WinPatterns[0]);

				if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(board.MachineFields)), comparand))
					return Winner.Machine;

				if (Vector.EqualsAny(Vector.BitwiseAnd(comparand, new Vector<int>(board.UserFields)), comparand))
					return Winner.User;
			}
			else
			{
				int[] patterns = s_WinPatterns;
				int machine    = board.MachineFields;
				int user 	   = board.UserFields;

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
		internal bool IsFinal()
		{
			Winner winner = this.Winner;
			bool result   = IsFinal(ref winner, _board);
			this.Winner   = winner;

			return result;
		}
		//---------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsFinal(ref Winner winner, Board board)
		{
			// The set fields are stored in a int, for machine and user separately.
			// So combine these int to one, to get all set fields.
			// Invert it, so that the set ones get 0, and the empty ones get 1.
			// Mask away all bits except the interesting first 9 (the game).
			// When there is any empty field, at least one 1 remains (due the inversion).

			if (winner != Winner.None) return true;

			int invertedSetFields = ~board.SetFields;
			invertedSetFields    &= 511;		// last 9 bits set to mask
			bool isFinal 		  = invertedSetFields == 0;

			if (isFinal) winner = Winner.Draw;

			return isFinal;
		}
	}
}
