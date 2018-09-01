using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TicTacToe.Engine.Engines
{
	public class MiniMaxEngine : IEngine
	{
		public int FindBestMove(Board board)
		{
			Debug.WriteLine($"{board}\n");

			int bestMove  = -1;
			int bestScore = int.MinValue;

			foreach (int index in board.GetEmptyFields())
			{
				Board newBoard 	= board;     // makes a copy
				newBoard[index] = FieldState.Machine;
				int score 		= -this.MiniMax(newBoard, machineMove: false, depth: 0);
				Debug.WriteLine($"field: {index}\tscore: {score}");

				if (score > bestScore)
				{
					bestScore = score;
					bestMove  = index;
				}
			}

			return bestMove;
		}
		//---------------------------------------------------------------------
		protected virtual int MiniMax(Board board, bool machineMove, int depth)
		{
			Winner winner = board.CheckWinner();
			if (winner != Winner.None)
				return this.GetScoreForWinner(winner, machineMove, depth);

			// Draw
			if (!board.HasEmptyFields())
				return 0;

			int maxScore = int.MinValue;
			foreach (int index in board.GetEmptyFields())
			{
				Board newBoard 	= board;     // makes a copy
				newBoard[index] = machineMove ? FieldState.Machine : FieldState.User;
				int score 		= -this.MiniMax(newBoard, !machineMove, depth + 1);

				if (score > maxScore)
					maxScore = score;
			}

			return maxScore;
		}
		//---------------------------------------------------------------------
		// internal for testing
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int GetScoreForWinner(Winner winner, bool machineMove, int depth)
		{
			Winner me = machineMove
				? Winner.Machine
				: Winner.User;

			return winner == me
				? 10 - depth
				: -10 + depth;
		}
	}
}
