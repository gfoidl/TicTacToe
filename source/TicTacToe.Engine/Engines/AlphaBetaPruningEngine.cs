﻿namespace TicTacToe.Engine.Engines
{
	public class AlphaBetaPruningEngine : IEngine
	{
		public int FindBestMove(Board board)
		{
			int bestScore = int.MinValue;
			int bestMove  = -1;

			foreach (int index in board.GetEmptyFields())
			{
				Board newBoard 	= board;     // makes a copy
				newBoard[index] = FieldState.Machine;
				int score 		= this.MiniMax(newBoard, false, 0, int.MinValue, int.MaxValue);

				if (score > bestScore)
				{
					bestMove  = index;
					bestScore = score;
				}
			}

			return bestMove;
		}
		//---------------------------------------------------------------------
		private int MiniMax(Board board, bool machineMove, int depth, int alpha, int beta)
		{
			Winner winner = board.CheckWinner();
			if (winner != Winner.None)
				return this.GetScoreForWinner(winner, machineMove, depth);

			// Draw
			if (!board.HasEmptyFields())
				return 0;

			int maxScore = alpha;
			foreach (int index in board.GetEmptyFields())
			{
				Board newBoard 	= board;     // makes a copy
				newBoard[index] = machineMove ? FieldState.Machine : FieldState.User;
				int score 		= -this.MiniMax(newBoard, !machineMove, depth++, -beta, -maxScore);

				if (score > maxScore)
				{
					maxScore = score;

					if (maxScore >= beta)
						break;
				}
			}

			return maxScore;
		}
		//---------------------------------------------------------------------
		// For testing
		internal int GetScoreForWinner(Winner winner, bool machineMove, int depth)
		{
			Winner me = machineMove
				? Winner.Machine
				: Winner.User;

			if (winner == me)
				return 10 - depth;
			else
				return -10 + depth;
		}
	}
}
