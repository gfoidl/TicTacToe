namespace TicTacToe.Engine.Engines
{
	public class AlphaBetaPruningEngine : MiniMaxEngine
	{
		protected override int MiniMax(Board board, bool machineMove, int depth)
		{
			const int alpha    = -10_000;
			const int beta 	   =  10_000;
			const int maxScore = alpha;

			// First call to "max" already done, so this is a call to "min"
			return this.AlphaBetaPruning(board, machineMove, depth, -beta, -maxScore);
		}
		//---------------------------------------------------------------------
		private int AlphaBetaPruning(Board board, bool machineMove, int depth, int alpha, int beta)
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
				Board newBoard  = board;     // makes a copy
				newBoard[index] = machineMove ? FieldState.Machine : FieldState.User;
				int score       = -this.AlphaBetaPruning(newBoard, !machineMove, depth + 1, -beta, -maxScore);

				if (score > maxScore)
				{
					maxScore = score;

					if (maxScore >= beta)
						break;
				}
			}

			return maxScore;
		}
	}
}
