using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TicTacToe.Engine
{
	internal class AlphaBetaPruning
	{
		public int FindBestMove(Board board)
		{
			int bestMove = -1;

			for (int i = 0; i < 9; ++i)
			{
				if (board[i] != FieldState.Empty) continue;

				return i;
			}

			return bestMove;
		}
		//---------------------------------------------------------------------
		//---------------------------------------------------------------------
		private int MiniMax(bool machineMove, int depth, int alpha, int beta)
		{
			//if (depth == 0 || _game.IsFinal())
			//	;

			throw new NotImplementedException();
		}
	}
}
