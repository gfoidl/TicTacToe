using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TicTacToe.Engine.Engines;

namespace TicTacToe.Engine
{
	[DebuggerTypeProxy(typeof(GameDebugView))]
	public class Game
	{
		private Board            _board = new Board();
		private readonly IEngine _engine;
		//---------------------------------------------------------------------
		public Game() : this(new AlphaBetaPruningEngine()) { }
		public Game(IEngine engine) => _engine = engine ?? throw new ArgumentNullException(nameof(engine));
		//---------------------------------------------------------------------
		public ref Board Board => ref _board;
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
		internal void CheckWinner() => this.Winner = _board.CheckWinner();
		//---------------------------------------------------------------------
		internal bool IsFinal()
		{
			if (this.Winner == Winner.None)
			{
				if (_board.HasEmptyFields())
					return false;

				this.Winner = Winner.Draw;
			}

			return true;
		}
	}
}
