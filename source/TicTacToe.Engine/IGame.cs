namespace TicTacToe.Engine
{
	public interface IGame
	{
		Winner Winner 	{ get; }

		MoveResult MakeMachineMove();
		MoveResult MakeMove(int fieldIdx);
	}
}
