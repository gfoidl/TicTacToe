using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TicTacToe.Engine.Engines
{
	public class AlphaBetaPruningEngine : IEngine
	{
		public int FindBestMove(Board board)
		{
			foreach (int index in board.GetEmptyFields())
				return index;

			return -1;
		}
		//---------------------------------------------------------------------
		private int MiniMax(bool machineMove, int depth, int alpha, int beta)
		{
			//if (depth == 0 || _game.IsFinal())
			//	;

			throw new NotImplementedException();
		}
	}
}
