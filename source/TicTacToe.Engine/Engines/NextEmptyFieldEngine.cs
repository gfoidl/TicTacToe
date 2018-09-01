namespace TicTacToe.Engine.Engines
{
	public class NextEmptyFieldEngine : IEngine
	{
		public int FindBestMove(Board board)
		{
			foreach (int index in board.GetEmptyFields())
				return index;

			return -1;
		}
	}
}
